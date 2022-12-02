using Common.Classes;
using Common.Commands;
using Common.Controllers;
using Common.Enums;

namespace Common.Helpers
{
    public static class ConsoleHelper
    {

        public static void ShowCommand(Command command)
        {
            ShowProperyHorizantal("Host Name", command.HostName);
            ShowProperyHorizantal("IpV4 Address", command.IPAddress);
            ShowProperyHorizantal("requested", command.HTTPCommand);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" command. ");
            Console.ResetColor();

        }

        public static void ShowCommandHostInfo(Command command)
        {
            ShowPropery("Host Name", command.HostName);
            ShowPropery("IpV4 Address", command.IPAddress);
        }

        public static void ShowStatus(Status status, NetworkSide networkSide)
        {

            string networkSideWord = networkSide == NetworkSide.Server ? "Request" : "Response";

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{networkSideWord} Status: ");

            Console.ForegroundColor = GetColorStatusType(status);
            Console.WriteLine(status.ToString());

            Console.ResetColor();
        }

        public static void ShowCars(List<Car> cars)
        {
            Console.WriteLine("\n~~~~~~~~~Cars");

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            foreach (var car in cars)
            {
                Console.WriteLine(car);

                Console.WriteLine("\n~~~~~~~~~~~~~~~~\n\n");
            }

            Console.ReadKey();

            Console.ResetColor();
        }

        public static void ShowPropery(string name, string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{name}: ");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(value);

            Console.ResetColor();
        }

        public static void ShowProperyHorizantal(string name, string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{name}: ");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"{value} ");

            Console.ResetColor();
        }

        public static void ShowMessage(string text, StatusTypes type, bool readKey = true)
        {
            Console.ForegroundColor = GetColorFromMessageType(type);
            Console.WriteLine($"  {GetIconFromMessageType(type)}  {text} ");

            if (readKey)
                Console.ReadKey();

            Console.ResetColor();
        }

        public static void ShowCommandInfoStyle(bool clearBeforeAfter = true)
        {
            if (clearBeforeAfter)
                Console.Clear();

            Console.WriteLine("~<>~ -> Show Parametres");
            Console.WriteLine("~!~  -> Show Ticks for paramters");
            Console.WriteLine("~?~  -> Show Info text");

            if (clearBeforeAfter)
                Console.Clear();

        }

        public static Car AskCar(bool askId = false)
        {
            Car car = new();
            int id = -1;

            while (true)
            {

                if (askId)
                {
                    id = GetIntInput("Id");
                }

                string name = GetInput("Name");

                string vendor = GetInput("Vendor");

                car.Id = id;
                car.Name = name;
                car.Vendor = vendor;

                if (Controller.Check(car))
                    break;

                ShowMessage("Invalid Car instance ! Please write correct value !", StatusTypes.Error);
                Console.Clear();
            }

            return car;
        }

        public static string GetInput(string info, bool toUpper = false)
        {
            string? input = string.Empty;

            while (true)
            {

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"{info}: ");

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    break;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n{info} can't be empty !\n");
                Console.ResetColor();

            }

            if (toUpper)
                input = input.ToUpper();

            Console.ResetColor();

            return input;
        }

        public static int GetIntInput(string propName)
        {
            int value = 0;
            string temp;

            while (true)
            {

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"{propName}: ");

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                temp = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(temp))
                {
                    try
                    {
                        value = int.Parse(temp);
                        break;
                    }
                    catch (Exception) { }
                }

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n{propName} is not valid !\n");
                Console.ResetColor();
            }

            Console.ResetColor();
            return value;
        }

        public static ConsoleColor GetColorFromMessageType(StatusTypes type)
        {
            return type switch
            {
                StatusTypes.Error => ConsoleColor.DarkRed,
                StatusTypes.Succes => ConsoleColor.DarkGreen,
                StatusTypes.Warning => ConsoleColor.DarkYellow,
                _ => ConsoleColor.White,
            };
        }

        public static ConsoleColor GetColorStatusType(Status type)
        {
            return type switch
            {
                Status.Succes => ConsoleColor.DarkGreen,
                Status.Failed => ConsoleColor.DarkRed,
                _ => ConsoleColor.White,
            };
        }


        public static string GetIconFromMessageType(StatusTypes type)
        {
            switch (type)
            {
                case StatusTypes.Error:
                    return "✘";
                case StatusTypes.Succes:
                    return "✔";
                case StatusTypes.Warning:
                    return "ⓘ";
                default:
                    break;
            }

            return string.Empty;
        }

        public static void Help(Type type, bool clearBeforeAfter = true)
        {
            if (clearBeforeAfter)
                Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.WriteLine("\n~~~~~~~~~HELP for Car Command");
            Console.WriteLine("\n\n~~~Command Info Style:");
            ShowCommandInfoStyle(false);

            Console.WriteLine("\n\n~~~Commands");
            ReflectionHelper.ShowCommandsInfo(type);

            Console.WriteLine();
            Console.ReadKey();

            Console.ResetColor();

            if (clearBeforeAfter)
                Console.Clear();
        }

    }
}
