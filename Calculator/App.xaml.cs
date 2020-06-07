using System;
using Calculator.Models;
using Calculator.Views;
using Calculator.ViewModels;
using System.Windows;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup (StartupEventArgs args)
        {
            CalculatorEngine ce = new CalculatorEngine();
            CalculatorViewModel vm = new CalculatorViewModel(ce);
            CalculatorView cv = new CalculatorView();
            cv.DataContext = vm;
            cv.Show();
        }
    }
}
