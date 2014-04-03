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
using System.Runtime.Serialization.Formatters;

namespace TDIN_chatclient
{
    public class ChatController
    {

        private static ChatController _singleton = null;

        public static ChatController getController()
        {
            if( _singleton == null )
                _singleton = new ChatController();

            return _singleton;
        }




        private TDIN_chatlib.IPAddress localAddress;
        private TDIN_chatlib.UserSession userSession;
        private TDIN_chatlib.ChatSeverInterface remoteServer;

        private string _uid = Guid.NewGuid().ToString();
        private string _handshakeSessionHash = null;

        private const string SERVER_ADDRESS = "localhost";
        private const int SERVER_PORT = 8081;

        private const string LOCAL_CHAT_SERVICE = "LocalChatObject";


        private ChatController()
        {

            registerLocalClientServer();
        }

        private void registerLocalClientServer()
        {

            //Creating a custom formatter for a TcpChannel sink chain. 
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            //Creating the IDictionary to set the port on the channel instance. 
            IDictionary props = new Hashtable();
            props["port"] = 0;
            //Pass the properties for the port setting and the server provider in the server chain argument. (Client remains null here.) 
            TcpChannel chan = new TcpChannel(props, null, provider);
            // register the channel
            ChannelServices.RegisterChannel(chan, false);

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

        public bool registerWithServer(string host, string port, TDIN_chatlib.LoginUser user)
        {

            string serverURL = "tcp://" + host + ":" + port + "/" + TDIN_chatlib.Constants.SERVER_SERVICE;


            // Create an instance of the remote object
            remoteServer = (TDIN_chatlib.ChatSeverInterface)Activator.GetObject(
                                        typeof(TDIN_chatlib.ChatSeverInterface), serverURL);

            userSession = remoteServer.registerClient(_uid, localAddress, user);

            if (    userSession.SessionHash == null
                 || userSession.SessionHash != this._handshakeSessionHash )
            {
                this._handshakeSessionHash = null;

                throw new TDIN_chatlib.ChatException("Received wrong session hash from handshake");
            }

            return userSession != null ;
        }


        public TDIN_chatlib.UserSession Session
        {
            get { return this.userSession; }
        }

        public TDIN_chatlib.ChatSeverInterface RemoteServer
        {
            get { return this.remoteServer; }
        }

        public string UID
        {
            get { return this._uid; }
        }

        public string SessionHash
        {
            get { return this._handshakeSessionHash; }
            set
            {
                if (this._handshakeSessionHash == null)
                    this._handshakeSessionHash = value;
            }
        }


    }
}
