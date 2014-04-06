using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDIN_chatlib
{
    public interface UserSubscribeInterface
    {
        /// <summary>
        /// Informa ao cliente que a lista dos users activos foi modificada, e se este o quiser poderá efectuar um novo pedido dessa lista acualizada
        /// </summary>
        void clientListUpdated(long count);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionHash"></param>
        /// <returns></returns>
        string handshake(string sessionHash);
    }
}
