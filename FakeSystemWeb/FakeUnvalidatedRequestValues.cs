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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.UnvalidatedRequestValuesBase"/>.
    /// </summary>
    public class FakeUnvalidatedRequestValues : UnvalidatedRequestValuesBase
    {
        private readonly HttpRequestBase request;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeUnvalidatedRequestValues"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public FakeUnvalidatedRequestValues(HttpRequestBase request)
        {
            this.request = request;
        }

        /// <summary>
        /// Gets the collection of cookies that the client sent, without triggering ASP.NET request validation.
        /// </summary>
        public override HttpCookieCollection Cookies
        {
            get
            {
                return this.request.Cookies;
            }
        }

        /// <summary>
        /// Gets the collection of files that the client uploaded, without triggering ASP.NET request validation.
        /// </summary>
        public override HttpFileCollectionBase Files
        {
            get
            {
                return this.request.Files;
            }
        }

        /// <summary>
        /// Gets the collection of form variables that the client submitted, without triggering ASP.NET request validation.
        /// </summary>
        public override NameValueCollection Form
        {
            get
            {
                return this.request.Form;
            }
        }

        /// <summary>
        /// Gets the collection of HTTP headers that the client sent, without triggering ASP.NET request validation.
        /// </summary>
        public override NameValueCollection Headers
        {
            get
            {
                return this.request.Headers;
            }
        }

        /// <summary>
        /// Gets the virtual path of the requested resource without triggering ASP.NET request validation.
        /// </summary>
        public override string Path
        {
            get
            {
                return this.request.Path;
            }
        }

        /// <summary>
        /// Gets additional path information for a resource that has a URL extension, without triggering ASP.NET request validation.
        /// </summary>
        public override string PathInfo
        {
            get
            {
                return this.request.PathInfo;
            }
        }

        /// <summary>
        /// Gets the collection of HTTP query string variables that the client submitted, without triggering ASP.NET request validation.
        /// </summary>
        public override NameValueCollection QueryString
        {
            get
            {
                return this.request.QueryString;
            }
        }

        /// <summary>
        /// Gets the part of the requested URL that follows the website name, without triggering ASP.NET request validation.
        /// </summary>
        public override string RawUrl
        {
            get
            {
                return this.request.RawUrl;
            }
        }

        /// <summary>
        /// Gets the URL data for the request without triggering request validation.
        /// </summary>
        public override Uri Url
        {
            get
            {
                return this.request.Url;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the specified object from the <see cref="P:System.Web.HttpRequest.Form" />, <see cref="P:System.Web.HttpRequest.Cookies" />, <see cref="P:System.Web.HttpRequest.QueryString" />, or <see cref="P:System.Web.HttpRequest.ServerVariables" /> collection, without triggering ASP.NET request validation.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The object specified by the field parameter.</returns>
        public override string this[string field]
        {
            get
            {
                string queryStringValue = this.QueryString[field];
                if (queryStringValue != null)
                {
                    return queryStringValue;
                }

                string formValue = this.Form[field];
                if (formValue != null)
                {
                    return formValue;
                }

                HttpCookie cookie = this.Cookies[field];
                if (cookie != null)
                {
                    return cookie.Value;
                }

                string serverVarValue = this.request.ServerVariables[field];
                if (serverVarValue != null)
                {
                    return serverVarValue;
                }

                return null;
            }
        }
    }
}
