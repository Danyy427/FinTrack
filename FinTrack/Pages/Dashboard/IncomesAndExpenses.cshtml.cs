using FinTrack.Data;
using FinTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using FinTrack.Common;
using Microsoft.AspNetCore.Authorization;

namespace FinTrack.Pages.Dashboard
{
    [Authorize]
    public class IncomesAndExpensesModel : PageModel
    {
        private readonly UserManager<FinTrackUser> _userManager;
        private readonly ApplicationDbContext _context;

        public List<Income> Incomes { get; set; }
        public List<Expense> Expenses { get; set; }   
        public List<RecurringIncome> RecurringIncomes { get; set; }
        public List<RecurringExpense> RecurringExpenses { get; set; }

        [BindProperty]
        public IncomeInputModel IncomeInput { get; set; }

        [BindProperty]
        public ExpenseInputModel ExpenseInput { get; set; }

        [BindProperty]
        public RecurringIncomeInputModel RecurringIncomeInput { get; set; }

        [BindProperty]
        public RecurringExpenseInputModel RecurringExpenseInput { get; set; }

        [BindProperty]
        public string DeleteId { get; set; }

        public class IncomeInputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string Category { get; set; }

            [Required]
            public decimal Amount { get; set; }

            [Required]
            public DateTime Date { get; set; } = DateTime.Now;
        }

        public class ExpenseInputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string Category { get; set; }

            [Required]
            public decimal Amount { get; set; }

