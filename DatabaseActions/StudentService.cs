using Microsoft.EntityFrameworkCore;
using Models;

namespace DatabaseActions
{
    public class StudentService
    {
        Dbcontext db = new Dbcontext();
        public StudentService(Dbcontext db)
        {
            this.db = db;
        }
        public void SaveDataToDatabase(List<ExcelStudent> list)
        {
            

            list.Where(s => db.Students.Select(st=>st.Id ).Contains(s.Id)== false).ToList().
                ForEach(s => db.Students.Add(new Student()
            { Id = s.Id, FirstName = s.FirstName, LastName = s.LastName,PhoneNumber =s.PhoneNumber , Age = s.Age.ToString(), Group = s.Group }));
            db.SaveChanges();
        }
        public Student?  GetStudentById(string id)=>
             db.Students.FirstOrDefault(s=>s.Id==id);
        public Student? GetStudentByPhoneNumber(string phone) =>
            db.Students.FirstOrDefault(s => s.PhoneNumber==phone);
        public void AddStudentPayment(Student student, Payment payment)
        {
            // payment.StudentId = student.Id;
            db.Payments.Add(payment);
            db.SaveChanges();
        }

        public Student? GetStudent(Func<Student, bool> func) =>
             db.Students.FirstOrDefault(func);

    }
}