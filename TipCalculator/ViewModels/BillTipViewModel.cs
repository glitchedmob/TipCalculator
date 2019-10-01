using System;

namespace TipCalculator.ViewModels
{
    public class BillTipViewModel : BaseViewModel
    {
        public enum InputType
        {
            Bill,
            Tip,
        }

        public InputType CurrentInput { get; set; } = InputType.Bill;

        private const string DefaultValue = "0";

        private string _currentValue;

        private string _bill = DefaultValue;
        public string Bill {
            get => _bill;
            set
            {
                _bill = value;
                OnPropertyChanged(nameof(Bill));
                OnPropertyChanged(nameof(BillTotal));
                OnPropertyChanged(nameof(TipTotal));
            }
        }

        private string _tip = DefaultValue;
        public string Tip
        {
            get => _tip;
            set
            {
                _tip = value;
                OnPropertyChanged(nameof(Tip));
                OnPropertyChanged(nameof(BillTotal));
                OnPropertyChanged(nameof(TipTotal));
            }
        }

        private double BillValue
        {
            get
            {
                if (double.TryParse(Bill, out var val))
                {
                    return val;
                }

                return 0;
            }
        }

        private double TipValue
        {
            get
            {
                if (double.TryParse(Tip, out var val))
                {
                    return val;
                }

                return 0; 
            }
        }

        public double BillTotal => BillValue * (1 + TipValue / 100);

        public double TipTotal => BillValue * (TipValue / 100);

        public void OnInput(string value)
        {

            _currentValue = CurrentInput == InputType.Bill ? Bill : Tip;
            
            switch (value)
            {
                case "DEL":
                    DeleteCharacter();
                    break;
                case "AC":
                    AllClear();
                    return;
                case "+":
                    Increment();
                    break;
                case "-":
                    Decrement();
                    break;
                case ".":
                    Period();
                    break;
                default:
                    Number(value);
                    break;
            }

            if (CurrentInput == InputType.Bill)
            {
                Bill = _currentValue;
                return;
            }

            Tip = _currentValue;
        }

        private void Decrement()
        {
            if (!double.TryParse(_currentValue, out var value)) return;
            value -= 1;

            if (value <= 0)
            {
                _currentValue = DefaultValue;
                return;
            }
            
            _currentValue = value.ToString();
        }

        private void Increment()
        {
            if (!double.TryParse(_currentValue, out var value)) return;
            
            value += 1;

            _currentValue = value.ToString();
        }

        private void Number(string value)
        {
            if (double.TryParse(_currentValue, out var parsed) && parsed <= 0)
            {
                _currentValue = value;
                return;
            }

            if (CurrentInput == InputType.Bill && _currentValue.Contains("."))
            {
                var index = _currentValue.IndexOf(".", StringComparison.Ordinal);

                if (index == _currentValue.Length - 3) return;
            }
            
            _currentValue += value;
        }

        private void Period()
        {
            if (!_currentValue.Contains("."))
            {
                _currentValue += ".";
            }
        }

        private void AllClear()
        {
            Bill = DefaultValue;
            Tip = DefaultValue;
        }

        private void DeleteCharacter()
        {
            if (_currentValue.Length > 1)
            {
                _currentValue = _currentValue.Substring(0, _currentValue.Length - 1);
                return;
            }

            _currentValue = DefaultValue;
        }
    }
}