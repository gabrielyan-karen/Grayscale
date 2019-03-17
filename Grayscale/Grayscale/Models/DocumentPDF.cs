using Grayscale.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Grayscale.Models
{
    public class DocumentPDF
    {
        public string Name { get; set; }
        public string PDFBytes { get; set; }
        public int DocumentID { get; set; }
        public string DocumentType { get; set; }
        public string FileName
        {
            get
            {
                return this.Name.Replace(FileType, "");
            }
        }
        public byte[] Bytes
        {
            get
            {
                return Convert.FromBase64String(this.PDFBytes);
            }
        }
        public MemoryStream Stream
        {
            get
            {
                return StreamHelper.ByteArrayToStream(this.Bytes);
            }
        }
        public string FileType
        {
            get
            {
                return Path.GetExtension(this.Name);
            }
        }

        public static List<DocumentPDF> Get(XmlDocument xmlDocument)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DocumentPDF));
            List<DocumentPDF> documentPDFs = new List<DocumentPDF>();

            foreach (XmlNode node in xmlDocument.GetElementsByTagName("DocumentPDF"))
            {
                try
                {
                    documentPDFs.Add((DocumentPDF)serializer.Deserialize(
                        StreamHelper.StringToStream(
                            $"<DocumentPDF>{node.InnerXml.Replace(" xmlns=\"http://www.docusign.net/API/3.0\"", "")}</DocumentPDF>"
                        )));
                }
                catch { }
            }

            return documentPDFs.OrderBy(x => x.DocumentID).ToList();
        }
    }
}
