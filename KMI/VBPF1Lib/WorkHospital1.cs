namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkHospital1 : WorkInvisible
    {
        public WorkHospital1()
        {
            base.HourlyWage = 25f;
            base.AcademicExperienceRequired.Add("Nursing Degree");
        }

        public override string Name()
        {
            return "Nurse";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for assisting physicians in patient care.");
        }
    }
}

