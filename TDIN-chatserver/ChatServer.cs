﻿using System;
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
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace TDIN_chatserver
{
    class ChatServer : MarshalByRefObject, TDIN_chatlib.ChatSeverInterface
    {

        //private IList<TDIN_chatlib.IPUser> activeClients = new List<TDIN_chatlib.IPUser>();
        // Redudant... The string is the same as inside de UserSession class. 
        // The users are also in the activeclients IList.
        private Dictionary<string, TDIN_chatlib.IPUser> sessions = new Dictionary<string, TDIN_chatlib.IPUser>();
        private Dictionary<string, TDIN_chatlib.UserSubscribeInterface> sessionInterface = new Dictionary<string, TDIN_chatlib.UserSubscribeInterface>();
        private IList<TDIN_chatlib.IPUser> _tempIPList = null;
        private readonly object syncLock = new object();
        private long _count = 0;

        public ChatServer()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<TDIN_chatlib.IPUser> getActiveClients(long count)
        {
            // build temporary list with all active clients, preventing this list from being repetitively built on every query
            Console.WriteLine("* Received update request: " + count);

            lock (syncLock)
            {
                if (_tempIPList == null)
                    _tempIPList = new List<TDIN_chatlib.IPUser>(this.sessions.Values);

                Console.WriteLine("* Returning client list to: " + count);
                return _tempIPList;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public TDIN_chatlib.UserSession registerClient(string uid, int port, TDIN_chatlib.LoginUser user)
        {
            //TODO: Complete according with the interface specification.

            if (uid.Length == 0 || port < 1 || port > 65535 || user == null)
                throw new TDIN_chatlib.ChatException("Badly constructed registration");

            if (!user.isValidLogin())
                throw new TDIN_chatlib.ChatException("Please fill username and password");

            TDIN_chatlib.LoginUser fromStore = Program.store.getUserByUsername(user.Username);

            if (fromStore != null)
            {
                if (!fromStore.comparePassword(user))
                    throw new TDIN_chatlib.ChatException("Invalid password for this username");
            }
            else
            {
                if (!user.isValidRegister())
                    throw new TDIN_chatlib.ChatException("User not registered please fill a name for this user");

                else
                {
                    fromStore = new TDIN_chatlib.LoginUser(user);
                    fromStore.generateUID();
                    
                    Program.addUser(fromStore);
                }
            }

            // Create user and add it to active users.
            TDIN_chatlib.IPUser ipUser = new TDIN_chatlib.IPUser(fromStore);
            TDIN_chatlib.UserSession session = new TDIN_chatlib.UserSession(fromStore);

            ipUser.IPAddress = new TDIN_chatlib.InternalIPAddress(
                                            CallContext.GetData("ClientIPAddress").ToString(),
                                            port);
            ipUser.UUID = session.UUID = fromStore.UUID;
            session.SessionHash = TDIN_chatlib.Utils.generateRandomHash();

            //TDIN_chatlib.InternalIPAddress address

            try
            {
                string url = "tcp://" + ipUser.IPAddress.IP + ":" + port + "/" + TDIN_chatlib.Constants.CLIENT_SUBSCRIBE_SERVICE,
                       handshakeUID;

                TDIN_chatlib.UserSubscribeInterface usi = (TDIN_chatlib.UserSubscribeInterface)Activator.GetObject(
                                                                                    typeof(TDIN_chatlib.ChatSeverInterface), url);

                handshakeUID = usi.handshake(session.SessionHash);

                if (handshakeUID != uid)
                    throw new TDIN_chatlib.ChatException("UIDs do not match");

                lock (syncLock)
                {
                    sessionInterface.Add(session.SessionHash, usi);
                    sessions.Add(session.SessionHash, ipUser);

                    // force active client list to be rebuilt on next user query
                    _tempIPList = null;

                    Console.WriteLine("* New user: " + session.Username + ", uid: " + uid + ", hash: " + session.SessionHash);
                }

                _createUpdateClientThread();
            }
            catch (TDIN_chatlib.ChatException ex1)
            {
                Console.WriteLine("Error: + " + ex1.Message);
                throw ex1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new TDIN_chatlib.ChatException("Error on handshake");
            }

            Console.WriteLine("* Registering client with user: " + session.Username + " terminated");

            return session;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashCode"></param>
        public void disconnectClient(string hashCode)
        {
            lock (syncLock)
            {
                if (sessionInterface.ContainsKey(hashCode))
                {
                    sessions.Remove(hashCode);
                    sessionInterface.Remove(hashCode);

                    _tempIPList = null;

                    _createUpdateClientThread();
                }
            }
        }

        private void _createUpdateClientThread()
        {
            Thread t = new Thread(informClientsListChanged);
            t.TrySetApartmentState(ApartmentState.STA);
            t.Start();
        }


        private void informClientsListChanged()
        {
            lock (syncLock)
            {
                Console.WriteLine("* Informing users client list has changed");

                List<string> itemsToRemove = new List<string>();

                foreach (KeyValuePair<string, TDIN_chatlib.UserSubscribeInterface> user in sessionInterface)
                {
                    try
                    {
                        long c = ++_count;
                        Console.WriteLine("* Informing: " + user.Key +  " width update count: " +  c);

                        user.Value.clientListUpdated(c);
                    }
                    catch (Exception e)
                    {
                        // if exception is thrown, its a sign the user has disconnected, so remove it from list;
                        Console.WriteLine("* Session: " + user.Key + " did not responde, removing");

                        itemsToRemove.Add(user.Key);
                    }
                }

                foreach (string s in itemsToRemove)
                {
                    sessions.Remove(s);
                    sessionInterface.Remove(s);
                }

                _tempIPList = null;

                Console.WriteLine("* All users informed");
            }
        }
    }
}
