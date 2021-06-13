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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using FenixLib.Core;

namespace FenixLib.IO
{
    public partial class NativeFormatWriter : BinaryWriter
    {

        private static readonly Encoding encoding = CodePagesEncodingProvider.Instance.GetEncoding ( 850 );

        public NativeFormatWriter ( Stream input ) : base ( input, encoding ) { }

        public void WriteAsciiZ ( string text, int maxLength )
        {
            if ( text == null )
            {
                text = "";
            }

            // Texts are encoded in ASCIZZ format
            var clippedText = text.Substring ( 0, Math.Min ( text.Length, maxLength ) );
            var bytes = encoding.GetBytes ( clippedText.ToCharArray () );
            Array.Resize ( ref bytes, maxLength );
            base.Write ( bytes );
        }

        public void WriteLegacyGlyphInfo ( ref GlyphInfo glyphInfo )
        {
            Write ( ( int ) glyphInfo.Width );
            Write ( ( int ) glyphInfo.Height );
            Write ( ( int ) glyphInfo.YOffset );
            Write ( ( int ) glyphInfo.FileOffset );
        }

        public void WriteExtendedGlyphInfo ( ref GlyphInfo glyphInfo )
        {
            Write ( ( int ) glyphInfo.Width );
            Write ( ( int ) glyphInfo.Height );
            Write ( ( int ) glyphInfo.XAdvance );
            Write ( ( int ) glyphInfo.YAdvance );
            Write ( ( int ) glyphInfo.XOffset );
            Write ( ( int ) glyphInfo.YOffset );
            Write ( ( int ) glyphInfo.FileOffset );
        }

        public void Write ( Palette palette )
        {
            if ( palette == null )
            {
                throw new ArgumentNullException ( nameof ( palette ) );
            }

            byte[] bytes = new byte[palette.Colors.Length * 3];
            for ( var n = 0 ; n < palette.Colors.Length ; n++ )
            {
                bytes[n * 3] = Convert.ToByte ( palette[n].R >> 2 );
                bytes[n * 3 + 1] = Convert.ToByte ( palette[n].G >> 2 );
                bytes[n * 3 + 2] = Convert.ToByte ( palette[n].B >> 2 );
            }

            base.Write ( bytes );
        }

        public void Write ( PivotPoint pivotPoint )
        {
            Write ( Convert.ToInt16 ( pivotPoint.X ) );
            Write ( Convert.ToInt16 ( pivotPoint.Y ) );
        }

        public void WritePaletteGammaSection ()
        {
            byte[] bytes = new byte[NativeFormat.PaletteGammaBytesSize];
            const int rowSize = 36;

            // Compatibility for DIV requires that at least the byte indicating the number
            // of colors of each Gamma stride is not 0.
            // Note: this is not required for BennuGD / PixTudio, but it does not harm
            for ( int row = 0 ; row < 16 ; row++ )
            {
                // Reference: Analysis of Div Games Studio beta map editor
                bytes[row * rowSize + 0] = 32; // Number of colors (8, 16, 32)
                bytes[row * rowSize + 1] = 0;  // Edit every N colors (0, 1, 4, 8)
                bytes[row * rowSize + 2] = 0;  // Editorial / Fix (0, 1)
                bytes[row * rowSize + 3] = 0;  // ?

                // The gamma color generation is done accordingly to Div Games Studio behaviour
                for ( int color = 0 ; color < 16 ; color++ )
                {
                    var value = ( byte ) ( row * 16 + color );
                    bytes[row * rowSize + 4 + color] = value;
                    bytes[row * rowSize + 4 + 16 + color] = ( byte ) ( ( value + 16 ) % 256 );
                }
            }

            base.Write ( bytes );
        }

        public void Write ( IEnumerable<PivotPoint> pivotPoints,
            int spriteWidth, int spriteHeight,
            PivotPointsCountFieldType pivotPointsCountFieldType
            )
        {
            var writablePoints = new WritablePivotPointsView ( pivotPoints,
                spriteWidth, spriteHeight );
            Write ( writablePoints, pivotPointsCountFieldType );
        }

        internal void Write ( WritablePivotPointsView writablePoints,
            PivotPointsCountFieldType pivotPointsCountFieldType )
        {

            if ( writablePoints == null )
            {
                throw new ArgumentNullException ( nameof ( writablePoints ) );
            }

            int count = writablePoints.Count;

            if ( pivotPointsCountFieldType == PivotPointsCountFieldType.TypeUInt16 )
            {
                Write ( Convert.ToUInt16 ( count ) );
            }
            else if ( pivotPointsCountFieldType == PivotPointsCountFieldType.TypeUInt32 )
            {
                Write ( Convert.ToUInt32 ( count ) );
            }
            else
            {
                throw new ArgumentOutOfRangeException ( nameof ( pivotPointsCountFieldType ) );
            }

            foreach ( var point in writablePoints )
            {
                Write ( point );
            }
        }

        public enum PivotPointsCountFieldType
        {
            TypeUInt16, TypeUInt32
        }
    }
}
