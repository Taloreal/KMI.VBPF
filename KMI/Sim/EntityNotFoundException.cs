namespace KMI.Sim
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EntityNotFoundException : SimApplicationException
    {
        protected long existingEntityID;

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string messageStringResource) : base(messageStringResource)
        {
            this.existingEntityID = -1L;
            if (S.State.Entity != null)
            {
                foreach (Entity entity in S.State.Entity.Values)
                {
                    this.existingEntityID = entity.ID;
                    break;
                }
            }
        }

        public EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.existingEntityID = (long) info.GetValue("existingEntityID", typeof(long));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("existingEntityID", this.existingEntityID);
        }

        public long ExistingEntityID
        {
            get
            {
                return this.existingEntityID;
            }
        }
    }
}

