namespace KMI.Utility
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    public class TableReader
    {
        protected static object ConvertStringToPropertyType(string s, PropertyInfo pi)
        {
            Type returnType = pi.GetGetMethod().ReturnType;
            try
            {
                if (returnType.Equals(typeof(int)))
                {
                    return Convert.ToInt32(s);
                }
                if (returnType.Equals(typeof(float)))
                {
                    return Convert.ToSingle(s);
                }
                if (returnType.Equals(typeof(DateTime)))
                {
                    return Convert.ToDateTime(s);
                }
                if (returnType.Equals(typeof(bool)))
                {
                    return Convert.ToBoolean(s);
                }
                if (returnType.Equals(typeof(string)))
                {
                    return Convert.ToString(s);
                }
            }
            catch (Exception exception2)
            {
                string[] textArray1 = new string[] { "The value ", s, " for the property ", pi.Name, " could not be converted to type ", returnType.ToString(), "." };
                Exception exception3 = new Exception(string.Concat(textArray1), exception2);
                throw exception3;
            }
            string[] textArray2 = new string[] { "The property ", pi.Name, " is not of a type that TableReader can convert. Add type ", returnType.ToString(), " to ConvertStringToProperty routine." };
            Exception exception = new Exception(string.Concat(textArray2));
            throw exception;
        }

        protected static PropertyInfo[] GetOrderedProperties(Type type, string r)
        {
            char[] separator = new char[] { '\t' };
            string[] strArray = r.Split(separator);
            PropertyInfo[] infoArray = new PropertyInfo[strArray.Length];
            for (int i = 0; i < infoArray.Length; i++)
            {
                infoArray[i] = type.GetProperty(strArray[i]);
                if (infoArray[i] == null)
                {
                    throw new Exception("The property name " + strArray[i] + " found in the table is not a property of the object.");
                }
            }
            return infoArray;
        }

        protected static object ObjectFromString(Type type, string text, PropertyInfo[] orderedProperties)
        {
            char[] separator = new char[] { '\t' };
            string[] strArray = text.Split(separator);
            object obj2 = type.GetConstructor(new Type[0]).Invoke(new object[0]);
            int index = 0;
            foreach (PropertyInfo info in orderedProperties)
            {
                object obj3 = ConvertStringToPropertyType(strArray[index], info);
                object[] parameters = new object[] { obj3 };
                info.GetSetMethod().Invoke(obj2, parameters);
                index++;
            }
            return obj2;
        }

        public static object[] Read(Type type, string resource)
        {
            return Read(type.Assembly, type, resource);
        }

        public static object[] Read(Assembly assembly, Type type, string resource)
        {
            Stream manifestResourceStream = assembly.GetManifestResourceStream(resource);
            string str = new StreamReader(manifestResourceStream).ReadToEnd();
            manifestResourceStream.Close();
            char[] separator = new char[] { '\n' };
            string[] strArray = str.Split(separator);
            ArrayList list = new ArrayList();
            bool flag = true;
            PropertyInfo[] orderedProperties = null;
            foreach (string str2 in strArray)
            {
                char[] trimChars = new char[] { '\r' };
                string r = str2.TrimEnd(trimChars);
                if (r != "")
                {
                    if (flag)
                    {
                        orderedProperties = GetOrderedProperties(type, r);
                        flag = false;
                    }
                    else
                    {
                        object obj2 = ObjectFromString(type, r, orderedProperties);
                        list.Add(obj2);
                    }
                }
            }
            return (object[]) list.ToArray(type);
        }
    }
}

