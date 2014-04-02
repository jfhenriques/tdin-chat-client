using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace TDIN_chatclient
{
    public class ChatController
    {

        private TDIN_chatlib.IPAddress localAddress;
        private TDIN_chatlib.UserSession userSession;
        private TDIN_chatlib.ChatSeverInterface remoteServer;

        private const string SERVER_ADDRESS = "localhost";
        private const int SERVER_PORT = 8081;

        private const string LOCAL_CHAT_SERVICE = "LocalChatObject";


        public ChatController()
        {

            registerLocalClientServer();
            //registerWithServer();
        }

        private void registerLocalClientServer()
        {
            IDictionary props = new Hashtable();
            props["port"] = 0;

            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            TcpChannel chan = new TcpChannel(props, clientProvider, serverProvider);  // instantiate the channel
            ChannelServices.RegisterChannel(chan, false);                             // register the channel

            ChannelDataStore data = (ChannelDataStore)chan.ChannelData;

            IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());
            string _ip = "127.0.0.1";


            foreach(IPAddress ip in IPHost.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    _ip = ip.ToString();
                    // yep, dont stop and grab the last one on the list
                }
            }

            localAddress = new TDIN_chatlib.IPAddress(
                                _ip, // get IP
                                new Uri(data.ChannelUris[0]).Port  // get the port
                            );

            //Console.WriteLine("a: " + localAddress.IP + ", p: " + localAddress.PORT);


            Console.WriteLine("* Registering Server Subscription Object");
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(UserSubscribe),
                            TDIN_chatlib.Constants.CLIENT_SUBSCRIBE_SERVICE,
                            WellKnownObjectMode.Singleton);

            Console.WriteLine("* Registering Local Chat Object");
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(LocalClient),
                            LOCAL_CHAT_SERVICE,
                            WellKnownObjectMode.Singleton);
        }

        public void registerWithServer(TDIN_chatlib.LoginUser user)
        {

            string serverURL = "tcp://" + SERVER_ADDRESS + ":" + SERVER_PORT + "/" + TDIN_chatlib.Constants.SERVER_SERVICE;


                // Create an instance of the remote object
                remoteServer = (TDIN_chatlib.ChatSeverInterface)Activator.GetObject(
                                            typeof(TDIN_chatlib.ChatSeverInterface), serverURL);

                userSession = remoteServer.registerClient(localAddress, user);


        }


    }
}
