using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeSolver
{

    internal class Program
    {
        static string maze =
@"########################
          #####       ##
######### ##### ########
######### ##### ########
######### ##### ########
########         #######
######## ###############
#        ###############
######## ###############
########         #######
######## ###############
######## ###############
###      ###############
######## ####      #####
######## #### #### #####
######## #### #### #####
####          #### #####
#### #############     #
###################### #
###################### #";

        static string[][] matrix;
        static string[][] trail;

        static int startX = 0;
        static int startY = 0;
        static int xMin = 0;
        static int xMax = 0;
        static int yMin = 0;
        static int yMax = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ConvertToMatrix();
            DetectStart();
            Console.WriteLine("Start: {0}:{1}", startX, startY);

            Walk(direction.east, startX, startY);
            Print();
            PrintTrail();

            Console.ReadKey();
        }

        static bool IsExit(int x, int y)
        {
            var exit = 
                (matrix[y][x] == " " &&
                (x == xMax ||
                y == yMax));

            if (y == startY || x == startX)
            {
                exit = false;
            }
            return exit;
        }

        static void Print()
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j]);
                }
                Console.WriteLine();
            }
        }

        static void PrintTrail()
        {
            for (int i = 0; i < trail.Length; i++)
            {
                for (int j = 0; j < trail[i].Length; j++)
                {
                    Console.Write(trail[i][j]??" ");
                }
                Console.WriteLine();
            }
        }

        enum direction { east, west, north, south }
        static bool Walk(direction dir, int x, int y)
        {
            
            switch (dir)
            {
                case direction.east:
                    x = x+1;
                    break;
                case direction.west:
                    x = x-1;
                    break;
                case direction.north:
                    y = y-1;
                    break;
                case direction.south:
                    y = y+1;
                    break;
                default:
                    break;
            }

            if (y> matrix[0].Length-1 || x > matrix.Length-1 || x < 0 || y < 0) return false;

            //if (IsExit(x, y)) return true;

            if (matrix[y][x] == " ")
            {
                //matrix[y][x] = ".";

                if (Walk(direction.east, x, y))
                {
                    trail[y][x] = "*";
                } 
                if (Walk(direction.south, x, y))
                {
                    trail[y][x] = "*";
                } 
                if (Walk(direction.west, x, y))
                {
                    trail[y][x] = "*";
                } 
                if (Walk(direction.north, x, y))
                {
                    trail[y][x] = "*";
                }
                return true;// IsExit(x,y);
            }
            else
            {
                return false;
            }

        }

        static void ConvertToMatrix()
        {
            var lines = maze.Split("\r\n");
            matrix = new string[lines.Length][];
            trail = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                matrix[i] = lines[i].ToCharArray().ToList().Select(s => s.ToString()).ToArray();
                trail[i] = new string[lines[i].Length];
            }
        }

        static void DetectStart()
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix[i][0] == " ")
                {
                    startX = 0;
                    startY = i;
                    break;
                }

                if (matrix[i].Last() == " ")
                {
                    startX = matrix[i].Length - 1;
                    startY = i;
                    break;
                }
            }

            yMax = matrix[0].Length-1;
            xMax = matrix.Length-1;

        }
    }
}
