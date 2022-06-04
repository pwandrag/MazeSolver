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
        static bool stop = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ConvertToMatrix();
            DetectStart();

            var trail = Seek(direction.east, startX, startY);
            trail.Add(new pos { x = startX, y = startY });

            PrintMaze();
            PrintTrail(trail);
            PrintDirections(trail);

            Console.ReadKey();
        }

        /// <summary>
        /// The premise of this is that when open space is detected 
        /// it continues to search in all directions exept the direction 
        /// it came from. Once the exit is detected it returns an empty list
        /// that starts the unwinding of the recusive search.
        /// If a return from a recusion is detected that is not null, it implies 
        /// the exit was found and a backtrack occurs by adding the positions to the 
        /// returning list in reverse order. If a wall is found the recursion stops
        /// and a null is retuned, effectively killing that thread
        /// 
        /// A global stop signal is used to stop all searches once the exit is found
        /// this prevents exhaustive searching of the maze
        /// 
        /// </summary>
        /// <param name="dir">Direction last search was from</param>
        /// <param name="x">last search pos x</param>
        /// <param name="y">last search pos y</param>
        /// <returns>list of positions from exit</returns>
        static List<pos> Seek(direction dir, int x, int y)
        {
            if (stop) return null;

            direction[] directions;

            //calculate next position and next directions to travel in (no backtracking)
            switch (dir)
            {
                case direction.east:
                    x = x + 1;
                    directions = new direction[] { direction.east, direction.south, direction.north };
                    break;
                case direction.west:
                    x = x - 1;
                    directions = new direction[] { direction.west, direction.south, direction.north };
                    break;
                case direction.north:
                    y = y - 1;
                    directions = new direction[] { direction.west, direction.north, direction.east };
                    break;
                case direction.south:
                    y = y + 1;
                    directions = new direction[] { direction.west, direction.east, direction.south };
                    break;
                default:
                    directions = new direction[] { };
                    break;
            }

            //prevent out of bounds
            if (y > matrix[0].Length - 1 || x > matrix.Length - 1 || x < 0 || y < 0) return null;

            //detect exit
            if (DetectExit(x, y)) return new List<pos>();

            //continue walking
            if (matrix[y][x] == " ")
            {
                foreach (var item in directions.ToList())
                {
                    var t = Seek(item, x, y);
                    if (t != null)
                    {
                        t.Add(new pos() { x = x, y = y });
                        return t;
                    }
                }
            }

            return null;
        }


        static void PrintMaze()
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

        static void PrintTrail(List<pos> track)
        {
            foreach (var item in track)
            {
                trail[item.y][item.x] = "*";
            }

            for (int i = 0; i < trail.Length; i++)
            {
                for (int j = 0; j < trail[i].Length; j++)
                {
                    Console.Write(trail[i][j] ?? " ");
                }
                Console.WriteLine();
            }
        }

        static void PrintDirections(List<pos> track)
        {
            track.Reverse();
            pos lastPos = null;
            var directions = "";
            foreach (var item in track)
            {
                if (lastPos == null)
                {
                    lastPos = item;
                } else
                {
                    if (item.x < lastPos.x) directions += "W";
                    if (item.x > lastPos.x) directions += "E";
                    if (item.y < lastPos.y) directions += "N";
                    if (item.y > lastPos.y) directions += "S";
                }
                lastPos = item;
            }
            Console.WriteLine("Directions:");
            Console.WriteLine(directions);
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

            yMax = matrix[0].Length - 1;
            xMax = matrix.Length - 1;

        }
        static bool DetectExit(int x, int y)
        {
            return
                (matrix[y][x] == " " &&
                (x == xMax || y == yMax || x == xMin || y == yMin)) &&
                (x != startX && y != startY);
        }

    }

    enum direction { east, west, north, south }
    public class pos
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
