using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FinanceApp.Views;


namespace FinanceApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Запуск с главной страницы
            NavigateToMainPage(null, null);
        }

        // Методы для навигации на разные страницы
        private void NavigateToMainPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPage());
        }

        private void NavigateToPlanningPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PlanningPage());
        }

        private void NavigateToAnalysisPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AnalysisPage());
        }
    }
}