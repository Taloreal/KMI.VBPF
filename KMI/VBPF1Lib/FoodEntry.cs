using System;
using System.Collections.Generic;

namespace KMI.VBPF1Lib {

    [Serializable]
    public class FoodEntry {

        public int Food { get; private set; }

        public int ShelfLife { get; private set; }

        public bool Perishable { get; private set; }

        public DateTime Bought { get; private set; }


        public static bool FoodExpires = true;
        public static bool FoodConsumes = true;

        public FoodEntry(int amount, DateTime when, int goodFor = 28, bool expires = true) {
            Food = amount;
            Bought = when;
            ShelfLife = goodFor;
            Perishable = expires;
        }

        /// <summary>
        /// Checks if food is expired compared to a specified date.
        /// </summary>
        /// <param name="now">The date to check against.</param>
        /// <returns>Returns true if expired, false if not.</returns>
        public bool Expired(DateTime now) {
            if (!Perishable) { return false; }
            TimeSpan since = now - Bought;
            return FoodExpires && since.Days > ShelfLife;
        }

        /// <summary>
        /// Halves the food in the entry.
        /// </summary>
        public void Expire() {
            if (!FoodExpires || !Perishable) { return; }
            int amount = 1 + Food / 2;
            amount = Math.Min(amount, Food);
            Food -= amount;
        }

        /// <summary>
        /// Consumes 1 food.
        /// </summary>
        /// <returns>Returns true if food remains, false if not.</returns>
        public bool Eat() {
            if (FoodConsumes) { Food -= 1; }
            return Food > 0;
        }

        /// <summary>
        /// Counts a list of FoodEntry's food amounts.
        /// </summary>
        /// <param name="entries">The list to count.</param>
        /// <returns>The amount of food in the list.</returns>
        public static int Count(List<FoodEntry> entries) {
            int val = 0;
            entries.ForEach(f => val += f.Food);
            return val;
        }
    }
}
