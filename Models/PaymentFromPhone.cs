using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PaymentFromPhone
    {
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }

        public string Student { get; set; }
        public decimal Schum { get; set; }
        public bool IsGroup { get; set; }
    }
}
