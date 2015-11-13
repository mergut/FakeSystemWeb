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
    using System.Web;

    /// <summary>
    /// Fake implementation of <see cref="System.Web.HttpFileCollectionBase"/>.
    /// </summary>
    public class FakeHttpFileCollection : HttpFileCollectionBase
    {
        private readonly NameObjectCollection<HttpPostedFileBase> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpFileCollection"/> class.
        /// </summary>
        public FakeHttpFileCollection()
        {
            this.collection = new NameObjectCollection<HttpPostedFileBase>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets an array that contains the keys (names) of all posted file objects in the collection.
        /// </summary>
        public override string[] AllKeys
        {
            get
            {
                return this.collection.GetAllKeys();
            }
        }

        /// <summary>
        /// Gets the number of posted file objects in the collection.
        /// </summary>
        public override int Count
        {
            get
            {
                return this.collection.Count;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether access to the collection is thread-safe.
        /// </summary>
        public override bool IsSynchronized
        {
            get
            {
                return ((ICollection)this.collection).IsSynchronized;
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> instance that contains all the keys in the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.
        /// </summary>
        public override KeysCollection Keys
        {
            get
            {
                return this.collection.Keys;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection.
        /// </summary>
        public override object SyncRoot
        {
            get
            {
                return ((ICollection)this.collection).SyncRoot;
            }
        }

        /// <summary>
        /// Gets the posted file object at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The <see cref="T:System.Web.HttpPostedFileBase" /> specified by <paramref name="index" />.</returns>
        public override HttpPostedFileBase this[int index]
        {
            get
            {
                return this.collection.Get(index);
            }
        }

        /// <summary>
        /// Gets the posted file object that has the specified name from the collection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="T:System.Web.HttpPostedFileBase" /> specified by <paramref name="name" />.</returns>
        public override HttpPostedFileBase this[string name]
        {
            get
            {
                return this.collection.Get(name);
            }
        }

        /// <summary>
        /// Adds the file to the collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="file">The file.</param>
        public void AddFile(string key, FakeHttpPostedFile file)
        {
            this.collection.Add(key, file);
        }

        /// <summary>
        /// Copies the elements of the collection to an array, starting at the specified index in the array.
        /// </summary>
        /// <param name="dest">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying starts.</param>
        public override void CopyTo(Array dest, int index)
        {
            ((ICollection)this.collection).CopyTo(dest, index);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.collection.Equals(obj);
        }

        /// <summary>
        /// Returns the posted file object at the specified index.
        /// </summary>
        /// <param name="index">The index of the object to return.</param>
        /// <returns>
        /// The posted file object specified by <paramref name="index" />.
        /// </returns>
        public override HttpPostedFileBase Get(int index)
        {
            return this.collection.Get(index);
        }

        /// <summary>
        /// Returns the posted file object that has the specified name from the collection.
        /// </summary>
        /// <param name="name">The name of the object to return.</param>
        /// <returns>
        /// The posted file object that is specified by <paramref name="name" />.
        /// </returns>
        public override HttpPostedFileBase Get(string name)
        {
            return this.collection.Get(name);
        }

        /// <summary>
        /// Returns an enumerator that can be used to iterate through the collection.
        /// </summary>
        /// <returns>
        /// An object that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.collection.GetHashCode();
        }

        /// <summary>
        /// Returns the name of the posted file object at the specified index.
        /// </summary>
        /// <param name="index">The index of the object name to return.</param>
        /// <returns>
        /// The name of the posted file object that is specified by <paramref name="index" />.
        /// </returns>
        public override string GetKey(int index)
        {
            return this.collection.GetKey(index);
        }

        /// <summary>
        /// Returns all files that match the specified name.
        /// </summary>
        /// <param name="name">The name to match.</param>
        /// <returns>
        /// The collection of files.
        /// </returns>
        public override IList<HttpPostedFileBase> GetMultiple(string name)
        {
            var list = new List<HttpPostedFileBase>();
            for (int i = 0; i < this.Count; i++)
            {
                string key = this.GetKey(i);
                if (string.Equals(key, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    list.Add(this.Get(i));
                }
            }

            return list.AsReadOnly();
        }

        /// <summary>
        /// Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.</param>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            this.collection.GetObjectData(info, context);
        }

        /// <summary>
        /// Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.
        /// </summary>
        /// <param name="sender">The source of the deserialization event.</param>
        public override void OnDeserialization(object sender)
        {
            this.collection.OnDeserialization(sender);
        }
    }
}
