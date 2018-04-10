using SmartSol.EasySoap.Http;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TVanClient
{
    class Program
    {
        const string tvanUrl = @"http://daotaonhantokhai.gdt.gov.vn/ihtkk_van/NhanHSoThueServicePort";

        static void Main(string[] args)
        {

            /*HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tvanUrl);
            request.Method = "POST";
            request.ContentType = @"multipart/related; boundary=MIME_boundary; charset=utf-8; type=text/xml";
            request.Accept = "text/xml";

            var buffer = File.ReadAllLines("SoapDocuments//RequestBody.txt");
            using (var requestStream = request.GetRequestStream())
            {
                using (var streamWriter = new StreamWriter(requestStream))
                {
                    streamWriter.Write(buffer);
                }
            }

            string result = "Failed";
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException webException)
            {
                using (var streamReader = new StreamReader(webException.Response.GetResponseStream()))
                {
                    Console.WriteLine(streamReader.ReadToEnd());
                }
                
            }

            Console.WriteLine(result);
            Console.ReadLine();*/

            var envelope = File.ReadAllText("SoapDocuments//NhanHSoXml.xml");

            SoapRequestBody body = new SoapRequestBody();
            body.AddPart(new EnvelopePart(envelope, "rootpart@easysoap.com"));
            body.AddPart(new FilePart(File.Open("TKhai//tkhai_seatech.xml", FileMode.Open), "text/xml charset=UTF-8", "quoted-printable", "tkhai_seatech.xml"));

            var bodyStream = body.RenderBody();

            Console.WriteLine(Encoding.UTF8.GetString(bodyStream));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tvanUrl);
            request.Method = "POST";
            request.ContentType = body.ContentType;
            request.Headers.Add("MIME-Version", "1.0");
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.Accept = "text/xml";
            request.ContentLength = bodyStream.Length;


            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bodyStream, 0, bodyStream.Length);
            }

            string result = "Failed";
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException webException)
            {
                using (var streamReader = new StreamReader(webException.Response.GetResponseStream()))
                {
                    Console.WriteLine(streamReader.ReadToEnd());
                }

            }

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
