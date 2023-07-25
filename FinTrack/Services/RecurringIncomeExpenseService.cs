using FinTrack.Data;
using FinTrack.Models;
using Microsoft.EntityFrameworkCore;
using FinTrack.Common;

namespace FinTrack.Services
{
    public class RecurringIncomeExpenseService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RecurringIncomeExpenseService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Here you can call your method to add recurring incomes and expenses.
                await AddRecurringIncomesAndExpenses(dbContext);
            }
        }

        public async Task AddRecurringIncomesAndExpenses(ApplicationDbContext _dbContext)
        {
            var now = DateTime.Now;
            var recurringIncomesDue = _dbContext.RecurringIncomes.Include(x => x.User)
                .Where(r => r.NextRecurrenceDate <= now).ToList();
            var recurringExpensesDue = _dbContext.RecurringExpenses.Include(x => x.User)
                .Where(r => r.NextRecurrenceDate <= now).ToList();

            if (recurringIncomesDue.Count + recurringExpensesDue.Count == 0)
            {
                return;
            }

            foreach (var recurringIncome in recurringIncomesDue)
            {

                var income = new Income
                {
                    Name = recurringIncome.Name,
                    Description = recurringIncome.Description,
                    Amount = recurringIncome.Amount,
                    Category = recurringIncome.Category,
                    Date = recurringIncome.NextRecurrenceDate,
                };

                if(recurringIncome.User == null) continue;
                var user = _dbContext.Users
                    .Include(x => x.Incomes)
                    .Include(x => x.RecurringIncomes)
                    .FirstOrDefault(x => x.Id == recurringIncome.User.Id);
                if (user == null) continue;
                _dbContext.CreateIncome(income, user);

                switch (recurringIncome.RecurrenceInterval)
                {
                    case RecurrenceInterval.Daily:
                        recurringIncome.NextRecurrenceDate = recurringIncome.NextRecurrenceDate.AddDays(1);
                        break;
                    case RecurrenceInterval.Weekly:
                        recurringIncome.NextRecurrenceDate = recurringIncome.NextRecurrenceDate.AddDays(7);
                        break;
                    case RecurrenceInterval.Monthly:
                        recurringIncome.NextRecurrenceDate = recurringIncome.NextRecurrenceDate.AddMonths(1);
                        break;
                    case RecurrenceInterval.Yearly:
                        recurringIncome.NextRecurrenceDate = recurringIncome.NextRecurrenceDate.AddYears(1);
                        break;
                }
            }

            foreach (var recurringExpense in recurringExpensesDue)
            {
                var expense = new Expense
                {
                    Name = recurringExpense.Name,
                    Description = recurringExpense.Description,
                    Amount = recurringExpense.Amount,
                    Category = recurringExpense.Category,
                    Date = recurringExpense.NextRecurrenceDate,
                };

                if (recurringExpense.User == null) continue;
                var user = _dbContext.Users
                    .Include(x => x.Expenses)
                    .Include(x => x.RecurringExpenses)
                    .FirstOrDefault(x => x.Id == recurringExpense.User.Id);
                if (user == null) continue;
                _dbContext.CreateExpense(expense, user);

                switch (recurringExpense.RecurrenceInterval)
                {
                    case RecurrenceInterval.Daily:
                        recurringExpense.NextRecurrenceDate = recurringExpense.NextRecurrenceDate.AddDays(1);
                        break;
                    case RecurrenceInterval.Weekly:
                        recurringExpense.NextRecurrenceDate = recurringExpense.NextRecurrenceDate.AddDays(7);
                        break;
                    case RecurrenceInterval.Monthly:
                        recurringExpense.NextRecurrenceDate = recurringExpense.NextRecurrenceDate.AddMonths(1);
                        break;
                    case RecurrenceInterval.Yearly:
                        recurringExpense.NextRecurrenceDate = recurringExpense.NextRecurrenceDate.AddYears(1);
                        break;
                }
            }

            await _dbContext.SaveChangesAsync();

            await AddRecurringIncomesAndExpenses(_dbContext);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop the background service.
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Dispose of the timer when the service is stopped.
            _timer?.Dispose();
        }
    }
}
