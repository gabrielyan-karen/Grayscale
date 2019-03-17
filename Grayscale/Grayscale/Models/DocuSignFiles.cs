using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Grayscale.Models
{
    public class DocuSignFiles
    {
        XmlDocument xmlDocument = new XmlDocument();

        public DocuSignFiles(string FileName)
        {
            xmlDocument.Load(FileName);
        }

        public List<DocumentPDF> DocumentPDFs
        {
            get
            {
                return DocumentPDF.Get(xmlDocument);
            }
        }
        public List<Attachment> Attachments
        {
            get
            {
                return Attachment.Get(xmlDocument);
            }
        }

        public List<SaveResult> SaveDocumentPDFs()
        {
            List<SaveResult> saveResult = new List<SaveResult>();

            foreach (DocumentPDF pdf in this.DocumentPDFs)
            {
                string fileName = pdf.Name.Replace(Path.GetExtension(pdf.Name), "");

                try
                {
                    File.WriteAllBytes($"{fileName}.pdf", pdf.Bytes);

                    saveResult.Add(new SaveResult() {
                        FileName = $"{fileName}.pdf",
                        Status = "OK"
                    });
                }
                catch (Exception ex)
                {
                    saveResult.Add(new SaveResult()
                    {
                        FileName = $"{fileName}.pdf",
                        Status = ex.Message
                    });
                }
            }

            return saveResult;
        }

        public List<SaveResult> SaveAttachments()
        {
            List<SaveResult> saveResult = new List<SaveResult>();

            foreach (Attachment pdf in this.Attachments)
            {
                try
                {
                    File.WriteAllBytes($"{pdf.Label}.xml", pdf.Bytes);

                    saveResult.Add(new SaveResult()
                    {
                        FileName = $"{pdf.Label}.pdf",
                        Status = "OK"
                    });
                }
                catch (Exception ex)
                {
                    saveResult.Add(new SaveResult()
                    {
                        FileName = $"{pdf.Label}.pdf",
                        Status = ex.Message
                    });
                }
            }

            return saveResult;
        }
    }

    public class SaveResult
    {
        public string FileName { get; set; }
        public string Status { get; set; }
    }
}
