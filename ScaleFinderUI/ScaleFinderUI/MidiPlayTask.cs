using NAudio.Gui;
using NAudio.Midi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Threading;
namespace ScaleFinderUI {
    class MidiPlayTask {
        //> Midi
        static MidiOut MidiOut = new MidiOut(0);
        public static MidiPlayItem MidiItem = new MidiPlayItem();
        private static Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        static bool newToPlay = false;
        //  static List<MidiEvent> events;
        public static void PlayMidiTask() {
            if (MidiOut == null || MidiItem.PitchList == null) {
                return;
            }
            while (true) {
                newToPlay = false;
                dispatcher.Invoke(() => {
                    if (MainWindow.IsChanged()) {
                        GetData();
                    }
                });                
                if (MidiItem.PitchList.Count < 1) {
                    Thread.Sleep(1);
                    continue;
                }
                newToPlay = false;
                Debug.WriteLine(">>>>>>>>>>>> Pitch Count" + MidiItem.PitchList.Count);
                PlayMidi();
                if (newToPlay == true) {
                    continue;
                }
                MidiItem.Clear();
            }            
        }
        private static void PlayMidi() {
            for (int i = 0; i < MidiItem.PitchList.Count; i++) {
                if (MainWindow.IsChanged()) {
                    GetData();
                    break;
                }
                MainWindow.CurrentPlayingNote(i);
                int pitchToPlay = MidiItem.PitchList[i] + 59 + MidiItem.Octave * 12;
                NoteOnEvent noteOnEvent = new NoteOnEvent(0, 2, pitchToPlay, MidiItem.Volume, 1);
                MidiOut.Send(noteOnEvent.GetAsShortMessage());
                Thread.Sleep(175);
                MidiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
                Thread.Sleep(1);
            }
        }
        private static void GetData() {
            MainWindow.SetPitchesToPlay();
            MainWindow.SetChanged(false);
            newToPlay = true;
        }
    }
}