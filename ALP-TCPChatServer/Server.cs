using System;
using System.Net;
using System.Net.Sockets;

namespace ALP_TCPChatServer
{
    public class Server
    {
        IPAddress IP;
        int portNum;
        TcpListener serverSock;
        TcpClient clientSock;


        public Server(IPAddress IP, int portNum)
        {
            this.IP = IP;
            this.portNum = portNum;

            InitialiseServer();
        }

        private void InitialiseServer()
        {
            serverSock = new TcpListener(IP, portNum);
            clientSock = default(TcpClient);
            serverSock.Start();
            Console.WriteLine("<< SERVER STARTED >> ");
            Console.WriteLine($"<< DETAILS >>\n IP: {IP}\nPort: {portNum}\n\n");

        }

        private void AcceptClient()
        {

        }
    }
}
