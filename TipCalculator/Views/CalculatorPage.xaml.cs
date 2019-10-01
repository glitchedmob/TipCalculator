using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TipCalculator.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TipCalculator.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculatorPage : ContentPage
    {
        public BillTipViewModel BillTipViewModel { get; set; }
        
        public CalculatorPage()
        {
            InitializeComponent();
            
            BillTipViewModel = new BillTipViewModel();
            
            BindingContext = BillTipViewModel;

            SetupTapReconginzers();
        }

        private void SetupTapReconginzers()
        {
            var billGesture = new TapGestureRecognizer();
            billGesture.Tapped += SetBillCurrentInput;
            BillTextLabel.GestureRecognizers.Add(billGesture);
            BillValueLabel.GestureRecognizers.Add(billGesture);
            
            var tipGesture = new TapGestureRecognizer();
            tipGesture.Tapped += SetTipCurrentInput;
            TipTextLabel.GestureRecognizers.Add(tipGesture);
            TipValueLabel.GestureRecognizers.Add(tipGesture);
        }

        private void CalculatorButton_OnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button == null) return;
            
            TryVibrate();
            BillTipViewModel.OnInput(button.Text);
        }

        private void SetBillCurrentInput(object sender, EventArgs e)
        {

            TryVibrate();
            BillTipViewModel.CurrentInput = BillTipViewModel.InputType.Bill;
        }
        
        private void SetTipCurrentInput(object sender, EventArgs e)
        {
            TryVibrate();
            BillTipViewModel.CurrentInput = BillTipViewModel.InputType.Tip;
        }

        private bool TryVibrate()
        {
            try
            {
                var duration = TimeSpan.FromMilliseconds(50);
                Vibration.Vibrate(duration);
                return true;
            }
            catch (FeatureNotEnabledException exception)
            {
                return false;
            }
        }
    }
}