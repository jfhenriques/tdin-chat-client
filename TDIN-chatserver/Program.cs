using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters;


namespace TDIN_chatserver
{
    class Program
    {

        private const string USER_STORE_FILE = "UserStore.xml";
        public static UserStore store = null;

        private static void registerServer()
        {

            store = UserStore.loadStore(USER_STORE_FILE);

            if (store != null)
                Console.WriteLine("* Loaded " + store.userStore.Count + " users from store");

            else
            {
                Console.WriteLine("* No users loaded from file");
                store = UserStore.getNewStore();
                //UserStore.saveStore(store, USER_STORE_FILE);
            }

            //Creating a custom formatter for a TcpChannel sink chain. 
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            //Creating the IDictionary to set the port on the channel instance. 
            IDictionary props = new Hashtable();
            props["port"] = TDIN_chatlib.Constants.DEFAULT_SERVER_PORT;
            //Pass the properties for the port setting and the server provider in the server chain argument. (Client remains null here.) 
            TcpChannel chan = new TcpChannel(props, null, provider);
            // register the channel
            ChannelServices.RegisterChannel(chan, false);


            //Console.WriteLine("a: " + localAddress.IP + ", p: " + localAddress.PORT);

            Console.WriteLine("* Registering Server Object.");
            Console.WriteLine("* Running on port: " + TDIN_chatlib.Constants.DEFAULT_SERVER_PORT);
            

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ChatServer),
                                                                   TDIN_chatlib.Constants.SERVER_SERVICE,
                                                                   WellKnownObjectMode.Singleton);

            System.Console.ReadLine();
        }

        public static bool addUser(TDIN_chatlib.LoginUser user)
        {
            if (user != null && user.Username != null)
            {
                store.userStore.Add(user.Username, user);
                UserStore.saveStore(store, USER_STORE_FILE);

                return true;
            }

            return false;
        }


        static void Main(string[] args)
        {
            //new ChatServer();
            registerServer();
        }

        
    }
}
