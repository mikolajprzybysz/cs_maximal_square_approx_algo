using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ApproximationAlgorithm
{
    public class Przybyszm
    {
        List<Rectangle> rectangleList;
        List<Rectangle> inputRectangleList;
        List<Rectangle> usedRectangleList;
        public int side;
        float areaOfUsedRectangles, areaOfInputRectangles;
        int[,] square = null;
        int usedRectangleCounter = 0;
        bool solution = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Przybyszm(System.Windows.Forms.ListBox.ObjectCollection Rectangles)
        {

            rectangleList = new List<Rectangle>();
            inputRectangleList = new List<Rectangle>();
            foreach (Rectangle r in Rectangles)
            {
                rectangleList.Add(r);
                inputRectangleList.Add(r);
            }
        }

        /// <summary>
        /// Prepares data to use
        /// </summary>
        public void prepareData()
        {
            usedRectangleList = new List<Rectangle>();
            usedRectangleList.Clear();
            areaOfUsedRectangles = 0;
            usedRectangleCounter = 0;
            foreach (Rectangle r in rectangleList)
                r.used = false;
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
        /// Finds the Position to put rectangle
        /// </summary>
        /// <returns>Position of rectangle to be put</returns>
        public Position findPositionToPutRectangle(int row)
        {            
            for (int i = 0; i < side; i++)
                if (square[row, i] == 0)
                    return new Position(i, row);
            return null;
        }

        /// <summary>
        /// Checks if given rectangle fits in square
        /// </summary>
        /// <param name="rec">Rectangle to be put</param>
        /// <param name="pos">Position to put rectangle</param>
        /// <returns>True - rectangle fits, False - there is no place for this rectangle</returns>
        public bool isRectangleOK(Rectangle rec, Position pos)
        {
            if ((rec.getWidth() != 0 && rec.getHeight() != 0) || !rec.used)
                if ((rec.getWidth() - 1 + pos.getPosX()) >= side || (rec.getHeight() - 1 + pos.getPosY()) >= side)
                    return false;
                else
                {
                    for (int i = pos.getPosX(); i < (rec.getWidth() + pos.getPosX()); i++)
                        for (int j = pos.getPosY(); j < (rec.getHeight() + pos.getPosY()); j++)
                            if (square[j, i] != 0)
                                return false;
                    return true;
                }           
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
                        square[j, i] = usedRectangleCounter;

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
        /// rectqangle sorting algorithm
        /// </summary>

        public void Sort()
        {

            // number of elements in array
            int x = rectangleList.Count();
            // array of integers to hold values
            // Insertion Sort Algorithm           
            int i;
            int j;
            Rectangle[] a = new Rectangle[x];
            for (i = 0; i < x; i++)
            {
                a[i] = rectangleList[i];
            }



            Rectangle index;

            for (i = 1; i < x; i++)
            {
                index = a[i];
                j = i;

                while ((j > 0) && (a[j - 1].getArea() < index.getArea()))
                {
                    a[j] = a[j - 1];
                    j = j - 1;
                }

                a[j] = index;
            }
            rectangleList = a.ToList<Rectangle>();
        }
        private bool ProperSquare()
        {
            for(int i=0; i<side;i++)
                for(int j=0; j<side; j++)
                    if(square[i,j]==0) return false;
            return true;
        }
        /// <summary>
        /// Main function running algorithm
        /// </summary>
        /// <returns>List of used rectangles with positions</returns>
        public List<Rectangle> Run(ref long time, ApproximationAlgorithmForm form)
        {
            prepareData();
            areaOfInputRectangles = calculateAreaOfAllRectangles(rectangleList);
            Stopwatch timer = new Stopwatch();            
            timer.Start();
            calculateSideOfPossibleSquare();
            
            while (removeRectangles())
                calculateSideOfPossibleSquare();

            Sort();
            List<Rectangle> originalSortedList = new List<Rectangle>();
            foreach (Rectangle r in rectangleList)
            {
                originalSortedList.Add(new Rectangle(r.getWidth(), r.getHeight())); 
            }

            side++;
            while (side-- > 0)
            {
                prepareSquare();
                usedRectangleList.Clear();
                usedRectangleCounter = 0;
                rectangleList.Clear();
                foreach (Rectangle r in originalSortedList)
                    rectangleList.Add(new Rectangle(r.getWidth(), r.getHeight()));
                int i = 0;
                while (i < rectangleList.Count)
                {
                    int row =0;
                    bool added = false;
                    Position p = null;
                    while (row != side && !added && i < rectangleList.Count)
                    {
                        while (p == null && row != side) p = findPositionToPutRectangle(row++);
                        if (added=putRectangleInSquare(rectangleList[i], p))
                        {
                            rectangleList.RemoveAt(i);
                            i = 0;
                        }
                        else
                        {
                            rectangleList[i].rotate();
                            if (added=putRectangleInSquare(rectangleList[i], p))
                            {
                                rectangleList.RemoveAt(i);
                                i = 0;
                            }
                            else
                            {
                                i++;
                            }

                        }
                    }
                    
                    areaOfUsedRectangles = calculateAreaOfUsedRectangles(usedRectangleList);
                    form.setProgressBarValue(progress());
                    //consoleWriteSquare();

                    if ((side * side) == areaOfUsedRectangles)
                    {
                        if (ProperSquare())
                        {
                            solution = true;
                            timer.Stop();
                            break;
                        }
                        else
                        {

                        }
                    }
                }

                if (solution) break;
            }
            if (solution)
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
            }

            //timer.Stop();
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
