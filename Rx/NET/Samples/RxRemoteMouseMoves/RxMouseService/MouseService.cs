using System;
using System.Drawing;

namespace RxMouseService
{
    public interface IMouseService
    {
        IObservable<Point> GetPoints();
    }
}
