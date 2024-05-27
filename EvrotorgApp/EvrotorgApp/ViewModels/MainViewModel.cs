using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace EvrotorgApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly CurrencyHttpClient _currencyHttpClient;
        private string _fileToSave;

        private DateTime _selectedDate = DateTime.Today;
        private ObservableCollection<RateViewModel> _rateRates;
        private bool _isLoadedFile;
        private bool _isListFill;

        public ICommand SearchCommand { get; }

        public ICommand SaveToFileCommand { get; }

        public ICommand SaveChangesCommand { get; }

        public ICommand ReadFromFileCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            SearchCommand = new AsyncRelayCommand(SearchCommandExecute);
            SaveToFileCommand = new AsyncRelayCommand(SaveToFileCommandExecute);
            ReadFromFileCommand = new AsyncRelayCommand(ReadFromFileCommandExecute);
            SaveChangesCommand = new AsyncRelayCommand(SaveChangesCommandExecute);

            _currencyHttpClient = serviceProvider.GetService<CurrencyHttpClient>();
        }

        public ObservableCollection<RateViewModel> RateRates
        {
            get => _rateRates;
            set
            {
                if (Equals(value, _rateRates))
                {
                    return;
                }

                _rateRates = value;
                OnPropertyChanged();
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (value.Equals(_selectedDate))
                {
                    return;
                }

                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoadedFile
        {
            get => _isLoadedFile;
            set
            {
                if (value == _isLoadedFile)
                {
                    return;
                }

                _isLoadedFile = value;
                OnPropertyChanged();
            }
        }

        public bool IsListFill
        {
            get => _isListFill;
            set
            {
                if (value == _isListFill)
                {
                    return;
                }

                _isListFill = value;
                OnPropertyChanged();
            }
        }

        private async Task SearchCommandExecute()
        {
            var (isSuccess, elements) = await _currencyHttpClient.GetRateByDateAsync(SelectedDate).ConfigureAwait(false);

            if (isSuccess)
            {
                Dispatcher.CurrentDispatcher.Invoke(() => RateRates = new ObservableCollection<RateViewModel>(elements));
                IsLoadedFile = false;
                IsListFill = true;
            }
        }

        private async Task SaveToFileCommandExecute()
        {
            const string explorerPath = "explorer.exe";
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "json files (*.json)|*.json",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() is true)
            {
                await Task.Run(() => SaveToFile(saveFileDialog.FileName)).ConfigureAwait(false);
                Process.Start(explorerPath, Path.GetDirectoryName($"{saveFileDialog.FileName}"));
            }
        }

        private async Task SaveChangesCommandExecute()
        {
            await Task.Run(() => SaveToFile(_fileToSave)).ConfigureAwait(false);
        }

        private void SaveToFile(string fileName)
        {
            var jsonString = JsonConvert.SerializeObject(RateRates, Formatting.Indented);
            File.WriteAllText(fileName, jsonString);

            IsLoadedFile = true;
            _fileToSave = fileName;
        }

        private async Task ReadFromFileCommandExecute()
        {
            var readFileDialog = new OpenFileDialog();

            if (readFileDialog.ShowDialog() is true)
            {
                IEnumerable<RateViewModel> elements = default;

                await Task.Run(() =>
                {
                    var jsonString = File.ReadAllText(readFileDialog.FileName);
                    elements = JsonConvert.DeserializeObject<IEnumerable<RateViewModel>>(jsonString);
                }).ConfigureAwait(false);

                Dispatcher.CurrentDispatcher.Invoke(() => RateRates = new ObservableCollection<RateViewModel>(elements));
            }

            IsListFill = true;
            IsLoadedFile = true;
            _fileToSave = readFileDialog.FileName;
        }
    }
}
