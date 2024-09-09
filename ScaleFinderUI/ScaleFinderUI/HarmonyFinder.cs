using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace MusicPeanut {
    class HarmonyFinder {
        public enum Pitch : byte {
            C = 0, D = 2, E = 4, F = 5, G = 7, A = 9, B = 11
        }
        public enum Accidental : sbyte {
            Natural = 0, Sharp = 1, Flat = -1
        }
        public static class Type {
            public static class Scale {
                public readonly static byte[] MajorScale = new byte[] { 2, 2, 1, 2, 2, 2, 1 };
                public readonly static byte[] NaturalMinorScale = new byte[] { 2, 1, 2, 2, 1, 2, 2 };
                public readonly static byte[] HarmonicMinorScale = new byte[] { 2, 1, 2, 2, 1, 3, 1 };
                public readonly static byte[] MelodicMinorScale = new byte[] { 2, 1, 2, 2, 2, 2, 1 };
                public readonly static byte[] MajorPentatonicScale = new byte[] { 2, 2, 3, 2, 3 };
                public readonly static byte[] MinorPentatonicScale = new byte[] { 3, 2, 2, 3, 2 };
                public readonly static byte[] MinorBluesScale = new byte[] { 3, 2, 1, 1, 3, 2 };
                public readonly static byte[] MajorBluesScale = new byte[] { 2, 1, 1, 3, 2, 3 };
                public readonly static byte[] IonianMode = new byte[] { 2, 2, 1, 2, 2, 2, 1 };
                public readonly static byte[] DorianMode = new byte[] { 2, 1, 2, 2, 2, 1, 2 };
                public readonly static byte[] PhrygianMode = new byte[] { 1, 2, 2, 2, 1, 2, 2 };
                public readonly static byte[] LydianMode = new byte[] { 2, 2, 2, 1, 2, 2, 1 };
                public readonly static byte[] MixolydianMode = new byte[] { 2, 2, 1, 2, 2, 1, 2 };
                public readonly static byte[] AeolianMode = new byte[] { 2, 1, 2, 2, 1, 2, 2 };
                public readonly static byte[] LocrainMode = new byte[] { 1, 2, 2, 1, 2, 2, 2 };
                public readonly static byte[] AlteredScale = new byte[] { 1, 2, 1, 2, 2, 2, 2 };
                public readonly static byte[] EnigmaticScale = new byte[] { 1, 3, 2, 2, 2, 1, 1 };
                public readonly static byte[] DoubleHarmonicMajorScale = new byte[] { 1, 3, 1, 2, 1, 3, 1 };
                public readonly static byte[] HungarianMinorScale = new byte[] { 2, 1, 3, 1, 1, 3, 1 };
                public readonly static byte[] HungarianMajorScale = new byte[] { 3, 1, 2, 1, 2, 1, 2 };
                public readonly static byte[] NeapolitanMajorScale = new byte[] { 1, 2, 2, 2, 2, 2, 1 };
                public readonly static byte[] NeapolitanMinorScale = new byte[] { 1, 2, 2, 2, 1, 3, 1 };
                public readonly static byte[] HalfDiminishedScale = new byte[] { 2, 1, 2, 1, 2, 2, 2 };
                public readonly static byte[] AcousticScale = new byte[] { 2, 2, 2, 1, 2, 1, 2 };
                public readonly static byte[] LydianAugmentedScale = new byte[] { 2, 2, 2, 2, 1, 2, 1 };
                public readonly static byte[] LydianDiminishedScale = new byte[] { 2, 1, 3, 1, 2, 2, 1 };
                public readonly static byte[] PhrygianDominantScale = new byte[] { 1, 3, 1, 2, 1, 2, 2 };
                public readonly static byte[] UkrainianDorianScale = new byte[] { 2, 1, 3, 1, 2, 1, 2 };
                public readonly static byte[] PersianScale = new byte[] { 1, 3, 1, 1, 2, 3, 1 };
                public readonly static byte[] InScale = new byte[] { 1, 4, 2, 1, 4 };
                public readonly static byte[] InsenScale = new byte[] { 1, 4, 2, 3, 2 };
                public readonly static byte[] IwatoScale = new byte[] { 1, 4, 1, 4, 2 };
            }
            public static class Chord {
                public readonly static byte[] Major = new byte[] { 4, 3 };
                public readonly static byte[] Minor = new byte[] { 3, 4 };
                public readonly static byte[] Augmented = new byte[] { 4, 4 };
                public readonly static byte[] Diminished = new byte[] { 3, 3 };
                public readonly static byte[] DominantSeventh = new byte[] { 4, 3, 3 };
                public readonly static byte[] MajorSeventh = new byte[] { 4, 3, 4 };
                public readonly static byte[] MinorSeventh = new byte[] { 3, 4, 3 };
                public readonly static byte[] MinorMajorSeventh = new byte[] { 3, 4, 4 };
                public readonly static byte[] HalfDiminishedSeventh = new byte[] { 3, 3, 4 };
                public readonly static byte[] DiminishedSeventh = new byte[] { 3, 3, 3 };
                public readonly static byte[] AugmentedSeventh = new byte[] { 4, 4, 2 };
                public readonly static byte[] AugmentedMajorSeventh = new byte[] { 4, 4, 3 };
                public readonly static byte[] SuspendedSecond = new byte[] { 2, 5 };
                public readonly static byte[] SuspendedFourth = new byte[] { 5, 2 };
                public readonly static byte[] DominantSeventhSuspendedSecond = new byte[] { 2, 5, 3 };
                public readonly static byte[] DominantSeventhSuspendedFourth = new byte[] { 5, 2, 3 };
                public readonly static byte[] MajorSixth = new byte[] { 4, 3, 2 };
                public readonly static byte[] MinorSixth = new byte[] { 3, 4, 2 };
            }
            public static class Intervals {
                public readonly static byte[] PerfectUnison = new byte[] { 0 };
                public readonly static byte[] MinorSecond = new byte[] { 1 };
                public readonly static byte[] MajorSecond = new byte[] { 2 };
                public readonly static byte[] MinorThird = new byte[] { 3 };
                public readonly static byte[] MajorThird = new byte[] { 4 };
                public readonly static byte[] PerfectFourth = new byte[] { 5 };
                public readonly static byte[] Tritone = new byte[] { 6 };
                public readonly static byte[] PerfectFifth = new byte[] { 7 };
                public readonly static byte[] MinorSixth = new byte[] { 8 };
                public readonly static byte[] MajorSixth = new byte[] { 9 };
                public readonly static byte[] MinorSeventh = new byte[] { 10 };
                public readonly static byte[] MajorSeventh = new byte[] { 11 };
                public readonly static byte[] PerfectOctave = new byte[] { 12 };
            }
            public static class Note {
                public readonly static byte[] Single = new byte[] { 0 };
            }
        }
        public Harmony FindHarmony(byte pitch, sbyte accidental, byte[] type) {
            Harmony result = new Harmony();
            byte omitLastNote = 0;
            if (type == Type.Scale.AeolianMode || type == Type.Scale.DorianMode || type == Type.Scale.HarmonicMinorScale || type == Type.Scale.IonianMode || type == Type.Scale.LocrainMode || type == Type.Scale.LydianMode || type == Type.Scale.MajorPentatonicScale || type == Type.Scale.MajorScale || type == Type.Scale.MelodicMinorScale || type == Type.Scale.MinorPentatonicScale || type == Type.Scale.MixolydianMode || type == Type.Scale.NaturalMinorScale || type == Type.Scale.PhrygianMode || type == Type.Scale.MinorBluesScale || type == Type.Scale.MajorBluesScale || type == Type.Scale.AlteredScale || type == Type.Scale.EnigmaticScale || type == Type.Scale.DoubleHarmonicMajorScale || type == Type.Scale.HungarianMinorScale || type == Type.Scale.HungarianMajorScale || type == Type.Scale.NeapolitanMajorScale || type == Type.Scale.NeapolitanMinorScale || type == Type.Scale.HalfDiminishedScale || type == Type.Scale.AcousticScale || type == Type.Scale.LydianAugmentedScale || type == Type.Scale.LydianDiminishedScale || type == Type.Scale.PhrygianDominantScale || type == Type.Scale.UkrainianDorianScale || type == Type.Scale.PersianScale || type == Type.Scale.InScale || type == Type.Scale.InsenScale || type == Type.Scale.IwatoScale || type == Type.Note.Single) {
                omitLastNote = 1;
            }
            Dictionary<byte[], byte[]> pitchTypeDictionary = new Dictionary<byte[], byte[]>
            {
                { new byte[] { 0, 1, 2, 4, 5, 7 }, Type.Scale.MajorPentatonicScale },
                { new byte[] { 0, 1, 3, 4, 5, 7 }, Type.Scale.MinorPentatonicScale },
                { new byte[] { 0, 2, 3, 4, 4, 6, 7 }, Type.Scale.MinorBluesScale },
                { new byte[] { 0, 1, 2, 2, 4, 5, 7 }, Type.Scale.MajorBluesScale },
                { new byte[] { 0, 1, 3, 4, 5 }, Type.Scale.InScale },
                { new byte[] { 0, 1, 3, 4, 6 }, Type.Scale.InsenScale },
                { new byte[] { 0, 1, 3, 4, 6 }, Type.Scale.IwatoScale },
                { new byte[] { 0, 2, 4 }, Type.Chord.Major },
                { new byte[] { 0, 2, 4 }, Type.Chord.Minor },
                { new byte[] { 0, 2, 4 }, Type.Chord.Augmented },
                { new byte[] { 0, 2, 4 }, Type.Chord.Diminished },
                { new byte[] { 0, 2, 4, 6 }, Type.Chord.DominantSeventh },
                { new byte[] { 0, 2, 4, 6 }, Type.Chord.MinorSeventh },
                { new byte[] { 0, 2, 4, 6 }, Type.Chord.MajorSeventh },
                { new byte[] { 0, 2, 4, 6 }, Type.Chord.AugmentedSeventh },
                { new byte[] { 0, 2, 4, 6 }, Type.Chord.DiminishedSeventh },
                { new byte[] { 0, 2, 4, 6 }, Type.Chord.HalfDiminishedSeventh },
                { new byte[] { 0, 1, 4 }, Type.Chord.SuspendedSecond },
                { new byte[] { 0, 3, 4 }, Type.Chord.SuspendedFourth },
                { new byte[] { 0, 1, 4, 6 }, Type.Chord.DominantSeventhSuspendedSecond },
                { new byte[] { 0, 3, 4, 6 }, Type.Chord.DominantSeventhSuspendedFourth },
                { new byte[] { 0, 2, 4, 5 }, Type.Chord.MajorSixth },
                { new byte[] { 0, 2, 4, 5 }, Type.Chord.MinorSixth },
                { new byte[] { 0, 0 }, Type.Intervals.PerfectUnison },
                { new byte[] { 0, 1 }, Type.Intervals.MinorSecond },
                { new byte[] { 0, 1 }, Type.Intervals.MajorSecond },
                { new byte[] { 0, 2 }, Type.Intervals.MinorThird },
                { new byte[] { 0, 2 }, Type.Intervals.MajorThird },
                { new byte[] { 0, 3 }, Type.Intervals.PerfectFourth },
                { new byte[] { 0, 3 }, Type.Intervals.Tritone },
                { new byte[] { 0, 4 }, Type.Intervals.PerfectFifth },
                { new byte[] { 0, 5 }, Type.Intervals.MinorSixth },
                { new byte[] { 0, 5 }, Type.Intervals.MajorSixth },
                { new byte[] { 0, 6 }, Type.Intervals.MinorSeventh },
                { new byte[] { 0, 6 }, Type.Intervals.MajorSeventh },
                { new byte[] { 0, 7 }, Type.Intervals.PerfectOctave },
                { new byte[] { 0 }, Type.Note.Single },
            };
            byte[] pitchArray = pitchTypeDictionary.FirstOrDefault(x => x.Value.SequenceEqual(type)).Key;
            if (pitchArray == null) {
                pitchArray = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            }
            List<string> basePitchText = new List<string>();
            List<byte> basePitchNote = new List<byte>();
            string[] basePitchTextArray = new string[] { "C", "D", "E", "F", "G", "A", "B", "C", "D", "E", "F", "G", "A", "B", "C" };
            byte[] basePitchNoteArray = new byte[] { 0, 2, 4, 5, 7, 9, 11, 12, 14, 16, 17, 19, 21, 23, 24 };
            byte pitchNoteStart = (byte)Array.IndexOf(basePitchNoteArray, pitch);
            if (type != Type.Note.Single) {
                for (byte i = 0; i < pitchArray.Length; i++) {
                    byte pitchTextStart = (byte)(pitchArray[i] + pitchNoteStart);
                    basePitchText.Add(basePitchTextArray[pitchTextStart]);
                    basePitchNote.Add(basePitchNoteArray[pitchTextStart]);
                }
            } else if (type == Type.Note.Single) {
                for (byte i = 0; i < pitchArray.Length - 1; i++) {
                    byte pitchTextStart = (byte)(pitchArray[i] + pitchNoteStart);
                    basePitchText.Add(basePitchTextArray[pitchTextStart]);
                    basePitchNote.Add(basePitchNoteArray[pitchTextStart]);
                }
            }
            List<sbyte> pitchList = new List<sbyte>();
            List<sbyte> basePitchList = new List<sbyte>();
            sbyte startPitch = (sbyte)(pitch + accidental);
            sbyte startBasePitch = 0;
            pitchList.Add(startPitch);
            basePitchList.Add(startBasePitch);
            if (type != Type.Note.Single) {
                for (byte i = 0; i < type.Length; i++) {
                    startPitch += (sbyte)type[i];
                    startBasePitch += (sbyte)type[i];
                    pitchList.Add(startPitch);
                    basePitchList.Add(startBasePitch);
                }
            }
            List<sbyte> integerNotationList = new List<sbyte>();
            for (byte i = 0; i < pitchList.Count - omitLastNote; i++) {
                sbyte currentNotation = (sbyte)((pitchList[i] < 0) ? pitchList[i] + 12 : pitchList[i] % 12);
                integerNotationList.Add(currentNotation);
            }
            List<sbyte> accidentalList = new List<sbyte>();
            List<sbyte> baseAccidentalList = new List<sbyte>();
            for (byte i = 0; i < basePitchNote.Count; i++) {
                accidentalList.Add((sbyte)(pitchList[i] - basePitchNote[i]));
                baseAccidentalList.Add((sbyte)(basePitchList[i] - basePitchNoteArray[pitchArray[i]]));
            }
            byte[] filteredArray = pitchArray.ToArray();
            List<string> degreesNumberList = new List<string>();            
            for (byte i = 0; i < pitchArray.Length - omitLastNote; i++) {
                degreesNumberList.Add(pitchArray[i] + 1 + GetAccidentalSymbol(baseAccidentalList[i]));
            }
            List<string> resultPitch = basePitchText.Zip(accidentalList, (text, acc) => text + GetAccidentalSymbol(acc)).ToList();
            string selectedHarmony = resultPitch[0] + " " + GetHarmonyName(type);
            string selectedChordSymbol = resultPitch[0] + GetChordSymbol(type);
            List<string> stepsList = type.Select(GetStepSymbol).ToList();
            List<string> toneList = type.Select(GetToneSymbol).ToList();
            Console.WriteLine("#Base Pitch Text: " + String.Join(" ", basePitchText));
            Console.WriteLine("#Base Pitch Note: " + String.Join(" ", basePitchNote));
            Console.WriteLine("#Base Pitch: " + String.Join(" ", basePitchList));
            Console.WriteLine("#Base Accidental: " + String.Join(" ", baseAccidentalList));
            Console.WriteLine("Selected Scale: " + selectedHarmony);
            Console.WriteLine("Selected Chord Symbol: " + selectedChordSymbol);
            Console.WriteLine("Notes: " + String.Join(" ", resultPitch));
            Console.WriteLine("Accidental: " + String.Join(" ", accidentalList));
            Console.WriteLine("Pitch: " + String.Join(" ", pitchList));
            Console.WriteLine("Degrees: " + String.Join(" ", degreesNumberList));
            Console.WriteLine("Steps: " + String.Join("-", stepsList));
            Console.WriteLine("Tones: " + String.Join("-", toneList));
            Console.WriteLine("Integer Notation: " + String.Join(" ", integerNotationList));
            result.SelectedHarmony = selectedHarmony;
            result.SelectedChordSymbol = selectedChordSymbol;
            return result;
        }
        private string GetAccidentalSymbol(sbyte accidentalValue) {
            string singleAccidentalText = "";
            string doubleAccidentalText = "";
            sbyte singleAccidentalValue = (sbyte)(accidentalValue % 2);
            sbyte doubleAccidentalValue = (sbyte)(accidentalValue / 2);
            if (singleAccidentalValue != 0) {
                if (accidentalValue > 0) {
                    singleAccidentalText += "♯";
                } else if (accidentalValue < 0) {
                    singleAccidentalText += "♭";
                }
            }
            if (doubleAccidentalValue != 0) {
                if (accidentalValue > 0) {
                    for (byte i = 0; i < doubleAccidentalValue; i++) {
                        doubleAccidentalText += "𝄪";
                    }
                } else if (accidentalValue < 0) {
                    for (sbyte i = 0; i > doubleAccidentalValue; i--) {
                        doubleAccidentalText += "𝄫";
                    }
                }
            }
            return singleAccidentalText + doubleAccidentalText;
        }
        private string GetStepSymbol(byte stepValue) {
            if (stepValue == 1) {
                return "H";
            } else if (stepValue == 2) {
                return "W";
            } else if (stepValue % 2 == 1) {
                return stepValue + "H";
            } else if (stepValue % 2 == 0) {
                return (byte)(stepValue / 2) + "W";
            }
            return "";
        }
        private string GetToneSymbol(byte toneValue) {
            switch (toneValue) {
                case 0: return "P1";
                case 1: return "m2";
                case 2: return "M2";
                case 3: return "m3";
                case 4: return "M3";
                case 5: return "P4";
                case 6: return "TT";
                case 7: return "P5";
                case 8: return "m6";
                case 9: return "M6";
                case 10: return "m7";
                case 11: return "M7";
                case 12: return "P8";
                default: return "";
            }
        }
        private string GetHarmonyName(byte[] type) {
            Dictionary<string, byte[]> selectedHarmonyNameDictionary = new Dictionary<string, byte[]>
            {
                { "major scale", Type.Scale.MajorScale },
                { "natural minor scale", Type.Scale.NaturalMinorScale },
                { "harmonic minor scale", Type.Scale.HarmonicMinorScale },
                { "melodic minor scale", Type.Scale.MelodicMinorScale },
                { "ionian mode", Type.Scale.IonianMode },
                { "dorian mode", Type.Scale.DorianMode },
                { "phrygian mode", Type.Scale.PhrygianMode },
                { "lydian mode", Type.Scale.LydianMode },
                { "mixolydian mode", Type.Scale.MixolydianMode },
                { "aeolian mode", Type.Scale.AeolianMode },
                { "locrian mode", Type.Scale.LocrainMode },
                { "major pentatonic scale", Type.Scale.MajorPentatonicScale },
                { "minor pentatonic scale", Type.Scale.MinorPentatonicScale },
                { "minor blues scale", Type.Scale.MinorBluesScale },
                { "major blues scale", Type.Scale.MajorBluesScale },
                { "altered scale", Type.Scale.AlteredScale },
                { "enigmatic scale", Type.Scale.EnigmaticScale },
                { "double harmonic major scale", Type.Scale.DoubleHarmonicMajorScale },
                { "hungarian minor scale", Type.Scale.HungarianMinorScale },
                { "hungarian major scale", Type.Scale.HungarianMajorScale },
                { "neapolitan major scale", Type.Scale.NeapolitanMajorScale },
                { "neapolitan minor scale", Type.Scale.NeapolitanMinorScale },
                { "half diminished scale", Type.Scale.HalfDiminishedScale },
                { "acoustic scale", Type.Scale.AcousticScale },
                { "lydian augmented scale", Type.Scale.LydianAugmentedScale },
                { "lydian diminished scale", Type.Scale.LydianDiminishedScale },
                { "phrygian dominant scale", Type.Scale.PhrygianDominantScale },
                { "ukrainian dorian scale", Type.Scale.UkrainianDorianScale },
                { "persian scale", Type.Scale.PersianScale },
                { "in scale", Type.Scale.InScale },
                { "insen scale", Type.Scale.InsenScale },
                { "iwato scale", Type.Scale.IwatoScale },
                { "augmented chord", Type.Chord.Augmented },
                { "augmented major seventh chord", Type.Chord.AugmentedMajorSeventh },
                { "augmented seventh chord", Type.Chord.AugmentedSeventh },
                { "diminished chord", Type.Chord.Diminished },
                { "diminished seventh chord", Type.Chord.DiminishedSeventh },
                { "dominant seventh chord", Type.Chord.DominantSeventh },
                { "dominant seventh suspended fourth chord", Type.Chord.DominantSeventhSuspendedFourth },
                { "dominant seventh suspended second chord", Type.Chord.DominantSeventhSuspendedSecond },
                { "half diminished seventh chord", Type.Chord.HalfDiminishedSeventh },
                { "major chord", Type.Chord.Major  },
                { "major seventh chord", Type.Chord.MajorSeventh  },
                { "major sixth chord", Type.Chord.MajorSixth },
                { "minor chord", Type.Chord.Minor },
                { "minor major seventh chord", Type.Chord.MinorMajorSeventh },
                { "minor seventh chord", Type.Chord.MinorSeventh },
                { "minor sixth chord", Type.Chord.MinorSixth },
                { "suspended fourth chord", Type.Chord.SuspendedFourth },
                { "suspended second chord", Type.Chord.SuspendedSecond },
                { "perfect unison", Type.Intervals.PerfectUnison },
                { "minor second", Type.Intervals.MajorSecond },
                { "major second", Type.Intervals.MinorSecond },
                { "minor third", Type.Intervals.MinorThird },
                { "perfect fourth", Type.Intervals.PerfectFourth },
                { "tritone", Type.Intervals.Tritone },
                { "perfect fifth", Type.Intervals.PerfectFifth },
                { "minor sixth", Type.Intervals.MinorSixth },
                { "major sixth", Type.Intervals.MajorSixth },
                { "minor seventh", Type.Intervals.MinorSeventh },
                { "major seventh", Type.Intervals.MajorSeventh },
                { "perfect octave", Type.Intervals.PerfectOctave },
                { "", Type.Note.Single },
            };
            return selectedHarmonyNameDictionary.FirstOrDefault(x => x.Value.SequenceEqual(type)).Key ?? "unknown";
        }
        private string GetChordSymbol(byte[] type) {
            Dictionary<string, byte[]> selectedChordSymbolDictionary = new Dictionary<string, byte[]>
            {
                { "+", Type.Chord.Augmented },
                { "+M7", Type.Chord.AugmentedMajorSeventh },
                { "+7", Type.Chord.AugmentedSeventh },
                { "°", Type.Chord.Diminished },
                { "°7", Type.Chord.DiminishedSeventh },
                { "7", Type.Chord.DominantSeventh },
                { "7sus4", Type.Chord.DominantSeventhSuspendedFourth },
                { "7sus2", Type.Chord.DominantSeventhSuspendedSecond },
                { "ø7", Type.Chord.HalfDiminishedSeventh },
                { "", Type.Chord.Major },
                { "M7", Type.Chord.MajorSeventh },
                { "6", Type.Chord.MajorSixth },
                { "m", Type.Chord.Minor },
                { "mM7", Type.Chord.MinorMajorSeventh },
                { "m7", Type.Chord.MinorSeventh },
                { "m6", Type.Chord.MinorSixth },
                { "sus4", Type.Chord.SuspendedFourth },
                { "sus2", Type.Chord.SuspendedSecond },
            };
            return selectedChordSymbolDictionary.FirstOrDefault(x => x.Value.SequenceEqual(type)).Key ?? "unknown";
        }
    }
}