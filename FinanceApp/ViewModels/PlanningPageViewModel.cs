using FinanceApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace FinanceApp.ViewModels
{
    // ViewModel для страницы планирования
    public class PlanningPageViewModel : ViewModelBase
    {
        private Dictionary<ExpenseCategory, decimal> _monthlyPlans = new Dictionary<ExpenseCategory, decimal>(); // Словарь для хранения планов расходов

        // Свойство для привязки планов расходов
        public Dictionary<ExpenseCategory, decimal> MonthlyPlans
        {
            get => _monthlyPlans;
            set => Set(ref _monthlyPlans, value);
        }

        // Коллекция всех категорий расходов
        public ObservableCollection<ExpenseCategory> Categories { get; } = new ObservableCollection<ExpenseCategory>(Enum.GetValues(typeof(ExpenseCategory)).Cast<ExpenseCategory>());

        // Команда для сохранения планов
        public RelayCommand SavePlansCommand { get; }

        public PlanningPageViewModel()
        {
            // Загрузка планов из файла
            LoadPlans();
            // Инициализация команды
            SavePlansCommand = new RelayCommand(SavePlans);
        }

        // Метод для сохранения планов в файл
        private void SavePlans()
        {
            try
            {
                // Использование StreamWriter для записи данных в файл
                using (var writer = new StreamWriter("monthly_plans.txt"))
                {
                    // Перебор всех планов и запись их в файл
                    foreach (var plan in MonthlyPlans)
                    {
                        // Запись данных плана в виде строки с разделителями ","
                        writer.WriteLine($"{plan.Key},{plan.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Вывод сообщения об ошибке, если что-то пошло не так
                MessageBox.Show($"Error saving plans: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}