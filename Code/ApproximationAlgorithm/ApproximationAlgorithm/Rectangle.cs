using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApproximationAlgorithm
{
    public class Position
    {
        private int x;
        private int y;

        public void setPosX(int a) { x = a; }
        public int getPosX() { return x; }
        public void setPosY(int a) { y = a; }
        public int getPosY() { return y; }

        public Position()
        {
            x = y = 0;
        }

        public Position(int a, int b)
        {
            x = a;
            y = b;
        }

        public void setPosition(int a, int b)
        {
            x = a;
            y = b;
        }
    }
    public class Rectangle : IComparable
    {
        private int width;
        private int height;
        private int area;
        public Position location = new Position();
        public bool used = false;

        public int getWidth() { return width; }
        public void setWidth(int w) { width = w; }
        public int getHeight() { return height; }
        public void setHeight(int h) { height = h; }
        public int getArea() { return area; }

        public Rectangle(int w, int h)
        {
            width = w;
            height = h;
            calculateArea();
        }

        public void rotate()
        {
            int tmp;
            tmp = width;
            width = height;
            height = tmp;
        }

        public void calculateArea()
        {
            area = width * height;
        }

        public override string ToString()
        {
            return "" + width.ToString() + " x " + height.ToString();
        }

        /// <summary>
        /// Override of compareTo used in sorting arrays
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Result of comparison of sizes of Rectangleangles</returns>
        public int CompareTo(object obj)
        {
            if (obj is Rectangle)
            {
                Rectangle r2 = (Rectangle)obj;
                return this.getArea().CompareTo(r2.getArea());
            }
            else
                throw new ArgumentException("Object is not of type Rectangle.");
        }


    }
}
