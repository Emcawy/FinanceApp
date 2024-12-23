using FinanceApp.Models;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace FinanceApp.ViewModels
{
    // ViewModel для страницы анализа
    public class AnalysisPageViewModel : ViewModelBase
    {
        private List<Transaction> _transactions = new List<Transaction>(); // Список для хранения транзакций
        private Dictionary<ExpenseCategory, decimal> _monthlyPlans = new Dictionary<ExpenseCategory, decimal>(); // Словарь для хранения планов расходов
        public PlotModel PieModel { get; } // Модель для круговой диаграммы

        // Свойства для отображения общей суммы доходов, расходов и разницы с планами
        public decimal TotalIncome
        {
            get => CalculateTotalIncome();
        }
        public decimal TotalExpenses
        {
            get => CalculateTotalExpenses();
        }
        public decimal TotalDifference
        {
            get => CalculateTotalDifference();
        }

        // Коллекция для хранения информации о расходах по каждой категории
        public ObservableCollection<CategoryInfo> CategoryInfos { get; set; } = new ObservableCollection<CategoryInfo>();

        public AnalysisPageViewModel()
        {
            // Загрузка транзакций и планов из файлов
            LoadTransactions();
            LoadPlans();
            // Вычисление информации по каждой категории
            CalculateCategoryInfo();
            // Создание круговой диаграммы
            PieModel = CreatePieChart();
        }
        // Метод для загрузки транзакций из файла
        private void LoadTransactions()
        {
            var filePath = "transactions.txt"; // Путь к файлу с транзакциями
            if (File.Exists(filePath))
            {
                try
                {
                    // Чтение всех строк из файла
                    foreach (var line in File.ReadLines(filePath))
                    {
                        // Разбиение строки на части по разделителю ","
                        var parts = line.Split(',');
                        // Проверка, что строка содержит все необходимые данные
                        if (parts.Length == 4)
                        {
                            // Попытка преобразовать части строки в соответствующие типы
                            if (decimal.TryParse(parts[0], out decimal amount) &&
                                Enum.TryParse(parts[1], out TransactionType type) &&
                                Enum.TryParse(parts[2], out ExpenseCategory category) &&
                                DateTime.TryParse(parts[3], out DateTime dateTime))
                            {
                                // Создание новой транзакции и добавление в список
                                _transactions.Add(new Transaction(amount, type, category, dateTime));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Вывод сообщения об ошибке, если что-то пошло не так
                    MessageBox.Show($"Error loading transactions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Метод для загрузки планов из файла
        private void LoadPlans()
        {
            var filePath = "monthly_plans.txt"; // Путь к файлу с планами
            if (File.Exists(filePath))
            {
                try
                {
                    // Чтение всех строк из файла
                    foreach (var line in File.ReadLines(filePath))
                    {
                        // Разбиение строки на части по разделителю ","
                        var parts = line.Split(',');
                        // Проверка, что строка содержит все необходимые данные
                        if (parts.Length == 2)
                        {
                            // Попытка преобразовать части строки в соответствующие типы
                            if (Enum.TryParse(parts[0], out ExpenseCategory category) && decimal.TryParse(parts[1], out decimal plan))
                            {
                                // Добавление плана в словарь
                                MonthlyPlans[category] = plan;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Вывод сообщения об ошибке, если что-то пошло не так
                    MessageBox.Show($"Error loading plans: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Метод для создания круговой диаграммы
        private PlotModel CreatePieChart()
        {
            var plotModel = new PlotModel();
            // Создание новой серии для круговой диаграммы
            var pieSeries = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0,
                OutsideLabelFormat = "{0}: {1:0} ({2:0%})",
            };

            // Перебор всех категорий и добавление их на круговую диаграмму
            foreach (var categoryInfo in CategoryInfos)
            {
                pieSeries.Slices.Add(new PieSlice(categoryInfo.Category.ToString(), (double)categoryInfo.Actual) { IsExploded = true });
            }
            // Добавление серии на модель
            plotModel.Series.Add(pieSeries);
            return plotModel;
        }
        // Метод для вычисления информации по каждой категории
        private void CalculateCategoryInfo()
        {
            // Получение транзакций за текущий месяц
            var currentMonthTransactions = _transactions.Where(t => t.DateTime.Month == DateTime.Now.Month).ToList();
            // Получение только расходных транзакций
            var expenseTransactions = currentMonthTransactions.Where(t => t.Type == TransactionType.Expense);
            // Очистка коллекции
            CategoryInfos.Clear();
            // Перебор всех категорий
            foreach (ExpenseCategory category in Enum.GetValues(typeof(ExpenseCategory)))
            {
                // Суммирование расходов по текущей категории
                var actualExpense = expenseTransactions.Where(t => t.Category == category).Sum(t => t.Amount);
                // Получение запланированной суммы расходов
                decimal planned = MonthlyPlans.ContainsKey(category) ? MonthlyPlans[category] : 0;
                // Вычисление разницы между запланированной и фактической суммой расходов
                decimal difference = planned - actualExpense;
                // Добавление информации в коллекцию
                CategoryInfos.Add(new CategoryInfo(category, planned, actualExpense, difference));
            }
        }
        // Метод для вычисления общей суммы доходов
        private decimal CalculateTotalIncome()
        {
            // Суммирование всех доходов за текущий месяц
            return _transactions
               .Where(t => t.Type == TransactionType.Income && t.DateTime.Month == DateTime.Now.Month)
               .Sum(t => t.Amount);
        }
        // Метод для вычисления общей суммы расходов
        private decimal CalculateTotalExpenses()
        {
            // Суммирование всех расходов за текущий месяц
            return _transactions
               .Where(t => t.Type == TransactionType.Expense && t.DateTime.Month == DateTime.Now.Month)
               .Sum(t => t.Amount);
        }
        // Метод для вычисления разницы между доходами и расходами
        private decimal CalculateTotalDifference()
        {
            return TotalIncome - TotalExpenses;
        }
    }
    // Класс для представления информации по каждой категории
    public class CategoryInfo
    {
        public ExpenseCategory Category { get; set; } // Категория расхода
        public decimal Planned { get; set; } // Запланированная сумма
        public decimal Actual { get; set; } // Фактическая сумма
        public decimal Difference { get; set; } // Разница между запланированной и фактической суммой

        public CategoryInfo(ExpenseCategory category, decimal planned, decimal actual, decimal difference)
        {
            Category = category;
            Planned = planned;
            Actual = actual;
            Difference = difference;
        }
    }
}