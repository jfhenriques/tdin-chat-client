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




        private TDIN_chatlib.InternalIPAddress localAddress;
        private TDIN_chatlib.UserSession userSession;
        private TDIN_chatlib.ChatSeverInterface remoteServer;

        private readonly object syncLock = new object();
        public readonly object syncLockChat = new object();

        private IList<TDIN_chatlib.IPUser> userList = null;

        private Dictionary<string, ChatWindow> activeChatsUUID = new Dictionary<string, ChatWindow>();
        private Dictionary<string, ChatWindow> activeChatsSESSION = new Dictionary<string, ChatWindow>();



        private string _session_uid = Guid.NewGuid().ToString();
        private string _handshakeSessionHash = null;

        private const string SERVER_ADDRESS = "localhost";
        private const int SERVER_PORT = 8081;

        public const string LOCAL_CHAT_SERVICE = "LocalChatObject";


        private ChatController()
        {

            registerLocalClientServer();
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
            get { return this._session_uid; }
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

        public IList<TDIN_chatlib.IPUser> UserList
        {
            get { return this.userList; }
        }







        private void registerLocalClientServer()
        {

            //Creating a custom formatter for a TcpChannel sink chain. 
            BinaryServerFormatterSinkProvider providerNext = new BinaryServerFormatterSinkProvider();
            providerNext.TypeFilterLevel = TypeFilterLevel.Full;

            TDIN_chatlib.ClientIPServerSinkProvider provider = new TDIN_chatlib.ClientIPServerSinkProvider();
            provider.Next = providerNext;
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

            localAddress = new TDIN_chatlib.InternalIPAddress(
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

            userSession = remoteServer.registerClient(_session_uid, localAddress, user);

            if (    userSession.SessionHash == null
                 || userSession.SessionHash != this._handshakeSessionHash )
            {
                this._handshakeSessionHash = null;

                throw new TDIN_chatlib.ChatException("Received wrong session hash from handshake");
            }

            return userSession != null ;
        }

        public void updateClientList(long count)
        {
            if (remoteServer == null)
                return;

            lock(syncLock)
            {
                userList = remoteServer.getActiveClients(count);
            }

            Console.WriteLine("* Received new client list, size: " + userList.Count + ", ref id: " + count);

            if (Program.window != null)
                Program.window.refreshCLientList(userList);
        }

        public void informServerExit()
        {
            remoteServer.disconnectClient(_handshakeSessionHash);
        }




        public void putChatSession(string sessionHash, ChatWindow chat)
        {
            activeChatsSESSION.Add(sessionHash, chat);
        }

        public void removeSession(string sessionHash, bool informClose)
        {
            if (sessionHash == null)
                return;

            lock(syncLock)
            {
                if (activeChatsSESSION.ContainsKey(sessionHash))
                {
                    activeChatsSESSION[sessionHash].SessionHash = null;

                    if(informClose)
                        activeChatsSESSION[sessionHash].AppendMsg("* The other user closed this chat or quit the aplication", System.Drawing.Color.Gray);

                    activeChatsSESSION.Remove(sessionHash);
                }
            }
        }

        public void removeUUID(string uuid, bool informClose)
        {
            if (uuid == null)
                return;

            lock (syncLock)
            {
                if (activeChatsUUID.ContainsKey(uuid))
                {
                    removeSession(activeChatsUUID[uuid].SessionHash, informClose);


                    activeChatsUUID.Remove(uuid);
                }
            }
        }


        public ChatWindow requestChat(string sessionHash)
        {
            if (sessionHash == null)
                return null;

            lock (syncLock)
            {
                return activeChatsSESSION.ContainsKey(sessionHash) ? activeChatsSESSION[sessionHash] : null;
            }
        }

        public ChatWindow getChatByUUID(string uuid)
        {
            if (uuid == null)
                return null;

            lock (syncLock)
            {
                if (activeChatsUUID.ContainsKey(uuid))
                    return activeChatsUUID[uuid];
            }

            return null;
        }


        public ChatWindow createChat(string uuid, string forceIP)
        {
            if (uuid == null)
                return null;

            lock (syncLock)
            {
                if (getChatByUUID(uuid) != null)
                    return null;

                TDIN_chatlib.IPUser user = null;

                foreach(var u in userList)
                {
                    if(u.UUID == uuid)
                    {
                        user = u;
                        break;
                    }
                }

                if( user == null )
                    return null;

                ChatWindow chat = new ChatWindow(user, forceIP);

                activeChatsUUID.Add(uuid, chat);

                return chat;
            }
        }

    }
}
