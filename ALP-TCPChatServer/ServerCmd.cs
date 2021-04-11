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
        private static Hashtable _clientsList = new Hashtable();
        private static TcpListener _server;
 
        public ServerCmd() { }

        public ServerCmd(TcpListener server) { _server = server; }

        public ServerCmd(Hashtable clientsList, TcpListener server) 
        { 
            _clientsList = clientsList;
            _server = server;
        }

        public void BroadcastMsg(string msg, string username, bool usrMsgFlag)
        {
            foreach (DictionaryEntry Item in _clientsList)
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
            _clientsList.Add(name, clientSock);
        }

        public void RemoveClient(string name, TcpClient clientSock)
        {
            _clientsList.Remove(name);
        }

        // Creates a duplicate list for the current users in the server
        public void SendList(TcpClient client)
        {
            string usernames = "/namelist/ ";

            foreach (string name in _clientsList.Keys)
            {
                usernames += $"{name}, ";
            }

            NetworkStream listStream = client.GetStream();
            byte[] listBytes = Encoding.UTF8.GetBytes(usernames);
            listStream.Write(listBytes);
            listStream.Flush();
        }

        public void SendJoin(string name)
        {
            string joinMsg = $"/join/ {name}";
            BroadcastMsg(joinMsg, "", false);
        }

        public void SendLeave(string name)
        {
            string leaveMsg = $"/leave/ {name}";
            BroadcastMsg(leaveMsg, "", false);
        }

        public void KillServer()
        {
            _server.Stop();
        }
    }
}
