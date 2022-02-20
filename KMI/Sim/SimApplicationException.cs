namespace KMI.Sim
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SimApplicationException : ApplicationException, ISerializable
    {
        protected string applicationMessage;

        public SimApplicationException()
        {
        }

        public SimApplicationException(string message)
        {
            this.applicationMessage = message;
        }

        public SimApplicationException(SerializationInfo info, StreamingContext context)
        {
            this.applicationMessage = (string) info.GetValue("applicationMessage", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("applicationMessage", this.applicationMessage);
        }

        public override string Message
        {
            get
            {
                return this.applicationMessage;
            }
        }
    }
}

