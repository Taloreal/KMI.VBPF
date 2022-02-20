namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkInternet1 : WorkInvisible
    {
        public WorkInternet1()
        {
            base.HourlyWage = 8f;
            base.AcademicExperienceRequired.Clear();
            base.AcademicExperienceRequired.Add("Web Design");
            base.BonusPotential = 0.2f;
        }

        public override string Name()
        {
            return "Web Designer";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for website programming/design.");
        }
    }
}

