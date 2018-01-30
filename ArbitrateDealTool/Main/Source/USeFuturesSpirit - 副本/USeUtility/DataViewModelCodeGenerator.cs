using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace USeUtility
{
    class DataViewModelCodeGenerator
    {
        /// <summary>
        /// 创建数据视图模型代码。
        /// </summary>
        /// <param name="assemblyPath">程序集路径。</param>
        /// <param name="classFullName">类全名。</param>
        /// <returns></returns>
        public string CreateViewModelCode(string assemblyPath, string classFullName,string remarkXmlFullName)
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            object objInstance = assembly.CreateInstance(classFullName); // 创建类的实例 
            Type objType = objInstance.GetType();
            PropertyInfo[] propertyArray = objType.GetProperties();

            XDocument remarkXmlDoc = null;

            if(string.IsNullOrEmpty(remarkXmlFullName) == false &&
               File.Exists(remarkXmlFullName))
            {
                remarkXmlDoc = XDocument.Load(remarkXmlFullName);
            }

            List<PropertyInfo> propertyList = new List<PropertyInfo>();
            if (propertyArray != null && propertyArray.Length > 0)
            {
                foreach (PropertyInfo propertyItem in propertyArray)
                {
                    if (propertyItem.PropertyType.IsPublic)
                    {
                        propertyList.Add(propertyItem);
                    }
                }
            }

            string viewModelClassName = CreateViewModelClassName(objType);
            string memberSegment = CreatePrivateMemberSegment(propertyList);
            string propertySegment = CreatePublicPropertySegment(propertyList, remarkXmlDoc);
            string updateMethodSegment = CreateUpdateMethodSeqmeng(propertyList);
            string text = string.Format(@"
public class {0}
{{
    #region member
{1}
    #endregion

    #region property
{2}
    #endregion

    #region update
    public void Update({3} entity)
    {{
    {4}
    }}
    #endregion
}}",
    viewModelClassName, memberSegment, propertySegment, objType.Name, updateMethodSegment);

            return text;
        }

        /// <summary>
        /// 创建ViewModel类名。
        /// </summary>
        /// <returns></returns>
        private string CreateViewModelClassName(Type objType)
        {
            return objType.Name + "ViewModel";
        }

        /// <summary>
        /// 创建成员项代码
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private string CreatePrivateMemberSegment(List<PropertyInfo> propertyList)
        {
            if (propertyList == null || propertyList.Count <= 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo propertyItem in propertyList)
            {
                string text = string.Format(@"private {0} {1} = {2};",
                propertyItem.GetPropertyTypeName(),
                propertyItem.GetPropertyMemberName(),
                propertyItem.GetPropertyMemberDefaultValue());

                sb.AppendLine(text);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private string CreatePublicPropertySegment(List<PropertyInfo> propertyList, XDocument remarkXmlDoc)
        {
            if (propertyList == null || propertyList.Count <= 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo propertyItem in propertyList)
            {
                string text = string.Format(@"
            /// <summary>
            /// {0}
            /// </summary>
            public {1} {2}
            {{
                get {{ return {3}; }}
                set
                {{
                    {3} = value;
                    SetProperty(() => this.{2});
                }}
            }}",
            propertyItem.GetPropertyRemark(remarkXmlDoc),
            propertyItem.GetPropertyTypeName(),
            propertyItem.GetPropertyName(),
            propertyItem.GetPropertyMemberName());
                sb.AppendLine(text);
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private string CreateUpdateMethodSeqmeng(List<PropertyInfo> propertyList)
        {
            if (propertyList == null || propertyList.Count <= 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo propertyItem in propertyList)
            {
                string text = string.Format("this.{0} = entity.{0};",
                    propertyItem.GetPropertyName());
                sb.AppendLine(text);
            }

            return sb.ToString();
        }

    }
}
