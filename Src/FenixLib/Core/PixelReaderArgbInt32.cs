/*  Copyright 2016 Darío Cutillas Carrillo
*
*   Licensed under the Apache License, Version 2.0 (the "License");
*   you may not use this file except in compliance with the License.
*   You may obtain a copy of the License at
*
*       http://www.apache.org/licenses/LICENSE-2.0
*
*   Unless required by applicable law or agreed to in writing, software
*   distributed under the License is distributed on an "AS IS" BASIS,
*   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*   See the License for the specific language governing permissions and
*   limitations under the License.
*/
namespace FenixLib.Core
{
    internal class PixelReaderArgbInt32 : PixelReader
    {
        public override bool HasPixels
        {
            get
            {
                return ( BaseStream.Position < BaseStream.Length );
            }
        }

        public override void Read ()
        {
            int value = Reader.ReadInt32 ();

            Alpha = ( value >> 24 ) & 0xFF;
            R = ( value >> 16 ) & 0xFF;
            G = ( value >> 8 ) & 0xFF ;
            B = value & 0xFF;
        }
    }
}
