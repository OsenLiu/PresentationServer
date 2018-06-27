using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.WebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationApp
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private const String senderKey = "F208F146F3C856B184AE48A69C018704F96218464C240009A0BEA7DB0A2C9610";
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();


        private WebSocketServer server;
        private ArrayList apps = new ArrayList();
        private OfficeControl control;

        public MainWindow()
        {

            InitializeComponent();
            server = new WebSocketServer();
            ServerConfig serverConfig = new ServerConfig();
            serverConfig.Port = 8089;
            serverConfig.Mode = SocketMode.Tcp;
            serverConfig.MaxConnectionNumber = 2;
            server.Setup(serverConfig);
            server.NewSessionConnected += session_connected;
            server.NewMessageReceived += server_NewMessageReceived;
            server.SessionClosed += server_SessionClosed;
            server.Start();


            control = new OfficeControl();

            initView();

        }

        private void initView()
        {
            if(server.State == ServerState.Starting ||
                server.State == ServerState.Running)
            {
                start_button.Content = "Stop";
            }
            else
            {
                start_button.Content = "Start";

            }

            showIPs();
        }

        private void showIPs()
        {
            StringBuilder sb = new StringBuilder();
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    Console.WriteLine(ni.Name);
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            Console.WriteLine(ip.Address.ToString());
                            sb.Append(ip.Address.ToString() + "\n");
                        }
                    }
                }
            }
            ip_label.Content = sb.ToString();

        }

        private void start_button_Click(object sender, RoutedEventArgs e)
        {


            if (server.State == SuperSocket.SocketBase.ServerState.Running)
            {
                server.Stop();
                start_button.Content = "Start";
            }
            else
            {
                server.Start();
                start_button.Content = "Stop";
            }


        }

        private void session_connected(WebSocketSession session)
        {
            Console.WriteLine();
            Console.WriteLine("New session connected! Sessions counter: " + server.SessionCount);
            Dispatcher.BeginInvoke(new Action(() => {
                this.WindowState = WindowState.Minimized;
            }));
            
        }

        private void server_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine();
            Console.WriteLine("Client disconnected! Sessions counter: " );
            if (apps.Contains(session))
            {
                apps.Remove(session);
            }
        }

        private void server_NewMessageReceived(WebSocketSession session, string message)
        {
           logger.Debug("Client said: " + message);
            try
            {
                dynamic msg = Newtonsoft.Json.Linq.JObject.Parse(message);
                String sender = msg.sender;
                if(sender != senderKey)
                {
                    return;
                }
                String intent = msg.Intent;
                MsgResponse res = new MsgResponse();

                if (intent == "page_down")
                {
                    logger.Debug("page down");
                    if (control.checkPowerpoint())
                    {
                        control.next(OFFICE_TYPE.POWERPOINT);

                        res.code = (int)MsgResponse.RESULT_CODE.SUCCESS;
                        res.message = "OK";
                        res.page = control.currentPage(OFFICE_TYPE.POWERPOINT);
                    }
                    else
                    {
                        res.code = (int)MsgResponse.RESULT_CODE.NOT_FOUND;
                        res.message = "Fail";
                    }
                    session.Send(JsonConvert.SerializeObject(res));

                }
                else if (intent == "page_up")
                {
                    logger.Debug("page up");
                    if (control.checkPowerpoint())
                    {
                        control.previous(OFFICE_TYPE.POWERPOINT);
                        res.code = (int)MsgResponse.RESULT_CODE.SUCCESS;
                        res.message = "OK";
                        res.page = control.currentPage(OFFICE_TYPE.POWERPOINT);

                    }
                    else
                    {
                        res.code = (int)MsgResponse.RESULT_CODE.NOT_FOUND;
                        res.message = "Fail";
                    }
                    session.Send(JsonConvert.SerializeObject(res));
                }
                else if (intent == "first")
                {
                    logger.Debug("move first");
                    if (control.checkPowerpoint())
                    {
                        control.first(OFFICE_TYPE.POWERPOINT);
                        res.code = (int)MsgResponse.RESULT_CODE.SUCCESS;
                        res.message = "OK";
                        res.page = control.currentPage(OFFICE_TYPE.POWERPOINT);

                    }
                    else
                    {
                        res.code = (int)MsgResponse.RESULT_CODE.NOT_FOUND;
                        res.message = "Fail";
                    }
                    session.Send(JsonConvert.SerializeObject(res));
                }
                else if (intent == "last")
                {
                    logger.Debug("move last");
                    if (control.checkPowerpoint())
                    {
                        control.last(OFFICE_TYPE.POWERPOINT);
                        res.code = (int)MsgResponse.RESULT_CODE.SUCCESS;
                        res.message = "OK";
                        res.page = control.currentPage(OFFICE_TYPE.POWERPOINT);

                    }
                    else
                    {
                        res.code = (int)MsgResponse.RESULT_CODE.NOT_FOUND;
                        res.message = "Fail";
                    }

                    session.Send(JsonConvert.SerializeObject(res));
                }
                else if (intent == "get_page")
                {
                    if (control.checkPowerpoint())
                    {
                        res.code = (int)MsgResponse.RESULT_CODE.SUCCESS;
                        res.message = "OK";
                        res.page = control.currentPage(OFFICE_TYPE.POWERPOINT);

                    }
                    else
                    {
                        res.code = (int)MsgResponse.RESULT_CODE.NOT_FOUND;
                        res.message = "Fail";
                    }

                    session.Send(JsonConvert.SerializeObject(res));
                }
                else if (intent == "play")
                {
                    if (control.checkPowerpoint())
                    {
                        control.play(OFFICE_TYPE.POWERPOINT);
                        res.code = (int)MsgResponse.RESULT_CODE.SUCCESS;
                        res.message = "OK";

                    }
                    else
                    {
                        res.code = (int)MsgResponse.RESULT_CODE.NOT_FOUND;
                        res.message = "Fail";
                    }

                    session.Send(JsonConvert.SerializeObject(res));
                }

                else if (intent == "stop_play")
                {
                    if (control.checkPowerpoint())
                    {
                        control.stopRun(OFFICE_TYPE.POWERPOINT);
                        res.code = (int)MsgResponse.RESULT_CODE.SUCCESS;
                        res.message = "OK";

                    }
                    else
                    {
                        res.code = (int)MsgResponse.RESULT_CODE.NOT_FOUND;
                        res.message = "Fail";
                    }

                    session.Send(JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

        }
    }
}
