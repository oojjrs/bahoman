using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public class LineArgs
    {
	    private int width = 1;
	    private Color color = Color.Black;
	    private List<Point> points = new List<Point>();

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

        public LineArgs(int width, Color color)
        {
            this.LineColor = color;
            this.LineWidth = width;
        }

        public void AddPoint(int x, int y)
        {
            this.AddPoint(new Point(x, y));
        }

        public void AddPoint(Point pt)
        {
            this.points.Add(pt);
        }

        public Color LineColor
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public int PointCount
        {
            get { return this.points.Count; }
        }

        public List<Point> Points
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
