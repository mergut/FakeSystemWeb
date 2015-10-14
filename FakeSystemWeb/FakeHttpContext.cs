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
        private readonly List<Exception> errors;
        private readonly Stack<IHttpHandler> handlerStack;
        private readonly Hashtable items;
        private readonly DateTime timestampUtc;

        private IHttpHandler currentHandler;

        public FakeHttpContext()
        {
            this.errors = new List<Exception>();
            this.handlerStack = new Stack<IHttpHandler>();
            this.items = new Hashtable();
            this.timestampUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets an array of errors (if any) that accumulated
        /// when an HTTP request was being processed.
        /// </summary>
        /// <value>
        /// An array of <see cref="T:System.Exception" /> objects for the current HTTP request, 
        /// or null if no errors accumulated during the HTTP request processing.
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
        /// Gets the <see cref="T:System.Web.HttpApplicationState" /> object 
        /// for the current HTTP request.
        /// </summary>
        public override HttpApplicationStateBase Application
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Web.HttpApplication" /> object 
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
        /// Gets the the <see cref="T:System.Web.Caching.Cache" /> object 
        /// for the current application domain.
        /// </summary>
        public override Cache Cache
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.IHttpHandler" /> object that represents 
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
        /// Gets a <see cref="T:System.Web.RequestNotification" /> value that indicates 
        /// the <see cref="T:System.Web.HttpApplication" /> event that is currently processing.
        /// </summary>
        public override RequestNotification CurrentNotification
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the first error (if any) that accumulated when an HTTP request was being processed.
        /// </summary>
        /// <value>
        /// The first exception for the current HTTP request/response process, 
        /// or null if no errors accumulated during the HTTP request processing.
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
        /// Gets or sets the <see cref="T:System.Web.IHttpHandler" /> object that is responsible 
        /// for processing the HTTP request.
        /// </summary>
        public override IHttpHandler Handler
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value that indicates whether custom errors are enabled 
        /// for the current HTTP request.
        /// </summary>
        public override bool IsCustomErrorEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the current HTTP request is in debug mode.
        /// </summary>
        public override bool IsDebuggingEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// a value that indicates whether an <see cref="T:System.Web.HttpApplication" /> event 
        /// has finished processing.
        /// </summary>
        public override bool IsPostNotification
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the request is 
        /// an <see cref="T:System.Web.WebSockets.AspNetWebSocket"/> connection request.
        /// </summary>
        public override bool IsWebSocketRequest
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the connection is upgrading from an HTTP connection 
        /// to an <see cref="T:System.Web.WebSockets.AspNetWebSocket"/> connection.
        /// </summary>
        public override bool IsWebSocketRequestUpgrading
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.IHttpHandler" /> object for the parent handler.
        /// </summary>
        /// <value>
        /// An <see cref="T:System.Web.IHttpHandler" /> object that represents the parent handler, 
        /// or null if no parent handler was found.
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

        public override ProfileBase Profile
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override HttpRequestBase Request
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override HttpServerUtilityBase Server
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool SkipAuthorization
        {
            get;
            set;
        }

        public override bool ThreadAbortOnTimeout
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override DateTime Timestamp
        {
            get
            {
                return this.timestampUtc.ToLocalTime();
            }
        }

        public override TraceContext Trace
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override IPrincipal User
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string WebSocketNegotiatedProtocol
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override IList<string> WebSocketRequestedProtocols
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void AcceptWebSocketRequest(Func<AspNetWebSocketContext, Task> userFunc)
        {
            throw new NotImplementedException();
        }

        public override void AcceptWebSocketRequest(Func<AspNetWebSocketContext, Task> userFunc, AspNetWebSocketOptions options)
        {
            throw new NotImplementedException();
        }

        public override void AddError(Exception errorInfo)
        {
            throw new NotImplementedException();
        }

        public override ISubscriptionToken AddOnRequestCompleted(Action<HttpContextBase> callback)
        {
            throw new NotImplementedException();
        }

        public override void ClearError()
        {
            throw new NotImplementedException();
        }

        public override ISubscriptionToken DisposeOnPipelineCompleted(IDisposable target)
        {
            throw new NotImplementedException();
        }

        public override object GetGlobalResourceObject(string classKey, string resourceKey)
        {
            throw new NotImplementedException();
        }

        public override object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object GetLocalResourceObject(string virtualPath, string resourceKey)
        {
            throw new NotImplementedException();
        }

        public override object GetLocalResourceObject(string virtualPath, string resourceKey, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public override object GetSection(string sectionName)
        {
            throw new NotImplementedException();
        }

        public override object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public override void RemapHandler(IHttpHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void RewritePath(string path)
        {
            throw new NotImplementedException();
        }

        public override void RewritePath(string path, bool rebaseClientPath)
        {
            throw new NotImplementedException();
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString)
        {
            throw new NotImplementedException();
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
        {
            throw new NotImplementedException();
        }

        public override void SetSessionStateBehavior(SessionStateBehavior sessionStateBehavior)
        {
            throw new NotImplementedException();
        }

        protected internal void RestoreCurrentHandler()
        {
            this.currentHandler = this.handlerStack.Pop();
        }

        protected internal void SetCurrentHandler(IHttpHandler handler)
        {
            this.handlerStack.Push(this.CurrentHandler);
            this.currentHandler = handler;
        }
    }
}
