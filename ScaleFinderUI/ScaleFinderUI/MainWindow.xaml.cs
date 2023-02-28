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
            if (tb == null) {
                return;
            }
            if ((bool)RBtnBaseC.IsChecked) {
                Scale result = finder.FindScale(ScaleFinder.PitchC, ScaleFinder.AccidNatural, ScaleFinder.ModeLocrain);

                if (!result.GetFound()) {
                    Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                    return;
                }
                result.PrintMyValues();
            }
            else if ((bool)RBtnBaseD.IsChecked) {
                Scale result = finder.FindScale(ScaleFinder.PitchD, ScaleFinder.AccidNatural, ScaleFinder.ModeLocrain);

                if (!result.GetFound()) {
                    Debug.WriteLine("Error.. I cannot find your scale. sigh....");
                    return;
                }
                result.PrintMyValues();
            }
            
            
        }
    }
}
