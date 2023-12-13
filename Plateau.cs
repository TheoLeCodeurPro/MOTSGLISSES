using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motsglisses
{
    internal class Plateau
    {
        string[,] plateau;

        public Plateau(string[,] plateau)
        {
            this.plateau = plateau;
        }
        public string toString()
        {
            string desplateau = "";
            for (int i = 0; i < this.plateau.GetLength(0); i++)
            {
                for (int j = 0; j < this.plateau.GetLength(1); j++) 
                {
                    desplateau += "|"+this.plateau[i, j];
                }
                desplateau += "|\n";
            }
            return desplateau;
        }
        public void ToFile(string nomfile)
        {

        }
    }
}
