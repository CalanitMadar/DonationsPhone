using BL;

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        BLActions bl;
        public StudentsController(BLActions bl)
        {
            this.bl = bl; 
        }
        [HttpGet]
        [Route ("GetSuchForDayByStudentId")]
        public (decimal?,int) GetSuchForDayByStudentId(string id)=>
                bl.GetSuchTodayPaymentsById (id);
        
        [HttpGet]
        [Route("GetSuchByStudentId")]
        public (decimal?,int) GetSuchByStudentId(string id)=>
           bl.GetTotalPaymentsById(id);
        
        [HttpPost]
        public void InitStudents()
        {
            var x = AppDomain.CurrentDomain.BaseDirectory;
            
            bl.ReadStudentDataAndSave(x);
        }

    }
}
