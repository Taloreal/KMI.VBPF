namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using System;
    using System.Collections;
    using System.Drawing;

    public class InsideView : View
    {
        public InsideView(string name, Bitmap background) : base(name, background)
        {
        }

        public override Drawable[] BuildDrawables(long entityID, params object[] args)
        {
            ArrayList list = new ArrayList();
            AppBuilding building = (AppBuilding) A.State.City[(int) args[0], (int) args[1], (int) args[2]];
            string backgroundImage = building.GetBackgroundImage();
            base.background = A.Resources.GetImage(backgroundImage);
            list.AddRange(building.GetInsideDrawables());
            list.Add(new CityNavDrawable(new Point(10, 50), "CityNavIcon", " "));
            return (Drawable[]) list.ToArray(typeof(Drawable));
        }

        public void SetBackground(int index)
        {
            switch (index)
            {
                case 1:
                    base.background = A.Resources.GetImage("HomeBack");
                    break;

                case 2:
                    base.background = A.Resources.GetImage("WorkBack");
                    break;

                case 3:
                    base.background = A.Resources.GetImage("OfficeBack");
                    break;

                case 5:
                    base.background = A.Resources.GetImage("ClassBack");
                    break;
            }
        }
    }
}

