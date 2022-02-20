namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkInternet3 : WorkInvisible
    {
        public WorkInternet3()
        {
            base.HourlyWage = 51f;
            base.WorkExperienceRequired.Add("Vice President IT|Software Engineer", 2f);
            base.AcademicExperienceRequired.Clear();
            base.AcademicExperienceRequired.Add("Bachelors Degree");
            base.BonusPotential = 0.3f;
        }

        public override string Name()
        {
            return "VP of Engineering";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for overseeing multiple software projects and engineers.");
        }
    }
}

