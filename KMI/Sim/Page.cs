namespace KMI.Sim
{
    using System;
    using System.Drawing;

    public class Page
    {
        private int anchorX = 0;
        private int anchorY = 0;
        private System.Drawing.Bitmap bitmap = null;
        private int cols = 1;
        private int height = 1;
        private int rows = 1;
        private int width = 1;

        public Page(System.Drawing.Bitmap bitmap, int cols, int rows, int anchorX, int anchorY)
        {
            this.bitmap = bitmap;
            this.rows = rows;
            this.cols = cols;
            this.width = bitmap.Width;
            this.height = bitmap.Height;
            this.anchorX = anchorX;
            this.anchorY = anchorY;
        }

        public int AnchorX
        {
            get
            {
                return this.anchorX;
            }
        }

        public int AnchorY
        {
            get
            {
                return this.anchorY;
            }
        }

        public System.Drawing.Bitmap Bitmap
        {
            get
            {
                return this.bitmap;
            }
        }

        public int CellHeight
        {
            get
            {
                return (this.height / this.rows);
            }
        }

        public int CellWidth
        {
            get
            {
                return (this.width / this.cols);
            }
        }

        public int Cols
        {
            get
            {
                return this.cols;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public int Rows
        {
            get
            {
                return this.rows;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
        }
    }
}

