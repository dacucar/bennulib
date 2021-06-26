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
using NUnit.Framework;
using System;
using FenixLib.Imaging;
using FenixLib.Core;
using Moq;

namespace FenixLib.Tests.Imaging
{
    [TestFixture ( typeof ( FormatConverter ) )]
    class IFormatConverterContract<T> where T : IFormatConverter, new()
    {
        protected IFormatConverter CreateConverter ()
        {
            return new T ();
        }

        [Test]
        public void Convert_NullGraphic_ThrowsNullArgumentException ()
        {
            IFormatConverter converter = CreateConverter ();

            Assert.Throws<ArgumentNullException>(() => 
                converter.Convert ( null, GraphicFormat.Format32bppArgb ));

        }

        [Test]
        public void Convert_NullFormat_ThrowsNullArgumentException ()
        {
            IGraphic stubGraphic = new Mock<IGraphic> ().Object;
            IFormatConverter converter = CreateConverter ();

            Assert.Throws<ArgumentNullException> ( () =>
                  converter.Convert ( stubGraphic, null ) );
        }
    }
}
