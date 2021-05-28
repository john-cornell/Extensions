using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class PointFExtensions
    {
        public static PointF Add(this PointF me, PointF offset)
        {
            return new PointF(me.X + offset.X, me.Y + offset.Y);
        }

        public static PointF Subtract(this PointF me, PointF offset)
        {
            return new PointF(me.X - offset.X, me.Y - offset.Y);
        }

        public static PointF Add(this PointF me, SizeF offset)
        {
            return new PointF(me.X + offset.Width, me.Y + offset.Height);
        }

        public static PointF Subtract(this PointF me, SizeF offset)
        {
            return new PointF(me.X - offset.Width, me.Y - offset.Height);
        }
    }
}
