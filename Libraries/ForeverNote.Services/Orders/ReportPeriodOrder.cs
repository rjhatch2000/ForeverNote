using System;

namespace ForeverNote.Services.Orders
{
    public partial class ReportPeriodOrder
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }
}
