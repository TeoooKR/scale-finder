using NAudio.Midi;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
namespace ScaleFinderUI {
    class MidiPlayTask {
        //> Midi
        private MidiOut MidiOut = new MidiOut(0);
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private bool newToPlay = false;
        public MidiPlayItem ?MidiItem { get; set; }
        private object objLock = new object();

        //  static List<MidiEvent> events;
        public MidiPlayTask() {
            MidiItem = new MidiPlayItem();
        }
        public void PlayMidiTask() {
            if (MidiOut == null || MidiItem.PitchList == null) {
                return;
            }
            while (true) {
                newToPlay = false;
                if (MainWindow.IsChanged()) {
                    dispatcher.Invoke(() => {
                        GetData();
                    });
                }
                if (MidiItem.PitchList.Count < 1) {
                    Thread.Sleep(1);
                    continue;
                }
                newToPlay = false;
                Debug.WriteLine(">>>>>>>>>>>> Pitch Count" + MidiItem.PitchList.Count);
                PlayMidi();                
                dispatcher.Invoke(() => {
                    lock (objLock) {
                        var mw = ((MainWindow)Application.Current.MainWindow);
                        if (mw != null)
                            mw.Dispatcher.Invoke(() => {                                
                                mw.ChangeRecVisibilityCollapsed();
                            });
                    }
                });
                if (newToPlay == true) {
                    continue;
                }
                MidiItem.Clear();
            }            
        }
        private void PlayMidi() {
            for (byte i = 0; i < MidiItem.PitchList.Count; i++) {
                if (MainWindow.IsChanged()) {
                    dispatcher.Invoke(() => {
                        GetData();
                    });
                    break;
                }

                Debug.WriteLine(">>>>>>>>>>>> IsChanged 3");

                //TODO Update Canvas Rectagle
                dispatcher.Invoke(() => {
                    lock (objLock) {
                        var mw = ((MainWindow)Application.Current.MainWindow);
                        if (mw != null)
                            mw.Dispatcher.Invoke(() => {
                                mw.CurrentPlayingNote(i);
                            });
                    }
                });



                // ▣ byte
                int pitchToPlay = MidiItem.PitchList[i] + 59 + MidiItem.Octave * 12;
                NoteOnEvent noteOnEvent = new NoteOnEvent(0, 2, pitchToPlay, MidiItem.Volume, 1);
                MidiOut.Send(noteOnEvent.GetAsShortMessage());
                Thread.Sleep(175);
                MidiOut.Send(noteOnEvent.OffEvent.GetAsShortMessage());
                Thread.Sleep(1);
                Debug.WriteLine(">>>>>>>>>>>> IsChanged 4");
            }
        }
        private void GetData() {
            lock(objLock) {
                var mw = ((MainWindow)Application.Current.MainWindow);
                if (mw != null)
                    mw.Dispatcher.Invoke(() => {
                        mw.SetPitchesToPlay();
                        mw.SetChanged(false);
                    });
            }
            newToPlay = true;
        }
    }
}