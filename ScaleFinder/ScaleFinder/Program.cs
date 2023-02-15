// See https://aka.ms/new-console-template for more information

namespace ScaleFinder
{
    class Program
    {
        static ScaleFinder finder = new ScaleFinder();
        int find = 0;
        static void Main(string[] args)
        {
            int result = finder.FindScale(ScaleFinder.pitch_C, ScaleFinder.accid_NATURAL, ScaleFinder.type_MAJOR);
            //Console.WriteLine(">> MUSIC SCALE : " + result);
        }
    }
}
