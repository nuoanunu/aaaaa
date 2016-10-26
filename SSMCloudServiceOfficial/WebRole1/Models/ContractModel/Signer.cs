using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebRole1.Models.ContractModel
{
    public class Signer
    {
        public void sign()
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;

            // Open the PDF file to be signed
            pdfReader = new PdfReader(@"C:\Users\Nguyen Nhat\Downloads\Documents\draftsla_us.pdf");
            if (pdfReader != null)
            {
                // Output stream to write the stamped PDF to
                using (FileStream outStream = new FileStream(@"C:\Users\Nguyen Nhat\Downloads\Documents\draftsla_us.pdf", FileMode.Create))
                {
                    try
                    {
                        // Stamper to stamp the PDF with a signature
                        pdfStamper = new PdfStamper(pdfReader, outStream);

                        // Load signature image
                        iTextSharp.text.Image sigImg = iTextSharp.text.Image.GetInstance(@"C:\Users\Nguyen Nhat\Downloads\Documents\donefixing.png");

                        // Scale image to fit
                        sigImg.ScaleToFit(210, 297);

                        // Set signature position on page
                        sigImg.SetAbsolutePosition(200, 200);

                        // Add signatures to desired page
                        PdfContentByte over = pdfStamper.GetOverContent(6);
                        over.AddImage(sigImg);
                    }
                    finally
                    {
                        // Clean up
                        if (pdfStamper != null)
                            pdfStamper.Close();

                        if (pdfReader != null)
                            pdfReader.Close();
                    }
                }
            }
        }
    }
}