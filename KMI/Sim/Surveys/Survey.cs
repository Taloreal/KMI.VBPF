namespace KMI.Sim.Surveys
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Survey
    {
        public static float BaseCost = 500f;
        public static bool BillForSurveys = true;
        protected DateTime date;
        protected long entityID;
        protected string[] entityNames;
        public static float ExecutionCostPerQuestionPerPerson = 0.25f;
        public static int MaxSurveys = 5;
        public static SurveyQuestion[] PossibleSurveyQuestions;
        public static string QualifyingQuestionShortName;
        public static float RecruitingCostPerPerson = 2f;
        protected ArrayList responses;
        protected ArrayList segmenters;
        public static bool ShowAllSurveyedWhenSegmented = false;
        public static bool ShowBuyMailingList = false;
        public static string SurveyableObjectName = "Respondents";
        protected ArrayList surveyQuestions;

        public Survey()
        {
            this.responses = new ArrayList();
            this.segmenters = new ArrayList();
        }

        public Survey(long entityID, DateTime date, string[] entityNames, ArrayList surveyQuestions)
        {
            this.responses = new ArrayList();
            this.segmenters = new ArrayList();
            this.entityID = entityID;
            this.date = date;
            this.entityNames = entityNames;
            if (QualifyingQuestionShortName != null)
            {
                bool flag2 = false;
                foreach (SurveyQuestion question in surveyQuestions)
                {
                    if (question.ShortName == QualifyingQuestionShortName)
                    {
                        flag2 = true;
                    }
                }
                if (!flag2)
                {
                    surveyQuestions.Add(GetSurveyQuestionByName(QualifyingQuestionShortName));
                }
            }
            foreach (SurveyQuestion question2 in surveyQuestions)
            {
                question2.Parent = this;
            }
            this.surveyQuestions = surveyQuestions;
        }

        public void AddUpdateSegmenter(SurveyQuestion question, bool[] includesAnswer)
        {
            SurveySegmenter segmenter = this.GetSegmenter(question);
            if (segmenter != null)
            {
                segmenter.IncludesAnswer = includesAnswer;
            }
            else
            {
                segmenter = new SurveySegmenter(this.QuestionIndex(question), question, includesAnswer);
                this.segmenters.Add(segmenter);
            }
            if (segmenter.AllChecked)
            {
                this.segmenters.Remove(segmenter);
            }
        }

        public void AddUpdateSegmenter(SurveyQuestion question, bool[] includesAnswer, int entityIndex)
        {
            SurveySegmenter segmenter = this.GetSegmenter(question, entityIndex);
            if (segmenter != null)
            {
                segmenter.IncludesAnswer = includesAnswer;
            }
            else
            {
                segmenter = new SurveySegmenter(this.QuestionIndex(question), question, includesAnswer, entityIndex);
                this.segmenters.Add(segmenter);
            }
            if (segmenter.AllChecked)
            {
                this.segmenters.Remove(segmenter);
            }
        }

        public virtual void BuyMailingListHook()
        {
            throw new NotImplementedException("Survey has ShowBuyMailingList on without implementing BuyMailingListHook.");
        }

        public void ClearSegmenters()
        {
            this.segmenters = new ArrayList();
        }

        public long[] DrawSegments(Graphics g, Rectangle box)
        {
            ArrayList segs = (ArrayList) this.segmenters.Clone();
            ArrayList resps = new ArrayList();
            foreach (SurveyResponse response in this.responses)
            {
                resps.Add(response);
            }
            return this.DrawSubSegments(segs, resps, false, g, box);
        }

        private long[] DrawSubSegments(ArrayList segs, ArrayList resps, bool vertical, Graphics g, Rectangle box)
        {
            float num;
            int num2;
            Rectangle rect = box;
            Rectangle layoutRectangle = box;
            string s = "";
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Center
            };
            if (segs.Count > 0)
            {
                SurveySegmenter segmenter = (SurveySegmenter) segs[0];
                segs.Remove(segmenter);
                s = segmenter.Question.Question;
                if (segmenter.Question.MultiEntity)
                {
                    s = s + " - " + this.entityNames[segmenter.EntityIndex];
                }
                int count = resps.Count;
                for (num2 = resps.Count - 1; num2 >= 0; num2--)
                {
                    if (segmenter.Excludes((SurveyResponse) resps[num2]))
                    {
                        resps.RemoveAt(num2);
                    }
                }
                num = 1f - (((float) resps.Count) / ((float) count));
            }
            else
            {
                num = 0f;
            }
            if (vertical)
            {
                rect.Height = (int) (rect.Height * num);
                layoutRectangle.Y = rect.Y + rect.Height;
                layoutRectangle.Height -= rect.Height;
            }
            else
            {
                rect.Width = (int) (rect.Width * num);
                layoutRectangle.X = rect.X + rect.Width;
                layoutRectangle.Width -= rect.Width;
            }
            if ((rect.Height > 0) && (rect.Width > 0))
            {
                g.FillRectangle(new SolidBrush(Color.LightGray), rect);
                g.DrawRectangle(new Pen(Color.Black, 1f), rect);
                g.DrawString(s, new Font("Arial", 8f), new SolidBrush(Color.Black), rect, format);
            }
            if ((layoutRectangle.Height > 0) && (layoutRectangle.Width > 0))
            {
                g.DrawString(resps.Count.ToString() + " left in segment", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.White), layoutRectangle, format);
            }
            if (segs.Count > 0)
            {
                return this.DrawSubSegments(segs, resps, !vertical, g, layoutRectangle);
            }
            long[] numArray = new long[resps.Count];
            for (num2 = 0; num2 < resps.Count; num2++)
            {
                numArray[num2] = ((SurveyResponse) resps[num2]).RespondantID;
            }
            return numArray;
        }

        public virtual void Execute(int numToSurvey)
        {
            throw new Exception("Survey.Execute not overridden.");
        }

        public SurveySegmenter GetSegmenter(SurveyQuestion question)
        {
            foreach (SurveySegmenter segmenter in this.segmenters)
            {
                if (segmenter.Question == question)
                {
                    return segmenter;
                }
            }
            return null;
        }

        public SurveySegmenter GetSegmenter(SurveyQuestion question, int entityIndex)
        {
            foreach (SurveySegmenter segmenter in this.segmenters)
            {
                if ((segmenter.Question == question) && (segmenter.EntityIndex == entityIndex))
                {
                    return segmenter;
                }
            }
            return null;
        }

        public static SurveyQuestion GetSurveyQuestionByName(string shortName)
        {
            foreach (SurveyQuestion question in PossibleSurveyQuestions)
            {
                if (question.ShortName == shortName)
                {
                    return question;
                }
            }
            return null;
        }

        public bool InAllSegments(SurveyResponse r)
        {
            foreach (SurveySegmenter segmenter in this.segmenters)
            {
                if (segmenter.Excludes(r))
                {
                    return false;
                }
            }
            return true;
        }

        public static void LoadQuestionsFromTable(Type type, string resource)
        {
            PossibleSurveyQuestions = (SurveyQuestion[]) TableReader.Read(type.Assembly, typeof(SurveyQuestion), resource);
            foreach (SurveyQuestion question in PossibleSurveyQuestions)
            {
                char[] separator = new char[] { '|' };
                question.PossibleResponses = question.PossibleResponsesConcatenated.Split(separator);
                question.PossibleResponsesConcatenated = null;
            }
        }

        private int QuestionIndex(SurveyQuestion question)
        {
            for (int i = 0; i < this.surveyQuestions.Count; i++)
            {
                if (this.surveyQuestions[i] == question)
                {
                    return i;
                }
            }
            return -1;
        }

        public int SurveyIndexOfEntity(string name)
        {
            int num = -1;
            for (int i = 0; i < this.EntityNames.Length; i++)
            {
                if (this.EntityNames[i] == name)
                {
                    num = i;
                }
            }
            return num;
        }

        public DateTime Date
        {
            get
            {
                return this.date;
            }
        }

        public long EntityID
        {
            get
            {
                return this.entityID;
            }
        }

        public string[] EntityNames
        {
            get
            {
                return this.entityNames;
            }
        }

        public ArrayList Responses
        {
            get
            {
                return this.responses;
            }
        }

        public bool Segmented
        {
            get
            {
                return (this.segmenters.Count > 0);
            }
        }

        public ArrayList SurveyQuestions
        {
            get
            {
                return this.surveyQuestions;
            }
        }
    }
}

