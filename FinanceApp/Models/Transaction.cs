using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Models
{
    // Класс для представления транзакции (расход или доход)
    public class Transaction
    {
        public decimal Amount { get; set; } // Сумма транзакции
        public TransactionType Type { get; set; } // Тип транзакции (расход или доход)
        public ExpenseCategory Category { get; set; } // Категория расхода
        public DateTime DateTime { get; set; } // Дата и время транзакции

        public Transaction(decimal amount, TransactionType type, ExpenseCategory category, DateTime dateTime)
        {
            Amount = amount;
            Type = type;
            Category = category;
            DateTime = dateTime;
        }
    }
}