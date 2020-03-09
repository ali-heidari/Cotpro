using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cotpro.UI.Web.Map
{
    public class Line
    {
        public Line()
        {
            this.Color = "#000000";
            this.Weight = 2;
            this.Opacity = 1f;
            this.Geodisc = true;
        }
        public string Color { get; set; }
        public byte Weight { get; set; }
        public bool Geodisc { get; set; }
        public float Opacity { get; set; }

        private List<Point> _points = new List<Point>();
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public List<Point> Points
        {
            get { return _points; }
        }


        public bool IsInTheArea(Point ObjectCoordinate, int WidthOfSurface, int HeightOfSurface)
        {
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(new System.Drawing.Bitmap(WidthOfSurface, HeightOfSurface));
            List<System.Drawing.PointF> points = new List<System.Drawing.PointF>();
            foreach (Point p in this.Points)
                points.Add(new System.Drawing.PointF(Convert.ToSingle(p.Longitude), Convert.ToSingle(p.Latitude)));

            System.Drawing.Drawing2D.GraphicsPath gpath = new System.Drawing.Drawing2D.GraphicsPath();
            gpath.AddPolygon(points.ToArray());
            System.Drawing.Region reg = new System.Drawing.Region(gpath);

            g.SetClip(reg, System.Drawing.Drawing2D.CombineMode.Replace);
            g.FillRegion(new System.Drawing.SolidBrush(System.Drawing.Color.Red), reg);

            return g.IsVisible(new System.Drawing.PointF(Convert.ToSingle(ObjectCoordinate.Longitude), Convert.ToSingle(ObjectCoordinate.Latitude)));
        }
        private bool IsInTheLine(Point ObjectCoordinate)
        {
            Point p1 = null, p2 = null;
            double Y;
            bool IsIn = false;
            for (int i = 0; i < this.Points.Count - 1; i++)
            {
                p1 = this.Points[i];
                p2 = this.Points[i + 1];

                Y = ((p2.Latitude - p1.Latitude) / (p2.Longitude - p1.Longitude) * (ObjectCoordinate.Longitude - p1.Longitude)) - p1.Latitude;

                if ((Y - ObjectCoordinate.Latitude) <= 0.00004 && (Y - ObjectCoordinate.Latitude) >= -0.00004)
                {
                    IsIn = true;
                    break;
                }
            }
            return IsIn;
        }

    }
}
