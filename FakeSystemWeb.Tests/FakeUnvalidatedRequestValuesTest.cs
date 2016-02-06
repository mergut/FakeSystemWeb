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
    public sealed class FakeUnvalidatedRequestValuesTest
    {
        [Test]
        public void FakeUnvalidatedRequestValues_Cookies_Default_ReturnsRequestCookies()
        {
            AssertPropertiesEquals(r => r.Cookies, u => u.Cookies);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_Files_Default_ReturnsRequestFiles()
        {
            AssertPropertiesEquals(r => r.Files, u => u.Files);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_Form_Default_ReturnsRequestForm()
        {
            AssertPropertiesEquals(r => r.Form, u => u.Form);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_Headers_Default_ReturnsRequestHeaders()
        {
            AssertPropertiesEquals(r => r.Headers, u => u.Headers);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_Path_Default_ReturnsRequestPath()
        {
            AssertPropertiesEquals(r => r.Path, u => u.Path);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_PathInfo_Default_ReturnsRequestPathInfo()
        {
            AssertPropertiesEquals(r => r.PathInfo, u => u.PathInfo);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_QueryString_Default_ReturnsRequestQueryString()
        {
            AssertPropertiesEquals(r => r.QueryString, u => u.QueryString);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_RawUrl_Default_ReturnsRequestRawUrl()
        {
            AssertPropertiesEquals(r => r.RawUrl, u => u.RawUrl);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_Url_Default_ReturnsRequestUrl()
        {
            AssertPropertiesEquals(r => r.Url, u => u.Url);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_Indexer_WithoutExistingKey_ReturnsNull()
        {
            var request = new FakeHttpRequest();
            var values = new FakeUnvalidatedRequestValues(request);

            var result = values["test"];

            Assert.That(result, Is.Null);
        }

        [Test]
        public void FakeUnvalidatedRequestValues_Indexer_WithAllValuesFilled_ReturnsQueryStringValue()
        {
            var request = GetIndexerConfiguredRequest();
            var values = new FakeUnvalidatedRequestValues(request);

            var result = values["key"];

            Assert.That(result, Is.EqualTo("QueryStringValue"));
        }
        
        [Test]
        public void FakeUnvalidatedRequestValues_Indexer_WithoutQueryStringValue_ReturnsFormValue()
        {
            var request = GetIndexerConfiguredRequest();
            request.QueryString.Remove("key");
            var values = new FakeUnvalidatedRequestValues(request);

            var result = values["key"];

            Assert.That(result, Is.EqualTo("FormValue"));
        }

        [Test]
        public void FakeUnvalidatedRequestValues_Indexer_WithoutFormValue_ReturnsCookieValue()
        {
            var request = GetIndexerConfiguredRequest();
            request.QueryString.Remove("key");
            request.Form.Remove("key");
            var values = new FakeUnvalidatedRequestValues(request);

            var result = values["key"];

            Assert.That(result, Is.EqualTo("CookieValue"));
        }

        [Test]
        public void FakeUnvalidatedRequestValues_Indexer_WithoutCookieValue_ReturnsServerValue()
        {
            var request = GetIndexerConfiguredRequest();
            request.QueryString.Remove("key");
            request.Form.Remove("key");
            request.Cookies.Remove("key");
            var values = new FakeUnvalidatedRequestValues(request);

            var result = values["key"];

            Assert.That(result, Is.EqualTo("ServerValue"));
        }

        private static void AssertPropertiesEquals<TReturn>(
            Func<FakeHttpRequest, TReturn> requestPropertySelector,
            Func<FakeUnvalidatedRequestValues, TReturn> unvalidatedPropertySelector)
        {
            var request = new FakeHttpRequest();
            var values = new FakeUnvalidatedRequestValues(request);
            var expected = requestPropertySelector(request);

            var result = unvalidatedPropertySelector(values);

            Assert.That(result, Is.EqualTo(expected));
        }

        private static FakeHttpRequest GetIndexerConfiguredRequest()
        {
            var request = new FakeHttpRequest();

            request.QueryString.Add("key", "QueryStringValue");
            request.Form.Add("key", "FormValue");
            request.Cookies.Add(new System.Web.HttpCookie("key", "CookieValue"));
            request.ServerVariables.Add("key", "ServerValue");

            return request;
        }
    }
}
