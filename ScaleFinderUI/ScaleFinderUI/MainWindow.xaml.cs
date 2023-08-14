using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NAudio.Midi;

namespace ScaleFinderUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int BasePitch = ScaleFinder.PitchC;
        private int Accid = ScaleFinder.AccidNatural;
        private int[] Type = ScaleFinder.IntervalMajorScale;
        private string SelectedBasePitchText = "C";
        private string SelectedAccidText = String.Empty;
        private string SelectedTypeText = " Major";
        static ScaleFinder Finder = new();
        Scale ScaleFindResult;

        public MainWindow() {
            this.Loaded += new RoutedEventHandler(WindowLoaded);
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e) {
            RBtnBaseC.IsChecked = true;
            RBtnAccidN.IsChecked = true;
            RBtnTypeMajor.IsChecked = true;
            RBtnSortAscending.IsChecked = true;
        }

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
                Type = ScaleFinder.IntervalMajorScale;
                SelectedTypeText = "Major Scale";
            }
            else if ((bool)RBtnTypeMinor.IsChecked) {
                Type = ScaleFinder.IntervalNaturalMinorScale;
                SelectedTypeText = "Natural Minor Scale";
            }
            else if ((bool)RBtnTypeIonian.IsChecked) {
                Type = ScaleFinder.IntervalIonianMode;
                SelectedTypeText = "Ionian Mode";
            }
            else if ((bool)RBtnTypeDorian.IsChecked) {
                Type = ScaleFinder.IntervalDorianMode;
                SelectedTypeText = "Dorian Mode";
            }
            else if ((bool)RBtnTypePhtygian.IsChecked) {
                Type = ScaleFinder.IntervalPhtygianMode;
                SelectedTypeText = "Phtygian Mode";
            }
            else if ((bool)RBtnTypeLydian.IsChecked) {
                Type = ScaleFinder.IntervalLydianMode;
                SelectedTypeText = "Lydian Mode";
            }
            else if ((bool)RBtnTypeMixolydian.IsChecked) {
                Type = ScaleFinder.IntervalMixolydianMode;
                SelectedTypeText = "Mixolydian Mode";
            }
            else if ((bool)RBtnTypeAeolian.IsChecked) {
                Type = ScaleFinder.IntervalAeolianMode;
                SelectedTypeText = "Aeolian Mode";
            }
            else if ((bool)RBtnTypeLocrain.IsChecked) {
                Type = ScaleFinder.IntervalLocrainMode;
                SelectedTypeText = "Locrain Mode";
            }
            UpdateResult();
        }

        private void HandleSortChecked(object sender, RoutedEventArgs e) {
            return;
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
        }

        private void OnLostFocusAccidCount(object sender, RoutedEventArgs e) {
            if (TBAccidCount.Text.Length < 1) {
                TBAccidCount.Text = "1";
            }
        }

        private void OnClickedAccidCount(object sender, RoutedEventArgs e) {
        }

        private void OnMouseDownAccidCount(object sender, RoutedEventArgs e) {
        }
        private void HandleBtnPlaySound(object sender, RoutedEventArgs e) {
            if (RBtnSortAscending.IsChecked == true) {
                PlayMidi(0);
            }
            else if (RBtnSortDescending.IsChecked == true) {
                PlayMidi(1);
            }
            else if (RBtnSortAscendingDescending.IsChecked == true) {
                PlayMidi(0);
                PlayMidi(1);
            }
            else if (RBtnSortDescendingAscending.IsChecked == true) {
                PlayMidi(1);
                PlayMidi(0);
            }
        }
        private void UpdateResult() {
            int accid = Accid;
            if (RBtnAccidN.IsChecked == true) {
                accid = 0;
            }
            else if (RBtnAccidF.IsChecked == true) {
                accid *= -1;
            }
            ScaleFindResult = Finder.FindScale(BasePitch, accid, Type);
            SelectedBasePitchText = ScaleFindResult.GetPitchText(0);
            SelectedAccidText = ScaleFindResult.GetAccidentalText(0);
            if (!ScaleFindResult.GetFound()) {
                Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                return;
            }
            if (TBSelectedScale == null) {
                return;
            }
            string[] texts = ScaleFindResult.GetPitchTexts();
            string scaleResultText = "";
            string degreesText = "";
            string intervalsText = "";
            for (int i = 0; i < texts.Length; i++) {
                scaleResultText += texts[i] + " ";
            }
            for (int i = 0; i < ScaleFindResult.GetAccidentalTexts().Length - 1; i++) {
                int j = i + 1;
                degreesText += ScaleFindResult.GetAccidentalText(i) + j + " ";
            }
            TBSelectedScale.Text = SelectedBasePitchText + " " + SelectedTypeText;
            TBScaleResult.Text = "Notes: " + scaleResultText;
            TBDegrees.Text = "Degrees: " + degreesText;
            ScaleFindResult.PrintMyValues();
        }

        private void PlayMidi(int sort) {
            MidiOut midiOut = new MidiOut(0);
            int[] pitchList = ScaleFindResult.GetPitchList();
            int pitchToPlay = 0;
            int speed = Convert.ToInt32(TBSoundSpeed.Text);
            if (sort == 0) {
                for (int i = 0; i < pitchList.Length; i++) {
                    pitchToPlay = pitchList[i] + 59;
                    midiOut.Send(MidiMessage.StartNote(pitchToPlay, 127, 1).RawData);
                    Thread.Sleep(speed);
                    midiOut.Send(MidiMessage.StopNote(pitchToPlay, 0, 1).RawData);
                    Thread.Sleep(1);
                }
                Thread.Sleep(400);
            }
            else if (sort == 1) {
                for (int i = 7; i >= 0; i--) {
                    pitchToPlay = pitchList[i] + 59;
                    midiOut.Send(MidiMessage.StartNote(pitchToPlay, 127, 1).RawData);
                    Thread.Sleep(speed);
                    midiOut.Send(MidiMessage.StopNote(pitchToPlay, 0, 1).RawData);
                    Thread.Sleep(1);
                }
                Thread.Sleep(400);
            }
            midiOut.Close();
            midiOut.Dispose();
        }
    }
}
