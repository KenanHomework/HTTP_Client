using Common.Commands;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class HTTPHelper
    {

        public static void SendCommand(BinaryWriter binaryWriter, Status status, string data = " ")
        {
            var command = new Command();

            command.Status = status;
            command.Data = data;

            binaryWriter.Write(JsonSerializer.Serialize(command));

        }

        public static void SendCommand(BinaryWriter binaryWriter, string commandName, string data = " ")
        {
            var command = new Command();
            command.HTTPCommand = commandName;
            command.Data = data;

            binaryWriter.Write(JsonSerializer.Serialize(command));

        }

    }
}
