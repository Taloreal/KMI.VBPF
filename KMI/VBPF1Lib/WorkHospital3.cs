namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkHospital3 : WorkInvisible
    {
        public WorkHospital3()
        {
            base.HourlyWage = 75f;
            base.AcademicExperienceRequired.Add("Medical Degree");
            base.WorkExperienceRequired.Add("Medical Resident", 3f);
        }

        public override string Name()
        {
            return "Medical Doctor";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for all aspects of patient care.");
        }
    }
}

