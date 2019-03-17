using Grayscale.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Grayscale.Models
{
    public class Attachment
    {
        public string Data { get; set; }
        public string Label { get; set; }
        public byte[] Bytes
        {
            get
            {
                return Convert.FromBase64String(this.Data);
            }
        }

        public static List<Attachment> Get(XmlDocument xmlDocument)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Attachment));
            List<Attachment> attachments = new List<Attachment>();

            foreach (XmlNode node in xmlDocument.GetElementsByTagName("Attachment"))
            {
                try
                {

                    attachments.Add((Attachment)serializer.Deserialize(
                        StreamHelper.StringToStream(
                            $"<Attachment>{node.InnerXml.Replace(" xmlns=\"http://www.docusign.net/API/3.0\"", "")}</Attachment>"
                        )));
                }
                catch { }
            }

            return attachments;
        }
    }
}
