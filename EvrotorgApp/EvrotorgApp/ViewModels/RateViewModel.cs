using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EvrotorgApp.ViewModels
{
    public class RateViewModel : ObservableObject
    {
        private int _curScale;
        private string _curAbbreviation;
        private decimal? _curOfficialRate;

        public int Cur_ID { get; set; }

        public DateTime Date { get; set; }

        public string Cur_Abbreviation
        {
            get => _curAbbreviation;
            set
            {
                if (value == _curAbbreviation)
                {
                    return;
                }

                _curAbbreviation = value;
                OnPropertyChanged();
            }
        }

        public int Cur_Scale { get; set; }

        public string Cur_Name { get; set; }

        public decimal? Cur_OfficialRate
        {
            get => _curOfficialRate;
            set
            {
                if (value == _curOfficialRate)
                {
                    return;
                }

                _curOfficialRate = value;
                OnPropertyChanged();
            }
        }
    }
}
