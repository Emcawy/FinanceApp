﻿using System;
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
using FinanceApp.ViewModels;


namespace FinanceApp.Views
{
    public partial class AnalysisPage : UserControl
    {
        public AnalysisPage()
        {
            InitializeComponent();
            // Установка DataContext для AnalysisPage
            DataContext = new AnalysisPageViewModel();
        }
    }
}