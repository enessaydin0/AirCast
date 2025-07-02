using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirCast
{
    public partial class RuzgarPusulasi : Form
    {
        private float angle = 0;
        private System.Windows.Forms.Timer timer;
        public RuzgarPusulasi()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void RuzgarPusulasi_Load(object sender, EventArgs e)
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick; 
            timer.Start(); 
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            angle += 15; 
            if (angle >= 360)
                angle = 0;

            this.Invalidate(); 
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawCompass(e.Graphics);
        }

        private void DrawCompass(Graphics g)
        {
            int width = pictureBox1.ClientSize.Width;
            int height = pictureBox1.ClientSize.Height;

            Point center = new Point(width / 2, height / 2); 
            int radius = Math.Min(width, height) / 3;

            g.DrawEllipse(Pens.Black, center.X - radius, center.Y - radius, 2 * radius, 2 * radius);

            Font font = new Font("Arial", 12, FontStyle.Bold);
            Brush brush = Brushes.Black;

            g.DrawString("N", font, brush, center.X - 10, center.Y - radius - 20);  // Kuzey
            g.DrawString("E", font, brush, center.X + radius + 5, center.Y - 10);  // Doğu
            g.DrawString("S", font, brush, center.X - 10, center.Y + radius + 5);  // Güney
            g.DrawString("W", font, brush, center.X - radius - 20, center.Y - 10);  // Batı

            // Rüzgarın yönünü gösteren ok
            DrawWindArrow(g, center, radius, angle);
        }

        private void DrawWindArrow(Graphics g, Point center, int radius, float angle)
        {
            float arrowAngle = -angle + 90;

            int arrowLength = radius - 10;
            Point arrowEnd = new Point(
                center.X + (int)(arrowLength * Math.Cos(Math.PI * arrowAngle / 180)),
                center.Y - (int)(arrowLength * Math.Sin(Math.PI * arrowAngle / 180))
            );

            Pen arrowPen = new Pen(Color.Red, 3);
            g.DrawLine(arrowPen, center, arrowEnd);

            int arrowHeadSize = 10;
            float headAngle1 = arrowAngle + 150;
            float headAngle2 = arrowAngle - 150;

            Point arrowHead1 = new Point(
                arrowEnd.X + (int)(arrowHeadSize * Math.Cos(Math.PI * headAngle1 / 180)),
                arrowEnd.Y - (int)(arrowHeadSize * Math.Sin(Math.PI * headAngle1 / 180))
            );

            Point arrowHead2 = new Point(
                arrowEnd.X + (int)(arrowHeadSize * Math.Cos(Math.PI * headAngle2 / 180)),
                arrowEnd.Y - (int)(arrowHeadSize * Math.Sin(Math.PI * headAngle2 / 180))
            );

            g.DrawLine(arrowPen, arrowEnd, arrowHead1);
            g.DrawLine(arrowPen, arrowEnd, arrowHead2);
        }
    }
}
