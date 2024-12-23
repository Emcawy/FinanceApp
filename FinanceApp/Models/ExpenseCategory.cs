using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Models
{
    // Перечисление для категорий расходов
    public enum ExpenseCategory
    {
        Housing,        // Жилье
        Food,           // Питание
        Transport,      // Транспорт
        Health,         // Здоровье
        Entertainment, // Развлечения
        Clothing,       // Одежда
        PersonalCare,   // Личная гигиена
        Education,      // Образование
        Unforeseen     // Непредвиденные расходы
    }
}