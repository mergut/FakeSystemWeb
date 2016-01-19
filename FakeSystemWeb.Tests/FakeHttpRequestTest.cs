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
    using NUnit.Framework;

    [TestFixture]
    public sealed class FakeHttpRequestTest
    {
        [Test]
        public void FakeHttpRequest_Constructor_WithNullUrlParameter_ThrowsArgumentNullException()
        {
            ExceptionAssert.ArgumentNull(() => new FakeHttpRequest(null, "unused"), "url");
        }

        [Test]
        public void FakeHttpRequest_Constructor_WithNullHttpMethodParameter_ThrowsArgumentNullException()
        {
            ExceptionAssert.ArgumentNull(() => new FakeHttpRequest(new Uri("http://127.0.0.1/"), null), "httpMethod");
        }

        [Test]
        public void FakeHttpRequest_Constructor_WithRelativeUrl_ThrowsArgumentException()
        {
            ExceptionAssert.Argument(() => new FakeHttpRequest(new Uri("/relative", UriKind.Relative), "GET"), "The url must be absolute.", "url");
        }

        [Test]
        public void FakeHttpRequest_AppRelativeCurrentExecutionFilePath_Default_ReturnsCorrectPath()
        {
            var request = new FakeHttpRequest(new Uri("http://127.0.0.1/app/folder/file.aspx/extra?var=val"), "GET");
            request.SetApplicationPath("/app");

            var result = request.AppRelativeCurrentExecutionFilePath;

            Assert.That(result, Is.EqualTo("~/folder/file.aspx"));
        }

        [Test]
        public void FakeHttpRequest_CurrentExecutionFilePath_Default_ReturnsCorrectPath()
        {
            var request = new FakeHttpRequest(new Uri("http://127.0.0.1/app/folder/file.aspx/extra?var=val"), "GET");
            request.SetApplicationPath("/app");

            var result = request.CurrentExecutionFilePath;

            Assert.That(result, Is.EqualTo("/app/folder/file.aspx"));
        }

        [Test]
        public void FakeHttpRequest_Path_Default_ReturnsCorrectPath()
        {
            var request = new FakeHttpRequest(new Uri("http://127.0.0.1/app/folder/file.aspx/extra?var=val"), "GET");
            request.SetApplicationPath("/app");

            var result = request.Path;

            Assert.That(result, Is.EqualTo("/app/folder/file.aspx/extra"));
        }

        [Test]
        public void FakeHttpRequest_PathInfo_Default_ReturnsPathInfo()
        {
            var request = new FakeHttpRequest(new Uri("http://127.0.0.1/app/folder/file.aspx/extra?var=val"), "GET");
            request.SetApplicationPath("/app");

            var result = request.PathInfo;

            Assert.That(result, Is.EqualTo("/extra"));
        }

        [Test]
        public void FakeHttpRequest_RawUrl_Default_ReturnsUrlWithoutHost()
        {
            var request = new FakeHttpRequest(new Uri("http://127.0.0.1/app/folder/file.aspx/extra?var=val"), "GET");
            request.SetApplicationPath("/app");

            var result = request.RawUrl;

            Assert.That(result, Is.EqualTo("/app/folder/file.aspx/extra?var=val"));
        }
    }
}
