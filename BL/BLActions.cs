using DatabaseActions;
using ExcelActions;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;

namespace BL
{
    public class BLActions
    {
        StudentExcelService ExcelStudent;
        StudentService DbStudent;
        PaymentService paymentService;
        CompanyExcelService companyExcelService;
        PaymentExcelService paymentExcelService;
        public static ServiceCollection ServiceBuilder()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<Dbcontext>();
            serviceCollection.AddTransient<PaymentService>();
            serviceCollection.AddTransient<StudentExcelService>();
            serviceCollection.AddTransient<PaymentExcelService>();
            serviceCollection.AddTransient<StudentService>();
            serviceCollection.AddTransient<CompanyExcelService>();
            return serviceCollection;

        }

        public BLActions()
        {
            var serviceCollection = ServiceBuilder();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            ExcelStudent = serviceProvider.GetService<StudentExcelService>();
            DbStudent = serviceProvider.GetService<StudentService>();
            paymentService = serviceProvider.GetService<PaymentService>();
            companyExcelService = serviceProvider.GetService<CompanyExcelService>();
            paymentExcelService = serviceProvider.GetService<PaymentExcelService>();
        
        }
        public void ReadStudentDataAndSave(string path)
        {

            var data = ExcelStudent.ReadExcelStudentDataFromExcel(path);

            DbStudent.SaveDataToDatabase(data);
        }

        // from database
        private Student GetStudent(string id)
        {
            Student? student = DbStudent.GetStudentByPhoneNumber(id);
            student ??= DbStudent.GetStudentById(id);
            if (student == null)
                throw new Exception("student not found");
            return student;
        }

       


        public Company GetCompanyByPhone(string phone)
        {
           return  companyExcelService.GetCompanyByPhoneNumber(phone);
        }
        public void InputPayment(PaymentFromUser p)
        {

            var student = GetStudent(p.Phone);

            Payment pay = new()
            {
                StudentId = student.Id,
                SumOfMoney = p.Schum,
                Date = DateTime.Now,
                Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                IsGroup = p.IsGroup
            };
            
            paymentService.Create(pay);

        }
        public void InputPaymentFromPhone(string company, PaymentFromUser p)
        {

          //  var student = GetStudent(company, p.Phone);

            PaymentFromPhone pay = new()
            {
                Student = p.Phone ,
                Schum = p.Schum,
                Date = DateTime.Now,
                Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                IsGroup = p.IsGroup,
             };
           
            paymentExcelService.AddPaymentFromPhoneToExcel(company,pay);

        }

        // מחזיר רשימת תרומות להיום
        public List<Payment> GetTodayPayments() =>
           paymentService.GetTodayPayments();

        // מחזיר רשימת תרומות לתלמיד
        public List<Payment> GetStudentPayments(string id) =>
            paymentService.GetStudentPayments(id);

        // סך ומספר התרומות להיום לתלמיד מסוים
        public (decimal?, int) GetSuchTodayPaymentsById(string id) =>
             paymentService.GetSuchTodayPaymentsById(id);

        // סך תרומות  ומספר תרומות לתלמיד מסוים
        public (decimal?, int) GetTotalPaymentsById(string id) =>
            paymentService.GetTotalPaymentsById(id);
        // סך ומספר תרומות כללי להיום
        public (decimal?, int) GetTodaySuchTotalPayments() =>
               paymentService.GetTodaySuchTotalPayments();
        // סך ומספר תרומות 
        public (decimal?, int) GetTotalPayments() =>
            paymentService.GetTotalPayments();
       
    }
}