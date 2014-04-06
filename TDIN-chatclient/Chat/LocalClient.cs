using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace TDIN_chatclient
{
    class LocalClient : MarshalByRefObject, LocalClientInterface
    {
        private static ChatController controller = null;
        private readonly object synObj = new object();

        public LocalClient()
        {
            if (controller == null)
                controller = ChatController.getController();
        }



        public string handshake(string sessionHash, string cuid)
        {
            ChatWindow chat = controller.requestChat(sessionHash);

            if (chat != null)
            {
                chat.EndpointCUID = cuid;
                return chat.CUID;
            }

            return chat != null ? chat.CUID : null;
        }


        public string startChat(string sessionHash, string cuid, string uuid)
        {
            if( uuid == controller.Session.UUID )
                throw new TDIN_chatlib.ChatException("Cannot open a chat with yourself");

            lock( controller.syncLockChat )
            {
                ChatWindow chat = null;
                try
                {
                    bool canCreate = true;
                    string clientIP = CallContext.GetData("ClientIPAddress").ToString();
                    chat = controller.getChatByUUID(uuid);

                    if (chat == null)
                    {
                        

                        if ((chat = controller.createChat(uuid, clientIP)) == null)
                            canCreate = false;
                    }
                    else
                    {
                        canCreate = false;

                        try
                        {
                            chat.EndPointObject.checkAlive();
                        }
                        catch (Exception e)
                        {
                            if (chat.SessionHash != null)
                                controller.removeSession(chat.SessionHash, false);

                            chat.createCOMMChannel(clientIP);

                            canCreate = true;
                        }

                        if (chat.SessionHash == null)
                            canCreate = true;
                    }

                    if (!canCreate)
                        throw new TDIN_chatlib.ChatException("A chat with this user as already been created");

                    chat.SessionHash = sessionHash;

                    if (cuid != chat.EndPointObject.handshake(sessionHash, chat.CUID))
                    {
                        controller.removeUUID(uuid, false);
                        chat.Dispose();

                        throw new TDIN_chatlib.ChatException("Could not establish handshake");
                    }

                    chat.EndpointCUID = cuid;
                    controller.putChatSession(chat.SessionHash, chat);

                    if (!chat.Visible)
                        Program.window.Invoke((System.Windows.Forms.MethodInvoker)delegate() { chat._safeShow(false); });

                }
                catch (TDIN_chatlib.ChatException ex1)
                {
                    throw ex1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new TDIN_chatlib.ChatException("An error occured while creating the chat window");
                }


                return chat != null ? chat.CUID : null;
            }
        }

        public void stopChat(string sessionHash)
        {
            controller.removeSession(sessionHash, true);
        }


        public void sendMessage(string sessionHash, string msg)
        {
            lock (controller.syncLockChat)
            {
                ChatWindow chat = controller.requestChat(sessionHash);

                chat.AppendMsg(chat.User.Username + ": " + msg, System.Drawing.Color.Blue);
            }
        }

        public bool checkAlive()
        {
            return true;
        }
        
    }
}
