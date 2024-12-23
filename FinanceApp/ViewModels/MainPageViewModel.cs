using FinanceApp.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace FinanceApp.ViewModels
{
    // ViewModel для главной страницы
    public class MainPageViewModel : ViewModelBase
    {
        private decimal _newTransactionAmount; // Сумма новой транзакции
        private TransactionType _newTransactionType; // Тип новой транзакции
        private ExpenseCategory _newTransactionCategory; // Категория новой транзакции
        private ObservableCollection<Transaction> _transactions = new ObservableCollection<Transaction>(); // Коллекция транзакций

        // Свойства для привязки к элементам интерфейса
        public decimal NewTransactionAmount
        {
            get => _newTransactionAmount;
            set => Set(ref _newTransactionAmount, value);
        }
        public TransactionType NewTransactionType
        {
            get => _newTransactionType;
            set => Set(ref _newTransactionType, value);
        }
        public ExpenseCategory NewTransactionCategory
        {
            get => _newTransactionCategory;
            set => Set(ref _newTransactionCategory, value);
        }
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set => Set(ref _transactions, value);
        }

        // Свойство для отображения общей суммы расходов за текущий день
        public string TodayTotalExpenses
        {
            get => CalculateTodayTotalExpenses().ToString("C");
        }

        // Команда для добавления транзакции
        public RelayCommand AddTransactionCommand { get; }

        public MainPageViewModel()
        {
            // Инициализация команды
            AddTransactionCommand = new RelayCommand(AddTransaction);
            // Загрузка транзакций из файла
            LoadTransactions();
        }

        // Метод для добавления новой транзакции
        private void AddTransaction()
        {
            // Проверка, чтобы сумма была больше 0
            if (NewTransactionAmount <= 0) return;
            // Создание новой транзакции
            Transaction newTransaction = new Transaction(NewTransactionAmount, NewTransactionType, NewTransactionCategory, DateTime.Now);
            // Добавление в коллекцию
            Transactions.Add(newTransaction);
            // Сохранение транзакций в файл
            SaveTransactions();
            // Обновление суммы расходов за день
            OnPropertyChanged(nameof(TodayTotalExpenses));
            // Обнуление суммы
            NewTransactionAmount = 0;
        }

        // Метод для вычисления общей суммы расходов за текущий день
        private decimal CalculateTodayTotalExpenses()
        {
            return Transactions
                .Where(t => t.Type == TransactionType.Expense && t.DateTime.Date == DateTime.Today)
                .Sum(t => t.Amount);
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
                                // Создание новой транзакции и добавление в коллекцию
                                Transactions.Add(new Transaction(amount, type, category, dateTime));
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

        // Метод для сохранения транзакций в файл
        private void SaveTransactions()
        {
            try
            {
                // Использование StreamWriter для записи данных в файл
                using (var writer = new StreamWriter("transactions.txt"))
                {
                    // Перебор всех транзакций и запись их в файл
                    foreach (var transaction in Transactions)
                    {
                        // Запись данных транзакции в виде строки с разделителями ","
                        writer.WriteLine($"{transaction.Amount},{transaction.Type},{transaction.Category},{transaction.DateTime}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Вывод сообщения об ошибке, если что-то пошло не так
                MessageBox.Show($"Error saving transactions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}