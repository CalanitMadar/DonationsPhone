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
    public  class BLCompaniesActions
    {

        PaymentExcelService paymentExcelService;
        CompanyExcelService companyExcelService;
        PaymentService paymentService;

        public static ServiceCollection ServiceBuilder()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<Dbcontext>();
            serviceCollection.AddTransient<PaymentService>();
            serviceCollection.AddTransient<PaymentExcelService>();
            serviceCollection.AddTransient<CompanyExcelService>();

            return serviceCollection;

        }
        public BLCompaniesActions()
        {
            var serviceCollection = ServiceBuilder();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            this.paymentExcelService = serviceProvider.GetService<PaymentExcelService>();
            this.companyExcelService = serviceProvider.GetService<CompanyExcelService>();
            this.paymentService = serviceProvider.GetService<PaymentService>();
        }


        public void SaveAndSendDaylyPayments()
        {
            companyExcelService.GetCompanies().ForEach(company =>
            {
                var simplepayments = companyExcelService.GetCompanyTodaySimplePayments(company);
                List<Payment> payments = new();
                simplepayments.ForEach(p => payments.Add(new Payment()
                {
                    Date = p.Date,
                    HebrewDate = new HebrewDate(p.Date ?? DateTime.Today).ToString(),
                    SumOfMoney = p.Schum,
                    IsGroup = p.IsGroup,
                    Time = p.Time,

                }));
                companyExcelService.SaveDaylyPaymentsToExcel(company, payments);
            });
           


        }
        public void SaveAndSendAllPayments()
        {
            companyExcelService.GetCompanies().ForEach(company =>
            {
                var simplepayments = companyExcelService.GetCompanySimplePayments(company);
                List<Payment> payments = new();
                simplepayments.ForEach(p => payments.Add(new Payment()
                {
                    Date = p.Date,
                    HebrewDate = new HebrewDate(p.Date ?? DateTime.Today).ToString(),
                    SumOfMoney = p.Schum,
                    IsGroup = p.IsGroup,
                    Time = p.Time,

                }));
                companyExcelService.SaveAllPaymentsToExcel(company, payments);

            });
        }

    }
}
