using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelActions
{
    public class CompanyExcelService
    {
        PaymentExcelService paymentExcelService;
        public CompanyExcelService(PaymentExcelService paymentExcel )
        {
                paymentExcelService=paymentExcel;
        }
        public List<Company> GetCompanies()
        {
            FileInfo fi = new FileInfo(Constants.COMPANIESDATA);
            List<Company> Companies = new List<Company>();

            FastExcel.FastExcel fast = new FastExcel.FastExcel(fi);
            var data = fast.Read(Constants.COMPANIESSHEETS).Rows.ToList();
            data.RemoveAt(0);
            data.ForEach(x => Companies.Add(
                new Company()
                {
                    Id = int.Parse(x.Cells.ToArray()[0].Value.ToString()),
                    Name = x.Cells.ToArray()[1].Value.ToString(),
                    PhoneNumber = x.Cells.ToArray()[3].Value.ToString()
                }
                )
            );
            return Companies;
        }
        public Company GetCompanyByPhoneNumber(string phoneNumber)=>
                GetCompanies().Where(c=>c.PhoneNumber==phoneNumber).FirstOrDefault()??throw new Exception("לא נמצא מוסד");
        
        public List<PaymentFromPhone> GetCompanySimplePayments(Company c)=>
               paymentExcelService.ReadSimplePayments(c.Name);

        public List<PaymentFromPhone> GetCompanyTodaySimplePayments(Company c) =>
               paymentExcelService.ReadSimplePayments(c.Name).Where(p => p.Date == DateTime.Today).ToList();
        public void SaveDaylyPaymentsToExcel(Company c,List<Payment> payments)
        {
            paymentExcelService.SaveTodayPaymentsToExcel(payments, c.Name);
        }
        public void SaveAllPaymentsToExcel(Company c, List<Payment> payments)
        {
            paymentExcelService.SaveAllPaymentsToExcel(payments, c.Name);
        }
    }
}
