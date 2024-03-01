using MaterialDesignThemes.Wpf.Internal;
using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleFinderUI {
    class MidiPlayItem {
        public List<int> PitchList { get; set; }
        public int Octave { get; set; }
        public int Volume { get; set; }
        
        public MidiPlayItem() {
            PitchList = new List<int>();
            Octave = 0;
            Volume = 32;
        }
        public void Clear() {
            Debug.WriteLine(">>>>>>>>>>>>> MidiPlayItem Clear");
            PitchList.Clear();
            Octave = 0;
        }
    }
}