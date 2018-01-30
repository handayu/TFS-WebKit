using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace USe.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class USeXmlSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="sourceObj"></param>
        public static void SaveToXml(string filePath, object sourceObj)
        {
            if (sourceObj == null) return;

            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Directory.Exists == false)
            {
                fileInfo.Directory.Create();
            }

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(sourceObj.GetType());
                xmlSerializer.Serialize(writer, sourceObj);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="sourceObj"></param>
        /// <param name="overrides"></param>
        public static void SaveToXml(string filePath, object sourceObj, XmlAttributeOverrides overrides)
        {
            if (sourceObj == null) return;

            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Directory.Exists == false)
            {
                fileInfo.Directory.Create();
            }

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(sourceObj.GetType(),overrides);
                xmlSerializer.Serialize(writer, sourceObj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadFromXml<T>(string filePath)
        {
            if (File.Exists(filePath) == false) return default(T);

            object result = null;
            using (StreamReader reader = new StreamReader(filePath))
            {
                if(reader.EndOfStream)
                {
                    return default(T);
                }
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                result = xmlSerializer.Deserialize(reader);
            }

            return (T)result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public static T LoadFromXml<T>(string filePath, XmlAttributeOverrides overrides)
        {
            if (File.Exists(filePath) == false) return default(T);

            object result = null;
            using (StreamReader reader = new StreamReader(filePath))
            {
                if (reader.EndOfStream)
                {
                    return default(T);
                }
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T), overrides);
                result = xmlSerializer.Deserialize(reader);
            }

            return (T)result;
        }
    }
}
