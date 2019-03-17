using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Grayscale.Helpers;
using Grayscale.Models;

namespace Grayscale
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            openFileDialog.ShowDialog();

            try
            {
                DocuSignFiles docuSignFiles = new DocuSignFiles(openFileDialog.FileName);

                txtResult.Text += $"DocumentPDF Files{Environment.NewLine}";
                foreach (SaveResult result in docuSignFiles.SaveDocumentPDFs())
                {
                    txtResult.Text += $"{result.FileName} - {result.Status}{Environment.NewLine}";
                }

                txtResult.Text += $"{Environment.NewLine}Attachment Files{Environment.NewLine}";
                foreach (SaveResult result in docuSignFiles.SaveAttachments())
                {
                    txtResult.Text += $"{result.FileName} - {result.Status}{Environment.NewLine}";
                }
            }
            catch (Exception ex)
            {
                Exception exp = ex;
            }
        }

        private void btnPages_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "XML files (*.xml)|*.xml";
            openFileDialog.ShowDialog();

            try
            {
                List<ResultPdf> pdfs = new List<ResultPdf>();
                DocuSignFiles docuSignFiles = new DocuSignFiles(openFileDialog.FileName);

                txtResult.Text += $"DocumentPDF Files{Environment.NewLine}";
                foreach (DocumentPDF result in docuSignFiles.DocumentPDFs)
                {
                    PdfSharp.Pdf.PdfPages pdfPages = PdfSharpHelper.Pages(result.Stream);
                    List<Bitmap> bitmaps = PdfSharpHelper.ToImages(pdfPages);

                    pdfs.Add(new ResultPdf() {
                        Bytes = PdfSharpHelper.CreatePdf(bitmaps),
                        Name  = result.Name
                    });

                    txtResult.Text += $"{result.FileName} - OK{Environment.NewLine}";
                }
            }
            catch (Exception ex)
            {
                Exception exp = ex;
            }
        }
    }
}
