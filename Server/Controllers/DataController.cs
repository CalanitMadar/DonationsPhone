using BL;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        BlDataActions actions;
        BLCompaniesActions companies;
        public DataController(BlDataActions actions, BLCompaniesActions companies)
        {
            this.actions = actions;
            this.companies = companies;
        }

       
        [HttpPost]
        [Route("SaveAndSendTodayPayments")]
        public bool SaveAndSendTodayPayments()
        {
            companies.SaveAndSendDaylyPayments();
            return true;
        }
      
        [HttpPost]
        [Route("SaveAndSendAllPayments")]
        public bool SaveAndSaveAllPayments()
        {
            companies.SaveAndSendAllPayments();
            return true;
        }

        [HttpPost]
        [Route("SaveTodayPayments")]
        public bool SaveTodayPayments(string company)
        {
            actions.SaveTodayPayments(company);
            return true;
        }

             
        [HttpPost]
        [Route("SaveAllCompanyPayments")]
        public bool SaveAllPayments(string company)
        {
            actions.SaveAllPayments(company);
            return true;
        }

       
        [HttpPost]
        [Route("SavePaymentsByClass")]
        public bool SavePaymentsByClass(string company)
        {
            actions.SavePaymentsByClass(company);
            return true;
        }
    }
}
