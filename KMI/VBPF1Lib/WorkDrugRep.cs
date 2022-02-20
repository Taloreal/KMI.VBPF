namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkDrugRep : WorkTravellingSalesman
    {
        public WorkDrugRep()
        {
            base.HourlyWage = 30f;
            base.VisitBuildingIndex = 1;
            base.WorkExperienceRequired.Add("worker of any kind", 2f);
            base.AcademicExperienceRequired.Add("Associates Degree");
        }

        public override string Description()
        {
            return (base.Description().Replace("Hourly Pay: $30.00", "Commissions: $300-$1300/wk") + Environment.NewLine + "Car: Required, Mileage Reimbursed");
        }

        public override string Name()
        {
            return "Pharmaceutical Salesperson";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for presentation of new drugs to prescribing physicians.");
        }
    }
}

