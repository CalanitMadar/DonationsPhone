using FastExcel;
using IronXL;
using IronXL.Styles;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Wordprocessing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExcelActions
{
    public class PaymentExcelService
    {
        private string GetFileName(string company)
        {

            HebrewDate d = new HebrewDate(DateTime.Now);
            string filename= $"{Constants.DAYLYPAYMENTFILENAME}\\{company}\\מגבית_{d.Month}_{d.Day}.xlsx";
            while (File.Exists(filename))
                filename = GetNextFileName(filename);
            return filename;

        }
        private string GetNextFileName(string FILENAME)
        {
            string s = FILENAME.Substring(0,FILENAME.Length - 5);
            string ver  = FILENAME.Substring(FILENAME.Length-6, 1);
            int num=1;
            int.TryParse(ver, out num);
            if (num > 0)
                s = s.Substring(0, s.Length - 1);
            num ++;
            s = $"{s}{num}.xlsx";
            return s;// FILENAME.Substring(0, FILENAME.IndexOf('.')) + '1' + ".xlsx";

        }

        public WorkBook CreateFile(string title)
        {
            WorkBook workBook = WorkBook.Create(ExcelFileFormat.XLSX);
            workBook.Metadata.Title = title;
            return workBook;
        }

        public WorkBook ExistingFile(string filename)=>
             WorkBook.Load (filename);
           
        private void AddHeadersToExcel(WorkSheet worksheet)
        {
            worksheet.SetCellValue(0, 0, "תאריך");
            worksheet.SetCellValue(0, 1, "תאריך עברי");
            worksheet.SetCellValue(0, 2, "שעה");
            worksheet.SetCellValue(0, 3, "סכום תרומה");
            worksheet.SetCellValue(0, 4, "תז תלמיד");
            worksheet.SetCellValue(0, 5, "שם תלמיד");
            worksheet.SetCellValue(0, 6, "שיעור");
            worksheet.SetCellValue(0, 7, "התרמה קבוצתית");
            worksheet.SetCellValue(0, 8, "שם קבוצה");

        }

        public int WritePaymentstoWorkSheet(List<Payment> payments, WorkSheet workSheet)
        {
            int row = 1;
            payments.ForEach(p =>
            {
                AddRow(workSheet, row, p);
                row++;
            });
            return row;
        }

        public void AddRow(WorkSheet workSheet, int row, Payment p)
        {
            workSheet.SetCellValue(row, 0, p.Date);
            workSheet.GetCellAt(row, 0).FormatString = "YYYY/mm/dd";
            workSheet.SetCellValue(row, 1, p.HebrewDate);
            workSheet.SetCellValue(row, 2, p.Time);
            workSheet.GetCellAt(row, 2).FormatString = "hh:mm:ss";
            workSheet.SetCellValue(row, 3, p.SumOfMoney);
            workSheet.GetCellAt(row, 3).FormatString = IronXL.Formatting.BuiltinFormats.Currency0;
            workSheet.SetCellValue(row, 4, p.StudentId);
            workSheet.SetCellValue(row, 5, p.Student?.LastName);
            workSheet.SetCellValue(row, 6, p.Student?.Age);
            if (p.IsGroup == true)
                workSheet.SetCellValue(row, 7, "כן");
            else
                workSheet.SetCellValue(row, 7, "לא");
            workSheet.SetCellValue(row, 8, p.Student?.Group);

        }
     
        public IStyle GetExcelHeaderStyle()
        {
            WorkBook workBook = IronXL.WorkBook.Load($@"..\\data\\הנתונים שלנו\\מגבית.xlsx");
            WorkSheet workSheet = workBook.WorkSheets.First();
            return workSheet.GetRange("a1:a2").Style;
        }
        public void TableDesign(int row, WorkSheet workSheet)
        {

            var style = workSheet.GetRange($"a1:i{row}").Style;
            workSheet.GetRange($"A2:I{row}").OrderBy(c => c.ColumnIndex == 5);
            style.ShrinkToFit = true;
            style.Font.FontScript = GetExcelHeaderStyle().Font.FontScript;
            style.TopBorder.Type = IronXL.Styles.BorderType.Medium | IronXL.Styles.BorderType.Double;
            style.BottomBorder.Type = IronXL.Styles.BorderType.Medium | IronXL.Styles.BorderType.Double;
            style.LeftBorder.Type = IronXL.Styles.BorderType.Medium | IronXL.Styles.BorderType.Double;
            style.RightBorder.Type = IronXL.Styles.BorderType.Medium | IronXL.Styles.BorderType.Double;
            workSheet.GetRange("A1:I1").Style.SetBackgroundColor(IronSoftware.Drawing.Color.Salmon);
            workSheet.GetRange("A1:I1").Style.ShrinkToFit = true;
        }
        public void AddFormula(int row, WorkSheet workSheet)
        {
            workSheet.GetRange($"A{row + 3}:I{row + 3}").Style.ShrinkToFit = true;
            workSheet.SetCellValue(row + 3, 4, "סך כל התרומות ליום זה");
            workSheet.SetCellValue(row + 3, 3, $"=SUM(D2:D{row + 1})");
            workSheet.GetCellAt(row + 3, 3).FormatString = IronXL.Formatting.BuiltinFormats.Currency0;
            workSheet.SetCellValue(row + 4, 3, $"=Max(D2:D{row + 1})");
            workSheet.SetCellValue(row + 4, 4, "התרומה הגבוהה ליום זה");

        }
        
        public void SaveTodayPaymentsToExcel(List<Payment> payments,string company)
        {
         
            string filename = GetFileName(company);          

            SavePaymentsToExcel(payments, filename);
        }
        public void SaveAllPaymentsToExcel(List<Payment> payments,string company)
        {
            HebrewDate d = new HebrewDate(DateTime.Now);
            string FILENAME =$"{Constants.ALLPAYMENTSFILENAME}\\{company}\\{d.Year}.xlsx";
            SavePaymentsToExcel(payments, FILENAME);
        }
        


        public void SavePaymentsToExcel(List<Payment> payments, string filename,string sheetName="1", bool newFile=true)
        {
            WorkBook workBook;
           
            workBook =(newFile)? 
                CreateFile("תרומות"): 
                ExistingFile(filename);

            WorkSheet workSheet = workBook.CreateWorkSheet(sheetName);

            AddHeadersToExcel(workSheet);

            int row=WritePaymentstoWorkSheet(payments, workSheet);

            TableDesign(row, workSheet);    
                      
            AddFormula(row,workSheet);  

            workBook.SaveAs(filename);          

        }


             
        public void SavePaymentsByGroupsToExcel(List<(string,List<Payment>)> groups, string company)
        {
            
            HebrewDate d = new HebrewDate(DateTime.Now);
            string FILENAME = $"{ Constants.ALLPAYMENTSFILENAME}\\{ company}\\{ d.Year}.xlsx";
            groups.ForEach(g =>
            {
                 SavePaymentsToExcel(g.Item2 , FILENAME, g.Item1, true);
            }
            );
         
        }



        //הוספת גביה שהתקבלה בטלפון לקובץ של המוסד
        public void AddSimpleRow(WorkSheet workSheet, int row, PaymentFromPhone p)
        {
            workSheet.SetCellValue(row, 0, p.Date);
            workSheet.GetCellAt(row, 0).FormatString = "YYYY/mm/dd";
           
            workSheet.SetCellValue(row, 1, p.Time);
            workSheet.GetCellAt(row, 1).FormatString = "hh:mm:ss";
            workSheet.SetCellValue(row, 2, p.Schum);
            workSheet.GetCellAt(row, 2).FormatString = IronXL.Formatting.BuiltinFormats.Currency0;

            workSheet.SetCellValue(row, 3, p.Student );
                       
            workSheet.SetCellValue(row, 4, p.IsGroup );
        
        }
        public void AddPaymentFromPhoneToExcel(string company,PaymentFromPhone payment)
        {
            string filename = $@"..\\data\\{company}\\מגבית יומית.xlsx";
            WorkBook workBook = IronXL.WorkBook.Load(filename);
            WorkSheet workSheet = workBook.WorkSheets.First();
            var rowNumber = workSheet.Rows.Count();
            AddSimpleRow(workSheet, rowNumber, payment);
            workBook.SaveAs(filename);
        }
   



        public List<PaymentFromPhone> ReadSimplePayments(string company)
        {
            string filename = $@"..\\data\\{company}\\מגבית יומית.xlsx";
            FileInfo fi = new FileInfo(filename);
            List<PaymentFromPhone> payments = new List<PaymentFromPhone>();
            //Stream reader = new (FILENAME);
            FastExcel.FastExcel fast = new FastExcel.FastExcel(fi);
            var data = fast.Read(Constants.SHEETNUMBER).Rows.ToList();
            data.RemoveAt(0);
            //  Date = Convert.ToDateTime(row.Cells.ToArray()[0].Value.ToString()),
            //Time  =TimeSpan.Parse(row.Cells.ToArray()[1].Value.ToString()),
            
            data.ForEach(row => payments.Add(new PaymentFromPhone()
            {
                
                
                Schum  = int.Parse(row.Cells.ToArray()[2].Value.ToString()),
                Student = row.Cells.ToArray()[3].Value.ToString(),
               // IsGroup = Convert.ToBoolean(row.Cells.ToArray()[4].Value.ToString())

            }));
            return payments;
        }
    }
}
