namespace KMI.Sim.Drawables
{
    using KMI.Sim;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    [Serializable]
    public class AlphaDrawable : Drawable
    {
        protected float alpha;

        public AlphaDrawable(Point location, string imageName, int alpha) : base(location, imageName)
        {
            this.alpha = ((float) alpha) / 100f;
        }

        public AlphaDrawable(Point location, string imageName, float alpha) : base(location, imageName)
        {
            this.alpha = alpha;
        }

        public AlphaDrawable(Point location, string imageName, string clickString, int alpha) : base(location, imageName, clickString)
        {
            this.alpha = ((float) alpha) / 100f;
        }

        public AlphaDrawable(Point location, string imageName, string clickString, float alpha) : base(location, imageName, clickString)
        {
            this.alpha = alpha;
        }

        public override void Draw(Graphics g)
        {
            Bitmap image = S.Resources.GetImage(base.imageName);
            float[][] numArray = new float[5][];
            float[] numArray2 = new float[5];
            numArray2[0] = 1f;
            numArray[0] = numArray2;
            numArray2 = new float[5];
            numArray2[1] = 1f;
            numArray[1] = numArray2;
            numArray2 = new float[5];
            numArray2[2] = 1f;
            numArray[2] = numArray2;
            numArray2 = new float[5];
            numArray2[3] = this.alpha;
            numArray[3] = numArray2;
            numArray2 = new float[5];
            numArray2[4] = 1f;
            numArray[4] = numArray2;
            float[][] newColorMatrix = numArray;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            g.DrawImage(image, new Rectangle(this.location.X, this.location.Y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
        }
    }
}

