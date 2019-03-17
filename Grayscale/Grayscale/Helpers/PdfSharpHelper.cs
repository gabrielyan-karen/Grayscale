using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.Helpers
{
    public class PdfSharpHelper
    {
        public PdfSharpHelper()
        {

        }

        public static List<Bitmap> ToImages(PdfPages Pages, bool Grayscale = false, bool BlackAndWhite = false, int Zoom = 1)
        {
            List<Bitmap> pages = new List<Bitmap>();

            foreach (PdfPage page in Pages)
            {
                XGraphics xGraphics = XGraphics.FromPdfPage(page);
                Bitmap bitmap = new Bitmap((int)page.Width.Point * Zoom, (int)page.Height.Point * Zoom, xGraphics.Graphics);
                BlackAndWhite = (Grayscale) ? false : BlackAndWhite;

                if (Grayscale)
                {
                    pages.Add(ImageHelper.Grayscale(bitmap));
                }
                else if (BlackAndWhite)
                {
                    pages.Add(ImageHelper.BlackAndWhite(bitmap));
                }
                else
                {
                    pages.Add(bitmap);
                }
            }

            return pages;
        }

        public static PdfPages Pages(MemoryStream File)
        {
            PdfDocument PDFNewDoc = new PdfDocument();

            PdfDocument PDFDoc = PdfReader.Open(File, PdfDocumentOpenMode.Import);

            foreach (PdfPage page in PdfReader.Open(File, PdfDocumentOpenMode.Import).Pages)
            {
                PDFNewDoc.AddPage(page);
            }

            return PDFNewDoc.Pages;
        }

        public static string CreatePdf(List<Bitmap> images)
        {
            PdfDocument pdfDocument = new PdfDocument();
            MemoryStream stream = new MemoryStream();
            List<MemoryStream> Result = new List<MemoryStream>();

            foreach (Bitmap image in images)
            {
                pdfDocument.Pages.Add(new PdfPage());

                XGraphics xGraphics = XGraphics.FromPdfPage(pdfDocument.Pages[pdfDocument.Pages.Count - 1]);
                XImage xImage = XImage.FromGdiPlusImage(image);
                xGraphics.DrawImage(xImage, 0, 0);

            }

            pdfDocument.Save(stream);
            pdfDocument.Close();

            return Convert.ToBase64String(
                stream.ToArray()
                );
        }

        public static void SavePdf(MemoryStream File)
        {
            PdfReader.Open(File, PdfDocumentOpenMode.Modify).Save("pdf.pdf");
        }
    }
}
