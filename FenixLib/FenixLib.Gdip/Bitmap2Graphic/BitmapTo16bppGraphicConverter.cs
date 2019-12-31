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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using FenixLib.Core;

namespace FenixLib.BitmapConvert
{
    internal class BitmapTo16bppGraphicConverter : Bitmap2GraphicConverter
    {
        public BitmapTo16bppGraphicConverter ( Bitmap src ) : base ( src ) { }
        protected override PixelFormat[] AcceptedFormats => new PixelFormat[]
        {
            PixelFormat.Format1bppIndexed,
            PixelFormat.Format4bppIndexed,
            PixelFormat.Format8bppIndexed,
            PixelFormat.Format16bppArgb1555,
            PixelFormat.Format16bppGrayScale,
            PixelFormat.Format16bppRgb555,
            PixelFormat.Format16bppRgb565
        };

        protected override GraphicFormat DestFormat => GraphicFormat.Format16bppRgb565;

        protected override PixelFormat LockBitsFormat => PixelFormat.Format16bppRgb565;
    }
}