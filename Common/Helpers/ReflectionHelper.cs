using Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class ReflectionHelper
    {

        public static void ShowCommandsInfo(Type type)
        {
            foreach (var field in type.GetRuntimeFields())
            {
                CommandAttribute command = (CommandAttribute)field.GetCustomAttribute(typeof(CommandAttribute));

                if (command == null)
                    continue;

                Console.WriteLine(command);
            }
        }

    }
}
