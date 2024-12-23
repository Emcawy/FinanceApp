using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FinanceApp.Models
{
    // Базовый класс для ViewModel, реализующий INotifyPropertyChanged
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Метод для вызова события PropertyChanged
        protected virtual void OnPropertyChangedName] string propertyName = null)
        {
            if (field == null && value == null) return false;
            if (field != null && field.Equals(value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}