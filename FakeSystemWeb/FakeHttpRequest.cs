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
    using System.Collections.Specialized;
    using System.IO;
    using System.Security.Authentication.ExtendedProtection;
    using System.Security.Principal;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.HttpRequestBase"/>.
    /// </summary>
    public class FakeHttpRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection cookies;
        private readonly NameValueCollection form;
        private readonly string filePath;
        private readonly NameValueCollection headers;
        private readonly string httpMethod;
        private readonly string pathInfo;
        private readonly NameValueCollection queryString;
        private readonly NameValueCollection serverVariables;
        private readonly Uri url;

        private string[] acceptTypes;
        private string applicationPath;
        private string anonymousID;
        private HttpBrowserCapabilitiesBase browser;
        private HttpClientCertificate clientCertificate;
        private int contentLength;
        private HttpFileCollectionBase files;
        private ChannelBinding httpChannelBinding;
        private Stream inputStream;
        private string physicalApplicationPath;
        private string requestType;
        private CancellationToken timedOutToken;
        private UnvalidatedRequestValuesBase unvalidated;
        private Uri urlReferrer;
        private string userAgent;
        private string userHostAddress;
        private string userHostName;
        private string[] userLanguages;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpRequest"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        public FakeHttpRequest(Uri url, string httpMethod = "GET")
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            if (!url.IsAbsoluteUri)
            {
                throw new ArgumentException("The url must be absolute.", "url");
            }

            this.url = url;
            this.httpMethod = httpMethod;

            this.cookies = new HttpCookieCollection();
            this.form = new NameValueCollection();
            this.headers = new NameValueCollection();
            this.queryString = new NameValueCollection();
            this.serverVariables = new NameValueCollection();

            this.acceptTypes = new string[0];
            this.applicationPath = "/";
            this.physicalApplicationPath = Environment.CurrentDirectory;

            // Extract pathinfo.
            var match = Regex.Match(url.AbsolutePath, @"^(?<filePath>[^.]+\.\w+)(?<pathInfo>/.+)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            if (match.Success)
            {
                this.filePath = match.Groups["filePath"].Value;
                this.pathInfo = match.Groups["pathInfo"].Value;
            }
            else
            {
                this.filePath = url.AbsolutePath;
                this.pathInfo = string.Empty;
            }
        }

        /// <summary>
        /// Gets an array of client-supported MIME accept types.
        /// </summary>
        public override string[] AcceptTypes
        {
            get
            {
                return this.acceptTypes;
            }
        }

        /// <summary>
        /// Gets the virtual root path of the ASP.NET application on the server.
        /// </summary>
        public override string ApplicationPath
        {
            get
            {
                return this.applicationPath;
            }
        }

        /// <summary>
        /// Gets the anonymous identifier for the user, if it is available.
        /// </summary>
        public override string AnonymousID
        {
            get
            {
                return this.anonymousID;
            }
        }

        /// <summary>
        /// Gets the virtual path of the application root and makes it relative by using the tilde (~) notation for the application root (as in "~/page.aspx").
        /// </summary>
        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return VirtualPathUtility.ToAppRelative(this.CurrentExecutionFilePath, this.ApplicationPath);
            }
        }

        /// <summary>
        /// Gets information about the requesting client's browser capabilities.
        /// </summary>
        public override HttpBrowserCapabilitiesBase Browser
        {
            get
            {
                return this.browser;
            }
        }

        /// <summary>
        /// Gets the current request's client security certificate.
        /// </summary>
        public override HttpClientCertificate ClientCertificate
        {
            get
            {
                return this.clientCertificate;
            }
        }

        /// <summary>
        /// Gets or sets the character set of the data that is provided by the client.
        /// </summary>
        public override Encoding ContentEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the length, in bytes, of content that was sent by the client.
        /// </summary>
        public override int ContentLength
        {
            get
            {
                return this.contentLength;
            }
        }

        /// <summary>
        /// Gets or sets the MIME content type of the request.
        /// </summary>
        public override string ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the collection of cookies that were sent by the client.
        /// </summary>
        public override HttpCookieCollection Cookies
        {
            get
            {
                return this.cookies;
            }
        }

        /// <summary>
        /// Gets the virtual path of the current request.
        /// </summary>
        public override string CurrentExecutionFilePath
        {
            get
            {
                return this.FilePath;
            }
        }

        /// <summary>
        /// Gets the extension of the file name that is specified in the <see cref="P:CurrentExecutionFilePath" /> property.
        /// </summary>
        public override string CurrentExecutionFilePathExtension
        {
            get
            {
                return System.IO.Path.GetExtension(this.CurrentExecutionFilePath);
            }
        }

        /// <summary>
        /// Gets the virtual path of the current request.
        /// </summary>
        public override string FilePath
        {
            get
            {
                return this.filePath;
            }
        }

        /// <summary>
        /// Gets the collection of files that were uploaded by the client, in multipart MIME format.
        /// </summary>
        public override HttpFileCollectionBase Files
        {
            get
            {
                return this.files;
            }
        }

        /// <summary>
        /// Gets or sets the filter to use when the current input stream is being read.
        /// </summary>
        public override Stream Filter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the collection of form variables that were sent by the client.
        /// </summary>
        public override NameValueCollection Form
        {
            get
            {
                return this.form;
            }
        }

        /// <summary>
        /// Gets the collection of HTTP headers that were sent by the client.
        /// </summary>
        public override NameValueCollection Headers
        {
            get
            {
                return this.headers;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Security.Authentication.ExtendedProtection.ChannelBinding" /> object of the current <see cref="T:System.Web.HttpWorkerRequest" /> instance.
        /// </summary>
        public override ChannelBinding HttpChannelBinding
        {
            get
            {
                return this.httpChannelBinding;
            }
        }

        /// <summary>
        /// Gets the HTTP data-transfer method (such as GET, POST, or HEAD) that was used by the client.
        /// </summary>
        public override string HttpMethod
        {
            get
            {
                return this.httpMethod;
            }
        }

        /// <summary>
        /// Gets the contents of the incoming HTTP entity body.
        /// </summary>
        public override Stream InputStream
        {
            get
            {
                return this.inputStream;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the request has been authenticated.
        /// </summary>
        /// <exception cref="System.NotSupportedException">This property is not supported.</exception>
        public override bool IsAuthenticated
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the request is from the local computer.
        /// </summary>
        public override bool IsLocal
        {
            get
            {
                return this.Url.IsLoopback;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the HTTP connection uses secure sockets (HTTPS protocol).
        /// </summary>
        public override bool IsSecureConnection
        {
            get
            {
                return this.Url.Scheme == Uri.UriSchemeHttps;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Security.Principal.WindowsIdentity" /> type for the current user.
        /// </summary>
        /// <exception cref="System.NotSupportedException">This property is not supported.</exception>
        public override WindowsIdentity LogonUserIdentity
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets a combined collection of <see cref="P:System.Web.HttpRequest.QueryString" />, <see cref="P:System.Web.HttpRequest.Form" />, 
        /// <see cref="P:System.Web.HttpRequest.Cookies" /> and <see cref="P:System.Web.HttpRequest.ServerVariables" /> items.
        /// </summary>
        public override NameValueCollection Params
        {
            get
            {
                return this.GetParams();
            }
        }

        /// <summary>
        /// Gets the virtual path of the current request.
        /// </summary>
        public override string Path
        {
            get
            {
                return this.url.AbsolutePath;
            }
        }

        /// <summary>
        /// Gets additional path information for a resource that has a URL extension.
        /// </summary>
        public override string PathInfo
        {
            get
            {
                return this.pathInfo;
            }
        }

        /// <summary>
        /// Gets the physical file-system path of the current application's root directory.
        /// </summary>
        public override string PhysicalApplicationPath
        {
            get
            {
                return this.physicalApplicationPath;
            }
        }

        /// <summary>
        /// Gets the physical file-system path of the requested resource.
        /// </summary>
        public override string PhysicalPath
        {
            get
            {
                return this.MapPath(this.AppRelativeCurrentExecutionFilePath);
            }
        }

        /// <summary>
        /// Gets the collection of HTTP query-string variables.
        /// </summary>
        public override NameValueCollection QueryString
        {
            get
            {
                return this.queryString;
            }
        }

        /// <summary>
        /// Gets the complete URL of the current request.
        /// </summary>
        public override string RawUrl
        {
            get
            {
                return this.url.PathAndQuery;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.Routing.RequestContext" /> instance of the current request.
        /// </summary>
        public override RequestContext RequestContext
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP data-transfer method (GET or POST) that was used by the client.
        /// </summary>
        public override string RequestType
        {
            get
            {
                if (this.requestType != null)
                {
                    return this.requestType;
                }

                return this.HttpMethod;
            }

            set
            {
                this.requestType = value;
            }
        }

        /// <summary>
        /// Gets a collection of Web server variables.
        /// </summary>
        public override NameValueCollection ServerVariables
        {
            get
            {
                return this.serverVariables;
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.CancellationToken" /> object that is tripped when a request times out.
        /// </summary>
        public override CancellationToken TimedOutToken
        {
            get
            {
                return this.timedOutToken;
            }
        }

        /// <summary>
        /// Gets the number of bytes in the current input stream.
        /// </summary>
        public override int TotalBytes
        {
            get
            {
                return this.InputStream != null ? (int)this.InputStream.Length : 0;
            }
        }

        /// <summary>
        /// Provides access to HTTP request values without triggering request validation.
        /// </summary>
        public override UnvalidatedRequestValuesBase Unvalidated
        {
            get
            {
                if (this.unvalidated == null)
                {
                    this.unvalidated = new FakeUnvalidatedRequestValues(this);
                }

                return this.unvalidated;
            }
        }

        /// <summary>
        /// Gets information about the URL of the current request.
        /// </summary>
        public override Uri Url
        {
            get
            {
                return this.url;
            }
        }

        /// <summary>
        /// Gets information about the URL of the client request that linked to the current URL.
        /// </summary>
        public override Uri UrlReferrer
        {
            get
            {
                return this.urlReferrer;
            }
        }

        /// <summary>
        /// Gets the complete user-agent string of the client.
        /// </summary>
        public override string UserAgent
        {
            get
            {
                return this.userAgent;
            }
        }

        /// <summary>
        /// Gets the IP host address of the client.
        /// </summary>
        public override string UserHostAddress
        {
            get
            {
                return this.userHostAddress;
            }
        }

        /// <summary>
        /// Gets the DNS name of the client.
        /// </summary>
        public override string UserHostName
        {
            get
            {
                return this.userHostName;
            }
        }

        /// <summary>
        /// Gets a sorted array of client language preferences.
        /// </summary>
        public override string[] UserLanguages
        {
            get
            {
                return this.userLanguages;
            }
        }

        /// <summary>
        /// Gets the specified object from the <see cref="P:System.Web.HttpRequestBase.QueryString" />, <see cref="P:System.Web.HttpRequestBase.Form" />, 
        /// <see cref="P:System.Web.HttpRequestBase.Cookies" /> or <see cref="P:System.Web.HttpRequestBase.ServerVariables" /> collections.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The <see cref="P:System.Web.HttpRequestBase.QueryString" />, <see cref="P:System.Web.HttpRequestBase.Form" />, 
        /// <see cref="P:System.Web.HttpRequestBase.Cookies" /> or <see cref="P:System.Web.HttpRequestBase.ServerVariables" /> 
        /// collection member that is specified by key. If the specified key value is not found, null is returned.
        /// </returns>
        public override string this[string key]
        {
            get
            {
                string val = this.QueryString[key];
                if (val != null)
                {
                    return val;
                }

                val = this.Form[key];
                if (val != null)
                {
                    return val;
                }

                HttpCookie httpCookie = this.Cookies[key];
                if (httpCookie != null)
                {
                    return httpCookie.Value;
                }

                val = this.ServerVariables[key];
                if (val != null)
                {
                    return val;
                }

                return null;
            }
        }

        /// <summary>Maps the specified virtual path to a physical path.</summary>
        /// <returns>The physical path on the server specified by <paramref name="virtualPath" />.</returns>
        /// <param name="virtualPath">The virtual path (absolute or relative) for the current request. </param>
        /// <exception cref="T:System.Web.HttpException">No <see cref="T:System.Web.HttpContext" /> object is defined for the request. </exception>
        public override string MapPath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                throw new ArgumentNullException("virtualPath");
            }

            if (!virtualPath.StartsWith("~/"))
            {
                throw new ArgumentException("Invalid virtual path.", "virtualPath");
            }

            return this.PhysicalApplicationPath + virtualPath.Substring(1).Replace('/', '\\');
        }

        /// <summary>Maps the specified virtual path to a physical path.</summary>
        /// <returns>The physical path on the server.</returns>
        /// <param name="virtualPath">The virtual path (absolute or relative) for the current request. </param>
        /// <param name="baseVirtualDir">The virtual base directory path used for relative resolution. </param>
        /// <param name="allowCrossAppMapping">true to indicate that <paramref name="virtualPath" /> may belong to another application; otherwise, false. </param>
        /// <exception cref="T:System.Web.HttpException">
        ///   <paramref name="allowCrossMapping" /> is false and <paramref name="virtualPath" /> belongs to another application. </exception>
        /// <exception cref="T:System.Web.HttpException">No <see cref="T:System.Web.HttpContext" /> object is defined for the request. </exception>
        public override string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the array of client-supported MIME accept types.
        /// </summary>
        /// <param name="acceptTypes">The MIME accept types.</param>
        public void SetAcceptTypes(string[] acceptTypes)
        {
            this.acceptTypes = acceptTypes;
        }

        /// <summary>
        /// Sets the virtual root path of the ASP.NET application on the server.
        /// </summary>
        /// <param name="applicationPath">The application path.</param>
        public void SetApplicationPath(string applicationPath)
        {
            this.applicationPath = applicationPath;
        }

        /// <summary>
        /// Sets the anonymous identifier for the user.
        /// </summary>
        /// <param name="anonymousID">The anonymous identifier.</param>
        public void SetAnonymousID(string anonymousID)
        {
            this.anonymousID = anonymousID;
        }

        /// <summary>
        /// Sets information about the requesting client's browser capabilities.
        /// </summary>
        /// <param name="browser">The browser capabilities.</param>
        public void SetBrowser(HttpBrowserCapabilitiesBase browser)
        {
            this.browser = browser;
        }

        /// <summary>
        /// Sets the client security certificate.
        /// </summary>
        /// <param name="clientCertificate">The client security certificate.</param>
        public void SetClientCertificate(HttpClientCertificate clientCertificate)
        {
            this.clientCertificate = clientCertificate;
        }

        /// <summary>
        /// Sets the length, in bytes, of content that was sent by the client.
        /// </summary>
        /// <param name="contentLength">Length of the content.</param>
        public void SetContentLength(int contentLength)
        {
            this.contentLength = contentLength;
        }

        /// <summary>
        /// Sets the collection of files that were uploaded by the client.
        /// </summary>
        /// <param name="files">The files collection.</param>
        public void SetFiles(HttpFileCollectionBase files)
        {
            this.files = files;
        }

        /// <summary>
        /// Sets the <see cref="T:System.Security.Authentication.ExtendedProtection.ChannelBinding" /> object.
        /// </summary>
        /// <param name="httpChannelBinding">The channel binding object.</param>
        public void SetHttpChannelBinding(ChannelBinding httpChannelBinding)
        {
            this.httpChannelBinding = httpChannelBinding;
        }

        /// <summary>
        /// Sets the contents of the incoming HTTP entity body.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        public void SetInputStream(Stream inputStream)
        {
            this.inputStream = inputStream;
        }

        /// <summary>
        /// Sets the physical file-system path of the current application's root directory.
        /// </summary>
        /// <param name="physicalApplicationPath">The physical application path.</param>
        public void SetPhysicalApplicationPath(string physicalApplicationPath)
        {
            this.physicalApplicationPath = physicalApplicationPath;
        }

        /// <summary>
        /// Sets the request timeout cancellation token.
        /// </summary>
        /// <param name="timedOutToken">The timed out token.</param>
        public void SetTimedOutToken(CancellationToken timedOutToken)
        {
            this.timedOutToken = timedOutToken;
        }

        /// <summary>
        /// Sets the URL of the client request that linked to the current URL.
        /// </summary>
        /// <param name="urlReferrer">The URL referrer.</param>
        public void SetUrlReferrer(Uri urlReferrer)
        {
            this.urlReferrer = urlReferrer;
        }

        /// <summary>
        /// Sets the complete user-agent string of the client.
        /// </summary>
        /// <param name="userAgent">The user agent.</param>
        public void SetUserAgent(string userAgent)
        {
            this.userAgent = userAgent;
        }

        /// <summary>
        /// Sets the IP host address of the client.
        /// </summary>
        /// <param name="userHostAddress">The IP host address of the client.</param>
        public void SetUserHostAddress(string userHostAddress)
        {
            this.userHostAddress = userHostAddress;
        }

        /// <summary>
        /// Sets the DNS name of the client.
        /// </summary>
        /// <param name="userHostName">The DNS name of the client.</param>
        public void SetUserHostName(string userHostName)
        {
            this.userHostName = userHostName;
        }

        /// <summary>
        /// Sets an array of client language preferences.
        /// </summary>
        /// <param name="userLanguages">The user languages.</param>
        public void SetUserLanguages(string[] userLanguages)
        {
            this.userLanguages = userLanguages;
        }

        private NameValueCollection GetParams()
        {
            var combined = new NameValueCollection();
            combined.Add(this.QueryString);
            combined.Add(this.Form);

            foreach (HttpCookie cookie in this.Cookies)
            {
                combined.Add(cookie.Name, cookie.Value);
            }

            combined.Add(this.ServerVariables);

            return combined;
        }
    }
}
