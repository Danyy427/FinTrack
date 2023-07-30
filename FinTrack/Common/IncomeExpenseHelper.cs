using FinTrack.Data;
using FinTrack.Models;
using Microsoft.Identity.Client;

namespace FinTrack.Common
{
    public static class IncomeExpenseHelper
    {
        public static void CreateIncome(this ApplicationDbContext context, Income income, FinTrackUser user)
        {
            // Create an income
            if (income == null)
            {
                  throw new ArgumentNullException(nameof(income));
            }
            else
            {
                user.Incomes.Add(income);
                context.Incomes.Add(income);
                context.SaveChanges();
            }
        }

        public static void CreateExpense(this ApplicationDbContext context, Expense expense, FinTrackUser user)
        {
            // Create an expense
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            else
            {
                user.Expenses.Add(expense);
                context.Expenses.Add(expense);
                context.SaveChanges();
            }
        }

        public static void DeleteIncome(this ApplicationDbContext context, Income income, FinTrackUser user)
        {
            if (income == null)
            {
                throw new ArgumentNullException(nameof(income));
            }
            else
            {
                user.Incomes.Remove(income);
                context.Incomes.Remove(income);
                context.SaveChanges();
            }
        }

        public static void DeleteExpense(this ApplicationDbContext context, Expense expense, FinTrackUser user)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            else
            {
                user.Expenses.Remove(expense);
                context.Expenses.Remove(expense);
                context.SaveChanges();
            }
        }

        public static void CreateRecurringIncome(this ApplicationDbContext context, RecurringIncome income, FinTrackUser user)
        {
            if (income == null)
            {
                throw new ArgumentNullException(nameof(income));
            }
            else
            {
                user.RecurringIncomes.Add(income);
                context.RecurringIncomes.Add(income);
                context.SaveChanges();
            }
        }

        public static void CreateRecurringExpense(this ApplicationDbContext context, RecurringExpense expense, FinTrackUser user)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            else
            {
                user.RecurringExpenses.Add(expense);
                context.RecurringExpenses.Add(expense);
                context.SaveChanges();
            }
        }

        public static void DeleteRecurringIncome(this ApplicationDbContext context, RecurringIncome income, FinTrackUser user)
        {
            if (income == null)
            {
                throw new ArgumentNullException(nameof(income));
            }
            else
            {
                user.RecurringIncomes.Remove(income);
                context.RecurringIncomes.Remove(income);
                context.SaveChanges();
            }
        }

        public static void DeleteRecurringExpense(this ApplicationDbContext context, RecurringExpense expense, FinTrackUser user)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            else
            {
                user.RecurringExpenses.Remove(expense);
                context.RecurringExpenses.Remove(expense);
                context.SaveChanges();
            }
        }
    }
}
