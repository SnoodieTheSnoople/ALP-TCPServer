using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ALP_TCPChatServer
{
    public class Server
    {
        private IPAddress IP;
        private int portNum;
        private static TcpListener serverSock;
        private static TcpClient clientSock;
        private Hashtable clientsList = new Hashtable();
        public static bool runServer;
        ServerCmd cmd = new ServerCmd();

        public Server() { }

        public Server(IPAddress IP, int portNum)
        {
            this.IP = IP;
            this.portNum = portNum;

            _InitialiseServer();
        }

        private void _InitialiseServer()
        {
            serverSock = new TcpListener(IP, portNum);
            clientSock = default(TcpClient);
            serverSock.Start();
            Console.WriteLine("<< SERVER STARTED >> ");
            Console.WriteLine($"<< DETAILS >>\nIP: {IP}\nPort: {portNum}\n--------------------" +
                $"---------------\n");

            _RunServer();
        }

        private string _GetName()
        {
            byte[] bytesFrom = new byte[clientSock.ReceiveBufferSize];
            string dataFromClient = null;

            NetworkStream networkStream = clientSock.GetStream();
            networkStream.Read(bytesFrom, 0, clientSock.ReceiveBufferSize);

            dataFromClient = Encoding.UTF8.GetString(bytesFrom);
            dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

            cmd.AddClient(dataFromClient, clientSock);

            return dataFromClient; // Username received
        }

        private void _RunServer()
        {
            int counter = 0;

            //while (true)
            while (!runServer)
            {
                try
                {
                    counter++;
                    //ERROR HERE WHEN RESTARTING
                    clientSock = serverSock.AcceptTcpClient();
                    //----
                    string username = _GetName();

                    //BroadcastMsg($"{username} has joined the server!", username, false);
                    cmd.BroadcastMsg($"{username} has joined the server!", username, false);
                    Console.WriteLine($"{username} has joined the server!");

                    cmd.SendJoin(username);
                    cmd.SendList(clientSock);

                    HandleClient handle = new HandleClient();
                    handle.StartClient(clientSock, username, clientsList);
                }
                catch
                {
                    break;
                }
            }

            //Kill server
            KillServer();
            Console.ReadLine();
        }

        public void KillServer()
        {
            //clientSock.Close();
            serverSock.Stop();
            Console.WriteLine("<< Server shutdown >>");
        }

        public void RestartServer()
        {
            clientSock.Close();
            serverSock.Stop();
            Console.WriteLine("<< Restarting server >>");
            _InitialiseServer();
        }

        public void ChangeStatusTrue()
        {
            runServer = true;
        }
    }
}
