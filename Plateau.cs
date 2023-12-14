using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motsglisses
{
public class Plateau
{
        char[,] plateau;
        int longueur;
        int hauteur;

        public Plateau(int longueur, int hauteur)
        {
            this.longueur = longueur;
            this.hauteur = hauteur;
            this.plateau = new char[longueur, hauteur];

            // Créer une instance de la classe Random
            Random random = new Random();

            char lettre = 'A';
            for (int i = 0; i < longueur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    plateau[i, j] = (char)('A' + random.Next(26)); ;
                }
            }
        }
        public string toString()
        {
            string desplateau = "";
            for (int i = 0; i < this.longueur; i++)
            {
                for (int j = 0; j < this.hauteur; j++) 
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
