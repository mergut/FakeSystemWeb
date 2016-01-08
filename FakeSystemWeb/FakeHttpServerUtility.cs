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
    using System.Text;
    using System.Web;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.HttpServerUtilityBase"/>.
    /// </summary>
    public class FakeHttpServerUtility : HttpServerUtilityBase
    {
        private readonly FakeHttpContext context;

        private string machineName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpServerUtility"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public FakeHttpServerUtility(FakeHttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }

        /// <summary>
        /// Gets the server's computer name.
        /// </summary>
        public override string MachineName
        {
            get
            {
                if (this.machineName == null)
                {
                    this.machineName = Environment.MachineName;
                }

                return this.machineName;
            }
        }

        /// <summary>
        /// Gets or sets the request time-out value in seconds.
        /// </summary>
        public override int ScriptTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Clears the most recent exception.
        /// </summary>
        public override void ClearError()
        {
            this.context.ClearError();
        }

        /// <summary>
        /// Returns the most recent exception.
        /// </summary>
        /// <returns>The most recent exception that was thrown.</returns>
        public override Exception GetLastError()
        {
            return this.context.Error;
        }

        /// <summary>
        /// Decodes an HTML-encoded string and returns the decoded string.
        /// </summary>
        /// <param name="s">The HTML string to decode.</param>
        /// <returns>The decoded text.</returns>
        public override string HtmlDecode(string s)
        {
            return HttpUtility.HtmlDecode(s);
        }

        /// <summary>
        /// Decodes an HTML-encoded string and returns the results in a stream.
        /// </summary>
        /// <param name="s">The HTML string to decode.</param>
        /// <param name="output">The stream to contain the decoded string.</param>
        public override void HtmlDecode(string s, TextWriter output)
        {
            HttpUtility.HtmlDecode(s, output);
        }

        /// <summary>
        /// HTML-encodes a string and returns the encoded string.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <returns>The HTML-encoded text.</returns>
        public override string HtmlEncode(string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        /// <summary>
        /// HTML-encodes a string and sends the resulting output to an output stream.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <param name="output">The stream to contain the encoded string.</param>
        public override void HtmlEncode(string s, TextWriter output)
        {
            HttpUtility.HtmlEncode(s, output);
        }

        /// <summary>
        /// Returns the physical file path that corresponds to the specified virtual path on the Web server.
        /// </summary>
        /// <param name="path">The virtual path to get the physical path for.</param>
        /// <returns>The physical file path that corresponds to <paramref name="path" />.</returns>
        public override string MapPath(string path)
        {
            return this.context.Request.MapPath(path);
        }

        /// <summary>
        /// Sets the server's computer name.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        public void SetMachineName(string machineName)
        {
            this.machineName = machineName;
        }

        /// <summary>
        /// Decodes a URL-encoded string and returns the decoded string.
        /// </summary>
        /// <param name="s">The string to decode.</param>
        /// <returns>The decoded text.</returns>
        public override string UrlDecode(string s)
        {
            return HttpUtility.UrlDecode(s, this.context.Request.ContentEncoding ?? Encoding.UTF8);
        }

        /// <summary>
        /// Decodes a URL-encoded string and sends the resulting output to a stream.
        /// </summary>
        /// <param name="s">The string to decode.</param>
        /// <param name="output">The stream to contain the decoded string.</param>
        public override void UrlDecode(string s, TextWriter output)
        {
            if (s != null)
            {
                output.Write(this.UrlDecode(s));
            }
        }

        /// <summary>
        /// URL-encodes a string and returns the encoded string.
        /// </summary>
        /// <param name="s">The text to URL-encode.</param>
        /// <returns>The URL-encoded text.</returns>
        public override string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s, this.context.Request.ContentEncoding ?? Encoding.UTF8);
        }

        /// <summary>
        /// URL-encodes a string and sends the resulting output to a stream.
        /// </summary>
        /// <param name="s">The string to encode.</param>
        /// <param name="output">The stream to contain the encoded string.</param>
        public override void UrlEncode(string s, TextWriter output)
        {
            if (s != null)
            {
                output.Write(this.UrlEncode(s));
            }
        }

        /// <summary>
        /// URL-encodes the path section of a URL string.
        /// </summary>
        /// <param name="s">The string to URL-encode.</param>
        /// <returns>The URL-encoded text.</returns>
        public override string UrlPathEncode(string s)
        {
            return HttpUtility.UrlPathEncode(s);
        }

        /// <summary>
        /// Decodes a URL string token into an equivalent byte array by using base64-encoded digits.
        /// </summary>
        /// <param name="input">The URL string token to decode.</param>
        /// <returns>The byte array that contains the decoded URL token.</returns>
        public override byte[] UrlTokenDecode(string input)
        {
            return HttpServerUtility.UrlTokenDecode(input);
        }

        /// <summary>
        /// Encodes a byte array into an equivalent string representation by using base64 digits,
        ///  which makes it usable for transmission on the URL.
        /// </summary>
        /// <param name="input">The byte array to encode.</param>
        /// <returns>
        /// The string that contains the encoded array if the length of <paramref name="input" /> is greater than 1;
        ///  otherwise, an empty string ("").
        /// </returns>
        public override string UrlTokenEncode(byte[] input)
        {
            return HttpServerUtility.UrlTokenEncode(input);
        }
    }
}
