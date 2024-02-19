using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HebrewDate
    {
        public string Year { get; set; } = "תשפד";
        public string Month { get; set; }   
        public string Day { get; set; }


        public string[] MONTH = { "תשרי", "חשון", "כסלו", "טבת", "שבט", "אדר", "ניסן", "אייר", "סיון", "תמוז", "אב", "אלול" };

        public string[] DAYS = {"א","ב", "ג","ד","ה","ו","ז","ח","ט","י"
                                ,"יא", "יב","יג","יד","טו","טז","יז","יח","יט","כ",
                                "כא","כב","כג","כד","כה","כו","כז","כח","כט","ל"};
        public HebrewDate()
        {
                
        }
        //public HebrewDate(int year,int month,int day)
        //{
            
        //    Month = MONTH[month];
        //    Day = DAYS[day];

        //}

        public HebrewDate(DateTime somedate)
        {
            HebrewCalendar cal = new HebrewCalendar();
            CultureInfo culture = CultureInfo.CreateSpecificCulture("he-IL");
            culture.DateTimeFormat.Calendar = cal;
            Thread.CurrentThread.CurrentCulture = culture;

            int year = cal.GetYear(DateTime.Now);
            int month = cal.GetMonth(DateTime.Now);
            int day = cal.GetDayOfMonth(DateTime.Now);

            Month = MONTH[month-1];
            Day = DAYS[day-1];
        }

        public override string ToString()
        {
            return $"{Day} {Month} {Year}";
        }
    }
}
