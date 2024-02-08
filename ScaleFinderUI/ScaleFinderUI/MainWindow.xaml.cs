/*
MIT License
Copyright(c) 2023 Teo Han[meteory.kr@gmail.com]
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files(the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE
*/
using NAudio.Midi;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private const string DoubleSharp = "𝄪";
        private const string DoubleFlat = "𝄫";
        private const int TabClefTypeG = 0;
        private const int TabClefTypeF = 1;
        private const int TabClefTypeC = 2;
        private int SelectedTabClefType = TabClefTypeG;
        private int BasePitch = ScaleFinder.PitchC;
        private int Accid = ScaleFinder.AccidNatural;
        private int[] Type = ScaleFinder.IntervalMajorScale;
        private string SelectedBasePitchText = "C";
        private string SelectedAccidText = String.Empty;
        private string SelectedTypeText = " Major";
        private double StartLineTop = 0;
        private int FirstNotePos = 0;
        private int ClefNotePos = 0;
        private int LineGap = 20;
        private int LineGapHalf = 10;
        //private int Padding = 14;
        private int Octave = 0;
        private int AdditionalGap = 0;
        static ScaleFinder Finder = new();
        Scale ScaleFindResult;
        Scale ScaleFindResultBaseC;
        //> Images
        Image GClefImg = new Image();
        Image FClefImg = new Image();
        Image CClefImg = new Image();
        Image[] WholeNoteImgs = new Image[8];
        Image[] SharpImgs = new Image[8];
        Image[] FlatImgs = new Image[8];
        Image[] DoubleSharpImgs = new Image[16];
        Image[] DoubleFlatImgs = new Image[16];
        private bool IsWindowLoaded = false;
        public MainWindow() {
            this.Loaded += new RoutedEventHandler(WindowLoaded);
            LoadMusicSheetImages();
            InitializeComponent();
        }
        private void WindowLoaded(object sender, RoutedEventArgs e) {
            RBtnBaseC.IsChecked = true;
            RBtnAccidNatural.IsChecked = true;
            RBtnTypeMajorScale.IsChecked = true;
            RBtnTrebleClef.IsChecked = true;
            StartLineTop = CVMusicSheet.Height / 2 - (LineGap * 2);
            Debug.Print("> WindowLoaded() StartLineTop ====== " + StartLineTop);
            IsWindowLoaded = true;
            DrawAll();
        }
        private void HandleBasePitchChecked(object sender, RoutedEventArgs e) {
            RadioButton rb = sender as RadioButton;
            if (rb == null) {
                return;
            }
            switch (rb.Name) {
                case "RBtnBaseC":
                    BasePitch = ScaleFinder.PitchC;
                    break;
                case "RBtnBaseD":
                    BasePitch = ScaleFinder.PitchD;
                    break;
                case "RBtnBaseE":
                    BasePitch = ScaleFinder.PitchE;
                    break;
                case "RBtnBaseF":
                    BasePitch = ScaleFinder.PitchF;
                    break;
                case "RBtnBaseG":
                    BasePitch = ScaleFinder.PitchG;
                    break;
                case "RBtnBaseA":
                    BasePitch = ScaleFinder.PitchA;
                    break;
                case "RBtnBaseB":
                    BasePitch = ScaleFinder.PitchB;
                    break;
                default:
                    break;
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
            switch (rb.Name) {
                case "RBtnAccidNatural":
                    ButtonDown.IsEnabled = false;
                    ButtonUp.IsEnabled = false;
                    TBAccidCount.IsEnabled = false;
                    break;
                case "RBtnAccidSharp":
                    ButtonDown.IsEnabled = true;
                    ButtonUp.IsEnabled = true;
                    TBAccidCount.IsEnabled = true;
                    break;
                case "RBtnAccidFlat":
                    ButtonDown.IsEnabled = true;
                    ButtonUp.IsEnabled = true;
                    TBAccidCount.IsEnabled = true;
                    break;
                default:
                    break;
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
            switch (rb.Name) {
                case "RBtnTypeMajorScale":
                    Type = ScaleFinder.IntervalMajorScale;
                    SelectedTypeText = "Major Scale";
                    break;
                case "RBtnTypeMinorScale":
                    Type = ScaleFinder.IntervalNaturalMinorScale;
                    SelectedTypeText = "Natural Minor Scale";
                    break;
                case "RBtnTypeHarmonicMinorScale":
                    Type = ScaleFinder.IntervalHarmonicMinorScale;
                    SelectedTypeText = "Harmonic Minor Scale";
                    break;
                case "RBtnTypeMelodicMinorScale":
                    Type = ScaleFinder.IntervalMelodicMinorScale;
                    SelectedTypeText = "Melodic Minor Scale";
                    break;
                case "RBtnTypeIonianMode":
                    Type = ScaleFinder.IntervalIonianMode;
                    SelectedTypeText = "Ionian Mode";
                    break;
                case "RBtnTypeDorianMode":
                    Type = ScaleFinder.IntervalDorianMode;
                    SelectedTypeText = "Dorian Mode";
                    break;
                case "RBtnTypePhtygianMode":
                    Type = ScaleFinder.IntervalPhtygianMode;
                    SelectedTypeText = "Phtygian Mode";
                    break;
                case "RBtnTypeLydianMode":
                    Type = ScaleFinder.IntervalLydianMode;
                    SelectedTypeText = "Lydian Mode";
                    break;
                case "RBtnTypeMixolydianMode":
                    Type = ScaleFinder.IntervalMixolydianMode;
                    SelectedTypeText = "Mixolydian Mode";
                    break;
                case "RBtnTypeAeolianMode":
                    Type = ScaleFinder.IntervalAeolianMode;
                    SelectedTypeText = "Aeolian Mode";
                    break;
                case "RBtnTypeLocrainMode":
                    Type = ScaleFinder.IntervalLocrainMode;
                    SelectedTypeText = "Locrain Mode";
                    break;
                default:
                    break;
            }
            UpdateResult();
        }
        private void HandleSortChecked(object sender, RoutedEventArgs e) {
            DrawAll();
        }
        protected void HandleTextChanged(object sender, EventArgs e) {
            switch (TBAccidCount.Text.Length) {
                case < 1:
                    return;
                case > 1:
                    TBAccidCount.Text = TBAccidCount.Text.Substring(0, 1);
                    return;
                default:
                    break;
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
            if (AccidCount > 2) {
                return;
            }
            AccidCount += 1;
            TBAccidCount.Text = AccidCount.ToString();
        }
        private void NumberValidationTextBoxAccidCount(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^1-3]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void OnLostFocusAccidCount(object sender, RoutedEventArgs e) {
            if (TBAccidCount.Text.Length < 1) {
                TBAccidCount.Text = "1";
            }
        }
        private void UpdateResult() {
            if (RBtnAccidNatural.IsChecked == true) {
                Accid = 0;
            }
            else if (RBtnAccidSharp.IsChecked == true) {
                Accid = Convert.ToInt32(TBAccidCount.Text);
            }
            else if (RBtnAccidFlat.IsChecked == true) {
                Accid = Convert.ToInt32(TBAccidCount.Text);
                Accid *= -1;
            }
            ScaleFindResult = Finder.FindScale(BasePitch, Accid, Type);
            ScaleFindResultBaseC = Finder.FindScale(ScaleFinder.PitchC, ScaleFinder.AccidNatural, Type);
            SelectedBasePitchText = ScaleFindResult.GetPitchText(0);
            SelectedAccidText = ScaleFindResult.GetAccidentalText(0);
            if (!ScaleFindResult.GetFound()) {
                Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                return;
            }
            if (TBSelectedScale == null) {
                return;
            }
            if (TBScaleResult == null) {
                return;
            }
            if (TBDegrees == null) {
                return;
            }
            string[] scaleResultTexts = ScaleFindResult.GetPitchTexts();
            int[] intervalsList = ScaleFindResultBaseC.GetIntervalsList();
            int[] pitchList = ScaleFindResult.GetPitchList();
            string scaleResultText = "";
            string degreesText = "";
            string intervalsText = "";
            for (int i = 0; i < scaleResultTexts.Length; i++) {
                scaleResultText += scaleResultTexts[i] + " ";
            }
            for (int i = 0; i < ScaleFindResultBaseC.GetAccidentalTexts().Length - 1; i++) {
                int j = i + 1;
                degreesText += ScaleFindResultBaseC.GetAccidentalText(i) + j + " ";
            }
            for (int i = 0; i < intervalsList.Length; i++) {
                switch (intervalsList[i]) {
                    case 1:
                        intervalsText += "H";
                        break;
                    case 2:
                        intervalsText += "W";
                        break;
                    case 3:
                        intervalsText += "3H";
                        break;
                    case 4:
                        intervalsText += "2W";
                        break;
                    case 5:
                        intervalsText += "5H";
                        break;
                    default:
                        break;
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
            TBDegrees.Text = "Degrees / Formulas: " + degreesText;
            TBIntervalsResult.Text = "Intervals / Steps: " + intervalsText;
            TBIntegerNotation.Text = "Integer notation: " + pitchListText;
            ScaleFindResult.PrintMyValues();
            DrawAll();
        }
        private void HandleClefTypeSelectionChanged(object sender, EventArgs e) {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Name as string;
            switch (tabItem) {
                case "TIGClef":
                    SelectedTabClefType = TabClefTypeG;
                    if (RBtnTrebleClef.IsChecked == true || RBtnFrenchClef.IsChecked == true) {
                        break;
                    }
                    RBtnTrebleClef.IsChecked = true;
                    break;
                case "TIFClef":
                    SelectedTabClefType = TabClefTypeF;
                    if (RBtnBassClef.IsChecked == true || RBtnBaritoneFClef.IsChecked == true || RBtnSubbassClef.IsChecked == true) {
                        break;
                    }
                    RBtnBassClef.IsChecked = true;
                    break;
                case "TICClef":
                    SelectedTabClefType = TabClefTypeC;
                    if (RBtnAltoClef.IsChecked == true || RBtnTenorClef.IsChecked == true || RBtnBaritoneCClef.IsChecked == true ||
                        RBtnMezzoSopranoClef.IsChecked == true || RBtnSopranoClef.IsChecked == true) {
                        break;
                    }
                    RBtnAltoClef.IsChecked = true;
                    break;
                default:
                    return;
            }
            ChangeSelectedClefImage();
            DrawAll();
        }
        private void ChangeSelectedClefImage() {
            // ● G Clef
            if (SelectedTabClefType == TabClefTypeG) {
                double clefTop = CVMusicSheet.Height / 2 - GClefImg.Height / 2;
                if (RBtnTrebleClef.IsChecked == true) {
                    Canvas.SetTop(GClefImg, clefTop);
                    ClefNotePos = 0;
                }
                else if (RBtnFrenchClef.IsChecked == true) {
                    Canvas.SetTop(GClefImg, clefTop + LineGap);
                    ClefNotePos = LineGap;
                }
            }
            if (RBtnBassClef == null || RBtnAltoClef == null) {
                return;
            }
            // ● F Clef
            if (SelectedTabClefType == TabClefTypeF) {
                double clefTop = CVMusicSheet.Height / 2 - FClefImg.Height / 2;
                if (RBtnBassClef.IsChecked == true) {
                    Canvas.SetTop(FClefImg, clefTop);
                    ClefNotePos = LineGap * 6 * -1;
                }
                else if (RBtnBaritoneFClef.IsChecked == true) {
                    Canvas.SetTop(FClefImg, clefTop + LineGap);
                    ClefNotePos = LineGap * 5 * -1;
                }
                else if (RBtnSubbassClef.IsChecked == true) {
                    Canvas.SetTop(FClefImg, clefTop - LineGap);
                    ClefNotePos = LineGap * 7 * -1;
                }
            }
            // ● C Clef
            if (SelectedTabClefType == TabClefTypeC) {
                double clefTop = CVMusicSheet.Height / 2 - CClefImg.Height / 2;
                if (RBtnAltoClef.IsChecked == true) {
                    Canvas.SetTop(CClefImg, clefTop);
                    ClefNotePos = LineGap * 3 * -1;
                }
                else if (RBtnTenorClef.IsChecked == true) {
                    Canvas.SetTop(CClefImg, clefTop - LineGap);
                    ClefNotePos = LineGap * 4 * -1;
                }
                else if (RBtnBaritoneCClef.IsChecked == true) {
                    Canvas.SetTop(CClefImg, clefTop - LineGap * 2);
                    ClefNotePos = LineGap * 5 * -1;
                }
                else if (RBtnMezzoSopranoClef.IsChecked == true) {
                    Canvas.SetTop(CClefImg, clefTop + LineGap);
                    ClefNotePos = LineGap * 2 * -1;
                }
                else if (RBtnSopranoClef.IsChecked == true) {
                    Canvas.SetTop(CClefImg, clefTop + LineGap * 2);
                    ClefNotePos = LineGap * -1;
                }
            }
        }
        private void HandleClefChecked(object sender, RoutedEventArgs e) {
            ClefNotePos = 0;
            Octave = 0;
            ChangeSelectedClefImage();
            DrawAll();
        }
        private void DrawAll() {
            if (!IsWindowLoaded) {
                return;
            }
            CVMusicSheet.Children.Clear();
            DrawClef();
            DrawNote();
            DrawMusicSheet();
        }
        private void DrawMusicSheet() {
            Debug.Print("> DrawMusicSheet() StartLineTop ====== " + StartLineTop);
            double startY = StartLineTop;
            for (int i = 0; i < 5; i++) {
                this.CVMusicSheet.Children.Add(CreateLine(20, startY, this.SPCanvas.ActualWidth - 80, startY));
                startY += LineGap;
            }
        }

        private void DrawClef() {
            switch (SelectedTabClefType) {
                case TabClefTypeG:
                    this.CVMusicSheet.Children.Add(GClefImg);
                    Canvas.SetLeft(GClefImg, 19.9);
                    break;
                case TabClefTypeF:
                    CVMusicSheet.Children.Add(FClefImg);
                    Canvas.SetLeft(FClefImg, 19.9);
                    break;
                case TabClefTypeC:
                    CVMusicSheet.Children.Add(CClefImg);
                    Canvas.SetLeft(CClefImg, 33);
                    break;
                default:
                    break;
            }
        }
        private void DrawNote() {
            if (RBtnSortDescending == null) {
                return;
            }
            int left = 130;
            int leftGap = 80;
            double top = 0;
            //double lineStart = LineGap + Padding;
            for (int i = 0; i < WholeNoteImgs.Length; i++) {
                CVMusicSheet.Children.Remove(WholeNoteImgs[i]);
            }
            Octave = 0;
            string pt = ScaleFindResult.GetPitchText(0);
            int pos = 0;
            if (pt.StartsWith("C")) {
                pos = 0;
            }
            else if (pt.StartsWith("D")) {
                pos = 1;
            }
            else if (pt.StartsWith("E")) {
                pos = 2;
            }
            else if (pt.StartsWith("F")) {
                pos = 3;
            }
            else if (pt.StartsWith("G")) {
                pos = 4;
            }
            else if (pt.StartsWith("A")) {
                pos = 5;
            }
            else if (pt.StartsWith("B")) {
                pos = 6;
            }
            FirstNotePos = LineGapHalf * pos;
            Debug.WriteLine("sizeof(char): {0}", sizeof(char));
            Debug.WriteLine("sizeof(int): {0}", sizeof(int));
            Debug.WriteLine("sizeof(float): {0}", sizeof(float));
            Debug.WriteLine("sizeof(double): {0}", sizeof(double));
            int accidList = 0;
            //Debug.WriteLine("accidText {0} " + accidList, accidList.Length);                        
            for (int i = 0; i < 8; i++) {
                leftGap = 60;
                top = StartLineTop - FirstNotePos + ClefNotePos + LineGapHalf * 9;
                accidList = ScaleFindResult.GetAccidentalList()[i];
                AdditionalGap = 18;
                if (i == 0) {
                    if (top < StartLineTop + LineGap + LineGapHalf && top >= StartLineTop - LineGap * 2) {
                        Octave = -1;
                    }
                    else if (top < StartLineTop - LineGap * 2) {
                        Octave = -2;
                    }
                    else if (top >= StartLineTop + LineGap * 5) {
                        Octave = 1;
                    }
                }
                top += Octave * (LineGap * 3 + LineGapHalf) * -1;
                if (RBtnSortDescending.IsChecked == true) {
                    top -= LineGapHalf * 7;
                }
                switch (accidList) {
                    case 3:
                        AdditionalGap += 14;
                        break;
                    case -3:
                        AdditionalGap += 18;
                        break;
                    case 4:
                        AdditionalGap += 8;
                        break;
                    case -4:
                        AdditionalGap += 28;
                        break;
                    default:
                        break;
                }
                leftGap += AdditionalGap;
                if (i == 0) {
                    leftGap = AdditionalGap;
                }
                //if (RBtnSortAscending.IsChecked == true) {
                left += leftGap;
                //}
                //else if (RBtnSortDescending.IsChecked == true) {
                //    left -= leftGap;
                // }
                switch (accidList) {
                    case 0:
                        break;
                    case 1:
                        CVMusicSheet.Children.Add(SharpImgs[i]);
                        Canvas.SetTop(SharpImgs[i], top - 19);
                        Canvas.SetLeft(SharpImgs[i], left - 22);
                        break;
                    case -1:
                        CVMusicSheet.Children.Add(FlatImgs[i]);
                        Canvas.SetTop(FlatImgs[i], top - 20);
                        Canvas.SetLeft(FlatImgs[i], left - 22);
                        break;
                    case 2:
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i]);
                        Canvas.SetTop(DoubleSharpImgs[i], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i], left - 20);
                        break;
                    case -2:
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i]);
                        Canvas.SetTop(DoubleFlatImgs[i], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i], left - 32);
                        break;
                    case 3:
                        CVMusicSheet.Children.Add(SharpImgs[i]);
                        Canvas.SetTop(SharpImgs[i], top - 19);
                        Canvas.SetLeft(SharpImgs[i], left - 41);
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i]);
                        Canvas.SetTop(DoubleSharpImgs[i], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i], left - 20);
                        break;
                    case -3:
                        CVMusicSheet.Children.Add(FlatImgs[i]);
                        Canvas.SetTop(FlatImgs[i], top - 20);
                        Canvas.SetLeft(FlatImgs[i], left - 57);
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i]);
                        Canvas.SetTop(DoubleFlatImgs[i], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i], left - 35);
                        break;
                    case 4:
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i]);
                        Canvas.SetTop(DoubleSharpImgs[i], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i], left - 39);
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i + 8]);
                        Canvas.SetTop(DoubleSharpImgs[i + 8], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i + 8], left - 20);
                        break;
                    case -4:
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i]);
                        Canvas.SetTop(DoubleFlatImgs[i], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i], left - 68);
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i + 8]);
                        Canvas.SetTop(DoubleFlatImgs[i + 8], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i + 8], left - 32);
                        break;
                    default:
                        break;
                }
                Canvas.SetTop(WholeNoteImgs[i], top);
                Canvas.SetLeft(WholeNoteImgs[i], left);

                Debug.Print("WNote POS : " + top + " , " + left);


                this.CVMusicSheet.Children.Add(WholeNoteImgs[i]);
                if (top < StartLineTop - LineGap) {
                    this.CVMusicSheet.Children.Add(CreateLine(left - 10, StartLineTop - LineGap, left + WholeNoteImgs[i].Width + 10, StartLineTop - LineGap));
                }
                else if (top > StartLineTop + LineGap * 4 + 5) {
                    this.CVMusicSheet.Children.Add(CreateLine(left - 10, StartLineTop + LineGap * 5, left + WholeNoteImgs[i].Width + 10, StartLineTop + LineGap * 5));
                }
                if (RBtnSortAscending.IsChecked == true) {
                    FirstNotePos += LineGapHalf;
                }
                else if (RBtnSortDescending.IsChecked == true) {
                    FirstNotePos -= LineGapHalf;
                }
            }
            Debug.Print("----------------------------------------");
        }
        private void HandleBtnPlaySound(object sender, RoutedEventArgs e) {
            if (RBtnSortAscending.IsChecked == true) {
                PlayMidi(0);
            }
            else if (RBtnSortDescending.IsChecked == true) {
                PlayMidi(1);
            }
        }
        private void PlayMidi(int sort) {
            MidiOut midiOut = new MidiOut(0);
            int[] pitchList = ScaleFindResult.GetPitchList();
            int pitchToPlay = 0;
            int speed = 200;
            switch (sort) {
                case 0:
                    for (int i = 0; i < pitchList.Length; i++) {
                        pitchToPlay = pitchList[i] + 59 + Octave * 12;
                        midiOut.Send(MidiMessage.StartNote(pitchToPlay, 127, 1).RawData);
                        Thread.Sleep(speed);
                        midiOut.Send(MidiMessage.StopNote(pitchToPlay, 0, 1).RawData);
                        Thread.Sleep(1);
                    }
                    Thread.Sleep(400);
                    break;
                case 1:
                    for (int i = 7; i >= 0; i--) {
                        pitchToPlay = pitchList[i] + 59 + Octave * 12;
                        midiOut.Send(MidiMessage.StartNote(pitchToPlay, 127, 1).RawData);
                        Thread.Sleep(speed);
                        midiOut.Send(MidiMessage.StopNote(pitchToPlay, 0, 1).RawData);
                        Thread.Sleep(1);
                    }
                    Thread.Sleep(400);
                    break;
                default:
                    break;
            }
            midiOut.Close();
            midiOut.Dispose();
        }
        private void LoadMusicSheetImages() {
            // ● G Clef
            BitmapImage gClefBtm = new BitmapImage();
            gClefBtm.BeginInit();
            gClefBtm.UriSource = new Uri("pack://application:,,,/assets/GClef.png");
            gClefBtm.EndInit();
            GClefImg.Height = 153;
            GClefImg.Source = gClefBtm;
            // ● F Clef
            BitmapImage fClefBtm = new BitmapImage();
            fClefBtm.BeginInit();
            fClefBtm.UriSource = new Uri("pack://application:,,,/assets/FClef.png");
            fClefBtm.EndInit();
            FClefImg.Height = 91.5;
            FClefImg.Source = fClefBtm;
            // ● C Clef
            BitmapImage cclefBtm = new BitmapImage();
            cclefBtm.BeginInit();
            cclefBtm.UriSource = new Uri("pack://application:,,,/assets/CClef.png");
            cclefBtm.EndInit();
            CClefImg.Height = LineGap * 4;
            CClefImg.Source = cclefBtm;
            // ● Whole Note
            BitmapImage wholeNoteBtm = new BitmapImage();
            wholeNoteBtm.BeginInit();
            wholeNoteBtm.UriSource = new Uri("pack://application:,,,/assets/WholeNote.png");
            wholeNoteBtm.EndInit();
            Image img;
            for (int i = 0; i < 8; i++) {
                if (WholeNoteImgs[i] == null) {
                    WholeNoteImgs[i] = new Image();
                }
                img = WholeNoteImgs[i];
                img.Source = wholeNoteBtm;
                img.Height = LineGap;
                img.Width = LineGap * img.Source.Width / img.Source.Height;
                Canvas.SetLeft(img, 100.0);
                Canvas.SetTop(img, StartLineTop);
            }
            // ● Sharp
            BitmapImage sharpBtm = new BitmapImage();
            sharpBtm.BeginInit();
            sharpBtm.UriSource = new Uri("pack://application:,,,/assets/Sharp.png");
            sharpBtm.EndInit();
            for (int i = 0; i < 8; i++) {
                if (SharpImgs[i] == null) {
                    SharpImgs[i] = new Image();
                }
                SharpImgs[i].Source = sharpBtm;
                SharpImgs[i].Height = LineGap * 3;
                SharpImgs[i].Width = (LineGap * SharpImgs[i].Source.Width / SharpImgs[i].Source.Height) * 3;
                Canvas.SetLeft(SharpImgs[i], 300);
                Canvas.SetTop(SharpImgs[i], 180);

            }
            // ● Flat
            BitmapImage flatBtm = new BitmapImage();
            flatBtm.BeginInit();
            flatBtm.UriSource = new Uri("pack://application:,,,/assets/Flat.png");
            flatBtm.EndInit();
            for (int i = 0; i < 8; i++) {
                if (FlatImgs[i] == null) {
                    FlatImgs[i] = new Image();
                }
                FlatImgs[i].Source = flatBtm;
                FlatImgs[i].Height = LineGap * 2.26;
                FlatImgs[i].Width = (LineGap * FlatImgs[i].Source.Width / FlatImgs[i].Source.Height) * 2.26;
                Canvas.SetLeft(FlatImgs[i], 300);
                Canvas.SetTop(FlatImgs[i], 180);
            }
            // ● Double Sharp
            BitmapImage doubleSharpBtm = new BitmapImage();
            doubleSharpBtm.BeginInit();
            doubleSharpBtm.UriSource = new Uri("pack://application:,,,/assets/DoubleSharp.png");
            doubleSharpBtm.EndInit();
            for (int i = 0; i < 16; i++) {
                if (DoubleSharpImgs[i] == null) {
                    DoubleSharpImgs[i] = new Image();
                }
                DoubleSharpImgs[i].Source = doubleSharpBtm;
                DoubleSharpImgs[i].Height = LineGap;
                DoubleSharpImgs[i].Width = (LineGap * DoubleSharpImgs[i].Source.Width / DoubleSharpImgs[i].Source.Height);
                Canvas.SetLeft(DoubleSharpImgs[i], 300);
                Canvas.SetTop(DoubleSharpImgs[i], 180);
            }
            // ● Double Flat
            BitmapImage doubleFlatBtm = new BitmapImage();
            doubleFlatBtm.BeginInit();
            doubleFlatBtm.UriSource = new Uri("pack://application:,,,/assets/DoubleFlat.png");
            doubleFlatBtm.EndInit();
            for (int i = 0; i < 16; i++) {
                if (DoubleFlatImgs[i] == null) {
                    DoubleFlatImgs[i] = new Image();
                }
                DoubleFlatImgs[i].Source = doubleFlatBtm;
                DoubleFlatImgs[i].Height = LineGap * 2.26;
                DoubleFlatImgs[i].Width = (LineGap * DoubleFlatImgs[i].Source.Width / DoubleFlatImgs[i].Source.Height) * 2.26;
                Canvas.SetLeft(DoubleFlatImgs[i], 300);
                Canvas.SetTop(DoubleFlatImgs[i], 180);

            }
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
        private void OpenGithubURL(object sender, EventArgs e) {            
            Process.Start(new ProcessStartInfo("https://github.com/TeoDevKR/scale-finder") { UseShellExecute = true });
        }
    }
}