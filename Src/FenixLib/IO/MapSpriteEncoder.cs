/*  Copyright 2016 Dar�o Cutillas Carrillo
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
using System;
using System.Data;
using System.Linq;
using FenixLib.Core;

namespace FenixLib.IO
{
    public class MapSpriteEncoder : NativeEncoder<ISprite>
    {
        private const int version = 0x00;

        protected override byte GetLastHeaderByte ( ISprite sprite ) => version;

        protected override void WriteNativeFormatBody ( ISprite sprite,
            NativeFormatWriter writer )
        {
            writer.Write ( Convert.ToUInt16 ( sprite.Width ) );
            writer.Write ( Convert.ToUInt16 ( sprite.Height ) );
            writer.Write ( Convert.ToUInt32 ( 0 ) );
            writer.WriteAsciiZ ( sprite.Description, 32 );

            // TODO: Add test case to ensure that the function fails if sprite.GraphicFormat
            // is Indexed but palette is null
            //if ( ( sprite.Palette != null ) )
            if ( ( sprite.GraphicFormat == GraphicFormat.Format8bppIndexed ) )
            {
                writer.Write ( sprite.Palette );
                writer.WritePaletteGammaSection ();
            }

            writer.Write ( sprite.PivotPoints, sprite.Width, sprite.Height, 
                NativeFormatWriter.PivotPointsCountFieldType.TypeUInt16 );
            writer.Write ( sprite.PixelData );
        }

        protected override string GetFileMagic ( ISprite sprite )
        {

            switch ( ( int ) sprite.GraphicFormat )
            {
                case 1:
                    return "m01";
                case 8:
                    return "map";
                case 16:
                    return "m16";
                case 32:
                    return "m32";
                default:
                    throw new ArgumentException ();
            }
        }

    }
}
