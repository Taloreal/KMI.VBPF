namespace KMI.VBPF1Lib
{
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    [Serializable]
    public class PaletteDrawable : FlexDrawable
    {
        private int paletteIndex;

        public PaletteDrawable(PointF location, string imageName, string clickString, int palIndex) : base(location, imageName, clickString)
        {
            this.paletteIndex = palIndex;
        }

        public override void Draw(Graphics g)
        {
            Bitmap image = A.Resources.GetImage(base.imageName);
            image.Palette = A.Resources.GetImage("Palette" + this.paletteIndex).Palette;
            switch (base.verticalAlignment)
            {
                case FlexDrawable.VerticalAlignments.Top:
                    base.offsetY = 0;
                    break;

                case FlexDrawable.VerticalAlignments.Middle:
                    base.offsetY = -image.Height / 2;
                    break;

                case FlexDrawable.VerticalAlignments.Bottom:
                    base.offsetY = -image.Height;
                    break;
            }
            switch (base.horizontalAlignment)
            {
                case FlexDrawable.HorizontalAlignments.Left:
                    base.offsetX = 0;
                    break;

                case FlexDrawable.HorizontalAlignments.Center:
                    base.offsetX = -image.Width / 2;
                    break;

                case FlexDrawable.HorizontalAlignments.Right:
                    base.offsetX = -image.Width;
                    break;
            }
            if (base.flip)
            {
                g.DrawImage(image, (this.location.X + base.offsetX) + image.Width, this.location.Y + base.offsetY, -image.Width, image.Height);
            }
            else
            {
                g.DrawImage(image, this.location.X + base.offsetX, this.location.Y + base.offsetY, image.Width, image.Height);
            }
        }

        public static void Ramp(ColorPalette pal, int start, int end, Color light, Color dark)
        {
            for (int i = start; i <= end; i++)
            {
                float num2 = ((float) (i - start)) / ((float) (end - start));
                int red = (int) (((dark.R - light.R) * num2) + light.R);
                int green = (int) (((dark.G - light.G) * num2) + light.G);
                int blue = (int) (((dark.B - light.B) * num2) + light.B);
                pal.Entries[i] = Color.FromArgb(red, green, blue);
            }
        }

        public static void Ramp(ColorPalette pal, int start, int end, Color light, float darkPct)
        {
            Color dark = Color.FromArgb((int) (light.R * darkPct), (int) ((light.G / 3) * darkPct), (int) ((light.B / 3) * darkPct));
            Ramp(pal, start, end, light, dark);
        }
    }
}

