using System;
using System.Collections.Generic;
using System.Threading;

namespace motsglisses
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Plateau plateau = new Plateau(8, 8);
            Jeu jeu = new Jeu();
            
            while (true)
            {
                Console.WriteLine("Menu principal:");
                Console.WriteLine("1 - Jouer à partir d'un fichier");
                Console.WriteLine("2 - Jouer à partir d'un plateau généré aléatoirement");
                Console.WriteLine("3 - Sortir");

                Console.Write("Votre choix : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        // Logique pour jouer à partir d'un fichier
                        Console.WriteLine("Logique pour jouer à partir d'un fichier");
                        jeu = new Jeu();
                        plateau.ToRead("SauvePlateau.txt");
                        jeu.Jouer(plateau);
                        break;
                    case "2":
                        // Logique pour jouer à partir d'un plateau généré aléatoirement
                        Console.WriteLine("Logique pour jouer à partir d'un plateau généré aléatoirement.");
                        jeu = new Jeu();
                        plateau = new Plateau(8, 8);
                        jeu.Jouer(plateau);
                        break;
                    case "3":
                        // Sortir du programme
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Choix invalide. Veuillez réessayer.");
                        break;
                }

                Console.WriteLine(); // Saut de ligne pour la lisibilité
            }
        }
    }
}
