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
        IPAddress IP;
        int portNum;
        TcpListener serverSock;
        TcpClient clientSock;
        Hashtable clientsList = new Hashtable();

        ServerCmd cmd = new ServerCmd();

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
            Console.WriteLine($"<< DETAILS >>\nIP: {IP}\nPort: {portNum}\n\n");

            _RunServer();
        }

        /*
        private void AcceptClient()
        {
            clientSock = serverSock.AcceptTcpClient();
        }
        */

        private string _GetName()
        {
            byte[] bytesFrom = new byte[clientSock.ReceiveBufferSize];
            string dataFromClient = null;

            NetworkStream networkStream = clientSock.GetStream();
            networkStream.Read(bytesFrom, 0, clientSock.ReceiveBufferSize);

            dataFromClient = Encoding.UTF8.GetString(bytesFrom);
            dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

            //AddClient(dataFromClient, clientSock);
            cmd.AddClient(dataFromClient, clientSock);
            /*
            foreach (string key in clientsList.Keys)
            {
                Console.WriteLine($"{key} : {clientsList[key]}");
            }
            */
            return dataFromClient; // Username received
        }

        public void BroadcastMsg(string msg, string username, bool userMsgFlag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;

                NetworkStream broadcastStream = broadcastSocket.GetStream();
                byte[] broadcastBytes;

                if (userMsgFlag == true)
                {
                    broadcastBytes = Encoding.UTF8.GetBytes($"{username} >> {msg}");
                }
                else
                {
                    broadcastBytes = Encoding.UTF8.GetBytes(msg);
                }

                broadcastStream.Write(broadcastBytes);
                broadcastStream.Flush();
            }
        }

        private void _RunServer()
        {
            int counter = 0;

            while (true)
            {
                counter++;
                clientSock = serverSock.AcceptTcpClient();
                string username = _GetName();

                //BroadcastMsg($"{username} has joined the server!", username, false);
                cmd.BroadcastMsg($"{username} has joined the server!", username, false);
                Console.WriteLine($"{username} has joined the server!");

                cmd.SendList();

                HandleClient handle = new HandleClient();
                handle.StartClient(clientSock, username, clientsList);
            }

            //Kill server
            KillServer();
            Console.ReadLine();
        }

        public void AddClient(string name, TcpClient clientSocket)
        {
            //Hashtable -> username, socket number
            clientsList.Add(name, clientSocket);
        }

        public void RemoveClient(TcpClient client, string clientName)
        {
            clientsList.Remove(clientName);
        }

        public void KillServer()
        {
            clientSock.Close();
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


        public void SendList()
        {
            /*
             * Send list:
             * Cycle through hashtable of users
             * Send each user in a message
             * Client receives string, separates by ','
             * Client adds each value in list to listbox
             */

            string usernames = "/namelist/ ";
            //List<string> usernames = new List<string>();

            foreach (string name in clientsList)
            {
                //usernames.Add(name);
                usernames += $"{name}, ";
            }
        }

        public void SendJoin(string name)
        {
            //Sets join protocol
            string userJoin = $"/join/ {name}";
        }
    }
}
