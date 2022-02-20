namespace KMI.VBPF1Lib
{
    using System;
    using System.Collections;

    [Serializable]
    public class JobApplication
    {
        public bool Car;
        public string Name;
        public ArrayList ReportedClassNames = new ArrayList();
        public Hashtable ReportedJobNamesAndMonths = new Hashtable();
    }
}

