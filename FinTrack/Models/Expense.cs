using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinTrack.Models
{
    public class Expense
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; } // Amount of expense
        public string Description { get; set; } // Description of expense
        public DateTime Date { get; set; } // Date of expense
        public string Category { get; set; } // Category of expense

        public Expense()
        {
            Id = Guid.NewGuid().ToString();
            Name = "";
            Description = "";
            Category = "";
        }
    }
}
