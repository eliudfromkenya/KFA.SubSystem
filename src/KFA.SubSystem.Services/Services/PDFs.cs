using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.SubSystem.Services.Services;

  internal static class PDFs
  {
      static void MergeMultiplePDFIntoSinglePDF(string outputFilePath, string[] pdfFiles)
      {
          Console.WriteLine("Merging started.....");
          PdfDocument outputPDFDocument = new();
          foreach (string pdfFile in pdfFiles)
          {
              PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);
              outputPDFDocument.Version = inputPDFDocument.Version;
              foreach (PdfPage page in inputPDFDocument.Pages)
              {
                  outputPDFDocument.AddPage(page);
              }
          }
          outputPDFDocument.Save(outputFilePath);
          Console.WriteLine("Merging Completed");
      }
  }
