using FinTrack.Data;
using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<FinTrackUser> _userManager;
        private readonly ApplicationDbContext _context;

        public List<Income> Incomes { get; set; }
        public List<Expense> Expenses { get; set; }

        public decimal TotalBalance { get; set; } = 0;
        public decimal MonthlyExpense { get; set; } = 0;
        public decimal MonthlyIncome { get; set; } = 0;
        public decimal MonthylBalance { get; set; } = 0;
        public string TopExpenseCategory { get; set; } = "";
        public decimal TopExpenseCategoryAmount { get; set; } = 0;
        public string TopIncomeCategory { get; set; } = "";
        public decimal TopIncomeCategoryAmount { get; set; } = 0;


        public IndexModel(UserManager<FinTrackUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            Incomes = new List<Income>();
            Expenses = new List<Expense>();
        }

        public void OnGet()
        {
            var user = _context.Users
                .Include(x => x.Incomes)
                .Include(x => x.Expenses)
                .FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user == null)
                return;

            Incomes = user.Incomes.OrderBy(x => x.Date).Take(5).ToList();
            Expenses = user.Expenses.OrderBy(x => x.Date).Take(5).ToList();

            TotalBalance = user.Incomes.Sum(x => x.Amount) - user.Expenses.Sum(x => x.Amount);
            MonthlyExpense = user.Expenses.Where(x => x.Date.Month == DateTime.Now.Month).Sum(x => x.Amount);
            MonthlyIncome = user.Incomes.Where(x => x.Date.Month == DateTime.Now.Month).Sum(x => x.Amount);
            MonthylBalance = MonthlyIncome - MonthlyExpense;
            TopExpenseCategory = user.Expenses
                .Where(x => x.Date.Month == DateTime.Now.Month)
                .GroupBy(x => x.Category)
                .OrderByDescending(x => x.Sum(y => y.Amount)).Select(x => x.Key).FirstOrDefault() ?? "";
            TopExpenseCategoryAmount = user.Expenses
                .Where(x => x.Date.Month == DateTime.Now.Month)
                .GroupBy(x => x.Category)
                .OrderByDescending(x => x.Sum(y => y.Amount)).Select(x => x.Sum(y => y.Amount)).FirstOrDefault();
            TopIncomeCategory = user.Incomes
                .Where(x => x.Date.Month == DateTime.Now.Month)
                .GroupBy(x => x.Category)
                .OrderByDescending(x => x.Sum(y => y.Amount)).Select(x => x.Key).FirstOrDefault() ?? "";
            TopIncomeCategoryAmount = user.Incomes
                .Where(x => x.Date.Month == DateTime.Now.Month)
                .GroupBy(x => x.Category)
                .OrderByDescending(x => x.Sum(y => y.Amount)).Select(x => x.Sum(y => y.Amount)).FirstOrDefault();
        }
    }
}
