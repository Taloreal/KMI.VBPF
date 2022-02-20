namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class WorkTask : Task
    {
        public ArrayList AcademicExperienceRequired = new ArrayList();
        public float AdditionalWitholding = 0f;
        public int Allowances = 1;
        public float BonusPotential = 0f;
        public int CreditScoreRequired = 0;
        public BankAccount DirectDepositAccount = null;
        public bool ExemptFromWitholding = false;
        public InsurancePolicy HealthInsurance;
        public float HourlyWage = 7.5f;
        public float HoursThisWeek;
        public ArrayList PayStubs = new ArrayList();
        public float[] R401KAllocations = new float[A.State.MutualFunds.Count];
        public float R401KMatch = -1f;
        public float R401KPercentWitheld;
        public Hashtable WorkExperienceRequired = new Hashtable();

        public override string CategoryName()
        {
            return A.Resources.GetString("(" + this.HourlyWage.ToString() + "/hr) Work\r\n" + this.Name());
        }

        public override string Description()
        {
            DateTime time = new DateTime(0x7d0, 1, 1);
            string s = "";
            foreach (string str6 in this.WorkExperienceRequired.Keys)
            {
                float num = (float) this.WorkExperienceRequired[str6];
                object[] objArray1 = new object[] { num, str6 };
                s = s + A.Resources.GetString("{0} year(s) as a {1}, ", objArray1);
            }
            if (s == "")
            {
                s = "None";
            }
            else
            {
                s = Utilities.FormatCommaSeries(s);
            }
            s = s.Replace("|", A.Resources.GetString(" or "));
            string str2 = "";
            foreach (string str7 in this.AcademicExperienceRequired)
            {
                object[] objArray2 = new object[] { str7 };
                str2 = str2 + A.Resources.GetString("{0}, ", objArray2);
            }
            if (str2 == "")
            {
                str2 = "None";
            }
            else
            {
                str2 = Utilities.FormatCommaSeries(str2);
            }
            str2 = str2.Replace("|", A.Resources.GetString(" or "));
            string str3 = "";
            if (this.HealthInsurance != null)
            {
                str3 = str3 + A.Resources.GetString("Healthcare Insurance");
            }
            if (this.R401KMatch > -1f)
            {
                if (str3.Length > 0)
                {
                    str3 = str3 + "; ";
                }
                object[] objArray3 = new object[] { Utilities.FP(this.R401KMatch) };
                str3 = str3 + A.Resources.GetString("401K Plan with {0} company match", objArray3);
            }
            if (str3 == "")
            {
                str3 = A.Resources.GetString("None");
            }
            string str4 = A.Resources.GetString("Mon. - Fri.");
            if (base.Weekend)
            {
                str4 = A.Resources.GetString("Sat. & Sun.");
            }
            object[] args = new object[] { this.Name().ToUpper(), Task.ToTimeString(base.StartPeriod), Task.ToTimeString(base.EndPeriod), str4, Utilities.FC(this.HourlyWage, 2, A.Instance.CurrencyConversion), s, str2, str3 };
            string str5 = A.Resources.GetString("{0}|Hours: {1} to {2}|Days: {3}|Hourly Pay: {4}|Experience Req'd: {5}|Courses Req'd: {6}|Benefits: {7}", args);
            if (this.BonusPotential > 0f)
            {
                object[] objArray5 = new object[] { Utilities.FP(this.BonusPotential) };
                str5 = str5.Replace("|Experience", A.Resources.GetString("|Qtly Bonus: up to {0}|Experience", objArray5));
            }
            return str5.Replace("|", Environment.NewLine);
        }

        public virtual void EvaluateApplicant(AppEntity e, Offering o, JobApplication jobApp)
        {
            bool flag;
            TimeSpan span;
            TimeSpan span2;
            object obj2 = e.FiredFrom[o.Building.ID + this.Name()];
            if ((obj2 != null) && ((span2 = span = (TimeSpan) (A.State.Now - ((DateTime) obj2))).Days < 180))
            {
                object[] args = new object[] { 180 };
                throw new SimApplicationException(A.Resources.GetString("You were recently fired from that job. You must wait at least {0} days before you can be rehired.", args));
            }
            obj2 = e.Quit[o.Building.ID + this.Name()];
            if ((obj2 != null) && ((span2 = span = (TimeSpan) (A.State.Now - ((DateTime) obj2))).Days < 90))
            {
                object[] objArray2 = new object[] { 90 };
                throw new SimApplicationException(A.Resources.GetString("You recently quit that job. You must wait at least {0} days before you can be rehired.", objArray2));
            }
            if (jobApp.Name.ToUpper() != e.Name.ToUpper())
            {
                throw new SimApplicationException(A.Resources.GetString("You got your name wrong on the application. Your application has been rejected."));
            }
            if (this is WorkTravellingSalesman)
            {
                if ((e.Car == null) && jobApp.Car)
                {
                    throw new SimApplicationException(A.Resources.GetString("A check of motor vehicle registrations shows you don't have a car. Your application has been rejected for lying."));
                }
                if (!jobApp.Car)
                {
                    throw new SimApplicationException(A.Resources.GetString("This job requires a car and your application indicates you don't have one. Your application has been rejected."));
                }
            }
            foreach (string str in jobApp.ReportedClassNames)
            {
                flag = false;
                foreach (AttendClass class2 in e.AcademicTaskHistory.Values)
                {
                    if (class2.Course.Name == str)
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    object[] objArray3 = new object[] { str };
                    throw new SimApplicationException(A.Resources.GetString("You reported completing the class: {0}. But reference checking revealed you did not. Your application has been rejected for lying.", objArray3));
                }
            }
            if (jobApp.ReportedClassNames.Contains("Bachelors Degree"))
            {
                jobApp.ReportedClassNames.Add("Associates Degree");
            }
            int num = 0;
            foreach (string str2 in jobApp.ReportedJobNamesAndMonths.Keys)
            {
                int num2 = (int) jobApp.ReportedJobNamesAndMonths[str2];
                int num3 = (int) Math.Floor((double) (e.YearsExperience(str2) * 12f));
                num += num2;
                if (num2 > num3)
                {
                    object[] objArray4 = new object[] { num2, str2, num3.ToString("N0") };
                    throw new SimApplicationException(A.Resources.GetString("You reported {0} months of experience as a {1}. But reference checking revealed you have {2} months experience at that job. Your application has been rejected for lying.", objArray4));
                }
            }
            jobApp.ReportedJobNamesAndMonths.Add("worker of any kind", num);
            foreach (string str3 in this.AcademicExperienceRequired)
            {
                flag = false;
                char[] separator = new char[] { '|' };
                string[] strArray = str3.Split(separator);
                foreach (string str4 in strArray)
                {
                    foreach (string str5 in jobApp.ReportedClassNames)
                    {
                        if (str5.IndexOf(str4) > -1)
                        {
                            flag = true;
                        }
                    }
                }
                if (!flag)
                {
                    object[] objArray5 = new object[] { A.Resources.GetString(str3.Replace("|", " or ")) };
                    throw new SimApplicationException(A.Resources.GetString("You did not get the job, because your application showed that you did not have enough education. This job requires the course: {0}.", objArray5));
                }
            }
            foreach (string str6 in this.WorkExperienceRequired.Keys)
            {
                float num5 = (float) this.WorkExperienceRequired[str6];
                int num6 = 0;
                char[] chArray2 = new char[] { '|' };
                string[] strArray3 = str6.Split(chArray2);
                foreach (string str7 in strArray3)
                {
                    if (jobApp.ReportedJobNamesAndMonths.ContainsKey(str7))
                    {
                        num6 += (int) jobApp.ReportedJobNamesAndMonths[str7];
                    }
                }
                if ((num5 * 12f) > num6)
                {
                    object[] objArray = new object[] { (num5 * 12f).ToString("N0"), str6.Replace("|", " or "), num6 };
                    throw new SimApplicationException(A.Resources.GetString("You did not get the job, because you did not have enough experience. This job requires {0} months of experience as a {1}, and your application lists only {2} months of experience.", objArray));
                }
            }
            if (e.CreditScore() < this.CreditScoreRequired)
            {
                object[] objArray7 = new object[] { e.CreditScore() };
                throw new SimApplicationException(A.Resources.GetString("You did not get the job, because your credit score was below {0}.", objArray7));
            }
        }

        public override bool Process(AppEntity entity) {
            if (entity.Person.Task is WorkTask) {
                WorkTask task = (WorkTask)entity.Person.Task;
                task.HoursThisWeek += 0.5f;
            } 
            else { return base.Process(entity); }
            return true;
        }

        public override Color GetColor()
        {
            return Color.Green;
        }

        public float GetValueYTD(string lineItem, DateTime date)
        {
            float num = 0f;
            foreach (PayStub stub in this.PayStubsYTD(date))
            {
                num += stub.GetValue(lineItem);
            }
            return num;
        }

        public virtual string Name()
        {
            return "Work Task";
        }

        public ArrayList PayStubsYTD(DateTime date)
        {
            ArrayList list = new ArrayList();
            foreach (PayStub stub in this.PayStubs)
            {
                if ((stub.WeekEnding.Year == date.Year) && (stub.WeekEnding <= date))
                {
                    list.Add(stub);
                }
            }
            return list;
        }

        public virtual string ResumeDescription()
        {
            return "Description of task";
        }
    }
}

