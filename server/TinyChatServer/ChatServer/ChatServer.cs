﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TinyChatServer.Model;
using TinyChatServer.Server;
using TinyChatServer.Server.ClientProcess;

namespace TinyChatServer.ChatServer
{
    public class ChatServer
    {
        public delegate void MessageHandler(string msg);
        public event MessageHandler OnMessageRecived;
        public event MessageHandler OnErrMessageRecived;

        public delegate void ClientDataHandler(ChatClient client, Packet msg);
        public event ClientDataHandler OnClientDataRecived;

        public delegate void SocketHandler(ChatClient client);
        public event SocketHandler OnClientConnected;
        public event SocketHandler OnClientDisConnect;
        public event SocketHandler OnClientDisConnected;

        private Dictionary<IPEndPoint, ClientSocket> ClientAuthenticationQueue;
        private SocketServer SocketServer;
        private ChatClientManager ChatClientManager;

        public ChatServer()
        {
            ClientAuthenticationQueue = new Dictionary<IPEndPoint, ClientSocket>();
            SocketServer = new SocketServer(1024);

            SocketServer.OnErrMessageRecived += SocketServer_OnErrMessageRecived;
            SocketServer.OnMessageRecived += SocketServer_OnMessageRecived;
            SocketServer.OnClientConnected += SocketServer_OnClientConnected;
            SocketServer.OnClientDataRecived += SocketServer_OnClientDataRecived;
            SocketServer.OnClientDisConnect += SocketServer_OnClientDisConnect;
            SocketServer.OnClientDisConnected += SocketServer_OnClientDisConnected;

            ChatClientManager = new ChatClientManager();
        }

        private void SocketServer_OnMessageRecived(string msg) => OnMessageRecived?.Invoke(msg);
        private void SocketServer_OnErrMessageRecived(string msg) => OnErrMessageRecived?.Invoke(msg);

        private void SocketServer_OnClientConnected(ClientSocket client)
        {
            //ChatClientManager.AddClient(client.IPEndPoint, );
            Console.WriteLine("SocketServer_OnClientConnected");
        }
        private void SocketServer_OnClientDataRecived(ClientSocket client, string msg)
        {
            Console.WriteLine("SocketServer_OnClientDataRecived {0}", msg);
        }
        private void SocketServer_OnClientDisConnect(ClientSocket client)
        {
            Console.WriteLine("SocketServer_OnClientDisConnect");
        }
        private void SocketServer_OnClientDisConnected(ClientSocket client)
        {
            Console.WriteLine("SocketServer_OnClientDisConnected");
        }

        public void Start()
        {
            SocketServer.Start(20310);
        }

        public void Stop()
        {
            SocketServer.Stop();
        }
    }
}