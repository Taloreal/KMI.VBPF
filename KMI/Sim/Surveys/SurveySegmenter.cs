namespace KMI.Sim.Surveys
{
    using System;

    [Serializable]
    public class SurveySegmenter
    {
        protected int entityIndex;
        protected bool[] includesAnswer;
        protected SurveyQuestion question;
        protected int questionIndex;

        public SurveySegmenter(int questionIndex, SurveyQuestion question, bool[] includesAnswer)
        {
            this.questionIndex = questionIndex;
            this.question = question;
            this.includesAnswer = includesAnswer;
        }

        public SurveySegmenter(int questionIndex, SurveyQuestion question, bool[] includesAnswer, int entityIndex)
        {
            this.questionIndex = questionIndex;
            this.question = question;
            this.includesAnswer = includesAnswer;
            this.entityIndex = entityIndex;
        }

        public bool Excludes(SurveyResponse r)
        {
            int num;
            int[] numArray = (int[]) r.Answers[this.questionIndex];
            if (this.question.MultiEntity)
            {
                num = numArray[this.entityIndex];
            }
            else
            {
                num = numArray[0];
            }
            return !this.includesAnswer[num];
        }

        public bool AllChecked
        {
            get
            {
                bool[] includesAnswer = this.includesAnswer;
                for (int i = 0; i < includesAnswer.Length; i++)
                {
                    if (!includesAnswer[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public int EntityIndex
        {
            get
            {
                return this.entityIndex;
            }
        }

        public bool[] IncludesAnswer
        {
            get
            {
                return this.includesAnswer;
            }
            set
            {
                this.includesAnswer = value;
            }
        }

        public SurveyQuestion Question
        {
            get
            {
                return this.question;
            }
        }
    }
}

