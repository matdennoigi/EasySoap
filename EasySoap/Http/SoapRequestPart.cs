using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSol.EasySoap.Http
{
    public abstract class SoapRequestPart
    {
        private string _contentType;
        private string _contentTransferEncoding;
        private string _contentID;
        public SoapRequestPart(string contentType, string contentTransferEncoding, string contentID)
        {
            _contentType = contentType;
            _contentTransferEncoding = contentTransferEncoding;
            _contentID = contentID;
        }

        public string ContentType
        {
            get
            {
                return _contentType;
            }
            set
            {
                _contentType = value;
            }
        }

        public string ContentTransferEncoding
        {
            set
            {
                _contentTransferEncoding = value;
            }
            get
            {
                return _contentTransferEncoding;
            }
        }

        public string ContentId
        {
            set
            {
                _contentID = value;
            }
            get
            {
                return _contentID;
            }
        }

        public abstract object Body();
    }
}
