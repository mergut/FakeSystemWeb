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
    using System.Web;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.HttpApplicationStateBase"/>.
    /// </summary>
    public class FakeHttpApplicationState : HttpApplicationStateBase
    {
        private readonly NameObjectCollection<object> items;
        private readonly HttpStaticObjectsCollectionBase staticObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpApplicationState"/> class.
        /// </summary>
        public FakeHttpApplicationState()
        {
            this.items = new NameObjectCollection<object>(StringComparer.InvariantCultureIgnoreCase);
            this.staticObjects = new HttpStaticObjectsCollectionWrapper(new HttpStaticObjectsCollection());
        }

        /// <summary>
        /// Gets the access keys for the objects in the collection.
        /// </summary>
        public override string[] AllKeys
        {
            get
            {
                return this.items.GetAllKeys();
            }
        }

        /// <summary>
        /// Gets a reference to the <see cref="System.Web.HttpApplicationStateBase" /> object.
        /// </summary>
        public override HttpApplicationStateBase Contents
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets the number of objects in the collection.
        /// </summary>
        public override int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether access to the collection is thread-safe.
        /// </summary>
        public override bool IsSynchronized
        {
            get
            {
                return ((ICollection)this.items).IsSynchronized;
            }
        }

        /// <summary>
        /// Gets all objects that are declared by an object element where the scope is set to "Application" in the ASP.NET application.
        /// </summary>
        public override HttpStaticObjectsCollectionBase StaticObjects
        {
            get
            {
                return this.staticObjects;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection.
        /// </summary>
        public override object SyncRoot
        {
            get
            {
                return ((ICollection)this.items).SyncRoot;
            }
        }

        /// <summary>
        /// Gets a state object by index.
        /// </summary>
        /// <param name="index">The index of the object in the collection.</param>
        /// <returns>
        /// The object referenced by <paramref name="index" />.
        /// </returns>
        public override object this[int index]
        {
            get
            {
                return this.Get(index);
            }
        }

        /// <summary>
        /// Gets or sets a state object by name.
        /// </summary>
        /// <param name="name">The name of the object in the collection.</param>
        /// <returns>
        /// The object referenced by <paramref name="name" />.
        /// </returns>
        public override object this[string name]
        {
            get
            {
                return this.Get(name);
            }

            set
            {
                this.Set(name, value);
            }
        }

        /// <summary>
        /// Adds a new object to the collection.
        /// </summary>
        /// <param name="name">The name of the object to add to the collection.</param>
        /// <param name="value">The value of the object.</param>
        public override void Add(string name, object value)
        {
            this.items.Add(name, value);
        }

        /// <summary>
        /// Removes all objects from the collection.
        /// </summary>
        public override void Clear()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at the specified index in the array.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination for the elements that are copied from the collection. The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which to begin copying.</param>
        public override void CopyTo(Array array, int index)
        {
            ((ICollection)this.items).CopyTo(array, index);
        }

        /// <summary>
        /// Gets a state object by index.
        /// </summary>
        /// <param name="index">The index of the application state object to get.</param>
        /// <returns>
        /// The object referenced by <paramref name="index" />.
        /// </returns>
        public override object Get(int index)
        {
            return this.items.Get(index);
        }

        /// <summary>
        /// Gets a state object by name.
        /// </summary>
        /// <param name="name">The name of the object to get.</param>
        /// <returns>
        /// The object referenced by <paramref name="name" />.
        /// </returns>
        public override object Get(string name)
        {
            return this.items.Get(name);
        }

        /// <summary>
        /// Returns an enumerator that can be used to iterate through the collection.
        /// </summary>
        /// <returns>
        /// An object that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <summary>
        /// Gets the name of a state object by index.
        /// </summary>
        /// <param name="index">The index of the application state object to get.</param>
        /// <returns>
        /// The name of the application state object.
        /// </returns>
        public override string GetKey(int index)
        {
            return this.items.GetKey(index);
        }

        /// <summary>
        /// Locks access to objects in the collection in order to enable synchronized access.
        /// </summary>
        public override void Lock()
        {
        }

        /// <summary>
        /// Removes the named object from the collection.
        /// </summary>
        /// <param name="name">The name of the object to remove from the collection.</param>
        public override void Remove(string name)
        {
            this.items.Remove(name);
        }

        /// <summary>
        /// Removes all objects from the collection.
        /// </summary>
        public override void RemoveAll()
        {
            this.Clear();
        }

        /// <summary>
        /// Removes a state object specified by index from the collection.
        /// </summary>
        /// <param name="index">The position in the collection of the item to remove.</param>
        public override void RemoveAt(int index)
        {
            this.items.RemoveAt(index);
        }

        /// <summary>
        /// Updates the value of an object in the collection.
        /// </summary>
        /// <param name="name">The name of the object to update.</param>
        /// <param name="value">The updated value of the object.</param>
        public override void Set(string name, object value)
        {
            this.items.Set(name, value);
        }

        /// <summary>
        /// Unlocks access to objects in the collection to enable synchronized access.
        /// </summary>
        public override void UnLock()
        {
        }
    }
}
