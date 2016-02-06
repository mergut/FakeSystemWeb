/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2015 Mehmet Ergut
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
namespace FakeSystemWeb.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    [TestFixture]
    public sealed class FakeHttpOutputTest
    {
        [Test]
        public void FakeHttpOutput_GetContentString_WithWrittenText_ReturnsText()
        {
            using (var output = new FakeHttpOutput())
            {
                output.OutputWriter.Write("test");

                var result = output.GetContentString();

                Assert.That(result, Is.EqualTo("test"));
            }
        }

        [Test]
        public void FakeHttpOutput_GetContentBytes_WithWrittenBytes_ReturnsBytes()
        {
            using (var output = new FakeHttpOutput())
            {
                var testBytes = new byte[] { 0x41, 0x42 };
                output.OutputStream.Write(testBytes, 0, testBytes.Length);

                var result = output.GetContentBytes();

                Assert.That(result, Is.EqualTo(testBytes));
            }
        }

        [Test]
        public void FakeHttpOutput_Clear_Default_ClearsContents()
        {
            using (var output = new FakeHttpOutput())
            {
                output.OutputWriter.Write("test");

                output.Clear();

                Assert.That(output.OutputStream.Length, Is.EqualTo(0));
            }
        }

        [Test]
        public void FakeHttpOutput_Close_AfterClose_Throws()
        {
            var output = new FakeHttpOutput();

            output.Close();

            Assert.That(() => output.OutputStream.Length, Throws.TypeOf<ObjectDisposedException>());
        }
    }
}
