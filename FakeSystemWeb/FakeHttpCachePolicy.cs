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
    using System.Web;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.HttpCachePolicyBase"/>.
    /// </summary>
    public class FakeHttpCachePolicy : HttpCachePolicyBase
    {
        private readonly List<string> cacheExtensions;
        private readonly List<Tuple<HttpCacheValidateHandler, object>> validationCallbacks;
        private readonly HttpCacheVaryByContentEncodings varyByContentEncodings;
        private readonly HttpCacheVaryByHeaders varyByHeaders;
        private readonly HttpCacheVaryByParams varyByParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpCachePolicy"/> class.
        /// </summary>
        public FakeHttpCachePolicy()
        {
            this.cacheExtensions = new List<string>();
            this.validationCallbacks = new List<Tuple<HttpCacheValidateHandler, object>>();
            this.varyByContentEncodings = new HttpCacheVaryByContentEncodings();
            this.varyByHeaders = new HttpCacheVaryByHeaders();
            this.varyByParams = new HttpCacheVaryByParams();
        }

        /// <summary>
        /// Gets a value indicating whether the response is available in the browser history cache, regardless of the <see cref="T:System.Web.HttpCacheability" /> setting made on the server.
        /// </summary>
        public bool? AllowResponseInBrowserHistory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the specified <see cref="T:System.Web.HttpCacheability" /> value.
        /// </summary>
        public HttpCacheability? Cacheability
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of cache extension values.
        /// </summary>
        public IReadOnlyCollection<string> CacheExtensions
        {
            get
            {
                return this.cacheExtensions;
            }
        }

        /// <summary>
        /// Gets the ETag HTTP header.
        /// </summary>
        public string ETag
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the absolute expiration time.
        /// </summary>
        public DateTime? Expires
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the ETag HTTP header is generated based on the time stamps of the handler's file dependencies.
        /// </summary>
        public bool GenerateEtagFromFiles
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the Last-Modified HTTP header is generated based on the time stamps of the handler's file dependencies.
        /// </summary>
        public bool GenerateLastModifiedFromFiles
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Last-Modified HTTP header value.
        /// </summary>
        public DateTime? LastModified
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Cache-Control: max-age HTTP header value.
        /// </summary>
        public TimeSpan? MaxAge
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether all origin-server caching is stopped for the current response.
        /// </summary>
        public bool NoServerCaching
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the Cache-Control: no-store HTTP header is set.
        /// </summary>
        public bool NoStore
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the Cache-Control: no-transform HTTP header is set.
        /// </summary>
        public bool NoTransforms
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the response contains the vary:* header when caching varies by parameters.
        /// </summary>
        public bool? OmitVaryStar
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Cache-Control: s-maxage HTTP header value.
        /// </summary>
        public TimeSpan? ProxyMaxAge
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Cache-Control HTTP header revalidation value.
        /// </summary>
        public HttpCacheRevalidation? Revalidation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the cache expiration is set to absolute or sliding.
        /// </summary>
        public bool? SlidingExpiration
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of registered validation callbacks.
        /// </summary>
        public IReadOnlyCollection<Tuple<HttpCacheValidateHandler, object>> ValidationCallbacks
        {
            get
            {
                return this.validationCallbacks;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the ASP.NET cache should ignore HTTP Cache-Control headers that are sent by the client that invalidate the cache.
        /// </summary>
        public bool? ValidUntilExpires
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of Content-Encoding headers that are used to vary the output cache.
        /// </summary>
        public override HttpCacheVaryByContentEncodings VaryByContentEncodings
        {
            get
            {
                return this.varyByContentEncodings;
            }
        }

        /// <summary>
        /// Gets the text string by which cached output responses are varied.
        /// </summary> 
        public string VaryByCustom
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of all HTTP headers that are used to vary cache output.
        /// </summary>
        public override HttpCacheVaryByHeaders VaryByHeaders
        {
            get
            {
                return this.varyByHeaders;
            }
        }

        /// <summary>
        /// Gets the list of parameters that are received by an HTTP GET or POST verb that affect caching.
        /// </summary>
        public override HttpCacheVaryByParams VaryByParams
        {
            get
            {
                return this.varyByParams;
            }
        }

        /// <summary>
        /// Registers a validation callback for the current response.
        /// </summary>
        /// <param name="handler">The object that will handle the request.</param>
        /// <param name="data">The user-supplied data that is passed to the <see cref="M:System.Web.HttpCachePolicyBase.AddValidationCallback(System.Web.HttpCacheValidateHandler,System.Object)" /> delegate.</param>
        public override void AddValidationCallback(HttpCacheValidateHandler handler, object data)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            this.validationCallbacks.Add(new Tuple<HttpCacheValidateHandler, object>(handler, data));
        }

        /// <summary>
        /// Appends the specified text to the Cache-Control HTTP header.
        /// </summary>
        /// <param name="extension">The text to append to the Cache-Control header.</param>
        public override void AppendCacheExtension(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            this.cacheExtensions.Add(extension);
        }

        /// <summary>
        /// Makes the response available in the browser history cache, regardless of the <see cref="T:System.Web.HttpCacheability" /> setting made on the server.
        /// </summary>
        /// <param name="allow">true to direct the client browser to store responses in the browser history cache; otherwise false.</param>
        public override void SetAllowResponseInBrowserHistory(bool allow)
        {
            this.AllowResponseInBrowserHistory = allow;
        }

        /// <summary>
        /// Sets the Cache-Control header to the specified <see cref="T:System.Web.HttpCacheability" /> value.
        /// </summary>
        /// <param name="cacheability">The <see cref="T:System.Web.HttpCacheability" /> enumeration value to set the header to.</param>
        public override void SetCacheability(HttpCacheability cacheability)
        {
            this.Cacheability = cacheability;
        }

        /// <summary>
        /// Sets the Cache-Control header to the specified <see cref="T:System.Web.HttpCacheability" /> value and appends an extension to the directive.
        /// </summary>
        /// <param name="cacheability">The <see cref="T:System.Web.HttpCacheability" /> enumeration value to set the header to.</param>
        /// <param name="field">The cache-control extension to add to the header.</param>
        public override void SetCacheability(HttpCacheability cacheability, string field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            this.Cacheability = cacheability;
            this.cacheExtensions.Add(field);
        }

        /// <summary>
        /// Sets the ETag HTTP header to the specified string.
        /// </summary>
        /// <param name="etag">The text to use for the ETag header.</param>
        public override void SetETag(string etag)
        {
            if (etag == null)
            {
                throw new ArgumentNullException(nameof(etag));
            }

            if (this.ETag != null)
            {
                throw new InvalidOperationException("ETag is already set.");
            }

            if (this.GenerateEtagFromFiles)
            {
                throw new InvalidOperationException("Cannot set ETag and also generate it from files.");
            }

            this.ETag = etag;
        }

        /// <summary>
        /// Sets the ETag HTTP header based on the time stamps of the handler's file dependencies.
        /// </summary>
        public override void SetETagFromFileDependencies()
        {
            if (this.ETag != null)
            {
                throw new InvalidOperationException("Cannot set ETag and also generate it from files.");
            }

            this.GenerateEtagFromFiles = true;
        }

        /// <summary>
        /// Sets the Expires HTTP header to an absolute date and time.
        /// </summary>
        /// <param name="date">The absolute expiration time.</param>
        public override void SetExpires(DateTime date)
        {
            this.Expires = date;
        }

        /// <summary>
        /// Sets the Last-Modified HTTP header to the specified date and time.
        /// </summary>
        /// <param name="date">The date-time value to set the Last-Modified header to.</param>
        public override void SetLastModified(DateTime date)
        {
            this.LastModified = date;
        }

        /// <summary>
        /// Sets the Last-Modified HTTP header based on the time stamps of the handler's file dependencies.
        /// </summary>
        public override void SetLastModifiedFromFileDependencies()
        {
            this.GenerateLastModifiedFromFiles = true;
        }

        /// <summary>
        /// Sets the Cache-Control: max-age HTTP header to the specified time span.
        /// </summary>
        /// <param name="delta">The time span to set the Cache-Control: max-age header to.</param>
        public override void SetMaxAge(TimeSpan delta)
        {
            if (delta < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(delta));
            }

            this.MaxAge = delta;
        }

        /// <summary>
        /// Stops all origin-server caching for the current response.
        /// </summary>
        public override void SetNoServerCaching()
        {
            this.NoServerCaching = true;
        }

        /// <summary>
        /// Sets the Cache-Control: no-store HTTP header.
        /// </summary>
        public override void SetNoStore()
        {
            this.NoStore = true;
        }

        /// <summary>
        /// Sets the Cache-Control: no-transform HTTP header.
        /// </summary>
        public override void SetNoTransforms()
        {
            this.NoTransforms = true;
        }

        /// <summary>
        /// Specifies whether the response contains the vary:* header when caching varies by parameters.
        /// </summary>
        /// <param name="omit">true to direct the <see cref="T:System.Web.HttpCachePolicy" /> object not to use the * value for its <see cref="P:System.Web.HttpCachePolicy.VaryByHeaders" /> property; otherwise, false.</param>
        public override void SetOmitVaryStar(bool omit)
        {
            this.OmitVaryStar = omit;
        }

        /// <summary>
        /// Sets the Cache-Control: s-maxage HTTP header to the specified time span.
        /// </summary>
        /// <param name="delta">The time span to set the Cache-Control: s-maxage header to.</param>
        public override void SetProxyMaxAge(TimeSpan delta)
        {
            if (delta < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(delta));
            }

            this.ProxyMaxAge = delta;
        }

        /// <summary>
        /// Sets the Cache-Control HTTP header to either the must-revalidate or the proxy-revalidate directives, based on the specified enumeration value.
        /// </summary>
        /// <param name="revalidation">The <see cref="T:System.Web.HttpCacheRevalidation" /> enumeration value to set the Cache-Control header to.</param>
        public override void SetRevalidation(HttpCacheRevalidation revalidation)
        {
            this.Revalidation = revalidation;
        }

        /// <summary>
        /// Sets cache expiration to absolute or sliding.
        /// </summary>
        /// <param name="slide">true to set a sliding cache expiration, or false to set an absolute cache expiration.</param>
        public override void SetSlidingExpiration(bool slide)
        {
            this.SlidingExpiration = slide;
        }

        /// <summary>
        /// Specifies whether the ASP.NET cache should ignore HTTP Cache-Control headers that are sent by the client that invalidate the cache.
        /// </summary>
        /// <param name="validUntilExpires">true to specify that ASP.NET should ignore Cache-Control invalidation headers; otherwise, false.</param>
        public override void SetValidUntilExpires(bool validUntilExpires)
        {
            this.ValidUntilExpires = validUntilExpires;
        }

        /// <summary>
        /// Specifies a text string to vary cached output responses by.
        /// </summary>
        /// <param name="custom">The text string to vary cached output by.</param>
        public override void SetVaryByCustom(string custom)
        {
            if (custom == null)
            {
                throw new ArgumentNullException(nameof(custom));
            }

            if (this.VaryByCustom != null)
            {
                throw new InvalidOperationException("VaryByCustom is already set.");
            }

            this.VaryByCustom = custom;
        }
    }
}
