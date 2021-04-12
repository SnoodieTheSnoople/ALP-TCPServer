using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ALP_TCPChatServer
{
    public class Server
    {
        private IPAddress IP;
        private int portNum;
        private TcpListener serverSock;
        private TcpClient clientSock;
        //private Hashtable clientsList = new Hashtable();
        public static bool runServer = true;
        ServerCmd cmd = new ServerCmd();

        Thread thread;

        public Server() { }

        public Server(IPAddress IP, int portNum)
        {
            this.IP = IP;
            this.portNum = portNum;

            _InitialiseServer();
        }

        private void _InitialiseServer()
        {
            serverSock = new TcpListener(IP, portNum);  //Listens and allows connections with the given IP and port number

            clientSock = default(TcpClient);    //Sets null

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
            //int counter = 0;

            //Allow the server to be shut down using a boolean
            while (runServer)
            {
                try
                {
                    //Prevent AcceptTcpClient() blocking
                    //Allows for the server to be shut from another thread
                    if (serverSock.Pending())
                    {
                        //counter++;
                        clientSock = serverSock.AcceptTcpClient();

                        string username = _GetName();

                        cmd.BroadcastMsg($"{username} has joined the server!", username, false);
                        Console.WriteLine($"{username} has joined the server!");

                        cmd.SendJoin(username);
                        cmd.SendList(clientSock);

                        HandleClient handle = new HandleClient();
                        handle.StartClient(clientSock, username);
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            //Kill server
            KillServer();
            //Console.ReadLine();
        }
        
        private void KillServer()
        {
            clientSock.Close();
            serverSock.Stop();
            Console.WriteLine("<< Server shutdown >>");
            Environment.Exit(0);
        }

        /*
        public void RestartServer()
        {
            clientThread.Join();
            clientSock.Close();
            serverSock.Stop();
            Console.WriteLine("<< Restarting server >>");
            _InitialiseServer();
        }
        */
        public void ChangeRunningStatus()
        {
            runServer = false;
        }
    }
}
