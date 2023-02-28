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

namespace ScaleFinderUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        static ScaleFinder finder = new();
        public MainWindow() {
            InitializeComponent();
        }

        private void HandleBasePitchChecked(object sender, RoutedEventArgs e) {
            RadioButton tb = sender as RadioButton;
            int basePitch = 0;
            
            if (tb == null) {
                return;
            }
            if ((bool)RBtnBaseC.IsChecked) {
                basePitch = ScaleFinder.PitchC;
            }
            else if ((bool)RBtnBaseD.IsChecked) {
                basePitch = ScaleFinder.PitchD;
            }

            TBSelectedScale.Text = (string)tb.Content;

            Scale result = finder.FindScale(basePitch, ScaleFinder.AccidNatural, ScaleFinder.ModeMajor);

            if (!result.GetFound()) {
                Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                return;
            }
            result.PrintMyValues();

        }
    }
}
