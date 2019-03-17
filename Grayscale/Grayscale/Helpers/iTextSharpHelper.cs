using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.Helpers
{
    public class iTextSharpHelper
    {
        public static void Edit(byte[] PDF)
        {
            PdfReader reader = new PdfReader(PDF);
        }

        public static byte[] ExtractPages(byte[] PDF)
        {
            PdfReader reader = new PdfReader(PDF);
            Document sourceDocument = new Document(reader.GetPageSizeWithRotation(1));
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;
            MemoryStream target = new MemoryStream();

            pdfCopyProvider = new PdfCopy(sourceDocument, target);

            sourceDocument.Open();

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                pdfCopyProvider.AddPage(importedPage);
            }

            sourceDocument.Close();
            reader.Close();

            return target.ToArray();
        }
    }
}
