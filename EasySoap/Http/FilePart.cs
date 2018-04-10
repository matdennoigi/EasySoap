using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartSol.EasySoap.Http
{
    public class FilePart : SoapRequestPart
    {
        private FileStream _body;
        public FilePart(FileStream file, string contentType, string contentTransferEncoding, string contentID) 
            : base(contentType, contentTransferEncoding, contentID)
        {
            _body = file;
        }

        public override object Body()
        {
            return _body;
        }
    }
}
