using System.Net;

namespace SmartSol.EasySoap
{
    public class SoapRequestBuilder
    {
        private string _url;
        private string _action;
        private UsernameToken _usernameToken;
        public SoapRequestBuilder(string url, string action)
        {
            _url = url;
            _action = action;
        }

        public void SetUsernameToken(string username, string password, PasswordTypes type = PasswordTypes.PasswordText)
        {
            _usernameToken = new UsernameToken { Username = username, Password = password, Type = type };
        }

        public void Build()
        {
            
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Headers.Add("SOAPAction", action);

            return request;
        }
    }
}
