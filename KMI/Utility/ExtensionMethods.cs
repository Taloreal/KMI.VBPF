using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KMI.Utility {

    public enum ForLoopDirection
    {
        Forward,
        Backward
    }

    public delegate void Action<T1, T2>(T1 p1, T2 p2);

    public static class ExtensionMethods
    {

        /// <summary>
        /// Splits a string by another string.
        /// Emulates the .net Core method. :)
        /// </summary>
        /// <param name="toSplit">The string to split.</param>
        /// <param name="delimiter">The string to split by.</param>
        /// <param name="strOps">Get rid of empty strings?</param>
        /// <returns>The string split into an array of strings.</returns>
        public static string[] Split(string toSplit, string delimiter, StringSplitOptions strOps = StringSplitOptions.None)
        {
            if (toSplit.Length < delimiter.Length) { return new string[] { toSplit }; }
            List<string> entries = new List<string>();
            string working = "";
            for (int i = 0; i < toSplit.Length; i++)
            {
                working += toSplit[i];
                if (working.EndsWith(delimiter))
                {
                    entries.Add(working.Substring(0, working.Length - delimiter.Length));
                    working = "";
                }
            }
            if (working != "") { entries.Add(working); }
            if (strOps == StringSplitOptions.RemoveEmptyEntries)
            {
                entries.RemoveAll(s => string.IsNullOrEmpty(s));
            }
            return entries.ToArray();
        }

        /// <summary>
        /// Gets a subset array from another array.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="arr">The original array.</param>
        /// <param name="start">The starting index.</param>
        /// <param name="count">The number of elements to copy.</param>
        /// <returns>The subset array.</returns>
        public static T[] SubArray<T>(T[] arr, int start, int count = -1)
        {
            start = (start < 0) ? 0 : start;
            count = (start + count >= arr.Length) ? arr.Length - start : count;
            if (count < 1) { return new T[0]; }
            List<T> objs = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                int pos = i + start;
                objs.Add(arr[pos]);
            }
            return objs.ToArray();
        }

        /// <summary>
        /// Attempts to parse a bool from a string.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <param name="result">The resulting bool.</param>
        /// <returns>Did the parsing work?</returns>
        public static bool TryParseBool(string str, out bool result)
        {
            result = false;
            str = str.ToLower();
            if (string.IsNullOrEmpty(str)) { return false; }
            if (str[0] == '0' || str.ToLower().StartsWith("false")) { return true; }
            if (str[0] == '1' || str.ToLower().StartsWith("true"))
            {
                result = true; return true;
            }
            if (str.StartsWith("no") || str == "n") { return true; }
            if (str.StartsWith("yes") || str.StartsWith("yea") || str == "y")
            {
                result = true; return true;
            }
            return false;
        }

        public static void ForEach<T>(IEnumerable<T> collection, Action<T> toDo) {
            foreach (T item in collection) { 
                toDo(item);
            }
        }

        public static void ForEach<T>(IEnumerable<T> collection, Action<T, int> toDo) {
            int ndx = 0;
            foreach (T item in collection) { 
                toDo(item, ndx);
                ndx += 1;
            }
        }

        public static void For<T>(T[] arr, Action<int> toDo, ForLoopDirection direction = ForLoopDirection.Forward) {
            int start = direction == ForLoopDirection.Forward ? 0 : arr.Length - 1;
            int end = direction == ForLoopDirection.Forward ? arr.Length : -1;
            int iterator = direction == ForLoopDirection.Forward ? 1 : -1;
            for (int i = start; direction == ForLoopDirection.Forward ? i < end : i > end; i += iterator) {
                toDo(i);
            }
        }

        public static T[] ShrinkArray<T>(T[] arr, int maxSize) {
            if (arr.Length <= maxSize) { return arr; }
            T[] newArray = new T[maxSize];
            for (int i = 0; i < maxSize; i++) {
                newArray[i] = arr[i];
            }
            return newArray;
        }

        public static T[] GrowArray<T>(T[] arr, int minSize) {
            if (arr.Length >= minSize) { return arr; }
            T[] newArray = new T[minSize];
            for (int i = 0; i < minSize; i++) {
                newArray[i] = i < arr.Length ? arr[i] : default(T);
            }
            return newArray;
        }

        public static bool SameValues<T>(T[] arr, T[] other) {
            if (arr.Length != other.Length) { return false; }
            for (int i = 0; i < arr.Length; i++) {
                if (arr[i].Equals(other[i]) == false) {
                    return false;
                }
            }
            return true;
        }

        public static List<T> FindAll<T>(this IEnumerable controls) where T : class {
            List<T> items = new List<T>();
            foreach (object item in controls) {
                if (item is T) {
                    items.Add(item as T);
                }
            }
            return items;
        }

        public static List<T> FindAll<T>(this IEnumerable collection, Func<T, bool> predicate) where T : class {
            List<T> items = new List<T>();
            foreach (object item in collection) {
                if (item is T) { 
                    T obj = item as T;
                    if (predicate(obj)) {
                        items.Add(obj);
                    }
                }
            }
            return items;
        }
    }
}
