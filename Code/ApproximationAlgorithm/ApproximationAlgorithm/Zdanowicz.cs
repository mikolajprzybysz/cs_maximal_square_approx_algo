using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;

namespace ApproximationAlgorithm
{
    public class Zdanowicz
    {
        public List<Rectangle> rectangleList;
        public List<Rectangle> inputRectangleList;
        public List<Rectangle> usedRectangleList;
        public int side;
        float areaOfUsedRectangles, areaOfInputRectangles;
        int[,] square;
        int usedRectangleCounter = 0;
        bool rotated = false;
        public bool solution = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Zdanowicz(Rectangle[] Rectangles)
        {

            rectangleList = new List<Rectangle>();
            inputRectangleList = new List<Rectangle>();
            foreach (Rectangle r in Rectangles)
            {
                rectangleList.Add(r);
                inputRectangleList.Add(r);
            }
        }
        public Zdanowicz(System.Windows.Forms.ListBox.ObjectCollection Rectangles)
        {

            rectangleList = new List<Rectangle>();
            inputRectangleList = new List<Rectangle>();
            foreach (Rectangle r in Rectangles)
            {
                rectangleList.Add(new Rectangle(r.getWidth(), r.getHeight()));
                inputRectangleList.Add(new Rectangle(r.getWidth(), r.getHeight()));
            }
        }

        /// <summary>
        /// Prepares data to use
        /// </summary>
        public void prepareData()
        {
            usedRectangleList = new List<Rectangle>();
            usedRectangleList.Clear();
            rotated = false;
            areaOfUsedRectangles = 0;
            usedRectangleCounter = 0;
            foreach (Rectangle r in rectangleList)
                r.used = false;
            rectangleList.Sort();
            rectangleList.Reverse();
        }

        /// <summary>
        /// Calculate side of possible square from set of rectangles
        /// </summary>
        public void calculateSideOfPossibleSquare()
        {
            float areaOfRectangles = 0;

            foreach (Rectangle r in rectangleList)
                areaOfRectangles += r.getArea();
            side = (int)Math.Floor(Math.Sqrt(areaOfRectangles));
        }

        /// <summary>
        /// Creates a matrix (square) filled with '0'
        /// </summary>
        public void prepareSquare()
        {
            if (side > 0)
            {
                square = new int[side, side];
                for (int i = 0; i < side; i++)
                    for (int j = 0; j < side; j++)
                        square[i, j] = 0;
            }
        }

