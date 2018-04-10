using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSol.EasySoap.Http
{
    public class EnvelopePart : SoapRequestPart
    {
        private string _body;
        public EnvelopePart(string body, string contentID = "envelopePart", string transferEncoding = "8bit")
            : base("text/xml; charset=UTF-8", transferEncoding, contentID)
        {
            _body = body;
        }

        public override object Body()
        {
            return _body;
        }
    }
}
