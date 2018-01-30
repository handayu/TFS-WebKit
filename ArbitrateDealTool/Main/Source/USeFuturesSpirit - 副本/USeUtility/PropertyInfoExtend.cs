using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace USeUtility
{
    public static class PropertyInfoExtend
    {
        /// <summary>
        /// 获取属性类型。
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetPropertyTypeName(this PropertyInfo property)
        {
            string propertyTypeName = property.PropertyType.Name;

            Type propertyType = property.PropertyType;
            if (propertyType == typeof(decimal))
            {
                return "decimal";
            }
            else if (propertyType == typeof(int))
            {
                return "int";
            }
            else if (propertyType == typeof(double))
            {
                return "double";
            }
            else if (propertyType == typeof(string))
            {
                return "string";
            }
            else
            {
                return propertyTypeName;
            }
        }

        /// <summary>
        /// 获取属性名称。
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetPropertyName(this PropertyInfo property)
        {
            return property.Name;
        }


        /// <summary>
        /// 获取属性成员。
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetPropertyMemberName(this PropertyInfo property)
        {
            string propertyName = property.Name;
            return string.Format("m_{0}{1}", propertyName.Substring(0, 1).ToLower(), propertyName.Substring(1));
        }

        /// <summary>
        /// 获取属性成员默认值。
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetPropertyMemberDefaultValue(this PropertyInfo property)
        {
            Type propertyType = property.PropertyType;
            if (propertyType == typeof(decimal))
            {
                return "0m";
            }
            else if (propertyType == typeof(int))
            {
                return "0";
            }
            else if (propertyType == typeof(double))
            {
                return "0d";
            }
            else if (propertyType == typeof(DateTime))
            {
                return "DateTime.MinValue";
            }
            else
            {
                return "null";
            }
        }

        /// <summary>
        /// 获取属性注释。
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetPropertyRemark(this PropertyInfo property, XDocument doc)
        {
            string propertyKey = string.Format("P:{0}.{1}", property.DeclaringType.FullName, property.Name);// USe.TradeDriver.Common.USeFee.Instrument";
            XElement docElement = doc.Element("doc");
            if (docElement == null) return string.Empty;

            XElement memberListElement = docElement.Element("members");
            if (memberListElement == null || memberListElement.HasElements == false) return string.Empty;

            XElement memberElement = (from p in memberListElement.Elements("member")
                                   where p.Attribute("name").Value == propertyKey
                                   select p).FirstOrDefault();
            if (memberElement == null) return string.Empty;

            XElement summaryElement = memberElement.Element("summary");
            if (summaryElement == null) return string.Empty;

            return summaryElement.Value.Trim();
        }
    }
}
