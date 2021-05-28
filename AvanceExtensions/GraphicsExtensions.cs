using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Avance
{
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics graphics, Rectangle rectangle, Brush brush, int radius)
        {
            SmoothingMode mode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRectangle(rectangle, radius))
            {
                graphics.FillPath(brush, path);
            }
            graphics.SmoothingMode = mode;
        }

        public static void DrawRoundedRectangle(this Graphics graphics, Rectangle rectangle, Pen pen, int radius)
        {
            SmoothingMode mode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRectangle(rectangle, radius))
            {
                graphics.DrawPath(pen, path);
            }
            graphics.SmoothingMode = mode;
        }

        public static GraphicsPath RoundedRectangle(Rectangle r, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddLine(r.Left + d, r.Top, r.Right - d, r.Top);
            path.AddArc(Rectangle.FromLTRB(r.Right - d, r.Top, r.Right, r.Top + d), -90, 90);
            path.AddLine(r.Right, r.Top + d, r.Right, r.Bottom - d);
            path.AddArc(Rectangle.FromLTRB(r.Right - d, r.Bottom - d, r.Right, r.Bottom), 0, 90);
            path.AddLine(r.Right - d, r.Bottom, r.Left + d, r.Bottom);
            path.AddArc(Rectangle.FromLTRB(r.Left, r.Bottom - d, r.Left + d, r.Bottom), 90, 90);
            path.AddLine(r.Left, r.Bottom - d, r.Left, r.Top + d);
            path.AddArc(Rectangle.FromLTRB(r.Left, r.Top, r.Left + d, r.Top + d), 180, 90);
            path.CloseFigure();

            return path;
        }
    }
}
