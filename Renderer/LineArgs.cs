using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;

namespace Renderer
{
    public class LineArgs
    {
	    private int width = 1;
	    private MyColor color = new MyColor();
        private List<System.Drawing.Point> points = new List<System.Drawing.Point>();

        public override string ToString()
        {
            var s = new StringBuilder().AppendFormat("W{0}{1}", this.LineWidth, this.LineColor);

            foreach (var pt in this.Points)
                s.Append(pt.ToString());
            return s.ToString();
        }

        public LineArgs()
        {
        }

        public LineArgs(int width)
        {
            this.LineWidth = width;
        }

        public LineArgs(int width, MyColor color)
        {
            this.LineColor = color;
            this.LineWidth = width;
        }

        public void AddPoint(int x, int y)
        {
            this.AddPoint(new System.Drawing.Point(x, y));
        }

        public void AddPoint(System.Drawing.Point pt)
        {
            this.points.Add(pt);
        }

        public MyColor LineColor
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public int PointCount
        {
            get { return this.points.Count; }
        }

        public List<System.Drawing.Point> Points
        {
            get { return this.points; }
        }

        public int LineWidth
        {
            get { return this.width; }
            set { this.width = value; }
        }
    }
}
