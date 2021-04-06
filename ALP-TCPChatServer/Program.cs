using System;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ALP_TCPChatServer
{
    /*
     * TODO: Implement ping/polling to check if socket is alive
     * TODO: Be able to send hashtable to clients and update
     * TODO: Kill server command
     * TODO: Restart server command
     * TODO: Create protocol cases
     * Notes:
     * - System works, disconnects without server crashing
     * 
     * - Remove ReadTimeout and replace with ping pong to check if alive, prevents closing the socket 
     */

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

            /*
            //System.Net.IPAddress IP = System.Net.IPAddress.Parse("192.168.1.66");
            string IP = "192.168.1.66";
            int portNum = 2693;

            TcpListener serverSock = new TcpListener(System.Net.IPAddress.Parse(IP), portNum);
            TcpClient clientSock = default(TcpClient);

            int counter = 0;

            serverSock.Start();
            Console.WriteLine("<< Server Details >>\n" +
                $"<< {IP}:{portNum} >>");

            Console.WriteLine("<< Server Started >>");

            while (true)
            {
                counter++;
                clientSock = serverSock.AcceptTcpClient();

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

            KillServer(clientSock, serverSock);
            Console.ReadLine();
            */
        }

        /*
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

        public static void KillServer(TcpClient cS, TcpListener sSock)
        {
            cS.Close();
            sSock.Stop();
            Console.WriteLine("<< Server shutdown >>");
        }

        public static void RestartServer()
        {
            //TODO: Create separate method so RestartServer() can call method to restart server
        }
        */
    }
}
