using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Calculator.Models;
using Calculator.Commands;

namespace Calculator.ViewModels
{
    public class CalculatorViewModel : DependencyObject, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(String), typeof(CalculatorViewModel));

        private CalculatorEngine _calculatorEngine;

        public CalculatorViewModel (CalculatorEngine calculatorEngine)
        {
            _calculatorEngine = calculatorEngine;
            Result = _calculatorEngine.Calculate('=').ToString();
            INotifyPropertyChanged("Result");
            KeyCommand = new RelayCommand( o => CalculatorKeyClicked (o));
            CloseCommand = new RelayCommand(o => Close(o));
        }

        public string Result {
            get { return (string)GetValue(ResultProperty);  }
            set { SetValue(ResultProperty, value); }
        }

        public void CanClose(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        public void Close(object parameter)
        {
            Application.Current.Shutdown();
        }

        public ICommand KeyCommand { get; private set; }

        public ICommand CloseCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CalculatorKeyClicked (object parameter)
        {
            try
            {
                Result = _calculatorEngine.Calculate((Char.Parse((string)parameter))).ToString();
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            INotifyPropertyChanged("Result");
        }

        private void INotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
