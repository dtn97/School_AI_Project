using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1512387_1_2
{
    public partial class Form1 : Form
    {
        Graph g;
        Graphics gp;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1000, 800);
            this.AutoScroll = true;
            this.DoubleBuffered = true;
            gp = this.CreateGraphics();
            gp.Clear(Color.White);
            gp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gp.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);
            g = new Graph();
            g.SetInput("input.txt");
            float dx = (float)g.size.Width / 1000;
            float dy = (float)g.size.Height / 800;
            gp.ScaleTransform((float)1 / dx, (float)1 / dy);
            this.AutoScrollMinSize = g.size;
            g.Astar();
            g.SaveOutput();
            Invalidate();
            timer1.Interval = 1;
            timer1.Start();
        }

        private void Moving()
        {
            g.DrawMap(gp);
            g.Astar();
            g.DrawPath(gp);
            g.MovePolygon();
            System.Threading.Thread.Sleep(500);  
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (!g.finish())
            {
                this.Moving();
            }
            else
            {
                g.DrawMap(gp);
                g.SaveOutputMoving();
                System.Threading.Thread.Sleep(3000);
                this.Close();
            }
            Invalidate();
        }
    }
}
