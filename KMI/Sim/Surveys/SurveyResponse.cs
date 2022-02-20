namespace KMI.Sim.Surveys
{
    using System;
    using System.Collections;

    [Serializable]
    public class SurveyResponse
    {
        protected ArrayList answers = new ArrayList();
        protected long respondantID;

        public SurveyResponse(long respondantID)
        {
            this.respondantID = respondantID;
        }

        public void AddAnswer(int[] answer)
        {
            this.answers.Add(answer);
        }

        public ArrayList Answers
        {
            get
            {
                return this.answers;
            }
        }

        public long RespondantID
        {
            get
            {
                return this.respondantID;
            }
        }
    }
}

