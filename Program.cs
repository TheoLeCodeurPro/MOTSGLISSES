using System;
using System.Collections.Generic;
using System.Threading;

namespace motsglisses
{
    internal class Program
    {
        public static string relativePath;
        static void Main(string[] args)
        {
            relativePath = "/../../../"; // Chemin relatif des fichiers de données par rapport au fichier EXE
            Plateau plateau = new Plateau(8, 8);
            Jeu jeu = new Jeu();
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Menu principal:");
                Console.WriteLine("1 - Jouer à partir d'un fichier ou du dernier plateau");
                Console.WriteLine("2 - Jouer à partir d'un plateau généré aléatoirement");
                Console.WriteLine("3 - Sauver le dernier plateau dans un fichier");
                Console.WriteLine("4 - Sortir");

                Console.Write("Votre choix : ");
                string choix = Console.ReadLine();
                string nomPlateauSauve = "SauvePlateau.txt";
                string userInput;
                int longueur;
                int hauteur;

                switch (choix)
                {
                    case "1":
                        // Jouer à partir d'un fichier
                        Console.Write("Nom du fichier à charger (si ENTER, chargement du dernier plateau) : ");
                        userInput = Console.ReadLine();
                        nomPlateauSauve = "LastPlateau.txt";
                        if (userInput != "") nomPlateauSauve = userInput;
                        jeu = new Jeu();
                        if (plateau.ToRead(nomPlateauSauve))
                        {
                            plateau.ToFile("LastPlateau.txt");
                            jeu.Jouer(plateau);
                        }
                        break;
                    case "2":
                        // Jouer à partir d'un plateau généré aléatoirement
                        jeu = new Jeu();
                        longueur = 8;
                        hauteur = 8;
                        Console.Write("Dimension horizontale du plateau (>=5) (ENTER = 8) :");
                        userInput = Console.ReadLine();
                        if (((!int.TryParse(userInput, out longueur)) && (userInput != "")) || ((longueur>0) && (longueur<5)) || (longueur>10))
                            Console.WriteLine("Dimension invalide !");
                        else if (longueur == 0) longueur = 8;
                        Console.Write("Dimension verticale du plateau   (entre 5 et 10) (ENTER = 8) :");
                        userInput = Console.ReadLine();
                        if (((!int.TryParse(userInput, out hauteur)) && (userInput != "")) || ((hauteur > 0) && (hauteur < 5)) || (hauteur>10))
                            Console.WriteLine("Dimension invalide !");
                        else if (hauteur == 0) hauteur = 8;
                        if ((longueur >= 5 && longueur <= 10) && (hauteur >=5 && hauteur <= 10))
                        {
                            plateau = new Plateau(longueur, hauteur);
                            plateau.ToFile("LastPlateau.txt");
                            jeu.Jouer(plateau);
                        }
                        break;
                    case "3":
                        // Sauver le dernier plateau dans un fichier
                        Console.Write("Nom du fichier (si ENTER, sauvegarde du dernier plateau) : ");
                        userInput = Console.ReadLine();
                        nomPlateauSauve = "LastPlateau.txt";
                        if (userInput != "") nomPlateauSauve = userInput;
                        plateau.ToRead("LastPlateau.txt");
                        plateau.ToFile(nomPlateauSauve);
                        break;
                    case "4":
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
