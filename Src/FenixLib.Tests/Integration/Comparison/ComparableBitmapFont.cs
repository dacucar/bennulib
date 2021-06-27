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
using System.Collections;
using System.Collections.Generic;
using FenixLib.Core;

namespace FenixLib.Tests.Integration.Comparison
{
    internal class ComparableBitmapFont : IBitmapFont
    {
        IBitmapFont decorated;

        public ComparableBitmapFont (IBitmapFont font)
        {
            decorated = font;
        }

        public bool ComparePalette { get; set; } = false;

        public bool CompareFormat { get; set; } = false;

        public bool CompareGlyphs { get; set; } = false;

        public IGraphicEqualityComparer<IGraphic> GlyphsComparer { get; set; }

        public virtual bool Equals ( IBitmapFont font )
        {
            if ( ReferenceEquals ( font, null ) )
                return false;

            if ( CompareFormat && GraphicFormat != font.GraphicFormat )
                return false;

            if ( ComparePalette && Palette != font.Palette )
                return false;

            if ( CompareGlyphs )
                foreach ( FontGlyph element in Glyphs )
                {
                   if ( !GlyphsComparer.Equals ( element, font[element.Character] ) )
                    {
                        return false;
                    }
                }

            return true;
        }

        public override int GetHashCode ()
        {
            return 0; // Force equallity via Equals
        }

        public override bool Equals ( object obj )
        {
            IBitmapFont objAsAssortment = obj as IBitmapFont;
            if ( objAsAssortment == null )
            {
                return false;
            }
            else
            {
                return Equals ( objAsAssortment );
            }
        }

        public IGlyph this[char character]
        {
            get
            {
                return decorated[character];
            }

            set
            {
                decorated[character] = value;
            }
        }

        public IGlyph this[int index]
        {
            get
            {
                return decorated[index];
            }

            set
            {
                decorated[index] = value;
            }
        }

        public FontEncoding Encoding => decorated.Encoding;

        public IEnumerable<FontGlyph> Glyphs => decorated.Glyphs;

        public GraphicFormat GraphicFormat => decorated.GraphicFormat;

        public Palette Palette => decorated.Palette;

        public IEnumerator<FontGlyph> GetEnumerator () => decorated.GetEnumerator ();

        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
    }
}
