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

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Print();
            ConvertToMatrix();
            DetectStart();

            Console.WriteLine("Start: {0}:{1}",startX,startY);
            Console.ReadKey();
        }

        static void Print()
        {
            Console.WriteLine(maze);
        }

        static void ConvertToMatrix() {
            var lines = maze.Split("\r\n");
            matrix = new string[lines.Length][];
            trail = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                matrix[i] = lines[i].ToCharArray().ToList().Select(s=>s.ToString()).ToArray();
                trail[i] = new string[lines[i].Length];
            }
        }

        static void DetectStart()
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (matrix[i][0]==" ")
                {
                    startX = 0;
                    startY = i;
                    break;
                }

                if (matrix[i].Last() == " ")
                {
                    startX = matrix[i].Length-1;
                    startY = i;
                    break;
                }
            }
        }
    }
}
