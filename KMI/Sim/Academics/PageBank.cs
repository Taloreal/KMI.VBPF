namespace KMI.Sim.Academics
{
    using KMI.Sim;
    using System;
    using System.IO;
    using System.Xml.Serialization;

    [Serializable]
    public class PageBank
    {
        public Level[] Levels;
        public string Name;

        public static PageBank LoadFromXML(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PageBank));
            try
            {
                TextReader textReader = new StreamReader(fileName);
                return (PageBank) serializer.Deserialize(textReader);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
            return null;
        }
    }
}

