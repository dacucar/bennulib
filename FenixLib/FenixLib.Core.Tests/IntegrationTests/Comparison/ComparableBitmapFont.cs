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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenixLib.Core.Tests.IntegrationTests.Comparison
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

        public AbstractGraphicComparer<IGraphic> GlyphsComparer { get; set; }

        public virtual bool Equals ( ISpriteAsset asset )
        {
            if ( ReferenceEquals ( asset, null ) )
                return false;

            if ( CompareFormat && GraphicFormat != asset.GraphicFormat )
                return false;

            if ( ComparePalette && Palette != asset.Palette )
                return false;

            if ( CompareGlyphs && GlyphsComparer != null )
                foreach ( FontGlyph element in Glyphs )
                {
                   if ( !GlyphsComparer.Equals ( element, asset[element.Character] ) )
                    {
                        return false;
                    }
                }

            return true;
        }

        public override int GetHashCode ()
        {
            return 0;
           // return GraphicFormat.GetHashCode () ^ Sprites.Count.GetHashCode ();
        }

        public override bool Equals ( object obj )
        {
            ISpriteAsset objAsAsset = obj as ISpriteAsset;
            if ( objAsAsset == null )
            {
                return false;
            }
            else
            {
                return Equals ( objAsAsset );
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

        public FontEncoding CodePage => decorated.CodePage;

        public IEnumerable<FontGlyph> Glyphs => decorated.Glyphs;

        public GraphicFormat GraphicFormat => decorated.GraphicFormat;

        public Palette Palette => decorated.Palette;

        public IEnumerator<FontGlyph> GetEnumerator () => decorated.GetEnumerator ();

        IEnumerator IEnumerable.GetEnumerator () => decorated.GetEnumerator ();
    }
}
