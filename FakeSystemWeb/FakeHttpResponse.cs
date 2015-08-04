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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Routing;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.HttpResponseBase"/>.
    /// </summary>
    public class FakeHttpResponse : HttpResponseBase
    {
        private readonly List<string> cacheItemDependencies;
        private readonly HttpCookieCollection cookies;
        private readonly NameValueCollection headers;

        private Func<string, string> appPathModifier;
        private bool isClientConnected;
        private Stream outputStream;
        private bool supportsAsyncFlush;

        public FakeHttpResponse()
        {
            this.cacheItemDependencies = new List<string>();
            this.cookies = new HttpCookieCollection();
            this.headers = new NameValueCollection();

            this.appPathModifier = p => p;
            this.isClientConnected = true;

            this.StatusCode = 200;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to buffer output and send it after the complete response has finished processing.
        /// </summary>
        /// <returns>true if the output is buffered; otherwise, false.</returns>
        public override bool Buffer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to buffer output and send it after the complete page has finished processing.
        /// </summary>
        /// <returns>true if the output is buffered; otherwise false.</returns>
        public override bool BufferOutput
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the caching policy (such as expiration time, privacy settings, and vary clauses) of the current Web page.
        /// </summary>
        /// <returns>The caching policy of the current response.</returns>
        public override HttpCachePolicyBase Cache
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the Cache-Control HTTP header that matches one of the <see cref="T:System.Web.HttpCacheability" /> enumeration values.
        /// </summary>
        /// <returns>The caching policy of the current response.</returns>
        public override string CacheControl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the cache item dependencies.
        /// </summary>
        public IReadOnlyCollection<string> CacheItemDependencies
        {
            get
            {
                return this.cacheItemDependencies;
            }
        }

        /// <summary>
        /// Gets or sets the HTTP character set of the current response.
        /// </summary>
        /// <returns>The HTTP character set of the current response.</returns>
        public override string Charset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.CancellationToken" /> object that is tripped when the client disconnects.
        /// </summary>
        public override CancellationToken ClientDisconnectedToken
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the content encoding of the current response.
        /// </summary>
        /// <returns>Information about the content encoding of the current response.</returns>
        public override Encoding ContentEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP MIME type of the current response.
        /// </summary>
        /// <returns>The HTTP MIME type of the current response. </returns>
        public override string ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the response cookie collection.
        /// </summary>
        /// <returns>The response cookie collection.</returns>
        public override HttpCookieCollection Cookies
        {
            get
            {
                return this.cookies;
            }
        }

        /// <summary>
        /// Gets or sets the number of minutes before a page that is cached on the client or proxy expires. 
        /// If the user returns to the same page before it expires, the cached version is displayed. 
        /// <see cref="P:System.Web.HttpResponseBase.Expires" /> is provided for compatibility with earlier versions of Active Server Pages (ASP).
        /// </summary>
        /// <returns>The number of minutes before the page expires.</returns>
        public override int Expires
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the absolute date and time at which cached information expires in the cache. 
        /// <see cref="P:System.Web.HttpResponseBase.ExpiresAbsolute" /> is provided for compatibility with earlier versions of Active Server Pages (ASP).
        /// </summary>
        /// <returns>The date and time at which the page expires.</returns>
        public override DateTime ExpiresAbsolute
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a filter object that is used to modify the HTTP entity body before transmission.
        /// </summary>
        /// <returns>An object that acts as the output filter.</returns>
        public override Stream Filter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the collection of response headers.
        /// </summary>
        /// <returns>The response headers.</returns>
        public override NameValueCollection Headers
        {
            get
            {
                return this.headers;
            }
        }

        /// <summary>
        /// Gets or sets the encoding for the header of the current response.
        /// </summary>
        /// <returns>Information about the encoding for the current header.</returns>
        public override Encoding HeaderEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value that indicates whether the client is connected to the server.
        /// </summary>
        /// <returns>true if the client is currently connected; otherwise, false.</returns>
        public override bool IsClientConnected
        {
            get
            {
                return this.isClientConnected;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the client is being redirected to a new location.
        /// </summary>
        /// <returns>true if the value of the location response header differs from the current location; otherwise, false.</returns>
        public override bool IsRequestBeingRedirected
        {
            get
            {
                return !string.IsNullOrEmpty(this.RedirectLocation);
            }
        }

        /// <summary>
        /// Gets a value indicating whether kernel caching is disabled for the current response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if kernel caching is disabled for the current response; otherwise, <c>false</c>.
        /// </value>
        public bool KernelCacheDisabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the object that enables text output to the HTTP response stream.
        /// </summary>
        /// <returns>An object that enables output to the client.</returns>
        public override TextWriter Output
        {
            get;
            set;
        }

        /// <summary>
        /// Enables binary output to the outgoing HTTP content body.
        /// </summary>
        /// <returns>An object that represents the raw contents of the outgoing HTTP content body.</returns>
        public override Stream OutputStream
        {
            get
            {
                return this.outputStream;
            }
        }

        /// <summary>
        /// Gets or sets the value of the HTTP Location header.
        /// </summary>
        /// <returns>The absolute URL of the HTTP Location header.</returns>
        public override string RedirectLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Status value that is returned to the client.
        /// </summary>
        /// <returns>The status of the HTTP output. For information about valid status codes, see HTTP Status Codes on the MSDN Web site.</returns>
        public override string Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP status code of the output that is returned to the client.
        /// </summary>
        /// <returns>The status code of the HTTP output that is returned to the client. For information about valid status codes, see HTTP Status Codes on the MSDN Web site.</returns>
        public override int StatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP status message of the output that is returned to the client.
        /// </summary>
        /// <returns>The status message of the HTTP output that is returned to the client. For information about valid status codes, see HTTP Status Codes on the MSDN Web site.</returns>
        public override string StatusDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that qualifies the status code of the response.
        /// </summary>
        /// <returns>The IIS 7.0 substatus code.</returns>
        public override int SubStatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value that indicates whether the connection supports asynchronous flush operation.
        /// </summary>
        public override bool SupportsAsyncFlush
        {
            get
            {
                return this.supportsAsyncFlush;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to send HTTP content to the client.
        /// </summary>
        /// <returns>true if output is suppressed; otherwise, false.</returns>
        public override bool SuppressContent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that specifies whether forms authentication redirection to the login page should be suppressed.
        /// </summary>
        public override bool SuppressFormsAuthenticationRedirect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that specifies whether IIS 7.0 custom errors are disabled.
        /// </summary>
        /// <returns>true if IIS custom errors are disabled; otherwise, false.</returns>
        public override bool TrySkipIisCustomErrors
        {
            get;
            set;
        }

        /// <summary>
        /// Makes the validity of a cached response dependent on the specified item in the cache.
        /// </summary>
        /// <param name="cacheKey">The key of the item that the cached response is dependent on.</param>
        public override void AddCacheItemDependency(string cacheKey)
        {
            this.cacheItemDependencies.Add(cacheKey);
        }

        /// <summary>
        /// Makes the validity of a cached response dependent on the specified items in the cache.
        /// </summary>
        /// <param name="cacheKeys">A collection that contains the keys of the items that the cached response is dependent on.</param>
        public override void AddCacheItemDependencies(ArrayList cacheKeys)
        {
            this.cacheItemDependencies.AddRange(cacheKeys.Cast<string>());
        }

        /// <summary>
        /// Makes the validity of a cached item dependent on the specified items in the cache.
        /// </summary>
        /// <param name="cacheKeys">An array that contains the keys of the items that the cached response is dependent on.</param>
        public override void AddCacheItemDependencies(string[] cacheKeys)
        {
            this.cacheItemDependencies.AddRange(cacheKeys);
        }

        /// <summary>
        /// Associates cache dependencies with the response that enable the response to be invalidated if it is cached and if the specified dependencies change.
        /// </summary>
        /// <param name="dependencies">A file, cache key, or <see cref="T:System.Web.Caching.CacheDependency" /> object to add to the list of application dependencies.</param>
        public override void AddCacheDependency(params CacheDependency[] dependencies)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a single file name to the collection of file names on which the current response is dependent.
        /// </summary>
        /// <param name="filename">The name of the file to add.</param>
        public override void AddFileDependency(string filename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds file names to the collection of file names on which the current response is dependent.
        /// </summary>
        /// <param name="filenames">The names of the files to add.</param>
        public override void AddFileDependencies(ArrayList filenames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds an array of file names to the collection of file names on which the current response is dependent.
        /// </summary>
        /// <param name="filenames">An array of file names to add.</param>
        public override void AddFileDependencies(string[] filenames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds an HTTP header to the current response. This method is provided for compatibility with earlier versions of ASP.
        /// </summary>
        /// <param name="name">The name of the HTTP header to add <paramref name="value" /> to.</param>
        /// <param name="value">The string to add to the header.</param>
        public override void AddHeader(string name, string value)
        {
            this.AppendHeader(name, value);
        }

        /// <summary>
        /// Adds an HTTP cookie to the HTTP response cookie collection.
        /// </summary>
        /// <param name="cookie">The cookie to add to the response.</param>
        public override void AppendCookie(HttpCookie cookie)
        {
            this.cookies.Set(cookie);
        }

        /// <summary>
        /// Adds an HTTP header to the current response.
        /// </summary>
        /// <param name="name">The name of the HTTP header to add to the current response.</param>
        /// <param name="value">The value of the header.</param>
        public override void AppendHeader(string name, string value)
        {
            this.headers[name] = value;
        }

        /// <summary>
        /// Adds custom log information to the Internet Information Services (IIS) log file.
        /// </summary>
        /// <param name="param">The text to add to the log file.</param>
        /// <exception cref="System.NotSupportedException">This method is not supported.</exception>
        public override void AppendToLog(string param)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds a session ID to the virtual path if the session is using <see cref="P:System.Web.Configuration.SessionStateSection.Cookieless" /> session state, and returns the combined path.
        /// </summary>
        /// <returns>The virtual path, with the session ID inserted.</returns>
        /// <param name="virtualPath">The virtual path of a resource.</param>
        public override string ApplyAppPathModifier(string virtualPath)
        {
            return this.appPathModifier(virtualPath);
        }

        /// <summary>
        /// Sends the currently buffered response to the client.
        /// </summary>
        /// <param name="callback">The callback object.</param>
        /// <param name="state">The response state.</param>
        /// <returns>The asynchronous result object.</returns>
        public override IAsyncResult BeginFlush(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a string of binary characters to the HTTP output stream.
        /// </summary>
        /// <param name="buffer">The binary characters to write to the current response.</param>
        public override void BinaryWrite(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears all headers and content output from the current response.
        /// </summary>
        public override void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, clears all content from the current response.
        /// </summary>
        public override void ClearContent()
        {
            this.Clear();
        }

        /// <summary>
        /// Clears all headers from the current response.
        /// </summary>
        public override void ClearHeaders()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Closes the socket connection to a client.
        /// </summary>
        public override void Close()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disables kernel caching for the current response.
        /// </summary>
        public override void DisableKernelCache()
        {
            this.KernelCacheDisabled = true;
        }

        /// <summary>
        /// Disables IIS user-mode caching for this response.
        /// </summary>
        public override void DisableUserCache()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends all currently buffered output to the client, stops execution of the requested process, and raises the <see cref="E:System.Web.HttpApplication.EndRequest" /> event.
        /// </summary>
        public override void End()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Completes an asynchronous flush operation.
        /// </summary>
        /// <param name="asyncResult">The asynchronous result object.</param>
        public override void EndFlush(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends all currently buffered output to the client.
        /// </summary>
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Appends an HTTP PICS-Label header to the current response.
        /// </summary>
        /// <param name="value">The string to add to the PICS-Label header.</param>
        public override void Pics(string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Redirects a request to the specified URL.
        /// </summary>
        /// <param name="url">The target location.</param>
        public override void Redirect(string url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Redirects a request to the specified URL and specifies whether execution of the current process should terminate.
        /// </summary>
        /// <param name="url">The target location. </param>
        /// <param name="endResponse">true to terminate the current process. </param>
        public override void Redirect(string url, bool endResponse)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Redirects the request to a new URL by using route parameter values.
        /// </summary>
        /// <param name="routeValues">The route parameter values.</param>
        public override void RedirectToRoute(object routeValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Redirects the request to a new URL by using a route name.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        public override void RedirectToRoute(string routeName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Redirects the request to a new URL by using route parameter values.
        /// </summary>
        /// <param name="routeValues">The route parameter values.</param>
        public override void RedirectToRoute(RouteValueDictionary routeValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Redirects the request to a new URL by using route parameter values and a route name.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The route parameter values.</param>
        public override void RedirectToRoute(string routeName, object routeValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Redirects the request to a new URL by using route parameter values and a route name.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The route parameter values.</param>
        public override void RedirectToRoute(string routeName, RouteValueDictionary routeValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a permanent redirection from the requested URL to a new URL by using route parameter values.
        /// </summary>
        /// <param name="routeValues">The route parameter values.</param>
        public override void RedirectToRoutePermanent(object routeValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a permanent redirection from the requested URL to a new URL by using a route name.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        public override void RedirectToRoutePermanent(string routeName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a permanent redirection from the requested URL to a new URL by using route parameter values.
        /// </summary>
        /// <param name="routeValues">The route parameter values.</param>
        public override void RedirectToRoutePermanent(RouteValueDictionary routeValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a permanent redirection from the requested URL to a new URL by using the route parameter values and the name of the route that correspond to the new URL.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The route parameter values.</param>
        public override void RedirectToRoutePermanent(string routeName, object routeValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a permanent redirection from the requested URL to a new URL by using route parameter values and a route name.
        /// </summary>
        /// <param name="routeName">The name of the route.</param>
        /// <param name="routeValues">The route parameter values.</param>
        public override void RedirectToRoutePermanent(string routeName, RouteValueDictionary routeValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a permanent redirect from the requested URL to the specified URL.
        /// </summary>
        /// <param name="url">The location to which the request is redirected.</param>
        public override void RedirectPermanent(string url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs a permanent redirect from the requested URL to the specified URL, and provides the option to complete the response.
        /// </summary>
        /// <param name="url">The location to which the request is redirected.</param>
        /// <param name="endResponse">true to terminate the response; otherwise false. The default is false.</param>
        public override void RedirectPermanent(string url, bool endResponse)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes from the cache all cached items that are associated with the specified path.
        /// </summary>
        /// <param name="path">The virtual absolute path to the items to be removed from the cache.</param>
        public override void RemoveOutputCacheItem(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uses the specified output-cache provider to remove all output-cache artifacts that are associated with the specified path.
        /// </summary>
        /// <param name="path">The virtual absolute path of the items that are removed from the cache. </param>
        /// <param name="providerName">The provider that is used to remove the output-cache artifacts that are associated with the specified path.</param>
        public override void RemoveOutputCacheItem(string path, string providerName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing cookie in the cookie collection.
        /// </summary>
        /// <param name="cookie">The cookie in the collection to be updated.</param>
        public override void SetCookie(HttpCookie cookie)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the specified file to the HTTP response output stream, without buffering it in memory.
        /// </summary>
        /// <param name="filename">The name of the file to write to the HTTP output stream.</param>
        public override void TransmitFile(string filename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the specified part of a file to the HTTP response output stream, without buffering it in memory.
        /// </summary>
        /// <param name="filename">The name of the file to write to the HTTP output stream.</param>
        /// <param name="offset">The position in the file where writing starts.</param>
        /// <param name="length">The number of bytes to write, starting at <paramref name="offset" />.</param>
        public override void TransmitFile(string filename, long offset, long length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a character to an HTTP response output stream.
        /// </summary>
        /// <param name="ch">The character to write to the HTTP output stream.</param>
        public override void Write(char ch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the specified array of characters to the HTTP response output stream.
        /// </summary>
        /// <param name="buffer">The character array to write.</param>
        /// <param name="index">The position in the character array where writing starts.</param>
        /// <param name="count">The number of characters to write, starting at <paramref name="index" />.</param>
        public override void Write(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the specified object to the HTTP response stream.
        /// </summary>
        /// <param name="obj">The object to write to the HTTP output stream.</param>
        public override void Write(object obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the specified string to the HTTP response output stream.
        /// </summary>
        /// <param name="s">The string to write to the HTTP output stream.</param>
        public override void Write(string s)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the contents of the specified file to the HTTP response output stream as a file block.
        /// </summary>
        /// <param name="filename">The name of the file to write to the HTTP output stream.</param>
        public override void WriteFile(string filename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the contents of the specified file to the HTTP response output stream and specifies whether the content is written as a memory block.
        /// </summary>
        /// <param name="filename">The name of the file to write to the current response.</param>
        /// <param name="readIntoMemory">true to write the file into a memory block.</param>
        public override void WriteFile(string filename, bool readIntoMemory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the specified file to the HTTP response output stream.
        /// </summary>
        /// <param name="filename">The name of the file to write to the HTTP output stream.</param>
        /// <param name="offset">The position in the file where writing starts.</param>
        /// <param name="size">The number of bytes to write, starting at <paramref name="offset" />.</param>
        public override void WriteFile(string filename, long offset, long size)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the specified file to the HTTP response output stream.
        /// </summary>
        /// <param name="fileHandle">The file handle of the file to write to the HTTP output stream.</param>
        /// <param name="offset">The position in the file where writing starts.</param>
        /// <param name="size">The number of bytes to write, starting at <paramref name="offset" />.</param>
        public override void WriteFile(IntPtr fileHandle, long offset, long size)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts substitution blocks into the response, which enables dynamic generation of regions for cached output responses.
        /// </summary>
        /// <param name="callback">The method, user control, or object to substitute.</param>
        public override void WriteSubstitution(HttpResponseSubstitutionCallback callback)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the application path modifier.
        /// </summary>
        /// <param name="appPathModifier">The application path modifier.</param>
        public void SetAppPathModifier(Func<string, string> appPathModifier)
        {
            this.appPathModifier = appPathModifier;
        }

        /// <summary>
        /// Sets a value that indicates whether the client is connected to the server.
        /// </summary>
        /// <param name="isClientConnected">if set to <c>true</c> the client is connected to the server.</param>
        public void SetIsClientConnected(bool isClientConnected)
        {
            this.isClientConnected = isClientConnected;
        }

        /// <summary>
        /// Sets the output stream.
        /// </summary>
        /// <param name="outputStream">The output stream.</param>
        public void SetOutputStream(Stream outputStream)
        {
            this.outputStream = outputStream;
        }

        /// <summary>
        /// Sets a value that indicates whether the connection supports asynchronous flush operation.
        /// </summary>
        /// <param name="supportsAsyncFlush">if set to <c>true</c> the connection supports asynchronous flush operation.</param>
        public void SetSupportsAsyncFlush(bool supportsAsyncFlush)
        {
            this.supportsAsyncFlush = supportsAsyncFlush;
        }
    }
}
