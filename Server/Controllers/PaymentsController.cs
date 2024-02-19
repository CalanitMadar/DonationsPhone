using BL;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        BLActions bl;
        public PaymentsController(BLActions bl)
        {
            this.bl = bl;
        }
        [HttpPost]
        // הוספת תרומה שהתקבלה בטלפון
        public void AddPaymentFromPhone(string phoneNumber,PaymentFromUser pay)
        {
           Company c=  bl.GetCompanyByPhone(phoneNumber);
           bl.InputPaymentFromPhone(c.Name , pay);
        }


        [HttpGet ]
        [Route ("GetTotalPayments")]
        public (decimal?,int) GetSuchAllPayments()=>
                     bl.GetTotalPayments();
        
        [HttpGet]
        [Route("GetTodayTotalPayments")]
        public (decimal?,int) GetTodaySuchAllPayments()=>
                    bl.GetTodaySuchTotalPayments();
        
    }
}
