using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;

namespace ALP_TCPChatServer
{
    /*
     * TODO: Implement ping/polling to check if socket is alive
     * TODO: Be able to send hashtable to clients and update
     * TODO: Kill server command
     * TODO: Restart server command
     * 
     * Notes:
     * - System works, disconnects without server crashing
     */

    class Program
    {
        /*
         * IP Addresses to use:
         * Loopback -> 127.0.0.1
         * Macbook Air Local (home): 192.168.1.66
         * 
         * Port Number: 2693 (unassigned)
         */

        public static Hashtable clientsList = new Hashtable();
        // List of clients and sockets connected to server

        static void Main(string[] args)
        {
            //System.Net.IPAddress IP = System.Net.IPAddress.Parse("192.168.1.66");
            string IP = "192.168.1.66";
            int portNum = 2693;

            TcpListener serverListen = new TcpListener(System.Net.IPAddress.Parse(IP), portNum);
            TcpClient clientSock = default(TcpClient);

            int counter = 0;

            serverListen.Start();
            Console.WriteLine($"<< Listening >>\n" +
                $"<< IP: {IP}:{portNum} >>");

            Console.WriteLine("<< Server Started >>");

            while (true)
            {
                counter++;
                clientSock = serverListen.AcceptTcpClient();

                byte[] bytesFrom = new byte[clientSock.ReceiveBufferSize];
                string dataFromClient = null;

                NetworkStream networkStream = clientSock.GetStream();
                networkStream.Read(bytesFrom, 0, clientSock.ReceiveBufferSize);

                dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                clientsList.Add(dataFromClient, clientSock);
                foreach (string key in clientsList.Keys)
                {
                    Console.WriteLine($"{key} : {clientsList[key]}");
                }

                BroadcastMsg($"{dataFromClient} has joined the server!", dataFromClient, false);
                Console.WriteLine($"{dataFromClient} has joined the server!");

                HandleClient handleClient = new HandleClient();
                handleClient.StartClient(clientSock, dataFromClient, clientsList);
                
            }

            // HERE
            // Kill server 

            clientSock.Close();
            serverListen.Stop();
            Console.WriteLine("<< Server Closed >>");
            Console.ReadLine();
        }

        public static void BroadcastMsg(string msg, string username, bool userMsgFlag)
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

        public static void RemoveFromList(TcpClient client, string clientNum)
        {
            clientsList.Remove(clientNum);
        }

        public static void KillServer()
        {

        }

        public static void RestartServer()
        {

        }
    }
}
