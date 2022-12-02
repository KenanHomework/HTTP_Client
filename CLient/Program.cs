using Common.Classes;
using Common.Commands;
using Common.Enums;
using Common.Helpers;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace CLient
{
    public class Program
    {

        public static IPAddress IpAddress = IPAddress.Loopback;
        //public static IPAddress IpAddress = IPAddress.Parse("10.2.28.1");
        public static int Port = 27001;
        public static TcpClient Client { get; set; }
        public static NetworkStream Stream { get; set; }
        public static BinaryReader BinaryReader { get; set; }
        public static BinaryWriter BinaryWriter { get; set; }

        static void Main(string[] args)
        {
            Client = new TcpClient();
            Client.Connect(IpAddress, Port);
            Stream = Client.GetStream();
            BinaryReader = new BinaryReader(Stream);
            BinaryWriter = new BinaryWriter(Stream);

            while (true)
            {
                CallCommand();
            }
        }

        static public void Get()
        {
            Console.Clear();

            HTTPHelper.SendCommand(BinaryWriter, Car.GetCommand);

            Command response = JsonSerializer.Deserialize<Command>(BinaryReader.ReadString());

            List<Car> cars = JsonSerializer.Deserialize<List<Car>>(response.Data);

            ConsoleHelper.ShowStatus(response.Status, NetworkSide.Client);

            if (response.Status == Status.Failed)
            {
                ConsoleHelper.ShowMessage(response.Data, StatusTypes.Error);

                Console.Clear();

                return;
            }

            if (cars.Count <= 0)
                ConsoleHelper.ShowMessage("Cars is empty.", StatusTypes.Warning);
            else
                ConsoleHelper.ShowCars(cars);

            Console.Clear();

        }

        static public void Put()
        {
            Console.Clear();

            HTTPHelper.SendCommand(BinaryWriter, Car.PutCommand, JsonSerializer.Serialize(ConsoleHelper.AskCar()));

            Command response = JsonSerializer.Deserialize<Command>(BinaryReader.ReadString());

            ConsoleHelper.ShowStatus(response.Status, NetworkSide.Client);

            if (response.Status == Status.Failed)
                ConsoleHelper.ShowMessage(response.Data, StatusTypes.Error, false);

            Console.ReadKey();

        }

        static public void Post()
        {
            Console.Clear();

            // Send ID

            HTTPHelper.SendCommand(BinaryWriter, Car.PostCommand, ConsoleHelper.GetIntInput("Id").ToString());

            // Check Sended Id
            Command response = JsonSerializer.Deserialize<Command>(BinaryReader.ReadString());
            if (response.Status == Status.Failed)
            {
                ConsoleHelper.ShowMessage(response.Data, StatusTypes.Error);
                Console.Clear();
                return;
            }

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Enter the details of the vehicle to be updated");
            Console.ResetColor();

            // Send Car
            HTTPHelper.SendCommand(BinaryWriter, Car.PostCommand, JsonSerializer.Serialize(ConsoleHelper.AskCar()));

            response = JsonSerializer.Deserialize<Command>(BinaryReader.ReadString());

            ConsoleHelper.ShowStatus(response.Status, NetworkSide.Client);

            if (response.Status == Status.Failed)
                ConsoleHelper.ShowMessage(response.Data, StatusTypes.Error, false);

            Console.ReadKey();
        }

        static public void Delete()
        {
            Console.Clear();

            HTTPHelper.SendCommand(BinaryWriter, Car.DeleteCommand, ConsoleHelper.GetInput("Id").ToString());
            Command response = JsonSerializer.Deserialize<Command>(BinaryReader.ReadString());

            ConsoleHelper.ShowStatus(response.Status, NetworkSide.Client);

            if (response.Status == Status.Failed)
                ConsoleHelper.ShowMessage(response.Data, StatusTypes.Error, false);

            Console.ReadKey();
        }

        static public void CallCommand()
        {
            Console.Clear();

            string command = ConsoleHelper.GetInput("Enter command or type HELP", true);

            if (string.IsNullOrEmpty(command))
                return;

            switch (command)
            {
                case "HELP": ConsoleHelper.Help(typeof(Car)); break;

                case Car.GetCommand: Get(); break;

                case Car.PostCommand: Post(); break;

                case Car.DeleteCommand: Delete(); break;

                case Car.PutCommand: Put(); break;

                default:
                    ConsoleHelper.ShowMessage("Unknow Command !", StatusTypes.Error);
                    Console.Clear();
                    break;
            }


        }

    }
}