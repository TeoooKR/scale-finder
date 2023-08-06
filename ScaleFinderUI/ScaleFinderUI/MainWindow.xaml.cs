using System;
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
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using ScaleFinderUI;
using Microsoft.Win32;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Windows.Automation.Text;

namespace ScaleFinderUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int BasePitch = ScaleFinder.PitchC;
        private int Accid = ScaleFinder.AccidNatural;
        private int Type = ScaleFinder.TypeMajorScale;
        private string SelectedBasePitchText = "C";
        private string SelectedAccidText = String.Empty;
        private string SelectedTypeText = " Major";
        static ScaleFinder Finder = new();
        public MainWindow() {
            this.Loaded += new RoutedEventHandler(WindowLoaded);
            InitializeComponent();
        }
        private void WindowLoaded(object sender, RoutedEventArgs e) {
            RBtnBaseC.IsChecked = true;
            RBtnAccidN.IsChecked = true;
            RBtnTypeMajor.IsChecked = true;
        }

        // Base Pitch
        private void HandleBasePitchChecked(object sender, RoutedEventArgs e) {
            RadioButton tb = sender as RadioButton;
            if (tb == null) {
                return;
            }
            if ((bool)RBtnBaseC.IsChecked) {
                BasePitch = ScaleFinder.PitchC;
            }
            else if ((bool)RBtnBaseD.IsChecked) {
                BasePitch = ScaleFinder.PitchD;
            }
            else if ((bool)RBtnBaseE.IsChecked) {
                BasePitch = ScaleFinder.PitchE;
            }
            else if ((bool)RBtnBaseF.IsChecked) {
                BasePitch = ScaleFinder.PitchF;
            }
            else if ((bool)RBtnBaseG.IsChecked) {
                BasePitch = ScaleFinder.PitchG;
            }
            else if ((bool)RBtnBaseA.IsChecked) {
                BasePitch = ScaleFinder.PitchA;
            }
            else if ((bool)RBtnBaseB.IsChecked) {
                BasePitch = ScaleFinder.PitchB;
            }
            if (TBSelectedScale == null) {
                return;
            }
            UpdateResult();
        }
        // Accidental
        private void HandleAccidChecked(object sender, RoutedEventArgs e) {
            RadioButton rb = sender as RadioButton;
            if (rb == null) {
                return;
            }
            if (rb.Name == "RBtnAccidN") {
                ButtonDown.IsEnabled = false;
                ButtonUp.IsEnabled = false;
                TBAccidCount.IsEnabled = false;
            }
            else {
                ButtonDown.IsEnabled = true;
                ButtonUp.IsEnabled = true;
                TBAccidCount.IsEnabled = true;
            }
            Accid = Convert.ToInt32(TBAccidCount.Text);
            if (TBSelectedScale == null) {
                return;
            }
            UpdateResult();
        }

        private void HandleTypeChecked(object sender, RoutedEventArgs e) {
            RadioButton rb = sender as RadioButton;
            if (rb == null) {
                return;
            }
            if ((bool)RBtnTypeMajor.IsChecked) {
                Type = ScaleFinder.TypeMajorScale;
                SelectedTypeText = "Major Scale";
            }
            else if ((bool)RBtnTypeMinor.IsChecked) {
                Type = ScaleFinder.TypeNaturalMinorScale;
                SelectedTypeText = "Natural Minor Scale";
            }
            else if ((bool)RBtnTypeIonian.IsChecked) {
                Type = ScaleFinder.TypeIonianMode;
                SelectedTypeText = "Ionian Mode";
            }
            else if ((bool)RBtnTypeDorian.IsChecked) {
                Type = ScaleFinder.TypeDorianMode;
                SelectedTypeText = "Dorian Mode";
            }
            else if ((bool)RBtnTypePhtygian.IsChecked) {
                Type = ScaleFinder.TypePhtygianMode;
                SelectedTypeText = "Phtygian Mode";
            }
            else if ((bool)RBtnTypeLydian.IsChecked) {
                Type = ScaleFinder.TypeLydianMode;
                SelectedTypeText = "Lydian Mode";
            }
            else if ((bool)RBtnTypeMixolydian.IsChecked) {
                Type = ScaleFinder.TypeMixolydianMode;
                SelectedTypeText = "Mixolydian Mode";
            }
            else if ((bool)RBtnTypeAeolian.IsChecked) {
                Type = ScaleFinder.TypeAeolianMode;
                SelectedTypeText = "Aeolian Mode";
            }
            else if ((bool)RBtnTypeLocrain.IsChecked) {
                Type = ScaleFinder.TypeLocrainMode;
                SelectedTypeText = "Locrain Mode";
            }
            UpdateResult();
        }
        protected void HandleTextChanged(object sender, EventArgs e) {
            if (TBAccidCount.Text.Length < 1) {
                return;
            }
            else if (TBAccidCount.Text.Length > 1) {
                TBAccidCount.Text = TBAccidCount.Text.Substring(0, 1);
                return;
            }
            Accid = Convert.ToInt32(TBAccidCount.Text);
            UpdateResult();
            ((TextBox)sender).SelectAll();
        }
        private void HandleAccidCountDown(object sender, EventArgs e) {
            int AccidCountInt = Convert.ToInt32(TBAccidCount.Text);
            if (AccidCountInt < 2) {
                return;
            }
            AccidCountInt -= 1;
            TBAccidCount.Text = AccidCountInt.ToString();
        }
        private void HandleAccidCountUp(object sender, EventArgs e) {
            int AccidCount = Convert.ToInt32(TBAccidCount.Text);
            if (AccidCount > 8) {
                return;
            }
            AccidCount += 1;
            TBAccidCount.Text = AccidCount.ToString();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^1-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void OnGotFocusAccidCount(object sender, RoutedEventArgs e) {
            ((TextBox)sender).SelectAll();
            //e.Handled = true;
        }
        private void OnLostFocusAccidCount(object sender, RoutedEventArgs e) {
            if (TBAccidCount.Text.Length < 1) {
                TBAccidCount.Text = "1";
            }
        }
        private void OnClickedAccidCount(object sender, RoutedEventArgs e) {
            ((TextBox)sender).SelectAll();
        }
        private void OnMouseDownAccidCount(object sender, RoutedEventArgs e) {
            ((TextBox)sender).SelectAll();
        }
        
        private void UpdateResult() {
            int accid = Accid;
            if (RBtnAccidN.IsChecked == true) {
                accid = 0;
            }
            else if (RBtnAccidF.IsChecked == true) {
                accid *= -1;
            }
            Scale result = Finder.FindScale(BasePitch, accid, Type);
            SelectedBasePitchText = result.GetBasePitchTexts();
            SelectedAccidText = result.GetBaseAccidentalTexts();
            if (!result.GetFound()) {
                Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                return;
            }
            if (TBSelectedScale == null) {
                return;
            }
            string[] texts = result.GetPitchTexts();
            string resultText = "";
            for (int i = 0; i < texts.Length; i++) {
                resultText += texts[i] + " ";
            }
            TBSelectedScale.Text = SelectedBasePitchText + SelectedAccidText + " " + SelectedTypeText;
            TBScaleResult.Text = "Notes: " + resultText;
            result.PrintMyValues();
        }

        private void AccidNumber_LostFocus(object sender, RoutedEventArgs e) {

        }
    }
}
