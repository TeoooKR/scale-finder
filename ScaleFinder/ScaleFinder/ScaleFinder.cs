namespace ScaleFinder {
    class ScaleFinder {
        //------ TYPE ------
        public const int ModeMajor = 1;
        public const int ModeMinor = 6;
        public const int ModeIonian = 1;
        public const int ModeDorian = 2;
        public const int ModePhtygian = 3;
        public const int ModeLydian = 4;
        public const int ModeMixolydian = 5;
        public const int ModeAeolian = 6;
        public const int ModeLocrain = 7;

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

        //------ Bass PITCH ------
        readonly private int[] BasePitchList = new int[13] { 1, 3, 5, 6, 8, 10, 12, 13, 15, 17, 18, 20, 22 };
        readonly private string[] BasePitchTextList = new string[7] { "C", "D", "E", "F", "G", "A", "B" };

        //------ MODE's Interval -------
        readonly private int[] IntervalIonian = new int[6] { 2, 2, 1, 2, 2, 2 };
        readonly private int[] IntervalDorian = new int[6] { 2, 1, 2, 2, 2, 1 };
        readonly private int[] IntervalPhtygian = new int[6] { 1, 2, 2, 2, 1, 2 };
        readonly private int[] IntervalLydian = new int[6] { 2, 2, 2, 1, 2, 2 };
        readonly private int[] IntervalMixolydian = new int[6] { 2, 2, 1, 2, 2, 1 };
        readonly private int[] IntervalAeolian = new int[6] { 2, 1, 2, 2, 1, 2 };
        readonly private int[] IntervalLocrain = new int[6] { 2, 1, 2, 1, 2, 2 };

        public int FindScale(int basePitch, int accidental, int type) {
            int[] pitchList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int[] accidentalList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int startPitch = basePitch + accidental;
            string[] pitchTexts = new string[7] { "", "", "", "", "", "", "" };
            string[] accidentalTexts = new string[7] { "", "", "", "", "", "", "" };

            if (type == ModeMajor || type == ModeIonian) {
                GetPitchListByInterval(pitchList, startPitch, IntervalIonian);
            }
            else if (type == ModeDorian) {
                GetPitchListByInterval(pitchList, startPitch, IntervalDorian);
            }
            else if (type == ModePhtygian) {
                GetPitchListByInterval(pitchList, startPitch, IntervalPhtygian);
            }
            else if (type == ModeLydian) {
                GetPitchListByInterval(pitchList, startPitch, IntervalLydian);
            }
            else if (type == ModeMixolydian) {
                GetPitchListByInterval(pitchList, startPitch, IntervalMixolydian);
            }
            else if (type == ModeAeolian || type == ModeMinor) {
                GetPitchListByInterval(pitchList, startPitch, IntervalAeolian);
            }
            else if (type == ModeLocrain) {
                GetPitchListByInterval(pitchList, startPitch, IntervalLocrain);
            }

            for (int i = 0; i < BasePitchList.Length; i++) {
                if (basePitch == BasePitchList[i]) {
                    CalcAccidentalFromBass(accidentalList, i, pitchList);
                    pitchTexts = GetPitchTextByOrder(BasePitchTextList, i);
                    break;
                }
            }

            accidentalTexts = GetSignFromAccidental(accidentalList);

            for (int i = 0; i < accidentalList.Length; i++) {
                Console.WriteLine("@@------ " + pitchTexts[i] + accidentalTexts[i] + " ------@@");
            }
            int result = startPitch;
            Console.WriteLine("I Found Scale!!!!!!!!!!!!!!!!");
            return result;
        }

        private void GetPitchListByInterval(int[] result, int startPitch, int[] interval) {
            result[0] = startPitch;
            for (int i = 0; i < interval.Length; i++) {
                result[i + 1] = result[i] + interval[i];
            }
        }

        private void CalcAccidentalFromBass(int[] accidental, int startPitchIndex, int[] pitchList) {
            //> pitch_order_index [0~6]
            int index = 0;
            for (int i = 0; i < pitchList.Length; i++) {
                index = i + startPitchIndex;
                accidental[i] = pitchList[i] - BasePitchList[index];
            }
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
            string[] accidentalText = new string[7] { "", "", "", "", "", "", "" };
            int[] accid = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int[] doubleAccid = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            string[] accidText = new string[7] { "", "", "", "", "", "", "" };
            string[] doubleAccidText = new string[7] { "", "", "", "", "", "", "" };
            for (int i = 0; i < accidentalList.Length; i++) {
                if (accidentalList[i] > 0) {                                          //> Sharp
                    accid[i] = accidentalList[i] % 2;
                    doubleAccid[i] = accidentalList[i] / 2;
                    if (accid[i] > 0) {
                        accidText[i] = "#";
                    }
                    if (doubleAccid[i] > 0) {
                        for (int j = 0; j < doubleAccid[i]; j++) {
                            doubleAccidText[i] = doubleAccidText[i] + "x";
                        }
                    }
                    accidentalText[i] = accidText[i] + doubleAccidText[i];
                }
                else if (accidentalList[i] < 0) {                                      //> Flat
                    int count = Math.Abs(accidentalList[i]);
                    for (int j = 0; j < count; j++) {
                        accidentalText[i] = accidentalText[i] + "b";
                    }
                }
            }
            return accidentalText;
        }
    }
}





