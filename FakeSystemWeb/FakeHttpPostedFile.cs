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
namespace FakeSystemWeb
{
    using System;
    using System.IO;
    using System.Web;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.HttpPostedFileBase"/>.
    /// </summary>
    public class FakeHttpPostedFile : HttpPostedFileBase
    {
        private readonly string contentType;
        private readonly string fileName;
        private readonly Stream inputStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpPostedFile"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="contentType">The MIME content type.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// fileName
        /// or
        /// contentType
        /// or
        /// inputStream
        /// </exception>
        public FakeHttpPostedFile(string fileName, string contentType, Stream inputStream)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException("contentType");
            }

            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }

            this.contentType = contentType;
            this.fileName = fileName;
            this.inputStream = inputStream;
        }

        /// <summary>
        /// Gets the size of an uploaded file, in bytes.
        /// </summary>
        public override int ContentLength
        {
            get
            {
                return (int)this.inputStream.Length;
            }
        }

        /// <summary>
        /// Gets the MIME content type of an uploaded file.
        /// </summary>
        public override string ContentType
        {
            get
            {
                return this.contentType;
            }
        }

        /// <summary>
        /// Gets the fully qualified name of the file on the client.
        /// </summary>
        public override string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.IO.Stream" /> object that points to an uploaded file to prepare for reading the contents of the file.
        /// </summary>
        public override Stream InputStream
        {
            get
            {
                return this.InputStream;
            }
        }

        /// <summary>
        /// Saves the contents of an uploaded file.
        /// </summary>
        /// <param name="filename">The name of the file to save.</param>
        public override void SaveAs(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
