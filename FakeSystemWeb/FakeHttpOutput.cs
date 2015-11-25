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

    /// <summary>
    /// Represents an object that can be used to capture the output from a <see cref="FakeHttpResponse"/>.
    /// </summary>
    public class FakeHttpOutput : IDisposable
    {
        private MemoryStream stream;
        private StreamWriter writer;
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpOutput"/> class.
        /// </summary>
        public FakeHttpOutput()
        {
            this.stream = new MemoryStream();
            this.writer = new StreamWriter(this.stream);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FakeHttpOutput"/> class.
        /// </summary>
        ~FakeHttpOutput()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the output stream.
        /// </summary>
        protected internal Stream OutputStream
        {
            get
            {
                return this.stream;
            }
        }

        /// <summary>
        /// Gets the output writer.
        /// </summary>
        protected internal TextWriter OutputWriter
        {
            get
            {
                return this.writer;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the content string.
        /// </summary>
        /// <returns>A <see cref="string"/> containing the output content.</returns>
        public string GetContentString()
        {
            return this.writer.Encoding.GetString(this.GetContentBytes());
        }

        /// <summary>
        /// Gets the content bytes.
        /// </summary>
        /// <returns>A <see cref="byte[]"/> containing the output content bytes.</returns>
        public byte[] GetContentBytes()
        {
            this.writer.Flush();

            return this.stream.ToArray();
        }

        /// <summary>
        /// Clears the output.
        /// </summary>
        protected internal virtual void Clear()
        {
            this.writer.Flush();
            this.stream.SetLength(0);
        }

        /// <summary>
        /// Closes the output.
        /// </summary>
        protected internal virtual void Close()
        {
            this.writer.Close();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.writer.Dispose();
            }

            this.stream = null;
            this.writer = null;
            this.isDisposed = true;
        }
    }
}
