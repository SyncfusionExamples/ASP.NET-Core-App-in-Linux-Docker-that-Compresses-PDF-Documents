
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PDFCompression.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace PDFCompression.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult CompressPDF()
        {
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "Data", "PDF_succinctly.pdf");

            FileStream inputDocument = new FileStream(path, FileMode.Open);

            //Load an existing PDF document
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(inputDocument);

            //Create a new compression option.
            PdfCompressionOptions options = new PdfCompressionOptions();

            //Enable the compress image.
            options.CompressImages = true;

            //Set the image quality.
            options.ImageQuality = 30;

            //Optimize the font in the PDF document
            options.OptimizeFont = true;

            //Optimize page contents 
            options.OptimizePageContents = true;

            //Remove metadata from the PDF document
            options.RemoveMetadata = true;

            //Flatted form fields in the PDF document
            if (loadedDocument.Form != null)
                loadedDocument.Form.Flatten = true;

            //Flatten all the annotation in PDF document
            foreach (PdfPageBase page in loadedDocument.Pages)
            {
                if (page.Annotations != null)
                    page.Annotations.Flatten = true;
            }

            //Assign the compression option and compress the PDF document
            loadedDocument.Compress(options);

            //Save the PDF document.
            MemoryStream outputDocument = new MemoryStream();

            //Save the PDF document
            loadedDocument.Save(outputDocument);
            outputDocument.Position = 0;

            //Close the document
            loadedDocument.Close(true);


            //Download the PDF document in the browser.
            FileStreamResult fileStreamResult = new FileStreamResult(outputDocument, "application/pdf");
            fileStreamResult.FileDownloadName = "Compressed_PDF_document.pdf";

            return fileStreamResult;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
