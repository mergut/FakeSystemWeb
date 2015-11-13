namespace FakeSystemWeb
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.SessionState;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.HttpSessionStateBase"/>.
    /// </summary>
    public class FakeHttpSessionState : HttpSessionStateBase
    {
        private readonly NameObjectCollection<object> sessionItems;
        private readonly HttpStaticObjectsCollectionBase staticObjects;

        private HttpCookieMode cookieMode;
        private bool isAbandoned;
        private bool isNewSession;
        private bool isReadOnly;
        private SessionStateMode mode;
        private string sessionID;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpSessionState"/> class.
        /// </summary>
        public FakeHttpSessionState()
        {
            this.sessionItems = new NameObjectCollection<object>(StringComparer.InvariantCulture);

            this.cookieMode = HttpCookieMode.UseCookies;
            this.isNewSession = true;
            this.mode = SessionStateMode.InProc;
            this.sessionID = Guid.NewGuid().ToString("N");
            this.staticObjects = new HttpStaticObjectsCollectionWrapper(new HttpStaticObjectsCollection());
        }

        /// <summary>
        /// Gets or sets the character-set identifier for the current session.
        /// </summary>
        public override int CodePage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a reference to the current session-state object.
        /// </summary>
        public override HttpSessionStateBase Contents
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the application is configured for cookieless sessions.
        /// </summary>
        public override HttpCookieMode CookieMode
        {
            get
            {
                return this.cookieMode;
            }
        }

        /// <summary>
        /// Gets the number of items in the session-state collection.
        /// </summary>
        public override int Count
        {
            get
            {
                return this.sessionItems.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current session has been abandoned.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current session has been abandoned; otherwise, <c>false</c>.
        /// </value>
        public bool IsAbandoned
        {
            get
            {
                return this.isAbandoned;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the session ID is embedded in the URL.
        /// </summary>
        /// <value>
        /// <c>true</c> if the session is embedded in the URL; otherwise, <c>false</c>.
        /// </value>
        public override bool IsCookieless
        {
            get
            {
                return this.cookieMode == HttpCookieMode.UseUri;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the session was created during the current request.
        /// </summary>
        /// <value>
        /// <c>true</c> the session was created with the current request; otherwise, <c>false</c>.
        /// </value>
        public override bool IsNewSession
        {
            get
            {
                return this.isNewSession;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the session is read-only.
        /// </summary>
        /// <value>
        /// <c>true</c> if the session is read-only; otherwise, <c>false</c>.
        /// </value>
        public override bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether access to the collection of session-state values is synchronized (thread safe).
        /// </summary>
        /// <value>
        /// <c>true</c> if access to the collection is synchronized (thread safe); otherwise, <c>false</c>.
        /// </value>
        public override bool IsSynchronized
        {
            get
            {
                return ((ICollection)this.sessionItems).IsSynchronized;
            }
        }

        /// <summary>
        /// Gets a collection of the keys for all values that are stored in the session-state collection.
        /// </summary>
        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return this.sessionItems.Keys;
            }
        }

        /// <summary>
        /// Gets or sets the locale identifier (LCID) of the current session.
        /// </summary>
        public override int LCID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current session-state mode.
        /// </summary>
        public override SessionStateMode Mode
        {
            get
            {
                return this.mode;
            }
        }

        /// <summary>
        /// Gets the unique identifier for the session.
        /// </summary>
        public override string SessionID
        {
            get
            {
                return this.sessionID;
            }
        }

        /// <summary>
        /// Gets a collection of objects that are declared by object elements that are marked as server controls 
        /// and scoped to the current session in the application's Global.asax file.
        /// </summary>
        public override HttpStaticObjectsCollectionBase StaticObjects
        {
            get
            {
                return this.staticObjects;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection of session-state values.
        /// </summary>
        public override object SyncRoot
        {
            get
            {
                return ((ICollection)this.sessionItems).SyncRoot;
            }
        }

        /// <summary>
        /// Gets or sets the time, in minutes, that can elapse between requests before the session-state provider ends the session.
        /// </summary>
        public override int Timeout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a session value by using the specified index.
        /// </summary>
        /// <param name="index">The index of the session value.</param>
        /// <returns>The session-state value stored at the specified index, or <c>null</c> if the item does not exist.</returns>
        public override object this[int index]
        {
            get
            {
                return this.sessionItems.Get(index);
            }

            set
            {
                this.sessionItems.Set(index, value);
            }
        }

        /// <summary>
        /// Gets or sets a session value by using the specified name.
        /// </summary>
        /// <param name="name">The key name of the session value.</param>
        /// <returns>The session-state value with the specified name, or <c>null</c> if the item does not exist.</returns>
        public override object this[string name]
        {
            get
            {
                return this.sessionItems.Get(name);
            }

            set
            {
                this.sessionItems.Set(name, value);
            }
        }

        /// <summary>
        /// Cancels the current session.
        /// </summary>
        public override void Abandon()
        {
            this.isAbandoned = true;
        }

        /// <summary>
        /// Adds an item to the session-state collection.
        /// </summary>
        /// <param name="name">The name of the item to add to the session-state collection.</param>
        /// <param name="value">The value of the item to add to the session-state collection.</param>
        public override void Add(string name, object value)
        {
            this.sessionItems.Add(name, value);
        }

        /// <summary>
        /// Removes all keys and values from the session-state collection.
        /// </summary>
        public override void Clear()
        {
            this.sessionItems.Clear();
        }

        /// <summary>
        /// Copies the collection of session-state values to a one-dimensional array, starting at the specified index in the array.
        /// </summary>
        /// <param name="array">The array to copy session values to.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying starts.</param>
        public override void CopyTo(Array array, int index)
        {
            ((ICollection)this.sessionItems).CopyTo(array, index);
        }

        /// <summary>
        /// Returns an enumerator that can be used to read all the session-state variable names in the current session.
        /// </summary>
        /// <returns>
        /// An enumerator that can iterate through the variable names in the session-state collection.
        /// </returns>
        public override IEnumerator GetEnumerator()
        {
            return this.sessionItems.GetEnumerator();
        }

        /// <summary>
        /// Deletes an item from the session-state collection.
        /// </summary>
        /// <param name="name">The name of the item to delete from the session-state collection.</param>
        public override void Remove(string name)
        {
            this.sessionItems.Remove(name);
        }

        /// <summary>
        /// Removes all keys and values from the session-state collection.
        /// </summary>
        public override void RemoveAll()
        {
            this.Clear();
        }

        /// <summary>
        /// Deletes the item at the specified index from the session-state collection.
        /// </summary>
        /// <param name="index">The index of the item to remove from the session-state collection.</param>
        public override void RemoveAt(int index)
        {
            this.sessionItems.RemoveAt(index);
        }

        /// <summary>
        /// Sets a value that indicates whether the application is configured for cookieless sessions.
        /// </summary>
        /// <param name="cookieMode">The cookie mode.</param>
        public void SetCookieMode(HttpCookieMode cookieMode)
        {
            this.cookieMode = cookieMode;
        }

        /// <summary>
        /// Sets a value that indicates whether the session was created during the current request.
        /// </summary>
        /// <param name="isNewSession">if set to <c>true</c> the session was created with the current request.</param>
        public void SetIsNewSession(bool isNewSession)
        {
            this.isNewSession = isNewSession;
        }

        /// <summary>
        /// Sets a value that indicates whether the session is read-only.
        /// </summary>
        /// <param name="isReadOnly">if set to <c>true</c> the session is read-only.</param>
        public void SetIsReadOnly(bool isReadOnly)
        {
            this.isReadOnly = isReadOnly;
        }

        /// <summary>
        /// Sets the current session-state mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        public void SetMode(SessionStateMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Sets the unique identifier for the session.
        /// </summary>
        /// <param name="sessionID">The unique identifier for the session.</param>
        public void SetSessionID(string sessionID)
        {
            this.sessionID = sessionID;
        }
    }
}
