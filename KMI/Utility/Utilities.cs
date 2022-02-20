namespace KMI.Utility
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Windows.Forms;

    public sealed class Utilities
    {
        private const int _EM_INVALID = 0x10;
        private const int _MCW_EM = 0x8001f;
        private static ResourceManager rm = new ResourceManager("KMI.Utility.Names", Assembly.GetAssembly(typeof(Utilities)));

        private Utilities()
        {
        }

        [DllImport("msvcrt.dll")]
        private static extern int _controlfp(int IN_New, int IN_Mask);
        public static string AddAOrAn(string s)
        {
            if ((((s.Substring(0, 1).ToLower() == "a") || (s.Substring(0, 1).ToLower() == "e")) || ((s.Substring(0, 1).ToLower() == "i") || (s.Substring(0, 1).ToLower() == "o"))) || (s.Substring(0, 1).ToLower() == "u"))
            {
                return ("an " + s);
            }
            return ("a " + s);
        }

        public static PointF AddPointFs(PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point AddPoints(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static string AddSpaces(string s)
        {
            if (s.Length > 0)
            {
                s = s.Substring(0, 1).ToUpper() + s.Substring(1);
                for (int i = s.Length - 1; i > 0; i--)
                {
                    if (s.Substring(i, 1).ToUpper() == s.Substring(i, 1))
                    {
                        s = s.Substring(0, i) + " " + s.Substring(i);
                    }
                }
            }
            return s;
        }

        public static PointF cartesianToIsometric(float x, float y)
        {
            return cartesianToIsometric(x, y, 0f, 0f, 1f, 1f);
        }

        public static PointF cartesianToIsometric(float x, float y, float offsetx, float offsety)
        {
            return cartesianToIsometric(x, y, offsetx, offsety, 1f, 1f);
        }

        public static PointF cartesianToIsometric(float x, float y, float offsetx, float offsety, float scalex, float scaley)
        {
            return new PointF(offsetx + (((x * scalex) * 1f) - ((y * scaley) * 1f)), offsety + (((x * scalex) * 0.5f) + ((y * scaley) * 0.5f)));
        }

        public static Point[] cartesianToIsometricRectangle(float offsetx, float offsety, float width, float height)
        {
            Point[] pointArray = new Point[4];
            PointF tf = cartesianToIsometric(0f, 0f, offsetx, offsety, width, height);
            pointArray[0] = new Point((int) tf.X, (int) tf.Y);
            tf = cartesianToIsometric(1f, 0f, offsetx, offsety, width, height);
            pointArray[1] = new Point((int) tf.X, (int) tf.Y);
            tf = cartesianToIsometric(1f, 1f, offsetx, offsety, width, height);
            pointArray[2] = new Point((int) tf.X, (int) tf.Y);
            tf = cartesianToIsometric(0f, 1f, offsetx, offsety, width, height);
            pointArray[3] = new Point((int) tf.X, (int) tf.Y);
            return pointArray;
        }

        public static float Clamp(float amount)
        {
            if (amount < 0f)
            {
                return 0f;
            }
            if (amount > 1f)
            {
                return 1f;
            }
            return amount;
        }

        public static float Clamp(float amount, float max)
        {
            if (amount < 0f)
            {
                return 0f;
            }
            if (amount > max)
            {
                return max;
            }
            return amount;
        }

        public static float Clamp(float amount, float min, float max)
        {
            if (amount < min)
            {
                return min;
            }
            if (amount > max)
            {
                return max;
            }
            return amount;
        }

        public static string ConvertNumberToDescription(int x)
        {
            string str = x.ToString();
            string str2 = str.Substring(str.Length - 1);
            if (str2 != null)
            {
                switch (str2)
                {
                    case "1":
                        str = str + "st";
                        goto Label_0093;

                    case "2":
                        str = str + "nd";
                        goto Label_0093;

                    case "3":
                        str = str + "rd";
                        goto Label_0093;
                }
            }
            str = str + "th";
        Label_0093:
            if (((str.Substring(str.Length - 2) == "11") || (str.Substring(str.Length - 2) == "12")) || (str.Substring(str.Length - 2) == "13"))
            {
                str = x + "th";
            }
            return str;
        }

        public static string Decrypt(string cipherString, string key)
        {
            byte[] buffer = Convert.FromBase64String(cipherString);
            byte[] bytes = Encoding.ASCII.GetBytes(key);
            byte[] buffer3 = new byte[buffer.Length];
            for (int i = 0; i < buffer3.Length; i++)
            {
                buffer3[i] = (byte) (buffer[i] ^ bytes[i % bytes.Length]);
            }
            return Encoding.ASCII.GetString(buffer3);
        }

        public static object DeepCopyBySerialization(object objectToCopy)
        {
            Stream serializationStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(serializationStream, objectToCopy);
            serializationStream.Position = 0L;
            object obj2 = formatter.Deserialize(serializationStream);
            serializationStream.Close();
            return obj2;
        }

        public static PointF[] DiamondFFromRect(RectangleF rect)
        {
            float x = (rect.Right + rect.Left) / 2f;
            float y = (rect.Top + rect.Bottom) / 2f;
            return new PointF[] { new PointF(x, y - (rect.Width / 4f)), new PointF(rect.Right, y), new PointF(x, y + (rect.Width / 4f)), new PointF(rect.Left, y) };
        }

        public static Point[] DiamondFromRect(RectangleF rect)
        {
            float x = (rect.Right + rect.Left) / 2f;
            float y = (rect.Top + rect.Bottom) / 2f;
            return new Point[] { Point.Round(new PointF(x, y - (rect.Width / 4f))), Point.Round(new PointF(rect.Right, y)), Point.Round(new PointF(x, y + (rect.Width / 4f))), Point.Round(new PointF(rect.Left, y)) };
        }

        public static float DistanceBetween(PointF p1, PointF p2)
        {
            return (float) Math.Sqrt(Math.Pow((double) (p1.X - p2.X), 2.0) + Math.Pow((double) (p1.Y - p2.Y), 2.0));
        }

        public static float DistanceBetweenIsometric(PointF p1, PointF p2)
        {
            return (float) Math.Sqrt(Math.Pow((double) (p1.X - p2.X), 2.0) + Math.Pow((double) (2f * (p1.Y - p2.Y)), 2.0));
        }

        public static float DistanceBetweenPointAndLine(PointF p1, PointF e1, PointF e2)
        {
            float maxValue;
            PointF empty = (PointF) Point.Empty;
            if (e1.Y == e2.Y)
            {
                maxValue = float.MaxValue;
            }
            else
            {
                maxValue = (e2.Y - e1.Y) / (e2.X - e1.X);
            }
            empty.X = (((((maxValue * maxValue) * e1.Y) + p1.X) + (maxValue * p1.Y)) - (maxValue * e1.Y)) / ((maxValue * maxValue) + 1f);
            empty.X = Math.Min(empty.X, Math.Max(e1.X, e2.X));
            empty.X = Math.Max(empty.X, Math.Min(e1.X, e2.X));
            empty.Y = (maxValue * (empty.X - e1.X)) + e1.Y;
            return DistanceBetween(p1, empty);
        }

        private static bool DoesIntersect(PointF point, PointF point1, PointF point2)
        {
            float x = point2.X;
            float y = point2.Y;
            float num3 = point1.X;
            float num4 = point1.Y;
            if (((x < point.X) && (num3 >= point.X)) || ((x >= point.X) && (num3 < point.X)))
            {
                float num5 = (((y - num4) / (x - num3)) * (point.X - num3)) + num4;
                return (num5 > point.Y);
            }
            return false;
        }

        public static void DrawComment(Graphics g, string text, Point anchor, Rectangle bounds)
        {
            DrawComment(g, text, anchor, bounds, 150);
        }

        public static void DrawComment(Graphics g, string text, Point anchor, Rectangle bounds, int commentWidth)
        {
            Font font = new Font("Arial", 9f);
            Color steelBlue = Color.SteelBlue;
            DrawComment(g, text, anchor, bounds, commentWidth, font, steelBlue);
        }

        public static void DrawComment(Graphics g, string text, Point anchor, Rectangle bounds, Font font, Color color)
        {
            DrawComment(g, text, anchor, bounds, 150, font, color);
        }

        public static void DrawComment(Graphics g, string text, Point anchor, Rectangle bounds, int commentWidth, Font font, Color color)
        {
            Point point;
            Point[] pointArray;
            Point[] pointArray2;
            Brush brush = new SolidBrush(Color.White);
            Brush brush2 = new SolidBrush(color);
            Pen pen = new Pen(brush2);
            SizeF ef = g.MeasureString(text, font, commentWidth);
            SizeF ef2 = new SizeF(ef.Width + 16f, ef.Height + 16f);
            bool flag = ((anchor.X + ef2.Width) + 15f) > bounds.Width;
            int num = 0;
            if (((anchor.Y - ef2.Height) - 15f) < 0f)
            {
                if (((anchor.Y + ef2.Height) + 15f) > bounds.Height)
                {
                    num = 1;
                }
                else
                {
                    num = 2;
                }
            }
            if (flag)
            {
                if (num == 2)
                {
                    point = new Point(anchor.X - 15, anchor.Y + 15);
                    g.FillRectangle(brush, point.X - ef2.Width, (float) point.Y, ef2.Width, ef2.Height);
                    g.DrawRectangle(pen, point.X - ef2.Width, (float) point.Y, ef2.Width, ef2.Height);
                    g.DrawString(text, font, brush2, new RectangleF((point.X - ef2.Width) + 8f, (float) (point.Y + 8), ef.Width, ef.Height));
                    pointArray2 = new Point[] { anchor, new Point(point.X, point.Y + 8), point, new Point(point.X - 8, point.Y), anchor };
                    pointArray = pointArray2;
                    g.FillPolygon(brush2, pointArray);
                }
                else if (num == 1)
                {
                    point = new Point(anchor.X - 15, anchor.Y - 15);
                    pointArray2 = new Point[] { anchor, new Point(point.X, point.Y - 8), point, new Point(point.X - 8, point.Y), anchor };
                    pointArray = pointArray2;
                    g.FillPolygon(brush2, pointArray);
                    g.FillRectangle(brush, point.X - ef2.Width, point.Y - (ef2.Height / 2f), ef2.Width, ef2.Height);
                    g.DrawRectangle(pen, point.X - ef2.Width, point.Y - (ef2.Height / 2f), ef2.Width, ef2.Height);
                    g.DrawString(text, font, brush2, new RectangleF((point.X - ef2.Width) + 8f, (point.Y - (ef2.Height / 2f)) + 8f, ef.Width, ef.Height));
                }
                else
                {
                    point = new Point(anchor.X - 15, anchor.Y - 15);
                    g.FillRectangle(brush, point.X - ef2.Width, point.Y - ef2.Height, ef2.Width, ef2.Height);
                    g.DrawRectangle(pen, point.X - ef2.Width, point.Y - ef2.Height, ef2.Width, ef2.Height);
                    g.DrawString(text, font, brush2, new RectangleF((point.X - ef2.Width) + 8f, (point.Y - ef2.Height) + 8f, ef.Width, ef.Height));
                    pointArray2 = new Point[] { anchor, new Point(point.X, point.Y - 8), point, new Point(point.X - 8, point.Y), anchor };
                    pointArray = pointArray2;
                    g.FillPolygon(brush2, pointArray);
                }
            }
            else if (num == 2)
            {
                point = new Point(anchor.X + 15, anchor.Y + 15);
                g.FillRectangle(brush, (float) point.X, (float) point.Y, ef2.Width, ef2.Height);
                g.DrawRectangle(pen, (float) point.X, (float) point.Y, ef2.Width, ef2.Height);
                g.DrawString(text, font, brush2, new RectangleF((float) (point.X + 8), (float) (point.Y + 8), ef.Width, ef.Height));
                pointArray2 = new Point[] { anchor, new Point(point.X, point.Y + 8), point, new Point(point.X + 8, point.Y), anchor };
                pointArray = pointArray2;
                g.FillPolygon(brush2, pointArray);
            }
            else if (num == 1)
            {
                point = new Point(anchor.X + 15, anchor.Y - 15);
                pointArray2 = new Point[] { anchor, new Point(point.X, point.Y - 8), point, new Point(point.X + 8, point.Y), anchor };
                pointArray = pointArray2;
                g.FillPolygon(brush2, pointArray);
                g.FillRectangle(brush, (float) point.X, point.Y - (ef2.Height / 2f), ef2.Width, ef2.Height);
                g.DrawRectangle(pen, (float) point.X, point.Y - (ef2.Height / 2f), ef2.Width, ef2.Height);
                g.DrawString(text, font, brush2, new RectangleF((float) (point.X + 8), (point.Y - (ef2.Height / 2f)) + 8f, ef.Width, ef.Height));
            }
            else
            {
                point = new Point(anchor.X + 15, anchor.Y - 15);
                g.FillRectangle(brush, (float) point.X, point.Y - ef2.Height, ef2.Width, ef2.Height);
                g.DrawRectangle(pen, (float) point.X, point.Y - ef2.Height, ef2.Width, ef2.Height);
                g.DrawString(text, font, brush2, new RectangleF((float) (point.X + 8), (point.Y - ef2.Height) + 8f, ef.Width, ef.Height));
                pointArray2 = new Point[] { anchor, new Point(point.X, point.Y - 8), point, new Point(point.X + 8, point.Y), anchor };
                pointArray = pointArray2;
                g.FillPolygon(brush2, pointArray);
            }
        }

        public static string Encrypt(string clearString, string key)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(clearString);
            byte[] buffer2 = Encoding.ASCII.GetBytes(key);
            byte[] inArray = new byte[bytes.Length];
            for (int i = 0; i < inArray.Length; i++)
            {
                inArray[i] = (byte) (bytes[i] ^ buffer2[i % buffer2.Length]);
            }
            return Convert.ToBase64String(inArray);
        }

        public static string FC(float amount, float currencyConversion)
        {
            float num = amount * currencyConversion;
            return num.ToString("C0");
        }

        public static string FC(float amount, int decimalPlaces, float currencyConversion)
        {
            float num = amount * currencyConversion;
            return num.ToString("C" + decimalPlaces);
        }

        public static ToolBarButton FindButtonEquivalent(ToolBar toolBar, string TargetButtonText)
        {
            foreach (ToolBarButton button in toolBar.Buttons)
            {
                if (button.Text == NoEllipsis(NoAmpersand(TargetButtonText)))
                {
                    return button;
                }
            }
            return null;
        }

        public static MenuItem FindMenuEquivalent(MainMenu SearchMenu, string TargetMenuText)
        {
            TargetMenuText = NoHyphen(NoForwardSlash(TargetMenuText));
            foreach (MenuItem item in SearchMenu.MenuItems)
            {
                if (FindMenuEquivalent(item, TargetMenuText) != null)
                {
                    return FindMenuEquivalent(item, TargetMenuText);
                }
            }
            return null;
        }

        public static MenuItem FindMenuEquivalent(MenuItem SearchMenu, string TargetMenuText)
        {
            if (NoEllipsis(NoAmpersand(NoHyphen(NoForwardSlash(SearchMenu.Text)))) == TargetMenuText)
            {
                return SearchMenu;
            }
            foreach (MenuItem item2 in SearchMenu.MenuItems)
            {
                if (FindMenuEquivalent(item2, TargetMenuText) != null)
                {
                    return FindMenuEquivalent(item2, TargetMenuText);
                }
            }
            return null;
        }

        public static string FormatCommaSeries(string s)
        {
            if (s != "")
            {
                char[] trimChars = new char[] { ' ', ',' };
                s = s.TrimEnd(trimChars);
                int length = s.LastIndexOf(", ");
                if (length > -1)
                {
                    return (s.Substring(0, length) + " and " + s.Substring(length + 2));
                }
            }
            return s;
        }

        public static string FormatCommaSeriesDuplicatesToNumbers(string series)
        {
            series = series.Replace(", ", ",");
            char[] trimChars = new char[] { ',' };
            series = series.TrimEnd(trimChars);
            char[] separator = new char[] { ',' };
            string[] strArray = series.Split(separator);
            Hashtable hashtable = new Hashtable();
            foreach (string str2 in strArray)
            {
                if (hashtable.ContainsKey(str2))
                {
                    hashtable[str2] = ((int) hashtable[str2]) + 1;
                }
                else
                {
                    hashtable.Add(str2, 1);
                }
            }
            string s = "";
            foreach (string str3 in hashtable.Keys)
            {
                int num2 = (int) hashtable[str3];
                if (num2 > 1)
                {
                    string str4 = s;
                    string[] textArray1 = new string[] { str4, str3, " (", num2.ToString(), "), " };
                    s = string.Concat(textArray1);
                }
                else
                {
                    s = s + str3 + ", ";
                }
            }
            return FormatCommaSeries(s);
        }

        public static string FormatCommaSeriesDuplicatesToNumbers2(string series)
        {
            series = series.Replace(", ", ",");
            char[] trimChars = new char[] { ',' };
            series = series.TrimEnd(trimChars);
            char[] separator = new char[] { ',' };
            string[] strArray = series.Split(separator);
            Hashtable hashtable = new Hashtable();
            foreach (string str2 in strArray)
            {
                if (hashtable.ContainsKey(str2))
                {
                    hashtable[str2] = ((int) hashtable[str2]) + 1;
                }
                else
                {
                    hashtable.Add(str2, 1);
                }
            }
            string s = "";
            foreach (string str3 in hashtable.Keys)
            {
                int num2 = (int) hashtable[str3];
                if (num2 > 1)
                {
                    string str4 = s;
                    string[] textArray1 = new string[] { str4, "(", num2.ToString(), ") ", str3, ", " };
                    s = string.Concat(textArray1);
                }
                else
                {
                    s = s + str3 + ", ";
                }
            }
            return FormatCommaSeries(s);
        }

        public static string FP(float amount)
        {
            return amount.ToString("P0");
        }

        public static string FP(float amount, int decimalPlaces)
        {
            return amount.ToString("P" + decimalPlaces);
        }

        public static string FU(int amount)
        {
            return amount.ToString("N0");
        }

        public static float GetNormalDistribution(float middle, float twoStdDev, Random random)
        {
            float num = 0f;
            int num2 = 10;
            for (int i = 0; i < num2; i++)
            {
                num += ((float) ((middle - (twoStdDev * 2.5)) + (((random.NextDouble() * 2.0) * twoStdDev) * 2.5))) / ((float) num2);
            }
            return num;
        }

        public static string GetRandomFemaleFirstName(Random rng)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = rm.GetString("FemaleFirstNames").Split(separator);
            return strArray[rng.Next(strArray.Length)];
        }

        public static string GetRandomFemaleFullName(Random rng)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = rm.GetString("FemaleFirstNames").Split(separator);
            char[] chArray2 = new char[] { '|' };
            string[] strArray2 = rm.GetString("LastNames").Split(chArray2);
            return (strArray[rng.Next(strArray.Length)] + " " + strArray2[rng.Next(strArray2.Length)]);
        }

        public static string GetRandomFirstName(Random rng)
        {
            if (rng.Next(2) == 0)
            {
                return GetRandomMaleFirstName(rng);
            }
            return GetRandomFemaleFirstName(rng);
        }

        public static string GetRandomFullName(Random rng)
        {
            if (rng.Next(2) == 0)
            {
                return GetRandomMaleFullName(rng);
            }
            return GetRandomFemaleFullName(rng);
        }

        public static string GetRandomLastName(Random rng)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = rm.GetString("LastNames").Split(separator);
            return strArray[rng.Next(strArray.Length)];
        }

        public static string GetRandomMaleFirstName(Random rng)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = rm.GetString("MaleFirstNames").Split(separator);
            return strArray[rng.Next(strArray.Length)];
        }

        public static string GetRandomMaleFullName(Random rng)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = rm.GetString("MaleFirstNames").Split(separator);
            char[] chArray2 = new char[] { '|' };
            string[] strArray2 = rm.GetString("LastNames").Split(chArray2);
            return (strArray[rng.Next(strArray.Length)] + " " + strArray2[rng.Next(strArray2.Length)]);
        }

        public static DateTime GetTimeViaInternet()
        {
            return Convert.ToDateTime(GetWebPage(WebRequest.Create("http://vbc.knowledgematters.com/vbccommon/time.asp")));
        }

        public static System.Type GetTypeFromEntryAssembly()
        {
            return Assembly.GetEntryAssembly().GetTypes()[0];
        }

        public static string GetWebPage(WebRequest r)
        {
            WebResponse response;
            try
            {
                response = r.GetResponse();
            }
            catch
            {
                return "";
            }
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadToEnd();
            reader.Close();
            return str;
        }

        public static string GetWebPage(WebRequest r, string proxyAddress, string proxyBypassList)
        {
            if ((proxyAddress != null) && (proxyAddress != ""))
            {
                WebProxy proxy2 = new WebProxy {
                    Address = new Uri(proxyAddress)
                };
                char[] separator = new char[] { ';' };
                proxy2.BypassList = proxyBypassList.Split(separator);
                WebProxy proxy = proxy2;
                r.Proxy = proxy;
            }
            return GetWebPage(r);
        }

        public static bool Intersects(RectangleF rect1, RectangleF rect2)
        {
            PointF[] cornerPoints = DiamondFFromRect(rect1);
            PointF[] tfArray2 = DiamondFFromRect(rect2);
            foreach (PointF tf in cornerPoints)
            {
                if (PolygonContains(tf, tfArray2))
                {
                    return true;
                }
            }
            foreach (PointF tf2 in tfArray2)
            {
                if (PolygonContains(tf2, cornerPoints))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Intersects(PointF line1Begin, PointF line1End, RectangleF rect)
        {
            PointF[] tfArray = DiamondFFromRect(rect);
            for (int i = 0; i < tfArray.Length; i++)
            {
                if (Intersects(line1Begin, line1End, tfArray[i], tfArray[(i + 1) % tfArray.Length]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Intersects(PointF line1Begin, PointF line1End, PointF line2Begin, PointF line2End)
        {
            float f = (line1End.Y - line1Begin.Y) / (line1End.X - line1Begin.X);
            float num2 = (line2End.Y - line2Begin.Y) / (line2End.X - line2Begin.X);
            if (float.IsInfinity(f))
            {
                f = 10000f;
            }
            if (float.IsInfinity(num2))
            {
                num2 = 10000f;
            }
            float num3 = line1Begin.Y - (f * line1Begin.X);
            float num4 = line2Begin.Y - (num2 * line2Begin.X);
            if (f == num2)
            {
                return false;
            }
            float x = (num4 - num3) / (f - num2);
            float num6 = (f * x) + num3;
            if (line1End.X == line1Begin.X)
            {
                x = line1End.X;
            }
            if (line2End.X == line2Begin.X)
            {
                x = line2End.X;
            }
            return (((((x <= Math.Max(line1Begin.X, line1End.X)) && (x <= Math.Max(line2Begin.X, line2End.X))) && ((x >= Math.Min(line1Begin.X, line1End.X)) && (x >= Math.Min(line2Begin.X, line2End.X)))) && (((num6 <= Math.Max(line1Begin.Y, line1End.Y)) && (num6 <= Math.Max(line2Begin.Y, line2End.Y))) && (num6 >= Math.Min(line1Begin.Y, line1End.Y)))) && (num6 >= Math.Min(line2Begin.Y, line2End.Y)));
        }

        public static string MakePlural(int n)
        {
            if (n != 1)
            {
                return "s";
            }
            return "";
        }

        public static string MakePossessive(string s)
        {
            return (s + "'s");
        }

        public static long MeasureBinaryFormattedSize(object objectToCopy)
        {
            Stream serializationStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(serializationStream, objectToCopy);
            long length = serializationStream.Length;
            serializationStream.Close();
            return length;
        }

        public static string NoAmpersand(string s)
        {
            s = s.Replace("&", "");
            return s.Replace("  ", " ");
        }

        public static string NoEllipsis(string s)
        {
            return s.Replace("...", "");
        }

        public static string NoForwardSlash(string s)
        {
            return s.Replace("/", " ");
        }

        public static string NoHyphen(string s)
        {
            return s.Replace("-", " ");
        }

        public static string Parse(string searchString, string delimiter1, string delimiter2)
        {
            int index = searchString.IndexOf(delimiter1);
            int num2 = searchString.IndexOf(delimiter2, index);
            if ((index < 0) || (num2 < 0))
            {
                throw new Exception("Delimiter not found.");
            }
            return searchString.Substring(index + delimiter1.Length, num2 - (index + delimiter1.Length));
        }

        public static string PathOnly(string path)
        {
            FileInfo info = new FileInfo(path);
            return info.Directory.ToString();
        }

        public static int PickFromDistribution(float[] pdf, Random random)
        {
            int num;
            int length = pdf.Length;
            float num3 = 0f;
            for (num = 0; num < length; num++)
            {
                num3 += pdf[num];
            }
            float num4 = ((float) random.NextDouble()) * num3;
            float num5 = 0f;
            for (num = 0; num < length; num++)
            {
                num5 += pdf[num];
                if (num5 >= num4)
                {
                    return num;
                }
            }
            return (length - 1);
        }

        public static object PickRandom(IList list, Random random)
        {
            if ((list == null) || (list.Count == 0))
            {
                return null;
            }
            return list[random.Next(list.Count)];
        }

        public static bool PolygonContains(PointF point, PointF[] cornerPoints)
        {
            int num = 0;
            float x = point.X;
            float y = point.Y;
            for (int i = 1; i < cornerPoints.Length; i++)
            {
                if (DoesIntersect(point, cornerPoints[i], cornerPoints[i - 1]))
                {
                    num++;
                }
            }
            if (DoesIntersect(point, cornerPoints[cornerPoints.Length - 1], cornerPoints[0]))
            {
                num++;
            }
            return ((num % 2) > 0);
        }

        public static void PrintWithExceptionHandling(string title, PrintPageEventHandler p)
        {
            PrintDialog dialog = new PrintDialog();
            PrintDocument document = new PrintDocument();
            document.PrintPage += p;
            dialog.Document = document;
            dialog.AllowPrintToFile = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    document.PrinterSettings = dialog.PrinterSettings;
                    document.Print();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Could not print. " + exception.Message + ".", "Error Printing");
                }
            }
        }

        public static void ResetFPU()
        {
            _controlfp(0x8001f, 0x10);
        }

        public static float RoundDownToPowerOfTen(float number, int zeroes)
        {
            return (float) (((long) (((double) number) / Math.Pow(10.0, (double) zeroes))) * ((long) Math.Pow(10.0, (double) zeroes)));
        }

        public static float RoundUpToPowerOfTen(float number, int zeroes)
        {
            return (float) (((long) ((((double) number) / Math.Pow(10.0, (double) zeroes)) + 1.0)) * ((long) Math.Pow(10.0, (double) zeroes)));
        }

        public static void SerializeBinaryFormatObjectToFile(object obj, string filename)
        {
            Stream serializationStream = null;
            string path = Application.StartupPath + @"\" + filename;
            try
            {
                serializationStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            catch
            {
                throw new IOException("Could not open file for writing:" + path);
            }
            IFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(serializationStream, obj);
            }
            catch
            {
                throw new Exception("Could not serialize binary configuration to " + path);
            }
            finally
            {
                serializationStream.Close();
            }
        }

        public static void Shuffle(object[] list, Random rnd)
        {
            if (list == null)
            {
                throw new NullReferenceException("In Utility.Shuffle, got a null list");
            }
            if (rnd == null)
            {
                throw new NullReferenceException("In Utility.Shuffle, got a null random number generator");
            }
            int length = list.Length;
            for (int i = 0; i < (3 * length); i++)
            {
                int index = rnd.Next(length);
                int num4 = rnd.Next(length);
                object obj2 = list[index];
                list[index] = list[num4];
                list[num4] = obj2;
            }
        }

        public static void Shuffle(ArrayList list, Random rnd)
        {
            if (list == null)
            {
                throw new NullReferenceException("In Utility.Shuffle, got a null array list");
            }
            if (rnd == null)
            {
                throw new NullReferenceException("In Utility.Shuffle, got a null random number generator");
            }
            int count = list.Count;
            for (int i = 0; i < (3 * count); i++)
            {
                int num3 = rnd.Next(count);
                int num4 = rnd.Next(count);
                object obj2 = list[num3];
                list[num3] = list[num4];
                list[num4] = obj2;
            }
        }

        public static float[] SplitStringsIntoFloats(string itemDelimString)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = itemDelimString.Split(separator);
            float[] numArray = new float[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                numArray[i] = float.Parse(strArray[i]);
            }
            return numArray;
        }

        public static PointF SpreadOut(PointF p, float spread, Random r)
        {
            float num = (((float) r.NextDouble()) * spread) - (spread / 2f);
            float num2 = ((((float) r.NextDouble()) * spread) / 2f) - (spread / 4f);
            return new PointF(p.X + num, p.Y + num2);
        }

        public static string URLEncode(string url)
        {
            url = url.Replace("%", "%25");
            url = url.Replace(" ", "%20");
            url = url.Replace("!", "%21");
            url = url.Replace("\"", "%22");
            url = url.Replace("#", "%23");
            url = url.Replace("$", "%24");
            url = url.Replace("&", "%26");
            url = url.Replace("'", "%27");
            url = url.Replace("(", "%28");
            url = url.Replace(")", "%29");
            url = url.Replace("*", "%2a");
            url = url.Replace("+", "%2b");
            url = url.Replace(",", "%2c");
            url = url.Replace("-", "%2d");
            url = url.Replace(".", "%2e");
            url = url.Replace("/", "%2f");
            return url;
        }

        public static float Vary(float center, float maxVariation, Random random)
        {
            return (center + (((((float) random.NextDouble()) * 2f) - 1f) * maxVariation));
        }
    }
}