            [Required]
            public DateTime Date { get; set; } = DateTime.Now;
        }

        public class RecurringIncomeInputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string Category { get; set; }

            [Required]
            public decimal Amount { get; set; }

            [Required]
            public DateTime Date { get; set; } = DateTime.Now;

            [Required]
            public RecurrenceInterval Interval { get; set; }
        }

        public class RecurringExpenseInputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string Category { get; set; }

            [Required]
            public decimal Amount { get; set; }

            [Required]
            public DateTime Date { get; set; } = DateTime.Now;

            [Required]
            public RecurrenceInterval Interval { get; set; }
        }


        public IncomesAndExpensesModel(UserManager<FinTrackUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            Incomes = new List<Income>();
            Expenses = new List<Expense>();
            RecurringIncomes = new List<RecurringIncome>();
            RecurringExpenses = new List<RecurringExpense>();
        }

        public void OnGet()
        {
            var user = _context.Users.Include(x => x.RecurringIncomes).Include(x => x.RecurringExpenses).Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user == null)
                return;

            Incomes = user.Incomes.OrderBy(x => x.Date).ToList();
            Expenses = user.Expenses.OrderBy(x => x.Date).ToList();
            RecurringIncomes = user.RecurringIncomes.OrderBy(x => x.Date).ToList();
            RecurringExpenses = user.RecurringExpenses.OrderBy(x => x.Date).ToList();
        }

        public async Task<IActionResult> OnPostCreateIncome()
        {
            var user = _context.Users.Include(x => x.RecurringIncomes).Include(x => x.RecurringExpenses).Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user != null)
            {
                Income income = new Income
                {
                    Name = IncomeInput.Name,
                    Description = IncomeInput.Description,
                    Category = IncomeInput.Category,
                    Amount = IncomeInput.Amount,
                    Date = IncomeInput.Date
                };

                _context.CreateIncome(income, user);
            }

            Incomes = user?.Incomes.OrderBy(x => x.Date).ToList() ?? new List<Income>();
            Expenses = user?.Expenses.OrderBy(x => x.Date).ToList() ?? new List<Expense>();
            RecurringIncomes = user?.RecurringIncomes.OrderBy(x => x.Date).ToList() ?? new List<RecurringIncome>();
            RecurringExpenses = user?.RecurringExpenses.OrderBy(x => x.Date).ToList() ?? new List<RecurringExpense>();

            return Page();
        }

        public async Task<IActionResult> OnPostCreateExpense()
        {
            var user = _context.Users.Include(x => x.RecurringIncomes).Include(x => x.RecurringExpenses).Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user != null)
            {
                Expense expense = new Expense
                {
                    Name = ExpenseInput.Name,
                    Description = ExpenseInput.Description,
                    Category = ExpenseInput.Category,
                    Amount = ExpenseInput.Amount,
                    Date = ExpenseInput.Date
                };

                _context.CreateExpense(expense, user);
            }

            Incomes = user?.Incomes.OrderBy(x => x.Date).ToList() ?? new List<Income>();
            Expenses = user?.Expenses.OrderBy(x => x.Date).ToList() ?? new List<Expense>();
            RecurringIncomes = user?.RecurringIncomes.OrderBy(x => x.Date).ToList() ?? new List<RecurringIncome>();
            RecurringExpenses = user?.RecurringExpenses.OrderBy(x => x.Date).ToList() ?? new List<RecurringExpense>();


            return Page();
        }

        public async Task<IActionResult> OnPostDeleteIncome()
        {
            var user = _context.Users.Include(x => x.RecurringIncomes).Include(x => x.RecurringExpenses).Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user != null)
            {
                var income = user.Incomes.FirstOrDefault(x => x.Id == DeleteId);

                _context.DeleteIncome(income, user);
            }

            Incomes = user?.Incomes.OrderBy(x => x.Date).ToList() ?? new List<Income>();
            Expenses = user?.Expenses.OrderBy(x => x.Date).ToList() ?? new List<Expense>();
            RecurringIncomes = user?.RecurringIncomes.OrderBy(x => x.Date).ToList() ?? new List<RecurringIncome>();
            RecurringExpenses = user?.RecurringExpenses.OrderBy(x => x.Date).ToList() ?? new List<RecurringExpense>();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteExpense()
        {
            var user = _context.Users.Include(x => x.RecurringIncomes).Include(x => x.RecurringExpenses).Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user != null)
            {
                var expense = user.Expenses.FirstOrDefault(x => x.Id == DeleteId);
                
                _context.DeleteExpense(expense, user);
            }

            Incomes = user?.Incomes.OrderBy(x => x.Date).ToList() ?? new List<Income>();
            Expenses = user?.Expenses.OrderBy(x => x.Date).ToList() ?? new List<Expense>();
            RecurringIncomes = user?.RecurringIncomes.OrderBy(x => x.Date).ToList() ?? new List<RecurringIncome>();
            RecurringExpenses = user?.RecurringExpenses.OrderBy(x => x.Date).ToList() ?? new List<RecurringExpense>();

            return Page();
        }

        public async Task<IActionResult> OnPostCreateRecurringIncome()
        {
            var user = _context.Users.Include(x => x.RecurringIncomes).Include(x => x.RecurringExpenses).Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user != null)
            {
                RecurringIncome income = new RecurringIncome
                {
                    Name = RecurringIncomeInput.Name,
                    Description = RecurringIncomeInput.Description,
                    Category = RecurringIncomeInput.Category,
                    Amount = RecurringIncomeInput.Amount,
                    Date = RecurringIncomeInput.Date,
                    RecurrenceInterval = RecurringIncomeInput.Interval,
                    NextRecurrenceDate = RecurringIncomeInput.Date,
                    User = user
                };

                _context.CreateRecurringIncome(income, user);
            }

            Incomes = user?.Incomes.OrderBy(x => x.Date).ToList() ?? new List<Income>();
            Expenses = user?.Expenses.OrderBy(x => x.Date).ToList() ?? new List<Expense>();
            RecurringIncomes = user?.RecurringIncomes.OrderBy(x => x.Date).ToList() ?? new List<RecurringIncome>();
            RecurringExpenses = user?.RecurringExpenses.OrderBy(x => x.Date).ToList() ?? new List<RecurringExpense>();

            return Page();
        }

        public async Task<IActionResult> OnPostCreateRecurringExpense()
        {
            var user = _context.Users.Include(x => x.RecurringIncomes).Include(x => x.RecurringExpenses).Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user != null)
            {
                RecurringExpense expense = new RecurringExpense
                {
                    Name = RecurringExpenseInput.Name,
                    Description = RecurringExpenseInput.Description,
                    Category = RecurringExpenseInput.Category,
                    Amount = RecurringExpenseInput.Amount,
                    Date = RecurringExpenseInput.Date,
                    RecurrenceInterval = RecurringExpenseInput.Interval,
                    NextRecurrenceDate = RecurringExpenseInput.Date,
                    User = user
                };

                _context.CreateRecurringExpense(expense, user);
            }

            Incomes = user?.Incomes.OrderBy(x => x.Date).ToList() ?? new List<Income>();
            Expenses = user?.Expenses.OrderBy(x => x.Date).ToList() ?? new List<Expense>();
            RecurringIncomes = user?.RecurringIncomes.OrderBy(x => x.Date).ToList() ?? new List<RecurringIncome>();
            RecurringExpenses = user?.RecurringExpenses.OrderBy(x => x.Date).ToList() ?? new List<RecurringExpense>();


            return Page();
        }

        public async Task<IActionResult> OnPostDeleteRecurringIncome()
        {
            var user = _context.Users.Include(x => x.RecurringIncomes).Include(x => x.RecurringExpenses).Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user != null)
            {
                var income = user.RecurringIncomes.FirstOrDefault(x => x.Id == DeleteId);

                _context.DeleteRecurringIncome(income, user);
            }

            Incomes = user?.Incomes.OrderBy(x => x.Date).ToList() ?? new List<Income>();
            Expenses = user?.Expenses.OrderBy(x => x.Date).ToList() ?? new List<Expense>();
            RecurringIncomes = user?.RecurringIncomes.OrderBy(x => x.Date).ToList() ?? new List<RecurringIncome>();
            RecurringExpenses = user?.RecurringExpenses.OrderBy(x => x.Date).ToList() ?? new List<RecurringExpense>();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteRecurringExpense()
        {
            var user = _context.Users.Include(x => x.RecurringIncomes).Include(x => x.RecurringExpenses).Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user != null)
            {
                var expense = user.RecurringExpenses.FirstOrDefault(x => x.Id == DeleteId);

                _context.DeleteRecurringExpense(expense, user);
            }

            Incomes = user?.Incomes.OrderBy(x => x.Date).ToList() ?? new List<Income>();
            Expenses = user?.Expenses.OrderBy(x => x.Date).ToList() ?? new List<Expense>();
            RecurringIncomes = user?.RecurringIncomes.OrderBy(x => x.Date).ToList() ?? new List<RecurringIncome>();
            RecurringExpenses = user?.RecurringExpenses.OrderBy(x => x.Date).ToList() ?? new List<RecurringExpense>();

            return Page();
        }
    }
}
