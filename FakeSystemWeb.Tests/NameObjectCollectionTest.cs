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
    using NUnit.Framework;

    [TestFixture]
    public sealed class NameObjectCollectionTest
    {
        [Test]
        public void NameObjectCollection_Constructor_Default_UsesKeyComparer()
        {
            NameObjectCollection<int> collection = new NameObjectCollection<int>(StringComparer.OrdinalIgnoreCase);
            collection.Add("test", 42);

            Assert.That(collection.Get("TEST"), Is.EqualTo(42));
        }

        [Test]
        public void NameObjectCollection_Add_Default_AddsTheEntry()
        {
            NameObjectCollection<int> collection = new NameObjectCollection<int>(StringComparer.Ordinal);

            collection.Add("test", 42);

            Assert.That(collection, Has.Count.EqualTo(1));
        }

        [Test]
        public void NameObjectCollection_Clear_Default_RemovesAllEntries()
        {
            NameObjectCollection<int> collection = GetTestCollection();

            collection.Clear();

            Assert.That(collection, Has.Count.EqualTo(0));
        }

        [Test]
        public void NameObjectCollection_Get_WithIndex_ReturnsEntryAtIndex()
        {
            NameObjectCollection<int> collection = GetTestCollection();

            var result = collection.Get(1);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void NameObjectCollection_Get_WithExistingKey_ReturnsEntry()
        {
            NameObjectCollection<int> collection = GetTestCollection();

            var result = collection.Get("test");

            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void NameObjectCollection_GetKey_WithIndex_ReturnsKey()
        {
            NameObjectCollection<int> collection = GetTestCollection();

            var result = collection.GetKey(1);

            Assert.That(result, Is.EqualTo("1"));
        }

        [Test]
        public void NameObjectCollection_GetAllKeys_Default_ReturnsAllKeys()
        {
            NameObjectCollection<int> collection = GetTestCollection();

            var result = collection.GetAllKeys();

            Assert.That(result, Is.EquivalentTo(new[] { "0", "1", "test" }));
        }

        [Test]
        public void NameObjectCollection_Remove_WithExistingKey_RemovesEntry()
        {
            NameObjectCollection<int> collection = GetTestCollection();
            int count = collection.Count;

            collection.Remove("test");

            Assert.That(collection, Has.Count.EqualTo(count - 1));
        }

        [Test]
        public void NameObjectCollection_RemoveAt_WithIndex_RemovesEntry()
        {
            NameObjectCollection<int> collection = GetTestCollection();
            int count = collection.Count;

            collection.RemoveAt(0);

            Assert.That(collection, Has.Count.EqualTo(count - 1));
        }

        [Test]
        public void NameObjectCollection_Set_WithIndex_SetsEntryValue()
        {
            NameObjectCollection<int> collection = GetTestCollection();

            collection.Set(1, 42);

            Assert.That(collection.Get(1), Is.EqualTo(42));
        }

        [Test]
        public void NameObjectCollection_Set_WithKey_SetsEntryValue()
        {
            NameObjectCollection<int> collection = GetTestCollection();

            collection.Set("test", 123);

            Assert.That(collection.Get("test"), Is.EqualTo(123));
        }

        private NameObjectCollection<int> GetTestCollection()
        {
            NameObjectCollection<int> collection = new NameObjectCollection<int>(StringComparer.Ordinal);
            collection.Add("0", 0);
            collection.Add("1", 1);
            collection.Add("test", 42);

            return collection;
        }
    }
}
