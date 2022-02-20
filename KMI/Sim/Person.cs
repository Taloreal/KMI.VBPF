namespace KMI.Sim
{
    using KMI.Utility;
    using System;

    [Serializable]
    public class Person : MovableActiveObject
    {
        protected string firstName;
        protected int footForward;
        protected GenderType gender;
        protected string lastName;
        protected RaceType race;

        public Person()
        {
            this.footForward = 0;
            Random rng = Simulator.Instance.SimState.Random;
            this.gender = (GenderType) rng.Next(2);
            this.race = (RaceType) rng.Next(3);
            if (this.gender == GenderType.Male)
            {
                this.firstName = Utilities.GetRandomMaleFirstName(rng);
            }
            else
            {
                this.firstName = Utilities.GetRandomFemaleFirstName(rng);
            }
            this.lastName = Utilities.GetRandomLastName(rng);
        }

        public Person(GenderType gender, RaceType race, string firstName, string lastName)
        {
            this.footForward = 0;
            this.gender = gender;
            this.race = race;
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public override bool Move()
        {
            this.footForward = 1 - this.footForward;
            return base.Move();
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
        }

        public string FullName
        {
            get
            {
                return (this.firstName + " " + this.lastName);
            }
        }

        public GenderType Gender
        {
            get
            {
                return this.gender;
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
        }

        public RaceType Race
        {
            get
            {
                return this.race;
            }
        }

        public enum GenderType
        {
            Male,
            Female
        }

        public enum RaceType
        {
            Caucasian,
            African,
            Hispanic
        }
    }
}

