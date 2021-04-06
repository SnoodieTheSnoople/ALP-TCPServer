using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ALP_TCPChatServer
{
    class ServerCmd
    {
        private Hashtable clientsList = new Hashtable();

        public void BroadcastMsg(string msg, string username, bool usrMsgFlag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;

                NetworkStream broadcastStream = broadcastSocket.GetStream();
                byte[] broadcastBytes;

                if (usrMsgFlag == true)
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

        public void AddClient(string name, TcpClient clientSock)
        {
            clientsList.Add(name, clientSock);
        }

        public void RemoveClient(string name, TcpClient clientSock)
        {
            clientsList.Remove(name);
        }

        public void SendList()
        {
            string usernames = $"/namelist/ ";

            foreach (string name in clientsList)
            {
                usernames += $"{name}, ";
            }
            BroadcastMsg(usernames, "", false);
        }

        public void SendJoin(string name)
        {

        }


    }
}
