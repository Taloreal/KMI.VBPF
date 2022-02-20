namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkPizzaGuy : WorkTravellingSalesman
    {
        public WorkPizzaGuy()
        {
            base.HourlyWage = 8f;
            base.VisitBuildingIndex = 1;
        }

        public override string Description()
        {
            return (base.Description().Replace("Hourly Pay", "Hourly Pay w/ Tips") + Environment.NewLine + "Car: Required, Mileage Reimbursed");
        }

        public override string Name()
        {
            return "Pizza Delivery Person";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for timely delivery of pizzas to customers throughout the city. Maintained own vehicle to perform job functions effectively.");
        }
    }
}

