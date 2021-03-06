﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DuathlonManager2.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetValue<T>(ref T backingField, T value, [CallerMemberName]string caller = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;
            backingField = value;
            OnPropertyChanged(caller);
            return true;
        }
    }
}
