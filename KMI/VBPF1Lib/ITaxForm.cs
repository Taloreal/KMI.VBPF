namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    public interface ITaxForm
    {
        void Print(Graphics g);
        int Year();
    }
}

