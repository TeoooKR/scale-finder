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
using System.Diagnostics;
namespace ScaleFinderUI {
  public class Scale {
    private string[] PitchTexts = new string[8] { "", "", "", "", "", "", "", "" };
    private string[] AccidentalTexts = new string[8] { "", "", "", "", "", "", "", "" };
    private int[] PitchList = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] AccidentalList = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] IntervalsList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
    private bool Found = false;
    public string GetPitchText(int index) {
      return PitchTexts[index];
    }
    public string GetAccidentalText(int index) {
      return AccidentalTexts[index];
    }
    public string[] GetPitchTexts() {
      return PitchTexts;
    }
    public void SetPitchTexts(string[] pitchTexts) {
      this.PitchTexts = pitchTexts;
    }
    public string[] GetAccidentalTexts() {
      return AccidentalTexts;
    }
    public void SetAccidentalTexts(string[] accidentalTexts) {
      this.AccidentalTexts = accidentalTexts;
    }
    public int[] GetPitchList() {
      return PitchList;
    }
    public void SetPitchList(int[] pitchList) {
      this.PitchList = pitchList;
    }
    public int[] GetAccidentalList() {
      return AccidentalList;
    }
    public void SetAccidentalList(int[] accidentalList) {
      this.AccidentalList = accidentalList;
    }
    public int[] GetIntervalsList() {
      return IntervalsList;
    }
    public void SetIntervalsList(int[] intervalsList) {
      this.IntervalsList = intervalsList;
    }
    public bool GetFound() {
      return Found;
    }
    public void SetFound(bool found) {
      this.Found = found;
    }
    public void PrintMyValues() {
      Debug.WriteLine("         " + "PITCH" + "\t\t" + "INDEX" + "\t\t" + "ACCID.");
      for (int i = 0; i < PitchTexts.Length; i++) {
        Debug.WriteLine("- [" + i + "]    " + PitchTexts[i] + "\t\t\t" + PitchList[i] + "\t\t\t" + AccidentalList[i]);
      }
    }
  }
}
