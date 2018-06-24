using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationApp
{
    class Session : AppSession<Session>
    {
        protected override void HandleException(Exception e)
        {
            this.Send("Application error: {0}", e.Message);
        }

        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            Console.Write(">>>HandleUnknownRequest");
            this.Send("Unknow request");
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.Write(">>>OnSessionClosed");
            base.OnSessionClosed(reason);
        }

        protected override void OnSessionStarted()
        {
            //this.Send("Welcome to SuperSocket Telnet Server");
        }


    }
}
