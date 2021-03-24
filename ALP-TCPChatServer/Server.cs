using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ALP_TCPChatServer
{
    public class Server
    {
        IPAddress IP;
        int portNum;
        TcpListener serverSock;
        TcpClient clientSock;
        Hashtable clientsList = new Hashtable();

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
            clientSock = serverSock.AcceptTcpClient();
        }

        private string GetName()
        {
            byte[] bytesFrom = new byte[clientSock.ReceiveBufferSize];
            string dataFromClient = null;

            NetworkStream networkStream = clientSock.GetStream();
            networkStream.Read(bytesFrom, 0, clientSock.ReceiveBufferSize);

            dataFromClient = Encoding.UTF8.GetString(bytesFrom);
            dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

            AddClient(dataFromClient, clientSock);
            foreach (string key in clientsList.Keys)
            {
                Console.WriteLine($"{key} : {clientsList[key]}");
            }

            return dataFromClient; // Username received
        }

        private void BroadcastMsg(string msg, string username, bool userMsgFlag)
        {
            throw new NotImplementedException();
        }

        private void RunServer()
        {
            int counter = 0;

            while (true)
            {
                counter++;
                AcceptClient();
                string username = GetName();

                BroadcastMsg($"{username} has joined the server!", username, false);
                Console.WriteLine($"{username} has joined the server!");

                HandleClient handle = new HandleClient();
                handle.StartClient(clientSock, username, clientsList);
            }
        }

        public void AddClient(string name, TcpClient clientSocket)
        {
            clientsList.Add(name, clientSocket);
        }
    }
}
