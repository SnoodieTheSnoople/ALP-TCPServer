using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ALP_TCPChatServer
{
    class Program
    {
        /*
         * IP Addresses to use:
         * Loopback -> 127.0.0.1
         * Macbook Air Local (home): 192.168.1.66
         * Windows PC local (home): 192.168.1.68
         * 
         * Port Number: 2693 (unassigned)
         */

        //public static Hashtable clientsList = new Hashtable();
        // List of clients and sockets connected to server

        static void Main(string[] args)
        {
            //Server server = new Server(System.Net.IPAddress.Parse("192.168.1.66"), 2693);
            //var server = new Server(System.Net.IPAddress.Parse("127.0.0.1"), 2693);
            var server = new Server(System.Net.IPAddress.Parse("192.168.1.68"), 2693);
        }
    }
}
