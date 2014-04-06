using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace TDIN_chatserver
{
    public class UserStore
    {
        public Dictionary<string, TDIN_chatlib.LoginUser> userStore = new Dictionary<string, TDIN_chatlib.LoginUser>();

        private UserStore()
        {
        }


        public TDIN_chatlib.LoginUser getUserByUsername(string username)
        {
            return userStore.ContainsKey(username) ? userStore[username] : null ;
        }



        public static UserStore getNewStore()
        {
            return new UserStore();
        }

        
        public static UserStore loadStore(string file)
        {
            UserStore store = null;

            try
            {
                using (var reader = new StreamReader(file, Encoding.UTF8, true))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(TDIN_chatlib.LoginUser[]));
                    TDIN_chatlib.LoginUser[] _load = (TDIN_chatlib.LoginUser[])ser.Deserialize(reader);
                    store = new UserStore();

                    foreach (var u in _load)
                    {
                        if (u != null && u.Username != null && u.UUID != null)
                        {
                            store.userStore.Add(u.Username, u);
                        }
                    }
                    reader.Close();
                }
                return store;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }

        public static bool saveStore(UserStore store, string file)
        {
            try
            {
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    var ser = new XmlSerializer(typeof(TDIN_chatlib.LoginUser[]));
                    TDIN_chatlib.LoginUser[] _save = new TDIN_chatlib.LoginUser[store.userStore.Count];
                    int count = 0;
                    foreach (var u in store.userStore.Values)
                    {
                        _save[count++] = u;
                    }

                    var memoryStream = new MemoryStream();
                    var streamWriter = new StreamWriter(memoryStream, System.Text.Encoding.UTF8);

                    ser.Serialize(streamWriter, _save);

                    byte[] utf8EncodedXml = memoryStream.ToArray();

                    memoryStream.Close();

                    fileStream.Write(utf8EncodedXml, 0, utf8EncodedXml.Length);
                    fileStream.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }
        }
    }
}
