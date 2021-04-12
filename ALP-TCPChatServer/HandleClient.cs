using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ALP_TCPChatServer
{
    public class HandleClient
    {
        private TcpClient _clientSock;
        private string _clientName;
        private Hashtable _clientsList;
        private Thread _clientThread;

        ServerCmd cmd = new ServerCmd();

        //New instance of ServerCmd() to access ChangeRunningStatus()
        Server server = new Server();

        public void StartClient(TcpClient clientSock, string clientName)
        {
            _clientSock = clientSock;
            _clientName = clientName;
            //_clientsList = clientList;
            _clientThread = new Thread(_DoChat);

            _clientThread.Start();
            //_DoChat();
        }

        private void _DoChat()
        {
            //int requestCount = 0;
            byte[] bytesFrom = new byte[_clientSock.ReceiveBufferSize];
            string dataFromClient = null;

            while (true)
            {
                try
                {
                    //requestCount++;
                    NetworkStream nS = _clientSock.GetStream();

                    int bytesRead = nS.Read(bytesFrom, 0, _clientSock.ReceiveBufferSize);

                    if (bytesRead == 0)
                    {
                        throw new System.IO.IOException("Lost connection to client");
                    }

                    dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                    if (dataFromClient.Contains("/kill/"))
                    {
                        //FIX
                        //server.ChangeStatusTrue();
                        //WSACancelBlockingCall where it is closed from another thread
                        //Abort all threads and close server
                        //_clientThread.Abort();
                        //_clientSock.Close();
                        //server.KillServer();
                        server.ChangeRunningStatus();
                    }

                    Console.WriteLine($"From {_clientName}: {dataFromClient}");

                    //rCount = Convert.ToString(requestCount);

                    cmd.BroadcastMsg(dataFromClient, _clientName, true);
                }
                catch (System.IO.IOException)
                {
                    //Console.WriteLine(e);
                    Console.WriteLine("Connection closed");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
            _EndProcess();
        }

        private void _EndProcess()
        {
            _clientSock.Close();
            cmd.RemoveClient(_clientName, _clientSock);
            cmd.SendLeave(_clientName);
        }
    }
}
