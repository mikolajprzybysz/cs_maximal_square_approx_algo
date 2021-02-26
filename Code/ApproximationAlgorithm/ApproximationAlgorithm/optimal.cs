using System;
using System.Collections;
using ApproximationAlgorithm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace algo_optimal
{
    class Permute
        {
                 private void swap (ref Rectangle a, ref Rectangle b)
                 {

                        if(a==b)
                            return;
                        Rectangle temp;
                        temp = a;
                        a=b;
                        b=temp;
                  }

                  public ArrayList setper(List<Rectangle> list)
                  {
                        int x=list.Count-1;
                        ArrayList solution = new ArrayList();
                        go(list,0,x,solution);
                        return solution;
                  }

                  private void go (List<Rectangle> input, int k, int m, ArrayList solution)
                  {
                      List<Rectangle> list = new List<Rectangle>(input);
                        int i;
                        if (k == m)
                        {
                            solution.Add(list);
                            //Console.Write (list);
                            //Console.WriteLine (" ");
                        }
                        else
                        {
                            for (i = k; i <= m; i++)
                            {
                                Rectangle temp;
                                temp = list[k];
                                list[k] = list[i];
                                list[i] = temp;
                                go(list, k + 1, m, solution);
                                temp = list[k];
                                list[k] = list[i];
                                list[i] = temp;

                            }
                            
                        }
                        
                   }
         }
    class optimal
    {
        public int side;
        int computationCounter;
        /// <summary>
        /// Gets the possibile square size from the input
        /// </summary>
        /// <param name="input">Array of Rectangleangles</param>
        /// <returns>Size of possibile square</returns>
        public static int getSquareSize(List<Rectangle> input)
        {
            int squareSize = 0;
            foreach (Rectangle r in input)
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
        public static bool fitRectangle(int[,] square, int squareSize, Rectangle input, int inputNr)
        {
            for (int y = 0; y < squareSize; y++)
                for (int x = 0; x < squareSize; x++)
                {
                    if (square[y, x] == 0)
                    {
                        // check if Rectangleangle fits as it is
                        
                            if (x + input.getWidth() <= squareSize)
                            {
                                if (y + input.getHeight() <= squareSize)
                                {

                                    for (int w = y; w < y + input.getHeight(); w++)
                                        for (int z = x; z < x + input.getWidth(); z++)
                                        {
                                            if (square[w, z] != 0)
                                            {
                                                try
                                                {
                                                    // if the rectangle doesnt fit, revert the process
                                                    for (w = y; w < squareSize; w++)
                                                        for (z = x; z < squareSize; z++)
                                                        {
                                                            if (square[w, z] == inputNr)
                                                                square[w, z] = 0;

                                                        }
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine("Fit rectangle: " + e.Message);
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
                        /*
                        input.rotate();

                        if (x + input.getWidth() <= squareSize)
                        {
                            if (y + input.getHeight() <= squareSize)
                            {

                                for (int w = y; w < y + input.getHeight(); w++)
                                    for (int z = x; z < x + input.getWidth(); z++)
                                        square[w, z] = inputNr;
                                return true;
                            }

                        }
                        */
                    }
                }
            return false;
        }


        List<Rectangle> input;

        public optimal(object[] Rectangleangles)
        {
            input = new List<Rectangle>();
            foreach (object o in Rectangleangles)
                input.Add((Rectangle)o);
        }

        public optimal(System.Windows.Forms.ListBox.ObjectCollection Rectangles)
        {
            input = new List<Rectangle>();
            foreach (Rectangle o in Rectangles)
                input.Add(new Rectangle(o.getWidth(),o.getHeight()));
        }

        public ArrayList combination(List<Rectangle> array) {
            ArrayList result = new ArrayList();
            result.Add(new List<Rectangle>());

            foreach (Rectangle element in array)
            {
                int y = result.Count;
                for (int x = 0; x < y; x++)
                {
                    List<Rectangle> combination = new List<Rectangle>((List<Rectangle>)result[x]);
                    combination.Add(element);
                    result.Add(combination);
                }
            }
            return result;
        }
      

        public List<Rectangle> Run(ref long time, ApproximationAlgorithmForm form)
        {

            Permute p = new Permute();

            List<List<Rectangle>> solutionSets = new List<List<Rectangle>>();
            List<Rectangle> solutionSet = new List<Rectangle>();
            bool solution = false;
            int squareSize = 0;
            int[,] square = null;

            input.Sort();
            input.Reverse();
            squareSize = optimal.getSquareSize(input);

            Stopwatch stop = new Stopwatch();
            stop.Start();

            Console.WriteLine("Size : " + squareSize.ToString());
            while (optimal.removeExtremes(input, squareSize) > 0)
            {
                squareSize = optimal.getSquareSize(input);
                Console.WriteLine("New size : " + squareSize.ToString());
            }
            ArrayList permutations = p.setper(input);
            ArrayList combinations = combination(input);
            ArrayList computationSets = new ArrayList();

            try
            {
                foreach (List<Rectangle> perm in permutations)
                {
                    computationSets.Add(perm);
                    foreach (List<Rectangle> comb in combinations)
                    {
                        List<Rectangle> set = new List<Rectangle>();
                        foreach (Rectangle r in perm)
                        {

                            set.Add(new Rectangle(r.getWidth(), r.getHeight()));
                        }
                        foreach (Rectangle r in comb)
                        {
                            //int pos = set.IndexOf(r);
                            int pos = set.FindIndex(delegate(Rectangle rt) { return rt.getWidth().Equals(r.getWidth()) && rt.getHeight().Equals(r.getHeight()); });

                            set[pos].rotate();
                        }
                        computationSets.Add(set);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("optymalny1 : " + e.Message);
            }
            try
            {
                while (!solution && squareSize > 0)
                {
                    computationCounter = 0;
                    foreach (List<Rectangle> set in computationSets)
                    {
                        side = squareSize;
                        form.setProgressForOptimal(progress(computationCounter, computationSets.Count), computationSets.Count);
                        computationCounter++;
                        optimal.removeExtremes(set, squareSize);
                        solutionSet.Clear();
                        square = new int[squareSize, squareSize];


                        for (int x = 0; x < set.Count; x++)
                        {
                            
                                //form.setProgressBarValue(progress(computationCounter, computationSets.Count));
                                if (optimal.fitRectangle(square, squareSize, set[x], x + 1))
                                {
                                    solutionSet.Add(set[x]);
                                    //printSquare(square, squareSize);
                                }
                           
                            if (optimal.isFull(square))
                            {
                                solution = true;
                                //return solutionSet;
                                //solutionSets.Add(solutionSet);
                                //solutionSets.Add(solutionSet);
                                break;
                            }

                        }
                        if (solution)
                            break;
                    }
                    //form.setProgressBarValue(progress(computationCounter, computationSets.Count));
                    squareSize--;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("optymalny2 : "+ squareSize+ e.Message);
            }

                    stop.Stop();
                    if (solution)
                    {
                        printSquare(square, squareSize+1);
                        foreach (Rectangle r in solutionSet)
                        {
                            Debug.Write(r.ToString() + " ");
                        }
                        Debug.Write("\n");
                        Debug.WriteLine(stop.ElapsedMilliseconds.ToString() + " msec [optimal]");
                        time = stop.ElapsedMilliseconds;
                        side = squareSize;
                        return solutionSet;
                    }      
            return null;
        }
        public static void printSquare(int[,] square, int size)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                    Debug.Write(square[x, y]);
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

            for (int x = 0; x < input.Count; x++)
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

        public int progress(int value, int max)
        {
            if (side > 0)
                return value / max * 100;
            else
                return 0;
        }

    }



}