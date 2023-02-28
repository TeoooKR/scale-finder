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

namespace ScaleFinder {
    class Program {
        static ScaleFinder finder = new ScaleFinder();
        static void Main(string[] args) {
            Scale result = finder.FindScale(ScaleFinder.PitchC, ScaleFinder.AccidNatural, ScaleFinder.ModeLocrain);
            //Scale result = finder.FindScale(ScaleFinder.PitchB, 30, ScaleFinder.ModeMajor);
            if (!result.GetFound()) {
                Console.WriteLine("Error.. I cannot find your scale. sigh....");
                return;
            }
            result.PrintMyValues();
        }
    }
}
