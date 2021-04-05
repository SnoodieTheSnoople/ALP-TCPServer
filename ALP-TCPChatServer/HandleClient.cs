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

        public void StartClient(TcpClient clientSock, string clientName, Hashtable clientList)
        {
            _clientSock = clientSock;
            _clientName = clientName;
            _clientsList = clientList;
            _clientThread = new Thread(_DoChat);

            _clientThread.Start();

        }

        private void _DoChat(object obj)
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[_clientSock.ReceiveBufferSize];
            string dataFromClient = null;
            //byte[] sendBytes = null;
            //string serverResponse = null;
            string rCount = null;

            while (true)
            {
                try
                {
                    requestCount++;
                    NetworkStream nS = _clientSock.GetStream();

                    nS.ReadTimeout = 100000;

                    int bytesRead = nS.Read(bytesFrom, 0, _clientSock.ReceiveBufferSize);

                    if (bytesRead == 0)
                    {
                        throw new System.IO.IOException("Connection refused/closed");
                    }

                    dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                    Console.WriteLine($"From {_clientName}: {dataFromClient}");

                    rCount = Convert.ToString(requestCount);

                    Program.BroadcastMsg(dataFromClient, _clientName, true);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Connection closed");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            _EndProcess();
        }

        private void _EndProcess()
        {
            _clientSock.Close();
            Program.RemoveFromList(_clientSock, _clientName);
        }
    }
}
