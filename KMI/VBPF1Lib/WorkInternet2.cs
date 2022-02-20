namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkInternet2 : WorkInvisible
    {
        public WorkInternet2()
        {
            base.HourlyWage = 18f;
            base.WorkExperienceRequired.Add("Data Entry Specialist|Web Designer", 2f);
            base.AcademicExperienceRequired.Clear();
            base.AcademicExperienceRequired.Add("Bachelors Degree");
            base.BonusPotential = 0.25f;
        }

        public override string Name()
        {
            return "Software Engineer";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for a complete software engineering project.");
        }
    }
}

