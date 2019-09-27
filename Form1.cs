using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawMe
{
    public partial class Form1 : Form
    {
      
        //bool Choose = false;
        bool Draw = false;
        int x, y, lx, ly = 0;

        Item currItem = Item.Pen; //the selected item

        public Pen p = new Pen(Color.Black, 1);
        public Pen eraser = new Pen(Color.White, 10);

        Brush mainBrush = new SolidBrush(Color.Black);

        public Point startPoint = new Point(0, 0);
        public Point endPoint = new Point(0, 0);

        


        public Form1()
        {
            InitializeComponent();

            //g = panel1.CreateGraphics();
            //to get a more crisp picture
            p.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
            eraser.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
        }

        public enum Item
        { 
        Pen, Eraser, Line, Rectangle, Ellipse, Text, Brush, ColorPicker, Bucket
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.Write("aa");
            startPoint = e.Location;



            if (e.Button != System.Windows.Forms.MouseButtons.Middle)
                Draw = true;
            x = e.X;
            y = e.Y;

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            endPoint = e.Location;
            Draw = false;

            lx = e.X;
            ly = e.Y;
            //novo

            if (currItem == Item.Line)
            {
                
                Graphics g = panel1.CreateGraphics();
                g.DrawLine(new Pen(p.Color), startPoint, endPoint);
                g.Dispose();
            }
          
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                endPoint = e.Location;
                //g.DrawLine(p, startPoint, endPoint);
                //startPoint = endPoint;
               
            }
           /* else if (e.Button == MouseButtons.Right)
            {
                endPoint = e.Location;
                g.DrawLine(e, startPoint, endPoint);
                startPoint = endPoint;
                
            }*/
            
            

            if (Draw)
            {
                Graphics g = panel1.CreateGraphics();

                if (currItem == Item.Pen && e.Button == MouseButtons.Left)
                {
                   
                    endPoint = e.Location;
                    g.DrawLine(p, startPoint, endPoint);
                    startPoint = endPoint;
                }
                else if (currItem == Item.Eraser && e.Button == MouseButtons.Right)
                {
                    endPoint = e.Location;
                    g.DrawLine(eraser, startPoint, endPoint);
                    startPoint = endPoint;

                }
                switch (currItem)
                {
                    case Item.Rectangle:
                        g.FillRectangle(new SolidBrush(p.Color), x, y, e.X - x, e.Y - y);
                        break;
                    case Item.Ellipse: 
                        g.FillEllipse(new SolidBrush(p.Color), x, y, e.X - x, e.X-x);
                        break;
                    case Item.Pen:
                        break;
                    case Item.Brush:
                        g.FillEllipse(new SolidBrush(p.Color), e.X - x + x, e.Y - y + y, Convert.ToInt32(toolStripTextBox1.Text), Convert.ToInt32(toolStripTextBox1.Text));
                        break;
                    case Item.Bucket:
                       /* Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
                        Graphics b = Graphics.FromImage(bmp);
                        Rectangle brect = panel1.RectangleToScreen(panel1.ClientRectangle);
                        g.CopyFromScreen(brect.Location, Point.Empty, panel1.Size);
                        g.Dispose();
                        FloodFill(new Point(endPoint.X, endPoint.Y), bmp.GetPixel(endPoint.X, endPoint.Y), p.Color, trackBar2.Value);*/
                        break;
                }
                g.Dispose();
            }
        }
        //upon clicking the color button:
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                p.Color = cd.Color;
        }

        private void toolStripButton7_Click(object sender, EventArgs e) //line
        {
            currItem = Item.Line;
        }

        private void toolStripButton8_Click(object sender, EventArgs e) //rectangle
        {
            currItem = Item.Rectangle;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            currItem = Item.Ellipse;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)//pen
        {
            currItem = Item.Pen;
        }

        private void toolStripButton10_Click(object sender, EventArgs e)//brush
        {
            currItem = Item.Brush;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            currItem = Item.Eraser;
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            currItem = Item.ColorPicker;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (currItem == Item.ColorPicker)
            {
                Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
                Graphics g = Graphics.FromImage(bmp);
                Rectangle rect = panel1.RectangleToScreen(panel1.ClientRectangle);
                g.CopyFromScreen(rect.Location, Point.Empty, panel1.Size);
                g.Dispose();
                p.Color = bmp.GetPixel(e.X, e.Y);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) //new
        {
            panel1.Refresh();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) //import
        {
            OpenFileDialog oFile = new OpenFileDialog();
            oFile.Filter = "Png files:|*.png|jpeg files:|*.jpeg|Bitmap files:|*.bmp";
            if (oFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                panel1.BackgroundImage = (Image)Image.FromFile(oFile.FileName).Clone();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)//save
        {
            Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
            Graphics g = Graphics.FromImage(bmp);
            Rectangle rect = panel1.RectangleToScreen(panel1.ClientRectangle);
            g.CopyFromScreen(rect.Location, Point.Empty, panel1.Size);
            g.Dispose();
            SaveFileDialog sFile = new SaveFileDialog();
            sFile.Filter = "Png files:|*.png|jpeg files:|*.jpeg|Bitmap files:|*.bmp ";
            if (sFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (System.IO.File.Exists(sFile.FileName))
                {
                    System.IO.File.Delete(sFile.FileName);
                }
                if(sFile.FileName.Contains(".jpg"))
                {
                    bmp.Save(sFile.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    
                }
                else if (sFile.FileName.Contains(".png"))
                {
                    bmp.Save(sFile.FileName, System.Drawing.Imaging.ImageFormat.Png);

                }
                else if (sFile.FileName.Contains(".bmp"))
                {
                    bmp.Save(sFile.FileName, System.Drawing.Imaging.ImageFormat.Bmp);

                }
            }


        }


    }
}
