using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PlantsWpf.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string itemCode = (string)value;

            return (itemCode == "yes");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool toBeInvoiced = (bool)value;

            return toBeInvoiced ? "yes" : "no";
        }
    }

    public class PdfDocumentCreator
    {
        public Document CreatePdfDocument()
        {
            Document doc = new Document(PageSize.A4);
            var output = new FileStream("MyFirstPDF.pdf", FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, output);
            doc.Open();
            
            PdfPTable table1 = new PdfPTable(2);
            table1.DefaultCell.Border = 0;
            table1.WidthPercentage = 80;


            PdfPCell cell11 = new PdfPCell();
            cell11.Colspan = 1;
            
            cell11.VerticalAlignment = Element.ALIGN_LEFT;

            PdfPCell cell12 = new PdfPCell();


            cell12.VerticalAlignment = Element.ALIGN_CENTER;


            table1.AddCell(cell11);

            table1.AddCell(cell12);


            PdfPTable table2 = new PdfPTable(3);

            //One row added

            PdfPCell cell21 = new PdfPCell();

            cell21.AddElement(new Paragraph("Photo Type"));

            PdfPCell cell22 = new PdfPCell();

            cell22.AddElement(new Paragraph("No. of Copies"));

            PdfPCell cell23 = new PdfPCell();

            cell23.AddElement(new Paragraph("Amount"));


            table2.AddCell(cell21);

            table2.AddCell(cell22);

            table2.AddCell(cell23);


            //New Row Added

            PdfPCell cell31 = new PdfPCell();

            cell31.AddElement(new Paragraph("Safe"));

            cell31.FixedHeight = 300.0f;

            PdfPCell cell32 = new PdfPCell();

            cell32.AddElement(new Paragraph("2"));

            cell32.FixedHeight = 300.0f;

            PdfPCell cell33 = new PdfPCell();

            cell33.AddElement(new Paragraph("20.00 * " + "2" + " = " + (20 * Convert.ToInt32("2")) + ".00"));

            cell33.FixedHeight = 300.0f;



            table2.AddCell(cell31);

            table2.AddCell(cell32);

            table2.AddCell(cell33);


            PdfPCell cell2A = new PdfPCell(table2);

            cell2A.Colspan = 2;

            table1.AddCell(cell2A);

            PdfPCell cell41 = new PdfPCell();

            cell41.AddElement(new Paragraph("Name : " + "ABC"));

            cell41.AddElement(new Paragraph("Advance : " + "advance"));

            cell41.VerticalAlignment = Element.ALIGN_LEFT;

            PdfPCell cell42 = new PdfPCell();

            cell42.AddElement(new Paragraph("Customer ID : " + "011"));

            cell42.AddElement(new Paragraph("Balance : " + "3993"));

            cell42.VerticalAlignment = Element.ALIGN_RIGHT;


            table1.AddCell(cell41);

            table1.AddCell(cell42);


            doc.Add(table1);

            doc.Close();

            return doc;
        }   
    }
}