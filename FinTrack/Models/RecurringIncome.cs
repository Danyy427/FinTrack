using FinTrack.Common;

namespace FinTrack.Models
{
    public class RecurringIncome : Income
    {
        public RecurrenceInterval RecurrenceInterval { get; set; }
        public DateTime NextRecurrenceDate { get; set; }
        public FinTrackUser User { get; set; }

        public RecurringIncome() : base()
        {

        }
    }
}
