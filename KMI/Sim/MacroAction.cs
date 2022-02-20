namespace KMI.Sim
{
    using System;
    using System.Reflection;

    [Serializable]
    public class MacroAction
    {
        protected object[] argumentValues;
        protected MethodBase method;
        protected DateTime timestamp;

        public MacroAction()
        {
        }

        public MacroAction(MethodBase method, object[] argumentValues, DateTime timestamp)
        {
            this.method = method;
            this.argumentValues = argumentValues;
            this.timestamp = timestamp;
        }

        public object[] ArgumentValues
        {
            get
            {
                return this.argumentValues;
            }
        }

        public MethodBase Method
        {
            get
            {
                return this.method;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return this.timestamp;
            }
        }
    }
}

