using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using FinanceApp.Models;
using FinanceApp.ViewModels;

namespace FinanceApp.Converters
{
    // Класс для преобразования данных из TextBox в ListView
    public class PlanConverter : IValueConverter
    {
        // Метод для преобразования данных в текстовый формат
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверка, что значение является категорией расхода
            if (value is ExpenseCategory category)
            {
                // Получение ViewModel из DataContext TextBox
                var viewModel = (PlanningPageViewModel)BindingOperations.GetBindingExpression((TextBox)parameter, TextBox.TextProperty)?.DataItem as PlanningPageViewModel;
                // Проверка, что ViewModel не null и содержит ключ
                if (viewModel != null && viewModel.MonthlyPlans.ContainsKey(category))
                    // Возвращение строкового представления плана
                    return viewModel.MonthlyPlans[category].ToString();
            }

            return ""; // Возврат пустой строки, если условия не выполнены
        }

        // Метод для преобразования текстового формата обратно в данные
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверка, что значение является строкой, и что TextBox имеет DataContext типа PlanningPageViewModel
            if (value is string stringValue && parameter is TextBox textBox && textBox.DataContext is PlanningPageViewModel context && BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty)?.DataItem is ExpenseCategory category)
            {
                // Попытка преобразовать строку в decimal
                if (decimal.TryParse(stringValue, out decimal plan))
                    // Если успешно, установка значения плана
                    context.MonthlyPlans[category] = plan;
                else
                    // Если преобразование не удалось, установка плана в 0
                    context.MonthlyPlans[category] = 0;

                // Возвращаем строковое представление обновленного плана
                return context.MonthlyPlans[category].ToString();
            }
            return null;
        }
    }
}