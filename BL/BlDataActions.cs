using DatabaseActions;
using ExcelActions;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BlDataActions
    {
        PaymentService paymentService;

        PaymentExcelService paymentExcelService;
        StudentExcelService studentExcelService;

        public static ServiceCollection ServiceBuilder()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<Dbcontext>();
            serviceCollection.AddScoped<PaymentService>();
            serviceCollection.AddScoped<PaymentExcelService>();
            serviceCollection.AddScoped<StudentExcelService>();

            return serviceCollection;

        }
        public BlDataActions()
        {
            var serviceCollection = ServiceBuilder();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.paymentExcelService= serviceProvider.GetService<PaymentExcelService>();    
            this.paymentService= serviceProvider.GetService<PaymentService>();
            this.studentExcelService = serviceProvider.GetService<StudentExcelService>();
        }
        #region database
        public void SaveTodayPayments()
        {
            var x = paymentService.GetTodayPayments();
            paymentExcelService.SaveTodayPaymentsToExcel(x, "");
        }
        public void SaveAllPayments()
        {
            var x = paymentService.GetPaymentList();
            paymentExcelService.SaveAllPaymentsToExcel(x, "");
        }

        public void SavePaymentsByClass()
        {
            var list = paymentService.GetPaymentList();
            var groups = (from p in list
                          group p by p.Student.Age into g
                          select new { g.Key, list = g.ToList<Payment>() }).ToList();

        }
        #endregion

        private List<Payment> ConvertSimplePaymentToPayment(List<PaymentFromPhone> lst,string company)
        {
            List<Payment > result = new List<Payment>();
            var students = studentExcelService.GetStudents(company);

            lst.ForEach(p => result.Add(new Payment()
            {
                Date = p.Date,
                HebrewDate = new HebrewDate(p.Date ?? DateTime.Today).ToString(),
                SumOfMoney = p.Schum,
                IsGroup = p.IsGroup,
                Time = p.Time,
                Student = studentExcelService.GetStudent(students,p.Student)
            })); 
            return result;
        }
        public void SaveTodayPayments(string company)
        {
            var x = paymentExcelService.ReadSimplePayments(company).
                    Where(p => p.Date == DateTime.Today).ToList();
            List<Payment> payments = ConvertSimplePaymentToPayment(x,company );        
            paymentExcelService.SaveTodayPaymentsToExcel (payments,company);
        }
        
        public void SaveAllPayments(string company)
        {
            var x = paymentExcelService.ReadSimplePayments(company);
            List<Payment> payments = ConvertSimplePaymentToPayment(x, company);
        
            paymentExcelService.SaveAllPaymentsToExcel(payments,company );
        }      
        public void SavePaymentsByClass(string company)
        {
            var x = paymentExcelService.ReadSimplePayments(company);
            List<Payment> payments = ConvertSimplePaymentToPayment(x, company);
            
            var groups = (from p in payments
                          group p by p.Student?.Age into g
                          select  (g.Key, g.ToList<Payment>())).ToList();

            paymentExcelService.SavePaymentsByGroupsToExcel(groups, company);
        }
    }
}
