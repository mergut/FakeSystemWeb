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
    using System.Globalization;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Configuration;
    using System.Web.Instrumentation;
    using System.Web.Profile;
    using System.Web.SessionState;
    using System.Web.WebSockets;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.HttpContextBase"/>.
    /// </summary>
    public class FakeHttpContext : HttpContextBase
    {
        private readonly FakeHttpApplicationState applicationState;
        private readonly List<Exception> errors;
        private readonly Stack<IHttpHandler> handlerStack;
        private readonly Hashtable items;
        private readonly PageInstrumentationService pageInstrumentation;
        private readonly FakeHttpRequest request;
        private readonly FakeHttpResponse response;
        private readonly FakeHttpServerUtility server;
        private readonly FakeHttpSessionState session;
        private readonly DateTime timestampUtc;

        private IHttpHandler currentHandler;
        private RequestNotification currentNotification;
        private IHttpHandler handler;
        private bool isCustomErrorEnabled;
        private bool isDebuggingEnabled;
        private bool isPostNotification;
        private bool isWebSocketRequest;
        private bool isWebSocketRequestUpgrading;
        private ProfileBase profile;
        private IHttpHandler remapHandler;
        private string webSocketNegotiatedProtocol;
        private IList<string> webSocketNegotiatedProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpContext"/> class.
        /// </summary>
        public FakeHttpContext()
            : this(new FakeHttpRequest(), new FakeHttpResponse(), new FakeHttpSessionState())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpContext"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public FakeHttpContext(Uri url)
            : this(new FakeHttpRequest(url))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public FakeHttpContext(FakeHttpRequest request)
            : this(request, new FakeHttpResponse(), new FakeHttpSessionState())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        public FakeHttpContext(FakeHttpRequest request, FakeHttpResponse response)
            : this(request, response, new FakeHttpSessionState())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="session">The session.</param>
        public FakeHttpContext(FakeHttpRequest request, FakeHttpResponse response, FakeHttpSessionState session)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.request = request;
            this.response = response;
            this.session = session;

            this.request.Context = this;
            this.response.Context = this;

            this.applicationState = new FakeHttpApplicationState();
            this.currentNotification = RequestNotification.BeginRequest;
            this.errors = new List<Exception>();
            this.handlerStack = new Stack<IHttpHandler>();
            this.items = new Hashtable();
            this.pageInstrumentation = new PageInstrumentationService();
            this.server = new FakeHttpServerUtility(this);
            this.timestampUtc = DateTime.UtcNow;

            this.ThreadAbortOnTimeout = true;
        }

        /// <summary>
        /// Gets an array of errors (if any) that accumulated
        /// when an HTTP request was being processed.
        /// </summary>
        /// <value>
        /// An array of <see cref="System.Exception" /> objects for the current HTTP request, 
        /// or <c>null</c> if no errors accumulated during the HTTP request processing.
        /// </value>
        public override Exception[] AllErrors
        {
            get
            {
                if (this.errors.Count > 0)
                {
                    return this.errors.ToArray();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether asynchronous operations are allowed 
        /// during parts of ASP.NET request processing when they are not expected.
        /// </summary>
        public override bool AllowAsyncDuringSyncStages
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="System.Web.HttpApplicationState" /> object 
        /// for the current HTTP request.
        /// </summary>
        public override HttpApplicationStateBase Application
        {
            get
            {
                return this.applicationState;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Web.HttpApplication" /> object 
        /// for the current HTTP request.
        /// </summary>
        public override HttpApplication ApplicationInstance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an object that contains flags that pertain to asynchronous preload mode.
        /// </summary>
        public override AsyncPreloadModeFlags AsyncPreloadMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the the <see cref="System.Web.Caching.Cache" /> object 
        /// for the current application domain.
        /// </summary>
        public override Cache Cache
        {
            get
            {
                return HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.IHttpHandler" /> object that represents 
        /// the handler that is currently executing.
        /// </summary>
        public override IHttpHandler CurrentHandler
        {
            get
            {
                if (this.currentHandler == null)
                {
                    this.currentHandler = this.Handler;
                }

                return this.currentHandler;
            }
        }

        /// <summary>
        /// Gets a <see cref="System.Web.RequestNotification" /> value that indicates 
        /// the <see cref="System.Web.HttpApplication" /> event that is currently processing.
        /// </summary>
        public override RequestNotification CurrentNotification
        {
            get
            {
                return this.currentNotification;
            }
        }

        /// <summary>
        /// Gets the first error (if any) that accumulated when an HTTP request was being processed.
        /// </summary>
        /// <value>
        /// The first exception for the current HTTP request/response process, 
        /// or <c>null</c> if no errors accumulated during the HTTP request processing.
        /// </value>
        public override Exception Error
        {
            get
            {
                if (this.errors.Count > 0)
                {
                    return this.errors[0];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Web.IHttpHandler" /> object that is responsible 
        /// for processing the HTTP request.
        /// </summary>
        public override IHttpHandler Handler
        {
            get
            {
                return this.handler;
            }

            set
            {
                this.handler = value;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether custom errors are enabled 
        /// for the current HTTP request.
        /// </summary>
        public override bool IsCustomErrorEnabled
        {
            get
            {
                return this.isCustomErrorEnabled;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the current HTTP request is in debug mode.
        /// </summary>
        public override bool IsDebuggingEnabled
        {
            get
            {
                return this.isDebuggingEnabled;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether an <see cref="System.Web.HttpApplication" /> event has finished processing.
        /// </summary>
        public override bool IsPostNotification
        {
            get
            {
                return this.isPostNotification;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the request is 
        /// an <see cref="System.Web.WebSockets.AspNetWebSocket"/> connection request.
        /// </summary>
        public override bool IsWebSocketRequest
        {
            get
            {
                return this.isWebSocketRequest;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the connection is upgrading from an HTTP connection 
        /// to an <see cref="System.Web.WebSockets.AspNetWebSocket"/> connection.
        /// </summary>
        public override bool IsWebSocketRequestUpgrading
        {
            get
            {
                return this.isWebSocketRequestUpgrading;
            }
        }

        /// <summary>
        /// Gets a key/value collection that can be used to organize and share data 
        /// between a module and a handler during an HTTP request.
        /// </summary>
        public override IDictionary Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// Gets a reference to the page-instrumentation service instance for this request.
        /// </summary>
        public override PageInstrumentationService PageInstrumentation
        {
            get
            {
                return this.pageInstrumentation;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.IHttpHandler" /> object for the parent handler.
        /// </summary>
        /// <value>
        /// An <see cref="System.Web.IHttpHandler" /> object that represents the parent handler, 
        /// or <c>null</c> if no parent handler was found.
        /// </value>
        public override IHttpHandler PreviousHandler
        {
            get
            {
                if (this.handlerStack.Count > 0)
                {
                    return this.handlerStack.Peek();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.Profile.ProfileBase" /> object for the current user profile.
        /// </summary>
        public override ProfileBase Profile
        {
            get
            {
                return this.profile;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.IHttpHandler" /> object that got remaped 
        /// using the <see cref="System.Web.HttpContextBase.RemapHandler(IHttpHandler)"/> method.
        /// </summary>
        public IHttpHandler RemapHandlerInstance
        {
            get
            {
                return this.remapHandler;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.HttpRequestBase" /> object for the current HTTP request.
        /// </summary>
        public override HttpRequestBase Request
        {
            get
            {
                return this.request;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.HttpResponseBase" /> object for the current HTTP response.
        /// </summary>
        public override HttpResponseBase Response
        {
            get
            {
                return this.response;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.HttpServerUtilityBase" /> object that provides methods 
        /// that are used when Web requests are being processed.
        /// </summary>
        public override HttpServerUtilityBase Server
        {
            get
            {
                return this.server;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.SessionState.HttpSessionStateBase" /> object for the current HTTP request.
        /// </summary>
        public override HttpSessionStateBase Session
        {
            get
            {
                return this.session;
            }
        }

        /// <summary>
        /// Gets or sets the session state behavior.
        /// </summary>
        public SessionStateBehavior SessionStateBehavior
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets a value that specifies whether 
        /// the <see cref="System.Web.Security.UrlAuthorizationModule" /> object 
        /// should skip the authorization check for the current request.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="System.Web.Security.UrlAuthorizationModule" /> should skip the authorization check; 
        /// otherwise, <c>false</c>.
        /// </value>
        public override bool SkipAuthorization
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that specifies whether the ASP.NET runtime should call 
        /// <see cref="System.Threading.Thread.Abort"/> on the thread that is servicing this request 
        /// when the request times out.
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="System.Threading.Thread.Abort"/> will be called when the thread times out; 
        /// otherwise, <c>false</c>. The default is <c>true</c>.
        /// </value>
        public override bool ThreadAbortOnTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the initial timestamp of the current HTTP request.
        /// </summary>
        public override DateTime Timestamp
        {
            get
            {
                return this.timestampUtc.ToLocalTime();
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.TraceContext" /> object for the current HTTP response.
        /// </summary>
        public override TraceContext Trace
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets security information for the current HTTP request.
        /// </summary>
        public override IPrincipal User
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the negotiated protocol that was sent from the server to the client 
        /// for an <see cref="System.Web.WebSockets.AspNetWebSocket"/> connection.
        /// </summary>
        public override string WebSocketNegotiatedProtocol
        {
            get
            {
                return this.webSocketNegotiatedProtocol;
            }
        }

        /// <summary>
        /// Gets the ordered list of protocols that were requested by the client.
        /// </summary>
        public override IList<string> WebSocketRequestedProtocols
        {
            get
            {
                return this.webSocketNegotiatedProtocols;
            }
        }

        /// <summary>
        /// Accepts an <see cref="System.Web.WebSockets.AspNetWebSocket"/> request using the specified user function.
        /// </summary>
        /// <param name="userFunc">The user function.</param>
        public override void AcceptWebSocketRequest(Func<AspNetWebSocketContext, Task> userFunc)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Accepts an <see cref="System.Web.WebSockets.AspNetWebSocket"/> request using the specified user function and options object.
        /// </summary>
        /// <param name="userFunc">The user function.</param>
        /// <param name="options">The options object.</param>
        public override void AcceptWebSocketRequest(Func<AspNetWebSocketContext, Task> userFunc, AspNetWebSocketOptions options)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds an exception to the exception collection for the current HTTP request.
        /// </summary>
        /// <param name="errorInfo">The exception to add to the exception collection.</param>
        public override void AddError(Exception errorInfo)
        {
            this.errors.Add(errorInfo);
        }

        /// <summary>
        /// Raises a virtual event that occurs when the HTTP part of the request is ending.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>The subscription token.</returns>
        public override ISubscriptionToken AddOnRequestCompleted(Action<HttpContextBase> callback)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Clears all errors for the current HTTP request.
        /// </summary>
        public override void ClearError()
        {
            this.errors.Clear();
        }

        /// <summary>
        /// Enables an object's <see cref="System.IDisposable.Dispose"/> method to be called 
        /// when the <see cref="System.Web.WebSockets.AspNetWebSocket"/> connection part of this request is completed.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <returns>The subscription token.</returns>
        public override ISubscriptionToken DisposeOnPipelineCompleted(IDisposable target)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets an application-level resource object based on
        /// the specified <see cref="System.Web.Compilation.ResourceExpressionFields.ClassKey" /> and
        /// <see cref="System.Web.Compilation.ResourceExpressionFields.ResourceKey" /> properties.
        /// </summary>
        /// <param name="classKey">A string that represents the <see cref="System.Web.Compilation.ResourceExpressionFields.ClassKey" /> property of the requested resource object.</param>
        /// <param name="resourceKey">A string that represents the <see cref="System.Web.Compilation.ResourceExpressionFields.ResourceKey" /> property of the requested resource object.</param>
        /// <returns>The requested application-level resource object, or <c>null</c> if no matching resource object is found.</returns>
        public override object GetGlobalResourceObject(string classKey, string resourceKey)
        {
            return this.GetGlobalResourceObject(classKey, resourceKey, null);
        }

        /// <summary>
        /// Gets an application-level resource object based on
        /// the specified <see cref="System.Web.Compilation.ResourceExpressionFields.ClassKey" /> and
        /// <see cref="System.Web.Compilation.ResourceExpressionFields.ResourceKey" /> properties,
        /// and on the <see cref="System.Globalization.CultureInfo" /> object.
        /// </summary>
        /// <param name="classKey">A string that represents the <see cref="P:System.Web.Compilation.ResourceExpressionFields.ClassKey" /> property of the requested resource object.</param>
        /// <param name="resourceKey">A string that represents the <see cref="P:System.Web.Compilation.ResourceExpressionFields.ResourceKey" /> property of the requested resource object.</param>
        /// <param name="culture">A string that represents the <see cref="T:System.Globalization.CultureInfo" /> object of the requested resource.</param>
        /// <returns>The requested application-level resource object, which is localized for the specified culture, or <c>null</c> if no matching resource object is found.</returns>
        public override object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a page-level resource object based on the specified 
        /// <see cref="System.Web.Compilation.ExpressionBuilderContext.VirtualPath" /> and 
        /// <see cref="System.Web.Compilation.ResourceExpressionFields.ResourceKey" /> properties.
        /// </summary>
        /// <param name="virtualPath">A string that represents the <see cref="P:System.Web.Compilation.ExpressionBuilderContext.VirtualPath" /> property of the local resource object.</param>
        /// <param name="resourceKey">A string that represents the <see cref="P:System.Web.Compilation.ResourceExpressionFields.ResourceKey" /> property of the requested resource object.</param>
        /// <returns>The requested page-level resource object, or <c>null</c> if no matching resource object is found.</returns>
        public override object GetLocalResourceObject(string virtualPath, string resourceKey)
        {
            return this.GetLocalResourceObject(virtualPath, resourceKey, null);
        }

        /// <summary>
        /// Gets a page-level resource object based on the specified 
        /// <see cref="System.Web.Compilation.ExpressionBuilderContext.VirtualPath" /> and 
        /// <see cref="System.Web.Compilation.ResourceExpressionFields.ResourceKey" /> properties, 
        /// and on the <see cref="System.Globalization.CultureInfo" /> object.
        /// </summary>
        /// <param name="virtualPath">A string that represents the <see cref="P:System.Web.Compilation.ExpressionBuilderContext.VirtualPath" /> property of the local resource object.</param>
        /// <param name="resourceKey">A string that represents the <see cref="P:System.Web.Compilation.ResourceExpressionFields.ResourceKey" /> property of the requested resource object.</param>
        /// <param name="culture">A string that represents the <see cref="T:System.Globalization.CultureInfo" /> object of the requested resource object.</param>
        /// <returns>The requested local resource object, which is localized for the specified culture, or <c>null</c> if no matching resource object is found.</returns>
        public override object GetLocalResourceObject(string virtualPath, string resourceKey, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the specified configuration section of the current application's default configuration.
        /// </summary>
        /// <param name="sectionName">The configuration section path (in XPath format) and the configuration element name.</param>
        /// <returns>The specified section, or <c>null</c> if the section does not exist.</returns>
        public override object GetSection(string sectionName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an object for the current service type.
        /// </summary>
        /// <param name="serviceType">The type of service object to get.</param>
        /// <returns>The current service type, or <c>null</c> if no service is found.</returns>
        public override object GetService(Type serviceType)
        {
            return null;
        }

        /// <summary>
        /// Specifies a handler for the request.
        /// </summary>
        /// <param name="handler">The object that should process the request.</param>
        public override void RemapHandler(IHttpHandler handler)
        {
            this.remapHandler = handler;
        }

        /// <summary>
        /// Rewrites the URL by using the specified path.
        /// </summary>
        /// <param name="path">The replacement path.</param>
        public override void RewritePath(string path)
        {
            this.RewritePath(path, true);
        }

        /// <summary>
        /// Rewrites the URL by using the specified path and a value 
        /// that specifies whether the virtual path for server resources is modified.
        /// </summary>
        /// <param name="path">The replacement path.</param>
        /// <param name="rebaseClientPath"><c>true</c> to reset the virtual path; <c>false</c> to keep the virtual path unchanged.</param>
        public override void RewritePath(string path, bool rebaseClientPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rewrites the URL by using the specified path, path information, and query string information.
        /// </summary>
        /// <param name="filePath">The replacement path.</param>
        /// <param name="pathInfo">Additional path information for a resource.</param>
        /// <param name="queryString">The request query string.</param>
        public override void RewritePath(string filePath, string pathInfo, string queryString)
        {
            this.RewritePath(filePath, pathInfo, queryString, false);
        }

        /// <summary>
        /// Rewrites the URL by using the specified path, path information, query string information, 
        /// and a value that specifies whether the client file path is set to the rewrite path.
        /// </summary>
        /// <param name="filePath">The replacement path.</param>
        /// <param name="pathInfo">Additional path information for a resource.</param>
        /// <param name="queryString">The request query string.</param>
        /// <param name="setClientFilePath"><c>true</c> to set the file path used for client resources to the value of the <paramref name="filePath" /> parameter; otherwise, <c>false</c>.</param>
        public override void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets a <see cref="System.Web.RequestNotification" /> value that indicates 
        /// the <see cref="System.Web.HttpApplication" /> event that is currently processing.
        /// </summary>
        /// <param name="currentNotification">The current notification.</param>
        public void SetCurrentNotification(RequestNotification currentNotification)
        {
            this.currentNotification = currentNotification;
        }

        /// <summary>
        /// Sets a value that indicates whether custom errors are enabled.
        /// </summary>
        /// <param name="isCustomErrorEnabled">if set to <c>true</c> custom errors are enabled.</param>
        public void SetIsCustomErrorEnabled(bool isCustomErrorEnabled)
        {
            this.isCustomErrorEnabled = isCustomErrorEnabled;
        }

        /// <summary>
        /// Sets a value that indicates whether the current HTTP request is in debug mode.
        /// </summary>
        /// <param name="isDebuggingEnabled">if set to <c>true</c> the current HTTP request is in debug mode.</param>
        public void SetIsDebuggingEnabled(bool isDebuggingEnabled)
        {
            this.isDebuggingEnabled = isDebuggingEnabled;
        }

        /// <summary>
        /// Sets a value that indicates whether an <see cref="System.Web.HttpApplication" /> event has finished processing.
        /// </summary>
        /// <param name="isPostNotification">if set to <c>true</c> the event has finished processing.</param>
        public void SetIsPostNotification(bool isPostNotification)
        {
            this.isPostNotification = isPostNotification;
        }

        /// <summary>
        /// Sets a value that indicates whether the request is an <see cref="System.Web.WebSockets.AspNetWebSocket"/> connection request.
        /// </summary>
        /// <param name="isWebSocketRequest">if set to <c>true</c> the request is an WebSocket request.</param>
        public void SetIsWebSocketRequest(bool isWebSocketRequest)
        {
            this.isWebSocketRequest = isWebSocketRequest;
        }

        /// <summary>
        /// Sets a value that indicates whether the connection is upgrading from an HTTP connection 
        /// to an <see cref="System.Web.WebSockets.AspNetWebSocket"/> connection.
        /// </summary>
        /// <param name="isWebSocketRequestUpgrading">if set to <c>true</c> the connection is upgrading.</param>
        public void SetIsWebSocketRequestUpgrading(bool isWebSocketRequestUpgrading)
        {
            this.isWebSocketRequestUpgrading = isWebSocketRequestUpgrading;
        }

        /// <summary>
        /// Sets the <see cref="System.Web.Profile.ProfileBase" /> object for the current user profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void SetProfile(ProfileBase profile)
        {
            this.profile = profile;
        }

        /// <summary>
        /// Sets the type of session state behavior that is required to support an HTTP request.
        /// </summary>
        /// <param name="sessionStateBehavior">One of the enumeration values that specifies what type of session state behavior is required.</param>
        public override void SetSessionStateBehavior(SessionStateBehavior sessionStateBehavior)
        {
            this.SessionStateBehavior = sessionStateBehavior;
        }

        /// <summary>
        /// Sets the negotiated protocol that was sent from the server to the client 
        /// for an <see cref="System.Web.WebSockets.AspNetWebSocket"/> connection.
        /// </summary>
        /// <param name="webSocketNegotiatedProtocol">The web socket negotiated protocol.</param>
        public void SetWebSocketNegotiatedProtocol(string webSocketNegotiatedProtocol)
        {
            this.webSocketNegotiatedProtocol = webSocketNegotiatedProtocol;
        }

        /// <summary>
        /// Sets the ordered list of protocols that were requested by the client.
        /// </summary>
        /// <param name="webSocketNegotiatedProtocols">The web socket negotiated protocols.</param>
        public void SetWebSocketNegotiatedProtocols(IList<string> webSocketNegotiatedProtocols)
        {
            this.webSocketNegotiatedProtocols = webSocketNegotiatedProtocols;
        }

        /// <summary>
        /// Restores the current handler.
        /// </summary>
        protected void RestoreCurrentHandler()
        {
            this.currentHandler = this.handlerStack.Pop();
        }

        /// <summary>
        /// Sets the current handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        protected void SetCurrentHandler(IHttpHandler handler)
        {
            this.handlerStack.Push(this.CurrentHandler);
            this.currentHandler = handler;
        }
    }
}
