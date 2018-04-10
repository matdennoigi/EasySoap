using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartSol.EasySoap.Http
{
    public class SoapRequestBody
    {
        const string startPartTempate = "--{0}";
        const string footerTemplate = "--{0}--";
        const string clrf = "\r\n";

        private string _boundary;
        private List<SoapRequestPart> _parts;
        private Encoding _encoding;
        public SoapRequestBody()
        {
            _boundary = Guid.NewGuid().ToString();
            _parts = new List<SoapRequestPart>();
            _encoding = Encoding.UTF8;
        }

        public void AddPart(SoapRequestPart part)
        {
            _parts.Add(part);
        }

        public string Boundary
        {
            get
            {
                return _boundary;
            }
        }

        public string ContentType
        {
            get
            {
                return string.Format("Multipart/Related; boundary=\"{0}\"; type=\"text/xml\"; start=\"<{1}>\"", Boundary, _parts[0].ContentId);
            }
        }

        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }
        }

        public byte[] RenderBody()
        {
            MemoryStream bodyStream = new MemoryStream();

            bool needCLRF = false;
            foreach (var part in _parts)
            {
                if (needCLRF)
                    RenderString(bodyStream, clrf);

                needCLRF = true;

                // Start Boundary
                RenderString(bodyStream, string.Format("--{0}", Boundary));
                RenderString(bodyStream, clrf);

                // Render Part Header
                string header = string.Format("Content-Type: {0}\r\nContent-Transfer-Encoding: {1}\r\nContent-ID: <{2}>", 
                    part.ContentType,
                    part.ContentTransferEncoding,
                    part.ContentId);

                RenderString(bodyStream, header);
                RenderString(bodyStream, clrf);

                object body = part.Body();
                if (body is string)
                {
                    // Redner Part's Body
                    RenderString(bodyStream, clrf);

                    string envelopeString = body as string;
                    RenderString(bodyStream, envelopeString);
                }
                else if (body is FileStream)
                {
                    FileStream fs = body as FileStream;

                    string contentDisposition = string.Format("Content-Disposition: attachment; name=\"{0}\"", Path.GetFileName(fs.Name));
                    RenderString(bodyStream, contentDisposition);
                    RenderString(bodyStream, clrf);

                    // Redner Part's Body
                    RenderString(bodyStream, clrf);

                    const int bufferLength = 4096;
                    byte[] copyBuffer = new byte[bufferLength];
                    int readCount = 0;
                    while ((readCount = fs.Read(copyBuffer, 0, bufferLength)) > 0)
                    {
                        bodyStream.Write(copyBuffer, 0, readCount);
                    }
                }
            }

            // Render Footer
            RenderString(bodyStream, clrf);
            RenderString(bodyStream, string.Format("--{0}--", Boundary));
            RenderString(bodyStream, clrf);

            return bodyStream.ToArray();
        }

        private void RenderString(Stream stream, string content)
        {
            var stringBuffer = Encoding.GetBytes(content);
            stream.Write(stringBuffer, 0, stringBuffer.Length);
        }
    }
}
