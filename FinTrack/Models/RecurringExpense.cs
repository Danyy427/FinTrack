using FinTrack.Common;

namespace FinTrack.Models
{
    public class RecurringExpense : Expense
    {
        public RecurrenceInterval RecurrenceInterval { get; set; }
        public DateTime NextRecurrenceDate { get; set; }
        public FinTrackUser User { get; set; }

        public RecurringExpense() : base()
        {
            
        }
    }
}
