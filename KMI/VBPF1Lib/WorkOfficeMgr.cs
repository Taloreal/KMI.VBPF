namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkOfficeMgr : WorkOfficeDesk
    {
        public WorkOfficeMgr()
        {
            base.HourlyWage = 41f;
            base.WorkExperienceRequired.Add("IT Supervisor", 1f);
            base.AcademicExperienceRequired.Clear();
            base.AcademicExperienceRequired.Add("Bachelors Degree");
        }

        public override string Name()
        {
            return "Vice President IT";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for entire data entry operations including management of IT supervisor.");
        }
    }
}

