using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using FinanceApp.Models;
using FinanceApp.ViewModels;
using System.Windows;

namespace FinanceApp.Converters
{
    // Класс для преобразования данных из TextBox в ListView
    public class PlanConverter : IValueConverter
    {
        // Метод для преобразования данных в текстовый формат
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is TextBox textBox && value is ExpenseCategory category)
            {
                if (textBox.DataContext is PlanningPageViewModel viewModel)
                {
                    if (viewModel.MonthlyPlans.ContainsKey(category))
                    {
                        return viewModel.MonthlyPlans[category].ToString();
                    }
                }
            }
            return "";
        }

        // Метод для преобразования текстового формата обратно в данные
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && parameter is TextBox textBox && parameter is ExpenseCategory category)
            {
                if (textBox.DataContext is PlanningPageViewModel context)
                {
                    if (decimal.TryParse(stringValue, out decimal plan))
                    {
                        context.MonthlyPlans[category] = plan;
                    }
                    else
                    {
                        MessageBox.Show($"Error parsing plan value: {stringValue}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
}