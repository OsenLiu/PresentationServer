using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationApp.websocket
{
    class Server : AppServer<Session>
    {
        public bool isRunning { get; set; }

        private const String senderKey = "F208F146F3C856B184AE48A69C018704F96218464C240009A0BEA7DB0A2C9610";

        private ArrayList apps = new ArrayList();

        public Server()
        {
            this.isRunning = false;
            ServerConfig serverConfig = new ServerConfig();
            serverConfig.Port = 8089;
            serverConfig.Mode = SocketMode.Tcp;
            Setup(serverConfig);
            this.NewSessionConnected += new SessionHandler<Session>(appServer_NewSessionConnected);
            this.SessionClosed += new SessionHandler<Session, CloseReason>(appServer_SessionClosed);
            //this.new += new SessionHandler<Session, string>(appServer_NewMessageReceived);

        }



        protected override void OnStartup()
        {
            base.OnStartup();
            Console.Write(">>>OnStartup");
            isRunning = true;
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            isRunning = false;
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        private void appServer_NewMessageReceived(Session session, string message)
        {
            Console.WriteLine("Client said: " + message);
            try
            {
                if (apps.Contains(session))
                {
                    dynamic msg = Newtonsoft.Json.Linq.JObject.Parse(message);
                    String intent = msg.Intent;
                    if(intent == "page_down")
                    {
                        Logger.Debug("page down");
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        private void appServer_NewSessionConnected(Session session)
        {
            Console.WriteLine();
            Console.WriteLine("New session connected! Sessions counter: " + SessionCount);
            if(apps.Count > 2)
            {
                session.Close();
                return;
            }
            apps.Add(session);
            session.Send("Hello new client!");
        }

        private void appServer_SessionClosed(Session session, CloseReason value)
        {
            Console.WriteLine();
            Console.WriteLine("Client disconnected! Sessions counter: " + SessionCount);
            if(apps.Contains(session))
            {
                apps.Remove(session);
            }
        }

    }
}
