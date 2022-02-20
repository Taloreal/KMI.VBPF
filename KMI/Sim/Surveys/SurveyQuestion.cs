namespace KMI.Sim.Surveys
{
    using System;

    [Serializable]
    public class SurveyQuestion
    {
        protected bool answersAreEntities;
        protected bool multiEntity;
        protected Survey parent;
        protected string[] possibleResponses;
        protected string possibleResponsesConcatenated;
        protected string question;
        protected string shortName;

        public bool AnswersAreEntities
        {
            get
            {
                return this.answersAreEntities;
            }
            set
            {
                this.answersAreEntities = value;
            }
        }

        public bool MultiEntity
        {
            get
            {
                return this.multiEntity;
            }
            set
            {
                this.multiEntity = value;
            }
        }

        public Survey Parent
        {
            set
            {
                this.parent = value;
            }
        }

        public string[] PossibleResponses
        {
            get
            {
                if (!this.answersAreEntities)
                {
                    return this.possibleResponses;
                }
                string[] array = new string[this.parent.EntityNames.Length + 1];
                array[0] = "None";
                this.parent.EntityNames.CopyTo(array, 1);
                return array;
            }
            set
            {
                this.possibleResponses = value;
            }
        }

        public string PossibleResponsesConcatenated
        {
            get
            {
                return this.possibleResponsesConcatenated;
            }
            set
            {
                this.possibleResponsesConcatenated = value;
            }
        }

        public string Question
        {
            get
            {
                return this.question;
            }
            set
            {
                this.question = value;
            }
        }

        public string ShortName
        {
            get
            {
                return this.shortName;
            }
            set
            {
                this.shortName = value;
            }
        }
    }
}

