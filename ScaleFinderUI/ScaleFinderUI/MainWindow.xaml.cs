using NAudio.Midi;
using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;

namespace ScaleFinderUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        //private Canvas MusicSheetCanvas;
        private int BasePitch = ScaleFinder.PitchC;
        private int Accid = ScaleFinder.AccidNatural;
        private int[] Type = ScaleFinder.IntervalMajorScale;
        private string SelectedBasePitchText = "C";
        private string SelectedAccidText = String.Empty;
        private string SelectedTypeText = " Major";
        private int FirstNotePos = 0;
        private int ClefNotePos = 0;
        private int LineGap = 26;
        private int LineGapHalf = 13;
        private int Padding = 14;
        private int Octave = 0;
        static ScaleFinder Finder = new();
        Scale ScaleFindResult;
        //> Images
        Image GClefImg = new Image();
        Image FClefImg = new Image();
        Image CClefImg = new Image();
        Image[] WholeNoteImgs = new Image[8];
        Image SharpImg = new Image();
        Image FlatImg = new Image();
        Image DoubleSharpImg = new Image();
        Image DoubleFlatImg = new Image();

        public MainWindow() {
            this.Loaded += new RoutedEventHandler(WindowLoaded);
            InitializeComponent();
            LoadMusicSheetImages();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e) {
            RBtnBaseC.IsChecked = true;
            RBtnAccidN.IsChecked = true;
            RBtnTypeMajor.IsChecked = true;
            RBtnSortAscending.IsChecked = true;
            RBtnTrebleClef.IsChecked = true;
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
            if (RBtnAccidN.IsChecked == true) {
                ButtonDown.IsEnabled = false;
                ButtonUp.IsEnabled = false;
                TBAccidCount.IsEnabled = false;
            }
            else {
                ButtonDown.IsEnabled = true;
                ButtonUp.IsEnabled = true;
                TBAccidCount.IsEnabled = true;
            }
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
            else if ((bool)RBtnTypeHarmonicMinor.IsChecked) {
                Type = ScaleFinder.IntervalHarmonicMinorScale;
                SelectedTypeText = "Harmonic Minor Scale";
            }
            else if ((bool)RBtnTypeMelodicMinor.IsChecked) {
                Type = ScaleFinder.IntervalMelodicMinorScale;
                SelectedTypeText = "Melodic Minor Scale";
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
        private void OnLostFocusAccidCount(object sender, RoutedEventArgs e) {
            if (TBAccidCount.Text.Length < 1) {
                TBAccidCount.Text = "1";
            }
        }

        private void UpdateResult() {
            if (RBtnAccidN.IsChecked == true) {
                Accid = 0;
            }
            else if (RBtnAccidS.IsChecked == true) {
                Accid = Convert.ToInt32(TBAccidCount.Text);
            }
            else if (RBtnAccidF.IsChecked == true) {
                Accid = Convert.ToInt32(TBAccidCount.Text);
                Accid *= -1;
            }
            ScaleFindResult = Finder.FindScale(BasePitch, Accid, Type);
            SelectedBasePitchText = ScaleFindResult.GetPitchText(0);
            SelectedAccidText = ScaleFindResult.GetAccidentalText(0);
            if (!ScaleFindResult.GetFound()) {
                Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                return;
            }
            if (TBSelectedScale == null) {
                return;
            }
            string[] scaleResultTexts = ScaleFindResult.GetPitchTexts();
            int[] intervalsList = ScaleFindResult.GetIntervalsList();
            int[] pitchList = ScaleFindResult.GetPitchList();           
            string scaleResultText = "";
            string degreesText = "";
            string intervalsText = "";         
            for (int i = 0; i < scaleResultTexts.Length; i++) {
                scaleResultText += scaleResultTexts[i] + " ";
            }
            for (int i = 0; i < ScaleFindResult.GetAccidentalTexts().Length - 1; i++) {
                int j = i + 1;
                degreesText += ScaleFindResult.GetAccidentalText(i) + j + " ";
            }
            for (int i = 0; i < intervalsList.Length; i++) {
                if (intervalsList[i] == 1) {
                    intervalsText += "H";
                }
                else if (intervalsList[i] == 2) {
                    intervalsText += "W";
                }
                else if (intervalsList[i] == 3) {
                    intervalsText += "3H";
                }
                if (i != intervalsList.Length - 1) {
                    intervalsText += "-";
                }
            }
            int[] tempList = new int[7];
            string pitchListText = "";
            for (int i = 0; i < pitchList.Length - 1; i++) {
                tempList[i] = pitchList[i] - 1;
                if (tempList[i] < 0) {
                    tempList[i] += 12;
                }
                tempList[i] %= 12;
                pitchListText += tempList[i] + " ";
            }
            TBSelectedScale.Text = SelectedBasePitchText + " " + SelectedTypeText;
            TBScaleResult.Text = "Notes: " + scaleResultText;
            TBDegrees.Text = "Degrees: " + degreesText;
            TBIntervalsResult.Text = "Intervals: " + intervalsText;
            TBIntegerNotation.Text = "Integer notation: " + pitchListText;
            ScaleFindResult.PrintMyValues();
            DrawWholeNote();
        }        

        private void HandleClefChecked(object sender, RoutedEventArgs e) {            
            ClefNotePos = 0;
            Octave = 0;
            // ● Clef Type
            DrawClef();
            // ● G Clef
            if (RBtnTrebleClef.IsChecked == true) {
                Canvas.SetTop(GClefImg, 0);
                ClefNotePos = 0;
            }
            else if (RBtnFrenchClef.IsChecked == true) {
                Canvas.SetTop(GClefImg, LineGap);
                ClefNotePos = LineGap;                
            }
            // ● F Clef
            if (RBtnBassClef.IsChecked == true) {
                Canvas.SetTop(FClefImg, 34);
                ClefNotePos = LineGap * 6 * -1;                
            }
            else if (RBtnBaritoneFClef.IsChecked == true) {
                Canvas.SetTop(FClefImg, 34 + LineGap);
                ClefNotePos = LineGap * 5 * -1;
            }
            else if (RBtnSubbassClef.IsChecked == true) {
                Canvas.SetTop(FClefImg, 34 - LineGap);
                ClefNotePos = LineGap * 7 * -1;
            }
            // ● C Clef
            if (RBtnAltoClef.IsChecked == true) {
                Canvas.SetTop(CClefImg, 38.9);
                ClefNotePos = LineGap * 3 * -1;                
            }
            else if (RBtnTenorClef.IsChecked == true) {
                Canvas.SetTop(CClefImg, 38.9 - LineGap);
                ClefNotePos = LineGap * 4 * -1;
            }
            else if (RBtnBaritoneCClef.IsChecked == true) {
                Canvas.SetTop(CClefImg, 38.9 - LineGap * 2);
                ClefNotePos = LineGap * 5 * -1;
            }
            else if (RBtnMezzoSopranoClef.IsChecked == true) {
                Canvas.SetTop(CClefImg, 38.9 + LineGap);
                ClefNotePos = LineGap * 2 * -1;
            }
            else if (RBtnSopranoClef.IsChecked == true) {
                Canvas.SetTop(CClefImg, 38.9 + LineGap * 2);
                ClefNotePos = LineGap * -1;
            }
            // ● Whole Note            
            DrawWholeNote();
        }
        
        private void DrawMusicSheet() {            
            int startY = LineGap + Padding;
            this.CVMusicSheet.Children.Clear();
            for (int i = 0; i < 5; i++) {
                this.CVMusicSheet.Children.Add(CreateLine(20, startY, this.SPCanvas.ActualWidth - 80, startY));
                startY += LineGap;
            }            
            this.CVMusicSheet.Children.Add(SharpImg);
        }

        private void DrawClef() {
            CVMusicSheet.Children.Clear();
            DrawMusicSheet();
            if (RBtnTrebleClef.IsChecked == true || RBtnFrenchClef.IsChecked == true) {
                this.CVMusicSheet.Children.Add(GClefImg);
                GClefImg.Width = 96;
                Canvas.SetLeft(GClefImg, 19.9);
            }
            else if (RBtnBassClef.IsChecked == true || RBtnBaritoneFClef.IsChecked == true || RBtnSubbassClef.IsChecked == true) {
                CVMusicSheet.Children.Add(FClefImg);
                FClefImg.Width = 96;
                Canvas.SetLeft(FClefImg, 19.9);
            }
            else if (RBtnAltoClef.IsChecked == true || RBtnTenorClef.IsChecked == true ||
                RBtnBaritoneCClef.IsChecked == true || RBtnMezzoSopranoClef.IsChecked == true || RBtnSopranoClef.IsChecked == true) {
                CVMusicSheet.Children.Add(CClefImg);
                CClefImg.Width = 75.2;
                Canvas.SetLeft(CClefImg, 33);
            }
        }

        private void DrawWholeNote() {
            CVMusicSheet.Children.Clear();
            DrawMusicSheet();
            DrawClef();
            int left = 130;
            int leftGap = 80;
            double top = 0;
            double lineStart = LineGap + Padding;
            for (int i = 0; i < WholeNoteImgs.Length; i++) {
                CVMusicSheet.Children.Remove(WholeNoteImgs[i]);
            }
            FirstNotePos = 0;
            Octave = 0;
            string pt = ScaleFindResult.GetPitchText(0);
            if (pt.StartsWith("C")) {
                FirstNotePos = 0;
            }
            else if (pt.StartsWith("D")) {
                FirstNotePos = LineGapHalf;
            }
            else if (pt.StartsWith("E")) {
                FirstNotePos = LineGapHalf * 2;
            }
            else if (pt.StartsWith("F")) {
                FirstNotePos = LineGapHalf * 3;
            }
            else if (pt.StartsWith("G")) {
                FirstNotePos = LineGapHalf * 4;
            }
            else if (pt.StartsWith("A")) {
                FirstNotePos = LineGapHalf * 5;
            }
            else if (pt.StartsWith("B")) {
                FirstNotePos = LineGapHalf * 6;
            }
            for (int i = 0; i < 8; i++) {
                top = 158.32 - FirstNotePos + ClefNotePos;
                if (i == 0) {
                    if (top < lineStart + LineGap + LineGapHalf && top > lineStart - LineGap * 2) {
                        Octave = -1;
                    }
                    else if (top < lineStart - LineGap * 2) {
                        Octave = -2;
                    }
                    else if (top > lineStart + LineGap * 5) {
                        Octave = 1;
                    }
                }
                top = 158.32 - FirstNotePos + ClefNotePos + Octave * (LineGap * 3 + LineGapHalf) * -1;
                Canvas.SetTop(WholeNoteImgs[i], top);
                Canvas.SetLeft(WholeNoteImgs[i], left);
                this.CVMusicSheet.Children.Add(WholeNoteImgs[i]);
                if (top < lineStart - LineGap) {
                    this.CVMusicSheet.Children.Add(CreateLine(left - 10, lineStart - LineGap, left + WholeNoteImgs[i].Width + 10, lineStart - LineGap));
                }
                else if (top > lineStart + LineGap * 4 + 5) {
                    this.CVMusicSheet.Children.Add(CreateLine(left - 10, lineStart + LineGap * 5, left + WholeNoteImgs[i].Width + 10, lineStart + LineGap * 5));
                }
                FirstNotePos += 13;
                left += leftGap;
            }
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

        private void PlayMidi(int sort) {
            MidiOut midiOut = new MidiOut(0);
            int[] pitchList = ScaleFindResult.GetPitchList();
            int pitchToPlay = 0;
            int speed = Convert.ToInt32(TBSoundSpeed.Text);
            if (sort == 0) {
                for (int i = 0; i < pitchList.Length; i++) {
                    pitchToPlay = pitchList[i] + 59 + Octave * 12;
                    midiOut.Send(MidiMessage.StartNote(pitchToPlay, 127, 1).RawData);
                    Thread.Sleep(speed);
                    midiOut.Send(MidiMessage.StopNote(pitchToPlay, 0, 1).RawData);
                    Thread.Sleep(1);
                }
                Thread.Sleep(400);
            }
            else if (sort == 1) {
                for (int i = 7; i >= 0; i--) {
                    pitchToPlay = pitchList[i] + 59 + Octave * 12;
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

        private void LoadMusicSheetImages() {
            // ● Treble Clef
            BitmapImage trebleClefBtm = new BitmapImage();
            trebleClefBtm.BeginInit();
            trebleClefBtm.UriSource = new Uri("pack://application:,,,/assets/TrebleClef.png");
            trebleClefBtm.EndInit();
            GClefImg.Source = trebleClefBtm;
            // ● Base Clef
            BitmapImage bassClefBtm = new BitmapImage();
            bassClefBtm.BeginInit();
            bassClefBtm.UriSource = new Uri("pack://application:,,,/assets/BassClef.png");
            bassClefBtm.EndInit();
            FClefImg.Source = bassClefBtm;
            // ● C Clef
            BitmapImage cclefBtm = new BitmapImage();
            cclefBtm.BeginInit();
            cclefBtm.UriSource = new Uri("pack://application:,,,/assets/CClef.png");
            cclefBtm.EndInit();
            CClefImg.Source = cclefBtm;
            // ● Whole Note
            BitmapImage wholeNoteBtm = new BitmapImage();
            wholeNoteBtm.BeginInit();
            wholeNoteBtm.UriSource = new Uri("pack://application:,,,/assets/WholeNote.png");
            wholeNoteBtm.EndInit();
            for (int i = 0; i < 8; i++) {
                if (WholeNoteImgs[i] == null) {
                    WholeNoteImgs[i] = new Image();
                }
                WholeNoteImgs[i].Width = 36.1;
                Canvas.SetLeft(WholeNoteImgs[i], 100.0);
                Canvas.SetTop(WholeNoteImgs[i], 158.32);
                WholeNoteImgs[i].Source = wholeNoteBtm;
            }
            // ● Sharp
            BitmapImage sharpBtm = new BitmapImage();
            sharpBtm.BeginInit();
            sharpBtm.UriSource = new Uri("pack://application:,,,/assets/Sharp.png");
            sharpBtm.EndInit();
            SharpImg.Width = 19;
            Canvas.SetTop(SharpImg, 160);
            Canvas.SetLeft(SharpImg, 180);
            SharpImg.Source = sharpBtm;
            // ● Flat
            BitmapImage flatBtm = new BitmapImage();
            flatBtm.BeginInit();
            flatBtm.UriSource = new Uri("pack://application:,,,/assets/Flat.png");
            flatBtm.EndInit();
            FlatImg.Width = 19;
            FlatImg.Source = sharpBtm;
            // ● Double Sharp
            BitmapImage doubleSharpBtm = new BitmapImage();
            doubleSharpBtm.BeginInit();
            doubleSharpBtm.UriSource = new Uri("pack://application:,,,/assets/DoubleSharp.png");
            doubleSharpBtm.EndInit();
            DoubleSharpImg.Width = 19;
            DoubleSharpImg.Source = doubleSharpBtm;
            // ● Double Flat
            BitmapImage doubleFlatBtm = new BitmapImage();
            doubleFlatBtm.BeginInit();
            doubleFlatBtm.UriSource = new Uri("pack://application:,,,/assets/DoubleFlat.png");
            doubleFlatBtm.EndInit();
            DoubleFlatImg.Width = 13;
            DoubleFlatImg.Source = sharpBtm;
        }

        private Line CreateLine(double x1, double y1, double x2, double y2) {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1; 
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = 1.5;
            line.Stroke = (Brush)(Brushes.Black);
            line.SnapsToDevicePixels = true;
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            return line;
        }
    }
}

