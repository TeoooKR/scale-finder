﻿using System;
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

namespace ScaleFinderUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int BasePitch = ScaleFinder.PitchC;
        private int Accid = ScaleFinder.AccidNatural;
        private int Type = ScaleFinder.TypeMajorScale;
        private string BasePitchText = "C";
        private string AccidText = String.Empty;
        private string TypeText = " Major";
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
            BasePitchText = (string)tb.Content;
            TBSelectedScale.Text = BasePitchText + AccidText + TypeText;
            Scale result = Finder.FindScale(BasePitch, Accid, Type);
            if (!result.GetFound()) {
                Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                return;
            }
            UpdateScaleResult(result.GetPitchTexts());
            result.PrintMyValues();
        }
        // Accidental
        private void HandleAccidChecked(object sender, RoutedEventArgs e) {
            RadioButton rb = sender as RadioButton;
            if (rb == null) {
                return;
            }
            if ((bool)RBtnAccidN.IsChecked) {
                Accid = ScaleFinder.AccidNatural;
                AccidText = "";
            }
            else if ((bool)RBtnAccidS.IsChecked) {
                Accid = ScaleFinder.AccidSharp;
                AccidText = "#";
            }
            else if ((bool)RBtnAccidF.IsChecked) {
                Accid = ScaleFinder.AccidFlat;
                AccidText = "♭";
            }

            if (TBSelectedScale == null) {
                return;
            }
            TBSelectedScale.Text = BasePitchText + AccidText + TypeText;
            Scale result = Finder.FindScale(BasePitch, Accid, Type);

            if (!result.GetFound()) {
                Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                return;
            }
            UpdateScaleResult(result.GetPitchTexts());
            result.PrintMyValues();
        }
        // Type
        private void HandleTypeChecked(object sender, RoutedEventArgs e) {
            RadioButton rb = sender as RadioButton;
            if (rb == null) {
                return;
            }
            if ((bool)RBtnTypeMajor.IsChecked) {
                Type = ScaleFinder.TypeMajorScale;
                TypeText = " Major Scale";
            }
            else if ((bool)RBtnTypeMinor.IsChecked) {
                Type = ScaleFinder.TypeNaturalMinorScale;
                TypeText = " Natural Minor Scale";
            }
            else if ((bool)RBtnTypeIonian.IsChecked) {
                Type = ScaleFinder.TypeIonianMode;
                TypeText = " Ionian Mode";
            }
            else if ((bool)RBtnTypeDorian.IsChecked) {
                Type = ScaleFinder.TypeDorianMode;
                TypeText = " Dorian Mode";
            }
            else if ((bool)RBtnTypePhtygian.IsChecked) {
                Type = ScaleFinder.TypePhtygianMode;
                TypeText = " Phtygian Mode";
            }
            else if ((bool)RBtnTypeLydian.IsChecked) {
                Type = ScaleFinder.TypeLydianMode;
                TypeText = " Lydian Mode";
            }
            else if ((bool)RBtnTypeMixolydian.IsChecked) {
                Type = ScaleFinder.TypeMixolydianMode;
                TypeText = " Mixolydian Mode";
            }
            else if ((bool)RBtnTypeAeolian.IsChecked) {
                Type = ScaleFinder.TypeAeolianMode;
                TypeText = " Aeolian Mode";
            }
            else if ((bool)RBtnTypeLocrain.IsChecked) {
                Type = ScaleFinder.TypeLocrainMode;
                TypeText = " Locrain Mode";
            }
            TBSelectedScale.Text = BasePitchText + AccidText + TypeText;
            Scale result = Finder.FindScale(BasePitch, Accid, Type);

            if (!result.GetFound()) {
                Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                return;
            }
            UpdateScaleResult(result.GetPitchTexts());
            result.PrintMyValues();
        }
        private void UpdateScaleResult(string[] texts) {
            string resultText = "";
            for (int i = 0; i < texts.Length; i++) {
                resultText += texts[i] + " ";
            }
            TBScaleResult.Text = "Notes: " + resultText;
        }
    }
}
