namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkOfficeSup : WorkOfficeDesk
    {
        public WorkOfficeSup()
        {
            base.HourlyWage = 21f;
            base.WorkExperienceRequired.Add("Data Entry Specialist", 1f);
            base.AcademicExperienceRequired.Clear();
            base.AcademicExperienceRequired.Add("IT Management");
        }

        public override string Name()
        {
            return "IT Supervisor";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for direct supervision of four data entry specialists.");
        }
    }
}

