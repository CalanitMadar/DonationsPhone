using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseActions
{
    public class PaymentService
    {
        Dbcontext db = new Dbcontext();
        public PaymentService(Dbcontext db)
        {
            this.db = db;
        }
        // החזר רשימת תרומות
        public List<Payment> GetPaymentList(Func<Payment, bool> func = null)
        {
            var x = db.Payments.Include(p => p.Student).ToList();
            if (func!=null)
               x=x.Where(x => func(x)).ToList();
            return x;
        }
        // מחזיר רשימת תרומות להיום
        public List<Payment> GetTodayPayments() =>
            GetPaymentList(p => p.Date == DateTime.Today);

        // מחזיר רשימת תרומות לתלמיד
        public List<Payment> GetStudentPayments(string id) =>
            GetPaymentList(p => p.StudentId ==id);

        // מחזיר רשימת תרומות לקבוצה 
        public List<Payment> GetGroupPayments(string groupid) =>
            GetPaymentList(p=> p.Student.Group == groupid);

        // מחזיר רשימת תרומות לקבוצה להיום
        public List<Payment> GetTodayGroupPayments(string groupid) =>
            GetPaymentList(p => p.Date == DateTime.Today && p.Student.Group==groupid);
        // סך ומספר התרומות להיום לתלמיד מסוים
        public (decimal?, int) GetSuchTodayPaymentsById(string id) =>
               (db.Payments.Where(p => p.Date == DateTime.Today && p.StudentId == id).Sum(p => p.SumOfMoney),
                db.Payments.Where(p => p.Date == DateTime.Today && p.StudentId == id).Count());
            
        // סך תרומות  ומספר תרומות לתלמיד מסוים
        public (decimal?,int) GetTotalPaymentsById(string id) =>
                     ( db.Payments.Where(p => p.StudentId==id).Sum(p => p.SumOfMoney),
                        db.Payments.Where(p => p.StudentId == id).Count() );

        // סך ומספר תרומות כללי להיום
        public (decimal?,int) GetTodaySuchTotalPayments() =>
                     ( db.Payments.Where(p => p.Date == DateTime.Today ).Sum(p => p.SumOfMoney),
                       db.Payments.Where(p => p.Date == DateTime.Today).Count());
        // סך ומספר תרומות 
        public( decimal?,int) GetTotalPayments() =>
            (
                     db.Payments.Sum(p => p.SumOfMoney),
             db.Payments.Count());
        // סך תרומות  ומספר תרומות קבוצה מסוים
        public (decimal?, int) GetSuchTotalPaymentsByGroup(string groupid) =>
                     (db.Payments.Where(p => p.Student.Group == groupid).Sum(p => p.SumOfMoney),
                        db.Payments.Where(p => p.Student.Group == groupid).Count());
        //   סך תרומות  ומספר תרומות קבוצה מסוים להיום
        public (decimal?, int) GetTodaySuchTotalPaymentsByGroup(string groupid) =>
                     (db.Payments.Where(p => p.Date == DateTime.Today && p.Student.Group == groupid).Sum(p => p.SumOfMoney),
                        db.Payments.Where(p => p.Date == DateTime.Today &&  p.Student.Group == groupid).Count());




        public void Create(Payment payment)
        {
            payment.HebrewDate = new HebrewDate(payment.Date?? DateTime.Today ).ToString();
            db.Payments.Add(payment);
            db.SaveChanges();
        }
    }
}
