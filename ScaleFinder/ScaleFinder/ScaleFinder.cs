namespace ScaleFinder
{
    class ScaleFinder
    {
        //------ TYPE ------
        public const int type_MAJOR = 1;
        public const int type_MINOR = 6;
        public const int type_IONIAN = 1;
        public const int type_DORIAN = 2;
        public const int type_PHTYGIAN = 3;
        public const int type_LYDIAN = 4;
        public const int type_MIXOLYDIAN = 5;
        public const int type_AEOLIAN = 6;
        public const int type_LOCRAIN = 7;

        //------ PITCH ------
        readonly public static int pitch_C = 1;
        readonly public static int pitch_D = 3;
        readonly public static int pitch_E = 5;
        readonly public static int pitch_F = 6;
        readonly public static int pitch_G = 8;
        readonly public static int pitch_A = 10;
        readonly public static int pitch_B = 12;

        //------ ACCIDENTAL ------
        readonly public static int accid_NATURAL = 0;
        readonly public static int accid_SHARP = 1;
        readonly public static int accid_FLAT = -1;

        readonly private int[] BasePitchList = new int[13] { 1, 3, 5, 6, 8, 10, 12, 13, 15, 17, 18, 20, 22 };
        readonly private string[] BasePitchTextList = new string[7] { "C", "D", "E", "F", "G", "A", "B" };

        //------ MODE's Interval -------
        readonly private int[] ionian_interval = new int[6] { 2, 2, 1, 2, 2, 2 };
        readonly private int[] dorian_interval = new int[6] { 2, 1, 2, 2, 2, 1 };
        readonly private int[] phtygian_interval = new int[6] { 1, 2, 2, 2, 1, 2 };
        readonly private int[] lydian_interval = new int[6] { 2, 2, 2, 1, 2, 2 };
        readonly private int[] mixolydian_interval = new int[6] { 2, 2, 1, 2, 2, 1 };
        readonly private int[] aeolian_interval = new int[6] { 2, 1, 2, 2, 1, 2 };
        readonly private int[] locrain_interval = new int[6] { 2, 1, 2, 1, 2, 2 };

        public int FindScale(int basePitch, int accidental, int type)
        {
            int[] pitchList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int[] accidentalList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int startPitch = basePitch + accidental;
            string[] sub_sc = new string[7] { "", "", "", "", "", "", "" };

            if (type == type_MAJOR || type == type_IONIAN)
            {
                GetPitchListByInterval(pitchList, startPitch, ionian_interval);
            }
            else if (type == type_DORIAN)
            {
                GetPitchListByInterval(pitchList, startPitch, dorian_interval);
            }
            else if (type == type_PHTYGIAN)
            {
                GetPitchListByInterval(pitchList, startPitch, phtygian_interval);
            }
            else if (type == type_LYDIAN)
            {
                GetPitchListByInterval(pitchList, startPitch, lydian_interval);
            }
            else if (type == type_MIXOLYDIAN)
            {
                GetPitchListByInterval(pitchList, startPitch, mixolydian_interval);
            }
            else if (type == type_AEOLIAN || type == type_MINOR)
            {
                GetPitchListByInterval(pitchList, startPitch, aeolian_interval);
            }
            else if (type == type_LOCRAIN)
            {
                GetPitchListByInterval(pitchList, startPitch, locrain_interval);
            }

            for (int i = 0; i < BasePitchList.Length; i++)
            {
                if (basePitch == BasePitchList[i])
                {
                    CalcAccidentalFromBass(accidentalList, i, pitchList);
                    sub_sc = GetbassPitchOrder(BasePitchTextList, i);
                    //Console.WriteLine("\nIndex\n---- " + i + " ----");
                    break;
                }
            }

            string[] sub = GetSignFromAccidental(accidentalList);

            for (int i = 0; i < accidentalList.Length; i++)
            {
                Console.WriteLine("@@------ " + sub_sc[i] + sub[i] + " ------@@");
            }


            //for (int i = 0; i < accidental_index.Length; i++)
            //{}

            int result = startPitch;
            Console.WriteLine("I Found Scale!!!!!!!!!!!!!!!!");
            return result;
        }

        private void GetPitchListByInterval(int[] result, int startPitch, int[] interval)
        {
            result[0] = startPitch;
            //Console.WriteLine("index = " + result[0]);
            for (int i = 0; i < interval.Length; i++)
            {
                result[i + 1] = result[i] + interval[i];
                //Console.WriteLine("index = " + result[i + 1]);
            }
        }

        private void CalcAccidentalFromBass(int[] accidental, int startPitchIndex, int[] pitchList)
        {
            //Console.WriteLine("\nCalcAccidentalFromBass");
            //> pitch_order_index [0~6]
            int index = 0;
            for (int i = 0; i < pitchList.Length; i++)
            {
                index = i + startPitchIndex;
                //Console.WriteLine("---- " + i + " ---- " + pitchList[i] + " / " + BasePitchList[index]);
                accidental[i] = pitchList[i] - BasePitchList[index];
                //Console.WriteLine(accidental[i]);
            }
        }

        private string[] GetbassPitchOrder(string[] BassPitchTextList, int BassPitchIndex)
        {
            string[] result = new string[7] { "", "", "", "", "", "", "" };
            for (int i = 0; i < BassPitchTextList.Length; i++)
            {
                int j = i + BassPitchIndex;
                if (j > 6)
                {
                    j = j % 7;
                }
                result[i] = BassPitchTextList[j];
            }
            return result;
        }
        private string[] GetSignFromAccidental(int[] accidentalList)
        {
            //Console.WriteLine("\n#x");
            string[] accidentalString = new string[7] { "", "", "", "", "", "", "" };
            int[] Accid = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int[] doubleAccid = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            string[] Accid_str = new string[7] { "", "", "", "", "", "", "" };
            string[] doubleAccid_str = new string[7] { "", "", "", "", "", "", "" };
            for (int j = 0; j < accidentalList.Length; j++)
            {
                if (accidentalList[j] > 0)
                {
                    Accid[j] = accidentalList[j] % 2;
                    doubleAccid[j] = accidentalList[j] / 2;
                    if (Accid[j] > 0)
                    {
                        Accid_str[j] = "#";
                    }
                    if (doubleAccid[j] > 0)
                    {
                        for (int k = 0; k < doubleAccid[j]; k++)
                        {
                            doubleAccid_str[j] = doubleAccid_str[j] + "x";
                        }
                    }
                }
            }
            //Console.WriteLine("\nGetSignFromAccidental");
            for (int i = 0; i < accidentalString.Length; i++)
            {
                //Console.WriteLine("---- " + i + " ----");
                if (accidentalList[i] == 0)
                {
                    accidentalString[i] = "";
                    //Console.WriteLine(accidentalString[i]);
                }
                else if (accidentalList[i] > 0)
                {
                    accidentalString[i] = Accid_str[i] + doubleAccid_str[i];
                    //Console.WriteLine(accidentalString[i]);
                }
                else if (accidentalList[i] < 0)
                {
                    for (int j = 0; j > accidentalList[i]; j--)
                    {
                        accidentalString[i] = accidentalString[i] + "b";
                    }
                    //Console.WriteLine(accidentalString[i]);
                }

            }
            return accidentalString;
        }
    }
}　






