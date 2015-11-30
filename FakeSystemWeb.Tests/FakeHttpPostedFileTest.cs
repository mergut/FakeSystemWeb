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
    using System.IO;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public sealed class FakeHttpPostedFileTest
    {
        [Test]
        public void FakeHttpPostedFile_Constructor_WithNullOrEmptyFileNameParameter_ThrowsArgumentNullException()
        {
            ExceptionAssert.ArgumentNull(() => new FakeHttpPostedFile(null, "unused", Stream.Null), "fileName");
            ExceptionAssert.ArgumentNull(() => new FakeHttpPostedFile(string.Empty, "unused", Stream.Null), "fileName");
        }

        [Test]
        public void FakeHttpPostedFile_Constructor_WithNullOrEmptyContentTypeParameter_ThrowsArgumentNullException()
        {
            ExceptionAssert.ArgumentNull(() => new FakeHttpPostedFile("unused", null, Stream.Null), "contentType");
            ExceptionAssert.ArgumentNull(() => new FakeHttpPostedFile("unused", string.Empty, Stream.Null), "contentType");
        }

        [Test]
        public void FakeHttpPostedFile_Constructor_WithNullInputStreamParameter_ThrowsArgumentNullException()
        {
            ExceptionAssert.ArgumentNull(() => new FakeHttpPostedFile("unused", "unused", null), "inputStream");
        }

        [Test]
        public void FakeHttpPostedFile_ContentLength_Default_ReturnsStreamLength()
        {
            var postedFile = new FakeHttpPostedFile("unused", "unused", Stream.Null);

            Assert.That(postedFile.ContentLength, Is.EqualTo(0));
        }

        [Test]
        public void FakeHttpPostedFile_ContentType_Default_ReturnsContentType()
        {
            var postedFile = new FakeHttpPostedFile("unused", "text/plain", Stream.Null);

            Assert.That(postedFile.ContentType, Is.EqualTo("text/plain"));
        }

        [Test]
        public void FakeHttpPostedFile_FileName_Default_ReturnsFileName()
        {
            var postedFile = new FakeHttpPostedFile("test-filename", "unused", Stream.Null);

            Assert.That(postedFile.FileName, Is.EqualTo("test-filename"));
        }

        [Test]
        public void FakeHttpPostedFile_InputStream_Default_ReturnsInputStream()
        {
            var stream = Stream.Null;
            var postedFile = new FakeHttpPostedFile("unused", "unused", stream);

            Assert.That(postedFile.InputStream, Is.EqualTo(stream));
        }

        [Test]
        public void FakeHttpPostedFile_SaveAs_Default_SavesStreamContentsInFile()
        {
            string content = "Test Content";
            string tempFilePath = Path.GetTempFileName();

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var postedFile = new FakeHttpPostedFile("test.txt", "text/plain", ms);

                postedFile.SaveAs(tempFilePath);
            }

            string tempFileContent = File.ReadAllText(tempFilePath);
            File.Delete(tempFilePath);

            Assert.That(tempFileContent, Is.EqualTo(content));
        }
    }
}
