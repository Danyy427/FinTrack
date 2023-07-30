using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Models
{
    public class FinTrackUser : IdentityUser
    {
        public string FullName { get; set; }

        public int Age { get; set; }

        public string Sex { get; set; }

        public string Country { get; set; }

        public List<Income> Incomes { get; set; }

        public List<Expense> Expenses { get; set; }

        public List<RecurringIncome> RecurringIncomes { get; set; }

        public List<RecurringExpense> RecurringExpenses { get; set; }

        public FinTrackUser() : base()
        {
            FullName = "";
            Age = 0;
            Sex = "";
            Country = "";
            Incomes = new List<Income>();
            Expenses = new List<Expense>();
            RecurringIncomes = new List<RecurringIncome>();
            RecurringExpenses = new List<RecurringExpense>();
        }
    }
}
