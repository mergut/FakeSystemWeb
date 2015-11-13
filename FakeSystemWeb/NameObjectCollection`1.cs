namespace FakeSystemWeb
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;

    /// <summary>
    /// Represents a strongly typed <see cref="System.Collections.Specialized.NameObjectCollectionBase"/> implementation.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    internal sealed class NameObjectCollection<T> : NameObjectCollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameObjectCollection{T}"/> class.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer used when comparing keys.</param>
        public NameObjectCollection(IEqualityComparer equalityComparer)
            : base(equalityComparer)
        {
        }

        /// <summary>
        /// Adds an entry with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the entry to add. The name can be <c>null</c>.</param>
        /// <param name="value">The value of the entry to add. The value can be <c>null</c>.</param>
        public void Add(string name, T value)
        {
            this.BaseAdd(name, value);
        }

        /// <summary>
        /// Removes all entries.
        /// </summary>
        public void Clear()
        {
            this.BaseClear();
        }

        /// <summary>
        /// Gets the value of the entry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the value to get.</param>
        /// <returns>An <see cref="T" /> that represents the value of the entry at the specified index.</returns>
        public T Get(int index)
        {
            return (T)this.BaseGet(index);
        }

        /// <summary>
        /// Gets the value of the first entry with the specified name.
        /// </summary>
        /// <param name="name">The name of the entry to add. The name can be <c>null</c>.</param>
        /// <returns>
        /// An <see cref="T" /> that represents the value of the first entry with the specified key, if found; 
        /// otherwise, <c>null</c>.
        /// </returns>
        public T Get(string name)
        {
            return (T)this.BaseGet(name);
        }

        /// <summary>
        /// Gets the key of the entry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the key to get.</param>
        /// <returns>A <see cref="System.String" /> that represents the key of the entry at the specified index.</returns>
        public string GetKey(int index)
        {
            return this.BaseGetKey(index);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> array that contains all the keys.
        /// </summary>
        /// <returns>A <see cref="System.String" /> array that contains all the keys.</returns>
        public string[] GetAllKeys()
        {
            return this.BaseGetAllKeys();
        }

        /// <summary>
        /// Removes the entries with the specified name.
        /// </summary>
        /// <param name="name">The name of the entries to remove. The key can be <c>null</c>.</param>
        public void Remove(string name)
        {
            this.BaseRemove(name);
        }

        /// <summary>
        /// Removes the entry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the entry to remove.</param>
        public void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        /// <summary>
        /// Sets the value of the entry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the entry to set.</param>
        /// <param name="value">The value of the entry to set. The value can be <c>null</c>.</param>
        public void Set(int index, T value)
        {
            this.BaseSet(index, value);
        }

        /// <summary>
        /// Sets the value of the first entry with the specified name.
        /// </summary>
        /// <param name="name">The name of the entry to set. The key can be <c>null</c>.</param>
        /// <param name="value">The value of the entry to set. The value can be <c>null</c>.</param>
        public void Set(string name, T value)
        {
            this.BaseSet(name, value);
        }
    }
}
