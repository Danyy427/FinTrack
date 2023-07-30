using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinTrack.Models
{
    public class Income
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; } // Amount of income
        public string Description { get; set; } // Description of income
        public DateTime Date { get; set; } // Date of income
        public string Category { get; set; } // Category of income

        public Income()
        {
            Id = Guid.NewGuid().ToString();
            Name = "";
            Description = "";
            Category = "";
        }
    }
}
