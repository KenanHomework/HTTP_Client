using Common.Classes;
using Common.Commands;
using Common.Controllers;
using Common.Enums;
using Common.Helpers;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Server
{
    public class Program
    {
        public static IPAddress IpAddress = IPAddress.Loopback;
        public static int Port = 27001;
        public static TcpClient Client { get; set; }
        public static TcpListener Listener { get; set; }
        public static NetworkStream Stream { get; set; }
        public static BinaryReader BinaryReader { get; set; }
        public static BinaryWriter BinaryWriter { get; set; }

        public static List<Car> Cars { get; set; } = new List<Car>();

        static void Main(string[] args)
        {

            #region Init Cars With Default

            Cars.Add(new Car()
            {
                Id = ++gloabalID,
                Name = "M8",
                Vendor = "BMW"
            });

            Cars.Add(new Car()
            {
                Id = ++gloabalID,
                Name = "M4I",
                Vendor = "BMW"
            });

            Cars.Add(new Car()
            {
                Id = ++gloabalID,
                Name = "S90",
                Vendor = "VOLVO"
            });

            Cars.Add(new Car()
            {
                Id = ++gloabalID,
                Name = "XC-90",
                Vendor = "Volvo"
            });


            #endregion

            Listener = new TcpListener(IpAddress, Port);
            Listener.Start(100);
            while (true)
            {

                Client = Listener.AcceptTcpClient();
                Stream = Client.GetStream();
                BinaryReader = new BinaryReader(Stream);
                BinaryWriter = new BinaryWriter(Stream);
                while (true)
                {
                    RecieveCommand();
                }
            }
        }

        public static int gloabalID = 0;

        static public void RecieveCommand()
        {
            string input = BinaryReader.ReadString();

            if (string.IsNullOrEmpty(input))
                return;

            var command = JsonSerializer.Deserialize<Command>(input);

            ConsoleHelper.ShowCommand(command);

            switch (command.HTTPCommand)
            {
                case Car.GetCommand: GetCommand(); break;

                case Car.PutCommand: PutCommand(command); break;

                case Car.PostCommand: PostCommand(command); break;

                case Car.DeleteCommand: DeleteCommand(command); break;

                default:
                    ConsoleHelper.ShowMessage("Unknow Command !", StatusTypes.Error);
                    Console.Clear();
                    break;
            }


        }

        public static void GetCommand()
        {
            HTTPHelper.SendCommand(BinaryWriter, Status.Succes, JsonSerializer.Serialize(Cars));
            ConsoleHelper.ShowStatus(Status.Succes, NetworkSide.Server);
        }

        public static void PutCommand(Command command)
        {
            Car car = JsonSerializer.Deserialize<Car>(command.Data);

            if (!Controller.Check(car))
            {
                HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "Car instance is invalid !");
                ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

                return;

            }

            int id = Cars.MaxBy(c => c.Id).Id;
            car.Id = ++id;

            foreach (var item in Cars)
            {
                if (item.Equals(car))
                {
                    HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "Car instance allready existed !");
                    ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

                    return;
                }
            }

            Cars.Add(car);

            HTTPHelper.SendCommand(BinaryWriter, Status.Succes, "Succes");
            ConsoleHelper.ShowStatus(Status.Succes, NetworkSide.Server);

        }

        public static void PostCommand(Command command)
        {
            #region Recive Id

            int id = int.Parse(command.Data);

            if (id < 0)
            {
                HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "Car ID can't be less tha zero !");
                ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

                return;

            }

            bool idExsited = false;

            foreach (var item in Cars)
            {
                if (item.Id == id)
                {
                    idExsited = true;
                    break;
                }
            }

            if (idExsited)
            {
                HTTPHelper.SendCommand(BinaryWriter, Status.Succes);
            }
            else
            {
                HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "No car was found with this ID !");
                ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

                return;
            }


            #endregion

            string input = BinaryReader.ReadString();

            if (string.IsNullOrEmpty(input))
            {
                HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "Command can't be empt !");
                ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

                return;
            }

            command = JsonSerializer.Deserialize<Command>(input);

            if (command == null)
            {
                HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "Comman can't be empty !");
                ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

                return;
            }

            Car? car = JsonSerializer.Deserialize<Car>(command.Data);

            // For Controller Check method because than id less than zero check return false
            car.Id = 0;

            if (!Controller.Check(car))
            {
                HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "Car instance is invalid !");
                ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

                return;
            }

            for (int i = 0; i < Cars.Count; i++)
            {
                Car? item = Cars[i];

                if (item.Id == id)
                {
                    Cars[i].Name = car.Name;
                    Cars[i].Vendor = car.Vendor;

                    HTTPHelper.SendCommand(BinaryWriter, Status.Succes, "Succes");
                    ConsoleHelper.ShowStatus(Status.Succes, NetworkSide.Server);

                    return;
                }
            }

            HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "Car instance not found !");
            ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

        }

        public static void DeleteCommand(Command command)
        {
            int id = int.Parse(command.Data);

            if (id < 0)
            {
                HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "Car ID can't be less tha zero !");
                ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

                return;

            }

            for (int i = 0; i < Cars.Count; i++)
            {
                Car? car = Cars[i];

                if (car.Id == id)
                {
                    Cars.RemoveAt(i);

                    HTTPHelper.SendCommand(BinaryWriter, Status.Succes, "Succes");
                    ConsoleHelper.ShowStatus(Status.Succes, NetworkSide.Server);

                    return;
                }
            }

            HTTPHelper.SendCommand(BinaryWriter, Status.Failed, "Car instance not found !");
            ConsoleHelper.ShowStatus(Status.Failed, NetworkSide.Server);

        }

    }
}