using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class HtmlTagAttributesMapperAttribute : Attribute
    {
        private string m_TagName;

        public string TagName
        {
            get { return m_TagName; }
            set { m_TagName = value; }
        }

        public HtmlTagAttributesMapperAttribute()
        { }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class OldNetscapeTagAttribute : Attribute
    {
        public OldNetscapeTagAttribute()
        { }
    }
    
}
