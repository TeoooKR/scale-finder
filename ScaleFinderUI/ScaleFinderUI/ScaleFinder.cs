/*
MIT License
Copyright(c) 2023 Teo Han [meteory.kr@gmail.com]
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
SOFTWARE.
*/
namespace ScaleFinderUI {
    class ScaleFinder {
        //------ Bass PITCH ------
        readonly private int[] BasePitchList = new int[13] { 1, 3, 5, 6, 8, 10, 12, 13, 15, 17, 18, 20, 22 };
        readonly private string[] BasePitchTextList = new string[7] { "C", "D", "E", "F", "G", "A", "B" };
        //------ PITCH ------
        readonly public static int PitchC = 1;
        readonly public static int PitchD = 3;
        readonly public static int PitchE = 5;
        readonly public static int PitchF = 6;
        readonly public static int PitchG = 8;
        readonly public static int PitchA = 10;
        readonly public static int PitchB = 12;
        //------ ACCIDENTAL ------
        readonly public static int AccidNatural = 0;
        readonly public static int AccidSharp = 1;
        readonly public static int AccidFlat = -1;
        //------ INTERVALS -------
        readonly public static int[] IntervalMajorScale = new int[6] { 2, 2, 1, 2, 2, 2 };
        readonly public static int[] IntervalNaturalMinorScale = new int[6] { 2, 1, 2, 2, 1, 2 };
        readonly public static int[] IntervalHarmonicMinorScale = new int[6] { 2, 1, 2, 2, 1, 3 };
        readonly public static int[] IntervalMelodicMinorScale = new int[6] { 2, 1, 2, 2, 2, 2 };
        readonly public static int[] IntervalIonianMode = new int[6] { 2, 2, 1, 2, 2, 2 };
        readonly public static int[] IntervalDorianMode = new int[6] { 2, 1, 2, 2, 2, 1 };
        readonly public static int[] IntervalPhtygianMode = new int[6] { 1, 2, 2, 2, 1, 2 };
        readonly public static int[] IntervalLydianMode = new int[6] { 2, 2, 2, 1, 2, 2 };
        readonly public static int[] IntervalMixolydianMode = new int[6] { 2, 2, 1, 2, 2, 1 };
        readonly public static int[] IntervalAeolianMode = new int[6] { 2, 1, 2, 2, 1, 2 };
        readonly public static int[] IntervalLocrainMode = new int[6] { 1, 2, 2, 1, 2, 2 };
        public Scale FindScale(int basePitch, int accidental, int[] intervals) {
            Scale result = new Scale();
            int[] accidentalList = result.GetAccidentalList();
            int[] pitchList = result.GetPitchList();
            int startPitch = basePitch + accidental;
            string[] pitchTexts = new string[7] { "", "", "", "", "", "", "" };
            GetPitchListByInterval(pitchList, startPitch, intervals);
            for (int i = 0; i < BasePitchList.Length; i++) {
                if (basePitch == BasePitchList[i]) {
                    CalcAccidentalFromBass(accidentalList, i, pitchList);
                    pitchTexts = GetPitchTextByOrder(BasePitchTextList, i);
                    break;
                }
            }
            string[] accidentalTexts = GetSignFromAccidental(accidentalList);
            string[] pitchTextResults = result.GetPitchTexts();
            int[] intervalResults = result.GetIntervalsList();
            for (int i = 0; i < accidentalList.Length - 1; i++) {
                pitchTextResults[i] = pitchTexts[i] + accidentalTexts[i];
            }
            pitchTextResults[7] = pitchTextResults[0];
            for (int i = 0; i < intervalResults.Length - 1; i++) {
                intervalResults[i] = intervals[i];
            }
            intervalResults[6] = pitchList[7] - pitchList[6];
            result.SetPitchTexts(pitchTextResults);
            result.SetAccidentalTexts(accidentalTexts);
            result.SetPitchList(pitchList);
            result.SetAccidentalList(accidentalList);
            result.SetIntervalsList(intervalResults);
            result.SetFound(true);
            return result;
        }
        private void GetPitchListByInterval(int[] result, int startPitch, int[] interval) {
            result[0] = startPitch;
            for (int i = 0; i < interval.Length; i++) {
                result[i + 1] = result[i] + interval[i];
            }
            result[7] = result[0] + 12;
        }
        private void CalcAccidentalFromBass(int[] accidental, int startPitchIndex, int[] pitchList) {
            int index = 0;
            for (int i = 0; i < pitchList.Length - 1; i++) {
                index = i + startPitchIndex;
                accidental[i] = pitchList[i] - BasePitchList[index];
            }
            accidental[7] = accidental[0];
        }
        private string[] GetPitchTextByOrder(string[] BasePitchTextList, int BassPitchIndex) {
            string[] result = new string[7] { "", "", "", "", "", "", "" };
            for (int i = 0; i < BasePitchTextList.Length; i++) {
                int j = i + BassPitchIndex;
                if (j > 6) {
                    j = j % 7;
                }
                result[i] = BasePitchTextList[j];
            }
            return result;
        }
        private string[] GetSignFromAccidental(int[] accidentalList) {
            string[] accidentalText = new string[8] { "", "", "", "", "", "", "", "" };
            int[] accid = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int[] doubleAccid = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            string[] accidText = new string[7] { "", "", "", "", "", "", "" };
            string[] doubleAccidText = new string[7] { "", "", "", "", "", "", "" };
            for (int i = 0; i < accidentalList.Length - 1; i++) {
                accid[i] = accidentalList[i] % 2;
                doubleAccid[i] = accidentalList[i] / 2;
                if (accid[i] != 0) {
                    switch (accidentalList[i]) {
                        case > 0:
                            accidText[i] += "♯";
                            break;
                        case < 0:
                            accidText[i] += "♭";
                            break;
                        default:
                            break;
                    }
                }
                if (doubleAccid[i] != 0) {
                    switch (accidentalList[i]) {
                        case > 0:
                            for (int j = 0; j < doubleAccid[i]; j++) {
                                doubleAccidText[i] += "𝄪";
                            }
                            break;
                        case < 0:
                            for (int j = 0; j > doubleAccid[i]; j--) {
                                doubleAccidText[i] += "𝄫";
                            }
                            break;
                        default:
                            break;
                    }
                }
                accidentalText[i] = accidText[i] + doubleAccidText[i];
            }
            accidentalText[7] = accidentalText[0];
            return accidentalText;
        }
    }
}