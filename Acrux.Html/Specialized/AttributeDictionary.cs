using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace Acrux.Html.Specialized
{
    [Serializable()]
    internal sealed class DuplicatedAttributeOverwrittenException<T> : Exception
    {
        private T m_ExistingValue;
        private T m_NewValue;
        private string m_Key;

        public T ExistingValue { get { return m_ExistingValue; } }
        public T NewValue { get { return m_NewValue; } }
        public string Key { get { return m_Key; } }

        internal DuplicatedAttributeOverwrittenException(string key, T existingValue, T newValue)
            : base(string.Format(CultureInfo.InvariantCulture, "Arrtibute '{0}' with value '{1}' is already defined. The value has been overwritten with '{2}'", key, existingValue, newValue))
        {
            m_ExistingValue = existingValue;
            m_NewValue = newValue;
            m_Key = key;
        }

        private DuplicatedAttributeOverwrittenException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        { }
    }

    // TODO: This class is used a lot. Simplifying it may improve the performance a lot !
    // TODO: Confitional compiulation ?? When UnitTesting then use List<T> otherwise use <T>

    internal sealed class AttributeDictionary<T> : Dictionary<string, List<T>>
    {
        private List<string> m_AttNamesUpper = new List<string>();

        private new void Add(string key, List<T> value)
        { /* Hide the base method */ }

        internal void Add(string key, T value)
        {
            Debug.Assert(key != null, "Attempting to add an attribute with a name which is null!");

            if (key == null)
                throw new ArgumentNullException("key");

            if (key.StartsWith("\x01"))
                throw new ArgumentException("key cannot start with '\\x01'");

            string keyLower = key.ToLower(CultureInfo.InvariantCulture);

            T oldKeyValue = default(T);
            bool duplicated = false;

            if (m_AttNamesUpper.IndexOf(keyLower) > -1)
            {
                List<T> valueList = base[keyLower];

                foreach (string existingKey in base.Keys)
                {
                    if (existingKey.Equals(keyLower))
                    {
                        oldKeyValue = valueList[0];
                        valueList.Insert(0, value);

                        duplicated = true;
                        break;
                    }
                }
            }
            else
            {
                m_AttNamesUpper.Add(keyLower);

                List<T> valueList = new List<T>();
                valueList.Add(value);

                base.Add(keyLower, valueList);
            }

            if (duplicated)
                // This is a way to communicate back the fact the key was duplicated
                // and in the same time to hide the [void Add(string, string)].
                // There are other ways to achieve the same, but this one works as well.
                throw new DuplicatedAttributeOverwrittenException<T>(keyLower, oldKeyValue, value);
        }

        internal new T this[string key]
        {
            get
            {
                return this[key, false];
            }
        }

        public IEnumerable<T> AllValues(string key)
        {
            return base[key];
        }

        internal bool HasDuplicatedValues(string key)
        {
            return base[key].Count > 1;
        }

        internal new void Clear()
        {
            base.Clear();
            m_AttNamesUpper.Clear();
        }

        internal T this[string key, bool returnNullOnKeyNotFound]
        {
            get
            {
                try
                {
                    // First try to see if the case is correct ...
                    List<T> valueList = base[key];
                    return valueList[0];
                }
                catch (KeyNotFoundException)
                {
                    // .. if not then try to do it case insensitive
                    string lowerKeyRequested = key.ToLower(CultureInfo.InvariantCulture);

                    foreach (string existingKey in base.Keys)
                    {
                        if (lowerKeyRequested.Equals(existingKey, StringComparison.InvariantCultureIgnoreCase))
                        {
                            List<T> valueList = base[existingKey];
                            return valueList[0];
                        }
                    }

                    // No luck finding it? 
                    if (returnNullOnKeyNotFound)
                        // Return null if the client has requested so
                        return default(T);
                    else
                        // Throw the exception then.
                        throw;
                }
            }
        }
    }
}
