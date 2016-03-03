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
using FenixLib.Core;
using static FenixLib.IO.NativeFormat;

namespace FenixLib.IO
{
    public class FpgSpriteAssetDecoder : NativeDecoder<SpriteAsset>
    {

        public override int MaxSupportedVersion { get; } = 0x00;

        protected override string[] KnownFileExtensions { get; } = { "fpg" };

        protected override string[] KnownFileMagics { get; } = { "f16", "f32", "fpg", "f01" };

        protected override SpriteAsset ReadBody ( Header header, NativeFormatReader reader )
        {
            SpriteAsset fpg;

            Palette palette = null;
            if ( header.BitsPerPixel == 8 )
            {
                palette = reader.ReadPalette ();
                reader.ReadUnusedPaletteGamma ();
            }

            fpg = new SpriteAsset( ( GraphicFormat ) header.BitsPerPixel, palette );

            try
            {
                do
                {
                    var code = reader.ReadInt32 ();
                    var maplen = reader.ReadInt32 ();
                    var description = reader.ReadAsciiZ ( 32 );
                    var name = reader.ReadAsciiZ ( 12 );
                    var width = reader.ReadInt32 ();
                    var height = reader.ReadInt32 ();
                    var numberOfPivotPoints = reader.ReadPivotPointsNumberLong ();
                    var pivotPoints = reader.ReadPivotPoints ( numberOfPivotPoints );

                    var mapDataLength = width * height * ( header.BitsPerPixel / 8 );

                    // Some tools such as FPG Edit are non conformant with the standard
                    // FPG files and will add data at the end. 
                    if ( mapDataLength + 64 + numberOfPivotPoints * 4 != maplen )
                    {
                        // It can be that many tools generate this field with wrong
                        // information. I am not even sure in SmartFpgEditor!
                        // break; 
                        // TODO: Consider if for example, we shall generate some 
                        // kind of event
                    }

                    byte[] pixels = reader.ReadPixels ( header.BitsPerPixel, width, height );
                    IGraphic graphic = new StaticGraphic ( 
                        ( GraphicFormat ) header. BitsPerPixel, 
                        width, height, pixels, palette );
                    ISprite sprite = new Sprite ( graphic );
                    sprite.Description = description;
                    foreach ( var point in pivotPoints )
                    {
                        sprite.DefinePivotPoint ( point.Id, point.X, point.Y );
                    }

                    fpg.Update ( code, sprite );

                } while ( true );

            }
            catch ( System.IO.EndOfStreamException )
            {
                // Do nothing. The file is consumed until it is not possible to 
                // read any more data.
            }

            return fpg;
        }
    }
}
