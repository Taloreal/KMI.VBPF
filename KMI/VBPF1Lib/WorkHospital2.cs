namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkHospital2 : WorkInvisible
    {
        public WorkHospital2()
        {
            base.HourlyWage = 12.5f;
            base.AcademicExperienceRequired.Add("Medical Degree");
        }

        public override string Name()
        {
            return "Medical Resident";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for patient care under supervision of attending physician.");
        }
    }
}

