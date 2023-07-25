using FinTrack.Data;
using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace FinTrack.Pages.Dashboard
{
    [Authorize]
    public class AnalyticsModel : PageModel
    {
        public List<Income> Incomes { get; set; }
        public List<Income> MonthlyTotalIncomes { get; set; }
        public List<Income> DailyTotalIncomes { get; set; }
        public List<Expense> Expenses { get; set; }
        public List<Expense> MonthlyTotalExpenses { get; set; }
        public List<Expense> DailyTotalExpenses { get; set; }

        public List<string> Recommendations { get; set; }

        public List<IGrouping<string, Expense>> TopSpendingExpenseCategories { get; set; }
        public List<IGrouping<string, Income>> TopIncomeCategories { get; set; }
        public string TimeSpan { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly UserManager<FinTrackUser> _userManager;

        public AnalyticsModel(ApplicationDbContext context, UserManager<FinTrackUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Incomes = new List<Income>();
            Expenses = new List<Expense>();
            Recommendations = new List<string>();
        }

        public string MakeIncomeLineChartData()
        {
            StringBuilder Data = new StringBuilder();
            if (TimeSpan == "year")
            {
                // Display from Monthy Total Incomes
                for (int i = 0; i < MonthlyTotalIncomes.Count; i++)
                {
                    Data.AppendLine($"[{MonthlyTotalIncomes[i].Date.Month}, {MonthlyTotalIncomes[i].Amount}], ");
                }
            }
            else
            {

                // Display from Daily Total Incomes
                for (int i = 0; i < DailyTotalIncomes.Count; i++)
                {
                    Data.AppendLine($"[{DailyTotalIncomes[i].Date.Day}, {DailyTotalIncomes[i].Amount}], ");
                }
            }
            return Data.ToString();
        }

        public string MakeExpenseLineChartData()
        {
            StringBuilder Data = new StringBuilder();
            if (TimeSpan == "year")
            {
                // Display from Monthy Total Expenses
                for (int i = 0; i < MonthlyTotalExpenses.Count; i++)
                {
                    Data.AppendLine($"[{MonthlyTotalExpenses[i].Date.Month}, {MonthlyTotalExpenses[i].Amount}], ");
                }
            }
            else
            {
                // Display from Daily Total Expenses
                for (int i = 0; i < DailyTotalExpenses.Count; i++)
                {
                    Data.AppendLine($"[{DailyTotalExpenses[i].Date.Day}, {DailyTotalExpenses[i].Amount}], ");
                }
            }
            return Data.ToString();
        }

        public string MakeBalanceLineChartData()
        {
            StringBuilder Data = new StringBuilder();

            if (TimeSpan == "year")
            {
                var allDates = MonthlyTotalIncomes.Select(x => x.Date)
                    .Union(MonthlyTotalExpenses.Select(x => x.Date))
                    .Distinct();

                foreach (var date in allDates)
                {
                    var income = MonthlyTotalIncomes.FirstOrDefault(x => x.Date == date);
                    var expense = MonthlyTotalExpenses.FirstOrDefault(x => x.Date == date);
                    var incomeAmount = income?.Amount ?? 0;
                    var expenseAmount = expense?.Amount ?? 0;

                    Data.AppendLine($"[{date.Month}, {incomeAmount - expenseAmount}], ");
                }
            }
            else
            {
                var allDates = DailyTotalIncomes.Select(x => x.Date)
                    .Union(DailyTotalExpenses.Select(x => x.Date))
                    .Distinct();

                foreach (var date in allDates)
                {
                    var income = DailyTotalIncomes.FirstOrDefault(x => x.Date == date);
                    var expense = DailyTotalExpenses.FirstOrDefault(x => x.Date == date);
                    var incomeAmount = income?.Amount ?? 0;
                    var expenseAmount = expense?.Amount ?? 0;

                    Data.AppendLine($"[{date.Day}, {incomeAmount - expenseAmount}], ");
                }
            }

            return Data.ToString();
        }

        // Group Incomes by category and make pie chart data
        public string MakeIncomePieChartData()
        {
            StringBuilder Data = new StringBuilder();
            Data.AppendLine($"['Category', 'Amount'], ");
            var incomes = Incomes.GroupBy(i => i.Category).Select(i => new Income { Amount = i.Sum(i => i.Amount), Category = i.Key }).OrderByDescending(x => x.Amount).ToList();
            for (int i = 0; i < incomes.Count; i++)
            {
                Data.AppendLine($"['{incomes[i].Category}', {incomes[i].Amount}], ");
            }
            return Data.ToString();
        }

        public string MakeExpensePieChartData()
        {
            StringBuilder Data = new StringBuilder();
            Data.AppendLine($"['Category', 'Amount'], ");
            var expenses = Expenses.GroupBy(e => e.Category).Select(e => new Expense { Amount = e.Sum(e => e.Amount), Category = e.Key }).OrderByDescending(x => x.Amount).ToList();
            for (int i = 0; i < expenses.Count; i++)
            {
                Data.AppendLine($"['{expenses[i].Category}', {expenses[i].Amount}], ");
            }
            return Data.ToString();
        }

        public void OnGet(string timeSpan = "year")
        {

            TimeSpan = timeSpan;
            var user = _context.Users.Include(x => x.Incomes).Include(x => x.Expenses).FirstOrDefault(x => x.Id == _userManager.GetUserId(User));

            if (user == null)
                return;

            var numWeeksWithExpense = user.Expenses
                .Select(e => System.Globalization.ISOWeek.GetWeekOfYear(e.Date.Date) + " " + e.Date.Year)
                .Distinct()
                .Count();

            var numMonthsWithExpense = user.Expenses
                .Select(e => e.Date.Month + " " + e.Date.Year)
                .Distinct()
                .Count();

            var numYearsWithExpense = user.Expenses
                .Select(e => e.Date.Year)
                .Distinct()
                .Count();

            var allTimeAverageWeeklyExpenditurePerCategory = user.Expenses
                .GroupBy(e => e.Category)
                .Select(e => new Expense
                {
                    Amount = e.Sum(e => e.Amount) / numWeeksWithExpense,
                    Category = e.Key
                })
                .OrderByDescending(x => x.Amount)
                .ToList();

            var allTimeAverageMonthlyExpenditurePerCategory = user.Expenses
                .GroupBy(e => e.Category)
                .Select(e => new Expense
                {
                    Amount = e.Sum(e => e.Amount) / numMonthsWithExpense,
                    Category = e.Key
                })
                .OrderByDescending(x => x.Amount)
                .ToList();

            var allTimeAverageYearlyExpenditurePerCategory = user.Expenses
                .GroupBy(e => e.Category)
                .Select(e => new Expense
                {
                    Amount = e.Sum(e => e.Amount) / numYearsWithExpense,
                    Category = e.Key
                })
                .OrderByDescending(x => x.Amount)
                .ToList();

            if (timeSpan == "year")
            {
                Incomes = user.Incomes.Where(i => i.Date.Year == DateTime.Now.Year).ToList();
                Expenses = user.Expenses.Where(e => e.Date.Year == DateTime.Now.Year).ToList();
                
                MonthlyTotalIncomes = user.Incomes
                    .Where(i => i.Date.Year == DateTime.Now.Year)
                    .GroupBy(i => i.Date.Month)
                    .Select(i => 
                        new Income { Amount = i.Sum(i => i.Amount), Date = new DateTime(DateTime.Now.Year, i.Key, 1) })
                    .OrderBy(x => x.Date)
                    .ToList();
                MonthlyTotalExpenses = user.Expenses.Where(e => e.Date.Year == DateTime.Now.Year).GroupBy(e => e.Date.Month).Select(e => new Expense { Amount = e.Sum(e => e.Amount), Date = new DateTime(DateTime.Now.Year, e.Key, 1) }).OrderBy(x => x.Date).ToList();
                TopSpendingExpenseCategories = user.Expenses.Where(e => e.Date.Year == DateTime.Now.Year).GroupBy(e => e.Category).OrderByDescending(e => e.Sum(e => e.Amount)).Take(3).ToList();
                TopIncomeCategories = user.Incomes.Where(i => i.Date.Year == DateTime.Now.Year).GroupBy(i => i.Category).OrderByDescending(i => i.Sum(i => i.Amount)).Take(3).ToList();

                var thisYearExpenditurePerCategory = user.Expenses.Where(e => e.Date.Year == DateTime.Now.Year).GroupBy(e => e.Category).Select(e => new Expense { Amount = e.Sum(e => e.Amount), Category = e.Key }).OrderByDescending(x => x.Amount).ToList();
            
                foreach (var category in thisYearExpenditurePerCategory)
                {
                    var averageYearlyExpenditureForCategory = allTimeAverageYearlyExpenditurePerCategory.FirstOrDefault(x => x.Category == category.Category)?.Amount ?? 0;

                    if (category.Amount > averageYearlyExpenditureForCategory)
                    {
                        Recommendations.Add($"You are spending more than the average yearly expenditure for {category.Category}. Try to reduce spending in this category.");
                    }
                }
            }
            else if(timeSpan == "this_month")
            {
                Incomes = user.Incomes.Where(i => i.Date.Month == DateTime.Now.Month).ToList();
                Expenses = user.Expenses.Where(e => e.Date.Month == DateTime.Now.Month).ToList();
                
                DailyTotalIncomes = user.Incomes.Where(i => i.Date.Month == DateTime.Now.Month && i.Date.Year == DateTime.Now.Year).GroupBy(i => i.Date.DayOfYear).Select(i => new Income { Amount = i.Sum(i => i.Amount), Date = new DateTime(DateTime.Now.Year, 1, 1).AddDays(i.Key - 1) }).OrderBy(x => x.Date).ToList();
                DailyTotalExpenses = user.Expenses.Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year).GroupBy(e => e.Date.DayOfYear).Select(e => new Expense { Amount = e.Sum(e => e.Amount), Date = new DateTime(DateTime.Now.Year, 1, 1).AddDays(e.Key - 1) }).OrderBy(x => x.Date).ToList();

                TopSpendingExpenseCategories = user.Expenses.Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year).GroupBy(e => e.Category).OrderByDescending(e => e.Sum(e => e.Amount)).Take(3).ToList();
                TopIncomeCategories = user.Incomes.Where(i => i.Date.Month == DateTime.Now.Month && i.Date.Year == DateTime.Now.Year).GroupBy(i => i.Category).OrderByDescending(i => i.Sum(i => i.Amount)).Take(3).ToList();
            
                var thisMonthExpenditurePerCategory = user.Expenses.Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year).GroupBy(e => e.Category).Select(e => new Expense { Amount = e.Sum(e => e.Amount), Category = e.Key }).OrderByDescending(x => x.Amount).ToList();
            
                foreach (var category in thisMonthExpenditurePerCategory)
                {
                    var averageMonthlyExpenditureForCategory = allTimeAverageMonthlyExpenditurePerCategory.FirstOrDefault(x => x.Category == category.Category)?.Amount ?? 0;

                    if (category.Amount > averageMonthlyExpenditureForCategory)
                    {
                        Recommendations.Add($"You are spending more than the average monthly expenditure for {category.Category}. Try to reduce spending in this category.");
                    }
                }
            }
            else if(timeSpan == "this_week")
            {
                Incomes = user.Incomes.Where(i => i.Date >= DateTime.Now.AddDays(-7)).ToList();
                Expenses = user.Expenses.Where(e => e.Date >= DateTime.Now.AddDays(-7)).ToList();
                
                DailyTotalIncomes = user.Incomes.Where(i => i.Date >= DateTime.Now.AddDays(-7) && i.Date.Year == DateTime.Now.Year).GroupBy(i => i.Date.DayOfYear).Select(i => new Income { Amount = i.Sum(i => i.Amount), Date = new DateTime(DateTime.Now.Year, 1, 1).AddDays(i.Key - 1) }).OrderBy(x => x.Date).ToList();
                DailyTotalExpenses = user.Expenses.Where(e => e.Date >= DateTime.Now.AddDays(-7) && e.Date.Year == DateTime.Now.Year).GroupBy(e => e.Date.DayOfYear).Select(e => new Expense { Amount = e.Sum(e => e.Amount), Date = new DateTime(DateTime.Now.Year, 1, 1).AddDays(e.Key - 1) }).OrderBy(x => x.Date).ToList();

                TopSpendingExpenseCategories = user.Expenses.Where(e => e.Date >= DateTime.Now.AddDays(-7) && e.Date.Year == DateTime.Now.Year).GroupBy(e => e.Category).OrderByDescending(e => e.Sum(e => e.Amount)).Take(3).ToList();
                TopIncomeCategories = user.Incomes.Where(i => i.Date >= DateTime.Now.AddDays(-7) && i.Date.Year == DateTime.Now.Year).GroupBy(i => i.Category).OrderByDescending(i => i.Sum(i => i.Amount)).Take(3).ToList();
            
                var thisWeekExpenditurePerCategory = user.Expenses.Where(e => e.Date >= DateTime.Now.AddDays(-7) && e.Date.Year == DateTime.Now.Year).GroupBy(e => e.Category).Select(e => new Expense { Amount = e.Sum(e => e.Amount), Category = e.Key }).OrderByDescending(x => x.Amount).ToList();
                
                foreach (var category in thisWeekExpenditurePerCategory)
                {
                    var averageWeeklyExpenditureForCategory = allTimeAverageWeeklyExpenditurePerCategory.FirstOrDefault(x => x.Category == category.Category)?.Amount ?? 0;

                    if (category.Amount > averageWeeklyExpenditureForCategory)
                    {
                        Recommendations.Add($"You are spending more than the average weekly expenditure for {category.Category}. Try to reduce spending in this category.");
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
