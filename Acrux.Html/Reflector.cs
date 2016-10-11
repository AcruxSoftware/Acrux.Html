using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;

namespace Acrux.Html
{
    internal class Reflector
    {
        private readonly object m_host;
        private readonly Type m_type;

        internal Reflector(object host)
        {
            Debug.Assert(host != null);

            if (host == null)
                throw new ArgumentNullException("host");

            this.m_host = host;
            this.m_type = this.m_host.GetType();
        }

        internal void SetReflectedValue(string name, object value)
        {
            SetReflectedValue(name, null, value);
        }

        internal void SetReflectedValue(string name, object[] index, object value)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("'name' must have value.");

            FieldInfo fi = m_type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (fi != null)
            {
                fi.SetValue(m_host, value);
                return;
            }

            PropertyInfo[] piColl = m_type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pi in piColl)
            {
                if (name.Equals(pi.Name))
                {
                    pi.SetValue(m_host, value, index);
                    return;
                }
            }

            throw new MemberAccessException("Member not found.");
        }
    }
}

