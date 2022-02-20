using System;
using System.Runtime.CompilerServices;

namespace KMI.VBPF1Lib {

    public delegate void Func();
    public delegate TResult Func<TResult>();
    public delegate TResult Func<p1, TResult>(p1 param);
    public delegate TResult Func<p1, p2, TResult>(p1 param1, p2 param2);
    public delegate TResult Func<p1, p2, p3, TResult>(p1 param1, p2 param2, p3 param3);
}

