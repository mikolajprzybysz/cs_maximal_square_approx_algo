using System;
using System.Collections;
using ApproximationAlgorithm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
namespace algo_approx{

class bwApprox{
/*class Rectangle : IComparable
{
    public int w;
    public int h;
    public int size;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Rectangle() { w = 0; h = 0; size = 0; }
    /// <summary>
    /// Constructor setting the width and height of the Rectangleangle
    /// </summary>
    /// <param name="width">Width of the new Rectangleangle</param>
    /// <param name="height">Height of the new Rectangleangle</param>
    public Rectangle(int width, int height) { w = width; h = height; size = h * w; }

    /// <summary>
    /// Rotates the Rectangleangle
    /// </summary>
    public void rotate(){ int t = w; w = h; h = t; }


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
            return size.CompareTo(r2.size);
        }
        else
            throw new ArgumentException("Object is not of type Rectangle.");
    }

    public override string ToString()
    {
        return "(" + this.getWidth() + "," + this.getHeight() + ")";
    }

*/

    public int squareSize = 0;
    int[,] square = null;
    List<Rectangle> input;
    public List<Rectangle> solutionSet;
    
    /// <summary>
    /// Gets the possibile square size from the input
    /// </summary>
    /// <param name="input">Array of Rectangleangles</param>
    /// <returns>Size of possibile square</returns>
    public static int getSquareSize(List<Rectangle> input)
    {
        int squareSize=0;
        foreach(Rectangle r in input)
        {
            squareSize += r.getArea();
        }

        return (int)Math.Floor(Math.Sqrt(squareSize));
    }

    /// <summary>
    /// Tries to fit the Rectangleangle int the square
    /// </summary>
    /// <param name="square">The sqare to fit the Rectangleangle into</param>
    /// <param name="squareSize"></param>
    /// <param name="input">Rectangle to be fit</param>
    /// <param name="inputNr">Id of the Rectangleangle to fill the cells of the square with</param>
    /// <returns></returns>
    public static bool fitRectangle(int[,] square, int squareSize, Rectangle input,int inputNr)
    {
        for(int y = 0; y < squareSize; y++)
            for (int x = 0; x < squareSize; x++)
            {
                if (square[y,x] == 0)
                {
                    // check if Rectangleangle fits as it is

                    if (x + input.getWidth() <= squareSize)
                    {
                        if (y + input.getHeight() <= squareSize)
                        {

                            for (int w = y; w < y+input.getHeight(); w++)
                                for (int z = x; z < x + input.getWidth(); z++)
                                {
                                    if (square[w, z] != 0)
                                    {
                                        // if the rectangle doesnt fit, revert the process
                                        for (; y <= w; y++)
                                            for (; x <= z; x++)
                                            {
                                                if(square[y,x] == inputNr)
                                                    square[y, x] = 0;
                                                //printSquare(square, squareSize);
                        
                                            }

                                        return false;
                                    }
                                    square[w, z] = inputNr;
                                }
                            input.location = new Position(x, y);
                            return true;
                        }

                    }

                    // check if Rectangleangle fits as rotated
                    
                    input.rotate();

                    if (x + input.getWidth() <= squareSize)
                    {
                        if (y + input.getHeight() <= squareSize)
                        {

                            for (int w = y; w < y + input.getHeight(); w++)
                                for (int z = x; z < x + input.getWidth(); z++)
                                {
                                    if (square[w, z] != 0)
                                    {
                                        // if the rectangle doesnt fit, revert the process
                                        for (; y <= w; y++)
                                            for (; x <= z; x++)
                                            {
                                                if (square[y, x] == inputNr)
                                                    square[y, x] = 0;
                                                //printSquare(square, squareSize);

                                            }
                                        input.rotate();
                                        return false;
                                    }
                                    square[w, z] = inputNr;
                                }
                            input.location = new Position(x, y);
                            return true;
                        }

                    }

                }
            }
        return false;    
    }
    

    
    public bwApprox(object[] Rectangleangles)
    {
        input = new List<Rectangle>();
        foreach(object o in Rectangleangles)
            input.Add((Rectangle)o);
    }

    public bwApprox(System.Windows.Forms.ListBox.ObjectCollection Rectangles)
    {
        input = new List<Rectangle>();
        foreach (Rectangle o in Rectangles)
            input.Add(new Rectangle(o.getWidth(),o.getHeight()));
    }
    

    public List<Rectangle> Run(ref long time, ApproximationAlgorithmForm form)
    {
        solutionSet = new List<Rectangle>();
        bool solution = false;
        this.squareSize = 0;
        this.square = null;

        input.Sort();
        input.Reverse();

        squareSize = bwApprox.getSquareSize(input);

        Stopwatch stop = new Stopwatch();
        stop.Start();
        while (!solution && squareSize > 0)
        {
            solutionSet.Clear();

            while (bwApprox.removeExtremes(input, squareSize) > 0)
            {
                squareSize = bwApprox.getSquareSize(input);
                //Console.WriteLine("New size : " + squareSize.ToString());
            }

            square = new int[squareSize, squareSize];
            
            for (int x = 0; x < input.Count; x++)
            {
                //form.setProgressBarValue(progress());
                if (bwApprox.fitRectangle(square, squareSize, input[x], x + 1))
                {
                    solutionSet.Add(input[x]);
                    
                }
                form.setProgressBarValue(progress());
                //printSquare(square, squareSize);
                    
                if (bwApprox.isFull(square))
                {
                    solution = true;
                    break;
                }

            }
            squareSize--;

        }
        stop.Stop();
        if (solution)
        {
            bwApprox.printSquare(square, squareSize + 1);
            foreach (Rectangle r in solutionSet)
            {
                Debug.Write(r.ToString() + " ");
            }
            Debug.Write("\n");
            Debug.WriteLine(stop.ElapsedMilliseconds.ToString() + " msec [aprox]");
            Debug.WriteLine("Size of solution square : " + (++squareSize).ToString() + " [aprox]");
            time = stop.ElapsedMilliseconds;
            return solutionSet;
        }
        else
        {
            foreach (Rectangle r in input)
            {
                if (r.getWidth() == r.getHeight())
                {
                    List<Rectangle> sol = new List<Rectangle>();
                    sol.Add(new Rectangle(r.getWidth(), r.getHeight()));
                    return sol;
                }
            }
            return null;
        }

    }
    public static void printSquare(int[,] square, int size)
    {
        for(int x = 0; x < size; x ++)
        {
            for(int y = 0; y < size;  y ++)
                Debug.Write(square[x,y] + "-");
            Debug.Write("\n");
        
        }
        Debug.Write("\n");
        
    }

    /// <summary>
    /// Removes Rectangleangles too big for the square
    /// </summary>
    /// <param name="input">List of Rectangleangles</param>
    /// <param name="squareSize"></param>
    /// <returns>Number of removed Rectangleangles</returns>
    public static int removeExtremes(List<Rectangle> input, int squareSize)
    {
        int removed = 0;

        for (int x = 0; x < input.Count; x++ )
        {
            if (((Rectangle)input[x]).getWidth() > squareSize || ((Rectangle)input[x]).getHeight() > squareSize)
            {
                input.Remove(input[x]);
                removed++;
                x--;
            }
        }

        return removed;


    }

    public static bool isFull(int[,] square)
    {
        foreach (int cell in square)
        {
            if (cell == 0)
                return false;

        }
        return true;

    }

    public int progress()
    {
        int area = 0;
        foreach (Rectangle r in solutionSet)
            area += r.getArea();
        if (squareSize > 0)
            return area / (squareSize * squareSize) * 100;
        else
            return 0;
    }
    
}



}