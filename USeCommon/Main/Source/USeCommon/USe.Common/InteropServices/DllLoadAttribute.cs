using System;
using System.Reflection;

namespace USe.Common.InteropServices
{
    /// <summary>
    /// 指示该属性化委托类型成员由非托管动态链接库中指定的静态入口点公开。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DllLoadAttribute : Attribute
    {
        /// <summary>
        /// 函数入口点名称或序号。
        /// </summary>
        public string EntryPoint;

        /// <summary>
        /// 委托类型。
        /// </summary>
        public Type DelegateType;

        /// <summary>
        /// 当前函数是否必须存在标志，默认为True。
        /// </summary>
        public bool IsRequired;


        /// <summary>
        /// 初始化DllLoadAttribute类的新实例。
        /// </summary>
        /// <remarks>
        /// 注：入口点名称将使用默认规则赋值，规则：被属性所标记的委托类型成员的名字。
        /// </remarks>
        public DllLoadAttribute()
        {
            this.EntryPoint = null;
            this.DelegateType = null;
            this.IsRequired = true;
        }

        /// <summary>
        /// 使用指定的函数入口点名称初始化DllLoadAttribute类的新实例。
        /// </summary>
        /// <param name="entryPoint">函数入口点名称或序号。</param>
        public DllLoadAttribute(string entryPoint)
            : this(entryPoint, true)
        {
        }

        /// <summary>
        /// 使用指定的函数入口点名称和必须标志初始化DllLoadAttribute类的新实例。
        /// </summary>
        /// <param name="entryPoint">函数入口点名称或序号。</param>
        /// <param name="isRequired">当前函数是否必须存在标志。</param>
        public DllLoadAttribute(string entryPoint, bool isRequired)
        {
            if (String.IsNullOrEmpty(entryPoint))
            {
                throw new ArgumentNullException("entryPoint", "EntryPoint can not be null or empty.");
            }

            this.EntryPoint = entryPoint;
            this.DelegateType = null;
            this.IsRequired = isRequired;
        }

        /// <summary>
        /// 返回当前DllLoadAttribute类对象的描述字符串。
        /// </summary>
        /// <returns>表示当前DllLoadAttribute类对象的描述字符串。</returns>
        public override string ToString()
        {
            return String.Format("{0}[{1}, {2}, {3}]", this.GetType().Name, this.EntryPoint, this.DelegateType.Name, this.IsRequired);
        }

        /// <summary>
        /// 使用指定的字段信息初始化本实例中还未初始化的成员。
        /// </summary>
        /// <param name="fieldInfo">字段信息对象。</param>
        internal void Initialize(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException("fieldInfo");
            }

            if (!fieldInfo.FieldType.IsSubclassOf(typeof(MulticastDelegate)))
            {
                throw new InvalidOperationException("Field type must be MulticastDelegate.");
            }

            this.DelegateType = fieldInfo.FieldType;

            if (String.IsNullOrEmpty(this.EntryPoint))
            {
                this.EntryPoint = fieldInfo.Name;
            }
        }
    }
}
