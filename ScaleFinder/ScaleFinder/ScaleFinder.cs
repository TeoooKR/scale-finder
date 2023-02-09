namespace ScaleFinder
{
    class ScaleFinder
    {
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

        readonly private int[] def_pitch_index = new int[7] { 1, 3, 5, 6, 8, 10, 12 };
        readonly private string[] sc = new string[7] { "C", "D", "E", "F", "G", "A", "B" };

        readonly private int[] ionian_interval = new int[6] { 2, 2, 1, 2, 2, 2 };
        readonly private int[] dorian_interval = new int[6] { 2, 1, 2, 2, 2, 1 };
        readonly private int[] phtygian_interval = new int[6] { 1, 2, 2, 2, 1, 2 };
        readonly private int[] lydian_interval = new int[6] { 2, 2, 2, 1, 2, 2 };
        readonly private int[] mixolydian_interval = new int[6] { 2, 2, 1, 2, 2, 1 };
        readonly private int[] aeolian_interval = new int[6] { 2, 1, 2, 2, 1, 2 };
        readonly private int[] locrain_interval = new int[6] { 2, 1, 2, 1, 2, 2 };

        public int FindScale(int start_pitch, int accid, int type)
        {
            int[] sc_index = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int[] accidental = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int[] accidental_index = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            string[] sub_sc = new string[7] { "", "", "", "", "", "", "" };
            string[] accidental_string = new string[7] { "", "", "", "", "", "", "" };
            string[] fin_sc = new string[7] { "", "", "", "", "", "", "" };
            int pitch_index = start_pitch + accid;

            if (type == type_MAJOR || type == type_IONIAN)
            {
                get_type_interval(sc_index, pitch_index, ionian_interval);
            }
            else if (type == type_DORIAN)
            {
                get_type_interval(sc_index, pitch_index, dorian_interval);
            }
            else if (type == type_PHTYGIAN)
            {
                get_type_interval(sc_index, pitch_index, phtygian_interval);
            }
            else if (type == type_LYDIAN)
            {
                get_type_interval(sc_index, pitch_index, lydian_interval);
            }
            else if (type == type_MIXOLYDIAN)
            {
                get_type_interval(sc_index, pitch_index, mixolydian_interval);
            }
            else if (type == type_AEOLIAN || type == type_MINOR)
            {
                get_type_interval(sc_index, pitch_index, aeolian_interval);
            }
            else if (type == type_LOCRAIN)
            {
                get_type_interval(sc_index, pitch_index, locrain_interval);
            }

            for (int i = 0; i < def_pitch_index.Length; i++)
            {
                if (start_pitch == def_pitch_index[i])
                {
                    accidental_index = get_accidental_index(accidental, i, sc_index);
                    sub_sc = get_sc_order(sc, i);
                }
            }

            //for (int i = 0; i < accidental_index.Length; i++)
            //{}

            int result = pitch_index;
            Console.WriteLine("I Found Scale!!!!!!!!!!!!!!!!");
            return result;
        }

        private int get_type_interval(int[] sc_index, int pitch_index, int[] interval)
        {
            sc_index[0] = pitch_index;
            Console.WriteLine("index = " + sc_index[0]);
            for (int i = 0; i < interval.Length; i++)
            {
                sc_index[i + 1] = sc_index[i] + interval[i];
                Console.WriteLine("index = " + sc_index[i + 1]);
            }
            return 1;
        }

        private int[] get_accidental_index(int[] accidental, int start_pitch_index, int[] sc_index)
        {
            //> pitch_order_index [0~6]
            int j = 0;
            if (start_pitch_index == 0)
            {
                for (int i = 0; i < def_pitch_index.Length; i++)
                {
                    Console.WriteLine("---- " + i + " ---- " + sc_index[i] + " / " + def_pitch_index[i]);
                    accidental[i] = sc_index[i] - def_pitch_index[i];
                    Console.WriteLine(accidental[i]);
                }
                return accidental;
            }

            for (int i = 0; i < def_pitch_index.Length; i++)
            {
                j = i + start_pitch_index;
                if (j > 6)
                {
                    j = j % 6;
                }
                if (sc_index[i] >= 12)
                {
                    sc_index[i] = sc_index[i] % 12;
                }
                Console.WriteLine("---- " + i + " ---- " + sc_index[i] + " / " + def_pitch_index[i]);
                accidental[i] = sc_index[i] - def_pitch_index[j];
                if (accidental[i] >= 12)
                {
                    accidental[i] = accidental[i] % 12;
                }
                Console.WriteLine(accidental[i]);
            }
            return accidental;
        }


        private string[] get_sc_order(string[] sc, int pitch_order_index)
        {
            string[] result = { "C", "D", "E", "F", "G", "A", "B" };
            return result;
        }
    }
}






