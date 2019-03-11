using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

namespace VoxelBusters.NativePlugins.Android
{
    public abstract class Element
    {
        private readonly string  Namespace = "http://schemas.android.com/apk/res/android";

        private List<Attribute>   m_attributes;
        private List<Element>     m_children;

        public void AddAttribute(string key, string value)
        {
            if (m_attributes == null)
                m_attributes = new List<Attribute>();

            m_attributes.Add(new Attribute(key, value));
        }

        protected virtual XmlElement ToXml(XmlDocument xmlDocument)
        {
            XmlElement element = xmlDocument.CreateElement(GetName());
            if(m_attributes != null)
            {
                foreach(Attribute attribute in m_attributes)
                {
                    XmlAttribute newAttribute = null;

                    string[] components = attribute.Key.Split(':');
                    if(attribute.Key.Contains("xmlns") || components.Length == 1)
                    {
                        element.SetAttribute(attribute.Key, attribute.Value);
                    }
                    else
                    {
                        newAttribute = xmlDocument.CreateAttribute(components[0], components[1], Namespace);
                        newAttribute.Value = attribute.Value;
                        element.Attributes.Append(newAttribute);
                    }
                }
            }

            if (m_children != null)
            {
                foreach(Element eachChild in m_children)
                {
                    XmlElement xmlElement = eachChild.ToXml(xmlDocument);
                    element.AppendChild(xmlElement);
                }
            }

            return element;
        }

        protected virtual void Add(Element element)
        {
            if(m_children == null)
                m_children = new List<Element>();

            m_children.Add(element);
        }

        protected abstract string GetName();
    }
}
