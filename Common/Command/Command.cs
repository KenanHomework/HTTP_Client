using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Commands
{
    public class Command
    {

        public Command()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    // Get Ipv4 Address
                    IPAddress = ip.ToString();

                    break;
                }
            }

            // Get the Name of HOST  
            HostName = Dns.GetHostName();

        }

        public string HTTPCommand { get; set; }

        public string Data { get; set; }

        public string IPAddress { get; set; }

        public string HostName { get; set; }

        public Status Status { get; set; }

    }
}
