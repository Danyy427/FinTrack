using FinTrack.Data;
using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string UserSearchEmail { get; set; }

        [BindProperty]
        public string IncomeSearchEmail { get; set; }

        [BindProperty]
        public string ExpenseSearchEmail { get; set; }

        public FinTrackUser UserSearchResult { get; set; }
        public List<Income> IncomeSearch { get; set; }
        public List<Expense> ExpenseSearch { get; set; }
        public List<RecurringIncome> RecurringIncomeSearch { get; set; }
        public List<RecurringExpense> RecurringExpenseSearch { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly UserManager<FinTrackUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<FinTrackUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSearchUserByEmail()
        {
            UserSearchResult = _context.Users
                .Include(x => x.RecurringIncomes)
                .Include(x => x.RecurringExpenses)
                .Include(x => x.Incomes)
                .Include(x => x.Expenses)
                .FirstOrDefault(x => x.Id == _userManager.FindByEmailAsync(UserSearchEmail).Result.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostSearchIncomeByEmail()
        {
            var user = _context.Users
                .Include(x => x.RecurringIncomes)
                .Include(x => x.Incomes)
                .FirstOrDefault(x => x.Id == _userManager.FindByEmailAsync(IncomeSearchEmail).Result.Id);

            if (user == null)
                return Page();

            IncomeSearch = user.Incomes.OrderBy(x => x.Date).ToList();
            RecurringIncomeSearch = user.RecurringIncomes.OrderBy(x => x.Date).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostSearchExpenseByEmail()
        {
            var user = _context.Users
                .Include(x => x.RecurringExpenses)
                .Include(x => x.Expenses)
                .FirstOrDefault(x => x.Id == _userManager.FindByEmailAsync(ExpenseSearchEmail).Result.Id);

            if (user == null)
                return Page();

            ExpenseSearch = user.Expenses.OrderBy(x => x.Date).ToList();
            RecurringExpenseSearch = user.RecurringExpenses.OrderBy(x => x.Date).ToList();

            return Page();
        }
    }
}
