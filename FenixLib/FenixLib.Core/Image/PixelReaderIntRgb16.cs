﻿/*  Copyright 2016 Darío Cutillas Carrillo
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
namespace FenixLib.Image
{
    internal class PixelReaderRgbInt16 : PixelReader
    {
        public override bool HasPixels
        {
            get
            {
                return ( BaseStream.Position + 2 < BaseStream.Length );
            }
        }

        public override void Read ()
        {
            int value = Reader.ReadInt16 ();

            r = value & 0xF800;
            g = value & 0x7E0;
            b = value & 0x1F;

            if ( value == 0 )
                alpha = 0;
            else
                alpha = 255;
        }
    }
}
