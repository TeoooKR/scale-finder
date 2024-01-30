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
        const string DoubleSharp = "𝄪";
        const string DoubleFlat = "𝄫";
        const int TabClefTypeG = 0;
        const int TabClefTypeF = 1;
        const int TabClefTypeC = 2;
        int SelectedTabClefType = TabClefTypeG;
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
        public MainWindow() {
            this.Loaded += new RoutedEventHandler(WindowLoaded);
            LoadMusicSheetImages();
            InitializeComponent();            
        }
        private void WindowLoaded(object sender, RoutedEventArgs e) {
            RBtnBaseC.IsChecked = true;
            RBtnAccidNatural.IsChecked = true;
            RBtnTypeMajorScale.IsChecked = true;
            RBtnSortAscending.IsChecked = true;
            RBtnTrebleClef.IsChecked = true;
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
            ScaleFindResultBaseC = Finder.FindScale(ScaleFinder.PitchC , ScaleFinder.AccidNatural, Type);
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
                if (RBtnTrebleClef.IsChecked == true) {
                    Canvas.SetTop(GClefImg, 0);
                    ClefNotePos = 0;
                }
                else if (RBtnFrenchClef.IsChecked == true) {
                    Canvas.SetTop(GClefImg, LineGap);
                    ClefNotePos = LineGap;
                }
            }
            if (RBtnBassClef == null || RBtnAltoClef == null) {
                return;
            }
            // ● F Clef
            if (SelectedTabClefType == TabClefTypeF) {
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
            }
            // ● C Clef
            if (SelectedTabClefType == TabClefTypeC) {
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
            }
        }
        private void HandleClefChecked(object sender, RoutedEventArgs e) {            
            ClefNotePos = 0;
            Octave = 0;
            ChangeSelectedClefImage();
            DrawAll();
        }
        private void DrawAll() {
            CVMusicSheet.Children.Clear();            
            DrawClef();
            DrawNote();
            DrawMusicSheet();
        }
        private void DrawMusicSheet() {            
            int startY = LineGap + Padding;            
            for (int i = 0; i < 5; i++) {
                this.CVMusicSheet.Children.Add(CreateLine(20, startY, this.SPCanvas.ActualWidth - 80, startY));
                startY += LineGap;
            }
        }

        private void DrawClef() {
            switch (SelectedTabClefType) {
                case TabClefTypeG:
                    this.CVMusicSheet.Children.Add(GClefImg);
                    GClefImg.Width = 96;
                    Canvas.SetLeft(GClefImg, 19.9);
                    break;
                case TabClefTypeF:
                    CVMusicSheet.Children.Add(FClefImg);
                    FClefImg.Width = 96;
                    Canvas.SetLeft(FClefImg, 19.9); 
                    break;
                case TabClefTypeC:
                    CVMusicSheet.Children.Add(CClefImg);
                    CClefImg.Width = 75.2;
                    Canvas.SetLeft(CClefImg, 33);
                    break;
                default:
                    break;
            }
        }
        private void DrawNote() {
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
            Debug.WriteLine("sizeof(char): {0}", sizeof(char));
            Debug.WriteLine("sizeof(int): {0}", sizeof(int));
            Debug.WriteLine("sizeof(float): {0}", sizeof(float));
            Debug.WriteLine("sizeof(double): {0}", sizeof(double));
            int accidList = 0;
            //Debug.WriteLine("accidText {0} " + accidList, accidList.Length);                        
            for (int i = 0; i < 8; i++) {
                top = 158.32 - FirstNotePos + ClefNotePos;
                accidList = ScaleFindResult.GetAccidentalList()[i];
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
                    if (RBtnSortDescending.IsChecked == true) {
                        left += leftGap * 7; 
                    }
                }
                top = 158.32 - FirstNotePos + ClefNotePos + Octave * (LineGap * 3 + LineGapHalf) * -1;
                Canvas.SetTop(WholeNoteImgs[i], top);
                switch (accidList) {
                    case 0:
                        break;
                    case 1:
                        CVMusicSheet.Children.Add(SharpImgs[i]);
                        Canvas.SetTop(SharpImgs[i], top - 20);
                        Canvas.SetLeft(SharpImgs[i], left - 22 + 13);
                        left += 13;
                        break;
                    case -1:
                        CVMusicSheet.Children.Add(FlatImgs[i]);
                        Canvas.SetTop(FlatImgs[i], top - 20);
                        Canvas.SetLeft(FlatImgs[i], left - 22 + 13);
                        left += 13;
                        break;
                    case 2:
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i]);
                        Canvas.SetTop(DoubleSharpImgs[i], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i], left - 25 + 13);
                        left += 13;
                        break;
                    case -2:
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i]);
                        Canvas.SetTop(DoubleFlatImgs[i], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i], left - 32 + 16);
                        left += 16;
                        break;
                    case 3:
                        CVMusicSheet.Children.Add(SharpImgs[i]);
                        Canvas.SetTop(SharpImgs[i], top - 20);
                        Canvas.SetLeft(SharpImgs[i], left - 22 - 19 - 5 + 26);
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i]);
                        Canvas.SetTop(DoubleSharpImgs[i], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i], left - 25 + 26);
                        left += 26;
                        break;
                    case -3:
                        CVMusicSheet.Children.Add(FlatImgs[i]);
                        Canvas.SetTop(FlatImgs[i], top - 20);
                        Canvas.SetLeft(FlatImgs[i], left - 22 - 28 - 5 + 29);
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i]);
                        Canvas.SetTop(DoubleFlatImgs[i], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i], left - 32 + 29);
                        left += 29;
                        break;
                    case 4:
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i]);
                        Canvas.SetTop(DoubleSharpImgs[i], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i], left - 25 - 19 - 5 + 26);
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i + 8]);
                        Canvas.SetTop(DoubleSharpImgs[i + 8], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i + 8], left - 25 + 26);
                        left += 26;
                        break;
                    case -4:
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i]);
                        Canvas.SetTop(DoubleFlatImgs[i], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i], left - 32 - 31 - 5 + 32);
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i + 8]);
                        Canvas.SetTop(DoubleFlatImgs[i + 8], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i + 8], left - 32 + 32);
                        left += 32;
                        break;
                    case 5:
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i]);
                        Canvas.SetTop(DoubleSharpImgs[i], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i], left - 25 - 19 - 5 + 63);
                        CVMusicSheet.Children.Add(DoubleSharpImgs[i + 8]);
                        Canvas.SetTop(DoubleSharpImgs[i + 8], top - 0);
                        Canvas.SetLeft(DoubleSharpImgs[i + 8], left - 25 + 63);
                        CVMusicSheet.Children.Add(SharpImgs[i]);
                        Canvas.SetTop(SharpImgs[i], top - 20);
                        Canvas.SetLeft(SharpImgs[i], left - 22 - 19 - 5 - 25 + 63);
                        left += 63;
                        break;
                    case -5:
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i]);
                        Canvas.SetTop(DoubleFlatImgs[i], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i], left - 32 - 31 - 5 + 63);
                        CVMusicSheet.Children.Add(DoubleFlatImgs[i + 8]);
                        Canvas.SetTop(DoubleFlatImgs[i + 8], top - 18);
                        Canvas.SetLeft(DoubleFlatImgs[i + 8], left - 32 + 63);
                        CVMusicSheet.Children.Add(FlatImgs[i]);
                        Canvas.SetTop(FlatImgs[i], top - 20);
                        Canvas.SetLeft(FlatImgs[i], left - 22 - 32 - 31 - 5 + 63);
                        left += 63;
                        break;
                    default:
                        break;
                }
                Canvas.SetLeft(WholeNoteImgs[i], left);                
                this.CVMusicSheet.Children.Add(WholeNoteImgs[i]);
                if (top < lineStart - LineGap) {
                    this.CVMusicSheet.Children.Add(CreateLine(left - 10, lineStart - LineGap, left + WholeNoteImgs[i].Width + 10, lineStart - LineGap));
                }
                else if (top > lineStart + LineGap * 4 + 5) {
                    this.CVMusicSheet.Children.Add(CreateLine(left - 10, lineStart + LineGap * 5, left + WholeNoteImgs[i].Width + 10, lineStart + LineGap * 5));
                }                
                FirstNotePos += 13;
                if (RBtnSortAscending.IsChecked == true) {
                    left += leftGap;
                }
                else if (RBtnSortDescending.IsChecked == true) {
                    left -= leftGap;
                }
            }
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
            int speed = Convert.ToInt32(TBSoundSpeed.Text);
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
            GClefImg.Source = gClefBtm;
            // ● F Clef
            BitmapImage fClefBtm = new BitmapImage();
            fClefBtm.BeginInit();
            fClefBtm.UriSource = new Uri("pack://application:,,,/assets/FClef.png");
            fClefBtm.EndInit();
            FClefImg.Source = fClefBtm;
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
            for (int i = 0; i < 8; i++) {
                if (SharpImgs[i] == null) {
                    SharpImgs[i] = new Image();
                }
                SharpImgs[i].Width = 19;
                Canvas.SetLeft(SharpImgs[i], 300);
                Canvas.SetTop(SharpImgs[i], 180);
                SharpImgs[i].Source = sharpBtm;
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
                FlatImgs[i].Width = 21;
                Canvas.SetLeft(FlatImgs[i], 300);
                Canvas.SetTop(FlatImgs[i], 180);
                FlatImgs[i].Source = flatBtm;
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
                DoubleSharpImgs[i].Width = 19;
                Canvas.SetLeft(DoubleSharpImgs[i], 300);
                Canvas.SetTop(DoubleSharpImgs[i], 180);
                DoubleSharpImgs[i].Source = doubleSharpBtm;
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
                DoubleFlatImgs[i].Width = 31;
                Canvas.SetLeft(DoubleFlatImgs[i], 300);
                Canvas.SetTop(DoubleFlatImgs[i], 180);
                DoubleFlatImgs[i].Source = doubleFlatBtm;
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
    }
}