        /// <summary>
        /// Removes rectangles with borders greater than side of possible square
        /// </summary>
        /// <returns>True - at least one was deleted, False - no rectangles were deleted</returns>
        public bool removeRectangles()
        {
            List<int> indexes = new List<int>();
            int i = -1;
            int counter = 0;
            List<Rectangle> tmp = rectangleList;
            foreach (Rectangle r in tmp)
            {
                i++;
                if ((r.getWidth() > side) || (r.getHeight() > side))
                    indexes.Add(i);
            }
            foreach (int x in indexes)
            {
                rectangleList.RemoveAt(x - counter);
                counter++;
            }

            if (indexes.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the possibile rectangle size that left to be covered
        /// </summary>
        /// <returns>Side of possible rectangle</returns>
        public int getSizeOfPossibleRectangleSize()
        {
            int counter;
            int max = -1;
            for (int j = 0; j < side; j++)
            {
                counter = 0;
                for (int i = 0; i < side; i++)
                {
                    if (square[i, j] == 0)
                        counter++;
                    if (max < counter)
                        max = counter;
                }
                if (max != 0)
                    break;
            }
            return max;
        }

        /// <summary>
        /// Finds the rectangle with side nearest to initial value
        /// </summary>
        /// <param name="size">An initial value</param>
        /// <returns>Rectangle</returns>
        public Rectangle findRectangle(int size)
        {
            int maxW = -1;
            int maxH = -1;
            int i = -1;
            int rec = -1;
            bool found = false;
            bool perfect = false;
            //List<int> tmp = new List<int>();

            foreach (Rectangle r in rectangleList)
            {
                i++;
                if (!r.used)
                {
                    if (r.getWidth() == size)
                    {
                        found = true;
                        rec = i;
                        break;
                    }
                    else if (r.getWidth() > maxW)
                    {
                        maxW = r.getWidth();
                        rec = i;
                    }
                    if (r.getHeight() == size)
                    {
                        found = true;
                        rec = i;
                        break;
                    }
                    else if (r.getHeight() > maxH)
                    {
                        maxH = r.getHeight();
                        rec = i;
                    }
                }
            }
            Rectangle result;
            if (rec != -1)
            {
                result = rectangleList[rec];
                int s;
                if (result.getWidth() > result.getHeight() && result.getWidth() <= size)
                    s = result.getWidth();
                else
                    s = result.getHeight();

                if (s != result.getWidth())
                    result.rotate();
            }
            else
                result = new Rectangle(0, 0);
            return result;
        }

        /// <summary>
        /// Finds the Position to put rectangle
        /// </summary>
        /// <returns>Position of rectangle to be put</returns>
        public Position findPositionToPutRectangle()
        {
            bool found = false;
            int i = 0, j = 0;
            for (j = 0; j < side; j++)
            {
                for (i = 0; i < side; i++)
                    if (square[i, j] == 0)
                    {
                        found = true;
                        break;
                    }
                if (found)
                    break;
            }
            return new Position(i, j);
        }

        /// <summary>
        /// Checks if given rectangle fits in square
        /// </summary>
        /// <param name="rec">Rectangle to be put</param>
        /// <param name="pos">Position to put rectangle</param>
        /// <returns>True - rectangle fits, False - there is no place for this rectangle</returns>
        public bool isRectangleOK(Rectangle rec, Position pos)
        {
            int counter = 0;
            if ((rec.getWidth() != 0 && rec.getHeight() != 0) || !rec.used)
                if ((rec.getWidth() + pos.getPosX()) > side || (rec.getHeight() + pos.getPosY()) > side)
                    return false;
                else
                {
                    for (int i = pos.getPosX(); i < (rec.getWidth() + pos.getPosX()); i++)
                        for (int j = pos.getPosY(); j < (rec.getHeight() + pos.getPosY()); j++)
                            if (square[i, j] == 0)
                                counter++;
                    rec.calculateArea();
                    if (counter == rec.getArea())
                        return true;
                    else
                        return false;
                }
            else
                return false;
        }

        /// <summary>
        /// Put rectangle at given position in square (first check isRactangleOK())
        /// </summary>
        /// <param name="rec">Rectangle to be put</param>
        /// <param name="pos">Position to put rectangle</param>
        /// <returns>True - was put, False - wasn't put</returns>
        public bool putRectangleInSquare(Rectangle rec, Position pos)
        {
            if (isRectangleOK(rec, pos))
            {
                usedRectangleCounter++;
                for (int i = pos.getPosX(); i < pos.getPosX() + rec.getWidth(); i++)
                    for (int j = pos.getPosY(); j < pos.getPosY() + rec.getHeight(); j++)
                        square[i, j] = usedRectangleCounter;

                rec.used = true;
                rec.location.setPosition(pos.getPosX(), pos.getPosY());
                usedRectangleList.Add(rec);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Calculates area of used rectangles
        /// </summary>
        /// <param name="used">List of used rectangles</param>
        public int calculateAreaOfUsedRectangles(List<Rectangle> used)
        {
            int result = 0;
            foreach (Rectangle r in used)
                result += r.getArea();
            return result;
        }

        /// <summary>
        /// Main function running algorithm
        /// </summary>
        /// <returns>List of used rectangles with positions</returns>
        public List<Rectangle> Run(ref long time, ApproximationAlgorithmForm form)
        {
            areaOfInputRectangles = calculateAreaOfAllRectangles(rectangleList);
            Stopwatch timer = new Stopwatch();
            //readRectanglesFromFile();
            timer.Start();
            calculateSideOfPossibleSquare();
            while (side > 0)
            {
                int i = 0;
                prepareData();
                while (removeRectangles())
                    calculateSideOfPossibleSquare();
                //Console.WriteLine("Side: " + side);
                prepareSquare();
                while (i != rectangleList.Count)
                {
                    if (!putRectangleInSquare(findRectangle(getSizeOfPossibleRectangleSize()),
                        findPositionToPutRectangle()))
                        i++;
                    else
                        i = 0;
                    areaOfUsedRectangles = calculateAreaOfUsedRectangles(usedRectangleList);
                    /*new Thread(new ThreadStart(delegate()
                        {
                            while (!solution)
                                form.setProgressBarValue(progress());
                        })).Start();*/
                    form.setProgressBarValue(progress());
                    //consoleWriteSquare();

                    if ((side * side) == areaOfUsedRectangles)
                    {
                        solution = true;
                        timer.Stop();
                        break;
                    }
                }
                if (!solution)
                {
                    side--;
                    form.setProgressBarValue(0);
                }
                else
                {
                    consoleWriteSquare();
                    Console.WriteLine("Square size: "
                        + side + "x" + side);
                    Console.WriteLine("-- Solution found in "
                        + timer.ElapsedMilliseconds.ToString() + "ms --");
                    consoleWriteUsedRectangles();
                    Console.WriteLine("Number of used rectanlges: "
                        + usedRectangleList.Count);
                    Console.WriteLine("Percentage of used rectangles: "
                        + (float)((float)usedRectangleList.Count / (float)inputRectangleList.Count * 100)
                        + "%");
                    Console.WriteLine("Percentage of area: "
                        + (areaOfUsedRectangles / areaOfInputRectangles * 100)
                        + "%");

                    break;
                }
            }
            //timer.Stop();
            solution = true;
            if (usedRectangleList.Count == 0)
            {
                Console.WriteLine("There is no solution!");
                usedRectangleList = null;
            }
            time = timer.ElapsedMilliseconds;

            return usedRectangleList;
        }

        /// <summary>
        /// Calculates area of all rectangles
        /// </summary>
        /// <param name="list">Input list of rectangles</param>
        public int calculateAreaOfAllRectangles(List<Rectangle> list)
        {
            int area = 0;
            foreach (Rectangle r in list)
                area += r.getArea();
            return area;
        }

        /// <summary>
        /// Write to console borders of used rectangles
        /// </summary>
        public void consoleWriteUsedRectangles()
        {
            Console.Write("Used Rectangleangles: ");
            foreach (Rectangle r in usedRectangleList)
                Console.Write(r.getWidth().ToString() + "x" + r.getHeight().ToString() + "  ");
            Console.Write("\n");
        }

        /// <summary>
        /// 'Draw' square in console
        /// </summary>
        public void consoleWriteSquare()
        {
            Console.WriteLine("Square:");
            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                    Console.Write(square[j, i].ToString() + "\t");
                Console.Write("\n");
            }
        }

        public int progress()
        {
            if (side > 0)
                return (int)areaOfInputRectangles / (side * side) * 100;
            else
                return 0;
        }
    }
}
