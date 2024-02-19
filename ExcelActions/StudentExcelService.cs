using Models;
using System.Linq;

namespace ExcelActions
{
    public class StudentExcelService
    {
        public List<ExcelStudent> ReadExcelStudentDataFromExcel(string path)
        {
            FileInfo fi = new FileInfo(path + Constants.STUDENTFILENAME);
            List<ExcelStudent> students = new List<ExcelStudent>();
            //Stream reader = new (FILENAME);
            FastExcel.FastExcel fast = new FastExcel.FastExcel(fi);
            var data = fast.Read(Constants.SHEETNUMBER).Rows.ToList();
            data.RemoveAt(0);
            data.ForEach(x =>students.Add
                        ( new ExcelStudent() 
                        {  Id=x.Cells.ToArray()[0].Value.ToString(),
                           FirstName = x.Cells.ToArray()[1].Value.ToString(),
                            LastName= x.Cells.ToArray()[2].Value.ToString(),
                            PhoneNumber = x.Cells.ToArray()[3].Value.ToString(),
                           Age= x.Cells.ToArray()[4].Value.ToString()[0],
                           Group =null 
                        }));
            return students;
        }

        public List<Student> ReadStudentsDataFromExcel(string path)
        {
            FileInfo fi = new FileInfo(path);
            List<Student> students = new List<Student>();
          
            FastExcel.FastExcel fast = new FastExcel.FastExcel(fi);
            var data = fast.Read(Constants.SHEETNUMBER).Rows.ToList();
            data.RemoveAt(0);
            data.ForEach(x => students.Add
                        (new Student()
                        {
                            Id = x.Cells.ToArray()[0].Value.ToString(),
                            FirstName = x.Cells.ToArray()[1].Value.ToString(),
                            LastName = x.Cells.ToArray()[2].Value.ToString(),
                            PhoneNumber = x.Cells.ToArray()[3].Value.ToString(),
                            
                            Group = null
                        }));
            return students;
        }
        public List<Student> GetStudents(string company)
        {
            string filename = $"..\\data\\{company}\\תלמידים.xlsx";
            return ReadStudentsDataFromExcel(filename);
        }

        public Student GetStudent(List<Student> students, string studentId)
        {
            Student? student = students .Where(s => s.Id == studentId).FirstOrDefault();
            student ??= students.Where(s => s.PhoneNumber == studentId).FirstOrDefault();
            if (student == null)
                throw new Exception("student not found");
            return student;
        }
        public Student GetStudent(string company, string studentId)
        {
           var data=GetStudents(company);
           return GetStudent(data, studentId);  
        }
      
    }
}