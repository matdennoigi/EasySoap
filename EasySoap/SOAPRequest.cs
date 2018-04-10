using System.Net;
using System.Threading;

namespace SmartSol.EasySoap
{
    public class SOAPRequest<TResponse>
    {
        private HttpWebRequest _webRequest;
        internal SOAPRequest(HttpWebRequest webRequest)
        {
            _webRequest = webRequest;
        }

        public TResponse Request()
        {
            return default(TResponse);
        }
    }
}
