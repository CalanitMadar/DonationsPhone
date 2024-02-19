
using ExcelActions;
using Models;
using DatabaseActions;
using System.Globalization;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using System;

namespace ExcelTest
{
    public  class Program
    {
        
        public void InputPayment(ServiceProvider serviceProvider)
        {
            StudentService service = serviceProvider.GetService<StudentService>();
            PaymentService paymentService = serviceProvider.GetService<PaymentService>();

            Console.WriteLine("enter phone number");
            string phone = Console.ReadLine();

            Student? student=  service.GetStudentByPhoneNumber(phone);
            student??= service.GetStudentById(phone);
            if (student == null) { Console.WriteLine("error"); }

            Console.WriteLine("enter schum");
            int money = int.Parse(Console.ReadLine());

            DateTime paymentdate = DateTime.Now;
            bool isgroup = false;
           if ( student.Group!="")
            {
                Console.WriteLine("is group?");
                 isgroup = bool.Parse(Console.ReadLine().ToString());
            }
            Payment pay = new()
            { StudentId=student.Id ,
             SumOfMoney=money,
              Date =paymentdate ,
              IsGroup=isgroup
             };
            paymentService.Create(pay);

        }

        public static void ReadStudentDataAndSave(ServiceProvider serviceProvider)
        {
            StudentExcelService work = serviceProvider.GetService<StudentExcelService>(); 
            var data = work.ReadExcelStudentDataFromExcel("");

            StudentService service = serviceProvider.GetService<StudentService>(); 
            service.SaveDataToDatabase(data);
        }


        public void SavePayments(ServiceProvider serviceProvider)
        {
            PaymentService? paymentService = serviceProvider.GetService<PaymentService>();
            
            PaymentExcelService? excelpayments = serviceProvider.GetService <PaymentExcelService>();

            excelpayments?.SaveTodayPaymentsToExcel(paymentService.GetTodayPayments(),"");

        }
       

        public void GetStudent(ServiceProvider serviceProvider) 
        {
            Student s = serviceProvider.GetService<StudentService>().GetStudentById("31440944");
            Console.WriteLine(s.LastName);
        }
        public static  ServiceCollection ServiceBuilder()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<Program>();
            serviceCollection.AddDbContext<Dbcontext>();
            serviceCollection.AddTransient<PaymentService>();
            serviceCollection.AddTransient<StudentExcelService>();
            serviceCollection.AddTransient<PaymentExcelService>();
            serviceCollection.AddTransient<StudentService>();

            return serviceCollection;

        }

        public  void DoActions(ServiceProvider serviceProvider)
        {

            //1
            ReadStudentDataAndSave(serviceProvider);
         
            ////2
            //InputPayment(serviceProvider);

            ////3
            // GetStudent(serviceProvider);
          
            //4 
          // SavePayments(serviceProvider);

        }
        public static void Main(string[] args)
        {

            var serviceCollection = ServiceBuilder();

            var serviceProvider = serviceCollection.BuildServiceProvider();


            var mainClass = serviceProvider.GetRequiredService<Program>();
            mainClass.DoActions(serviceProvider);


            // Helpers.MailActions mail = new Helpers.MailActions();
            // mail.SendMail();

            //PaymentExcelService excel = new();
            //PaymentService payments = new();

            //excel.SaveTodayPaymentsToExcel(lst);
            Console.WriteLine("Hello, World!");
        }
    }
}