using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ApproximationAlgorithm
{
    public partial class RectangleViewer : UserControl
    {
        public List<Rectangle> listRectangles;
        List<Rectangle> listRectanglesZoomed;
        int defaultZoom;
        int curRectangleZoom;
        double zoomFactor;
        int viewportWidth;
        int viewportHeight;
        bool useColors = false;
        List<string> colors;

        public RectangleViewer()
        {
            InitializeComponent();
            colors = GetColors();
            zoomLabel.DataBindings.Add("Text", zoomTrackBar, "Value");
            zoomTrackBar.ValueChanged += new EventHandler(zoomTrackBar_ValueChanged);
            listRectangles = new List<Rectangle>();
            listRectanglesZoomed = new List<Rectangle>();
            defaultZoom = 100;
            viewportHeight = pictureBox.Height;
            viewportWidth = pictureBox.Width;
        }

        private List<string> GetColors()
        {
            //create a generic list of strings
            List<string> colors = new List<string>();
            //get the color names from the Known color enum
            string[] colorNames = Enum.GetNames(typeof(KnownColor));
            //iterate thru each string in the colorNames array
            foreach (string colorName in colorNames)
            {
                //cast the colorName into a KnownColor
                KnownColor knownColor = (KnownColor)Enum.Parse(typeof(KnownColor), colorName);
                //check if the knownColor variable is a System color
                if (knownColor > KnownColor.Transparent)
                {
                    //add it to our list
                    colors.Add(colorName);
                }
            }
            //return the color list
            return colors;
        }

        void zoomTrackBar_ValueChanged(object sender, EventArgs e)
        {
            zoom(zoomTrackBar.Value);
            draw();
        }
        public void setListSafety(List<Rectangle> newList)
        {
            new Thread(new ThreadStart(delegate()
            {
                setList(newList);
            })).Start();
        }

        public void setList(List<Rectangle> newList)
        {
            if(listRectangles != null)
                listRectangles.Clear();
            //foreach (Object o in newList)   listRectangles.Add((Rectangle)o);
            listRectangles = newList;
            //zoom(zoomTrackBar.Value);
            //draw();
        }

        public void add(Rectangle Rectangle)
        {
            listRectangles.Add(Rectangle);
        }

        public void rem(int pos)
        {
            int i = pos;
            if (listRectangles.Count > i && i > 0)
            {
                listRectangles.RemoveAt(i);
            }
        }
        public Rectangle get(int pos)
        {
            int i=0;
            foreach (Rectangle Rectangle in listRectangles)
            {
                if (i == pos)
                    return Rectangle;
                i++;
            }
            return null;
        }

        public void zoom(int targetZoom)
        {
            Rectangle workingRectangle;
            zoomFactor = (double)defaultZoom / (targetZoom*30);
            listRectanglesZoomed.Clear();
            if( listRectangles != null )
            foreach (Rectangle Rectangleangle in listRectangles)
            {
                if (Rectangleangle != null)
                {
                    workingRectangle = new Rectangle((int)Math.Ceiling((double)Rectangleangle.getWidth() / zoomFactor), (int)Math.Ceiling((double)Rectangleangle.getHeight() / zoomFactor));
                    workingRectangle.location.setPosition((int)Math.Ceiling((double)Rectangleangle.location.getPosX() / zoomFactor), (int)Math.Ceiling((double)Rectangleangle.location.getPosY() / zoomFactor));
                    listRectanglesZoomed.Add(workingRectangle);
                }
            }
            curRectangleZoom = targetZoom;
        }

        /* zoom function must be called before draw function */
        public void draw()
        {
            pictureBox.Refresh();
            Graphics gfx = pictureBox.CreateGraphics();
            Random random;
            int i = colors.Count-1;

            foreach (Rectangle Rectangleangle in listRectanglesZoomed)
            {
                if (useColors)
                {
                    if (i < 0)
                        i = colors.Count - 1;
                    gfx.FillRectangle(new SolidBrush(Color.FromName(colors[i])), Rectangleangle.location.getPosX(), Rectangleangle.location.getPosY(), Rectangleangle.getWidth(), Rectangleangle.getHeight());
                    i--;
                }
                else
                {
                    gfx.DrawRectangle(new Pen(Color.Black), Rectangleangle.location.getPosX(), Rectangleangle.location.getPosY(), Rectangleangle.getWidth(), Rectangleangle.getHeight());
                    gfx.DrawRectangle(new Pen(Color.Black), Rectangleangle.location.getPosX(), Rectangleangle.location.getPosY(), Rectangleangle.getWidth(), Rectangleangle.getHeight());
                }
               // random = new Random();
               // int r, g, b;
               // r = random.Next(0, 255);
               // g = random.Next(0, 255);
               // b = random.Next(0, 255);             
            }
            pictureBox.Update();
    
            
        }

        public void update()
        {
            pictureBox.Update();
            zoom(zoomTrackBar.Value);
            draw();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            //draw();
        }
     

    }
}
