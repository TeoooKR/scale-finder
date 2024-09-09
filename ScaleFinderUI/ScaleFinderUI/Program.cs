using System;
namespace MusicPeanut {
    internal class Program {
        static Harmony ScaleFindResult;
        static void Main() {
            Console.WriteLine("Hello, World!");
            ScaleFindResult = new HarmonyFinder().FindHarmony((byte)HarmonyFinder.Pitch.C, (sbyte)HarmonyFinder.Accidental.Natural, HarmonyFinder.Type.Scale.MajorScale);
            Console.WriteLine("\nProgram.cs");
            Console.WriteLine("SelectedHarmony: " + ScaleFindResult.SelectedHarmony);
            Console.WriteLine("SelectedChordSymbol: " + ScaleFindResult.SelectedChordSymbol);
            Console.ReadKey();
        }
    }
}