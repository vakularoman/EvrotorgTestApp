using System;
using EvrotorgApp.ViewModels;
using System.Windows;

namespace EvrotorgApp
{
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel { get; }

        public MainWindow(IServiceProvider serviceProvider)
        {
            MainViewModel = new MainViewModel(serviceProvider);
            DataContext = this;

            InitializeComponent();
        }
    }
}
