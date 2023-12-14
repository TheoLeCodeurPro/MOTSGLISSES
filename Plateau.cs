using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace motsglisses
{
    public class Plateau
    {
        char[,] plateau;
        int longueur;
        int hauteur;
        private Dictionary<char, LetterInfo> lettre;

        public Plateau(int longueur, int hauteur)
        {
            this.longueur = longueur;
            this.hauteur = hauteur;
            this.plateau = new char[longueur, hauteur];
            this.lettre = new Dictionary<char, LetterInfo>();

            // Créer une instance de la classe Random

            try
            {
                string repertoireCourant = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                repertoireCourant = repertoireCourant + "/../../../Lettre.txt";
                List<char> listCar = new List<char>();

                using (StreamReader reader = new StreamReader(repertoireCourant))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');

                        if (parts.Length == 3)
                        {
                            char l = parts[0][0];
                            int occMax = int.Parse(parts[1]);
                            int poids = int.Parse(parts[2]);

                            this.lettre[l] = new LetterInfo(occMax, poids);
                            // Console.WriteLine(l + "," + lettre[l].OccMax + "," + lettre[l].Poids);
                            for (int k = 0; k < this.lettre[l].OccMax; k++)
                            {
                                listCar.Add(l);
                            }
                        }
                    }
                }
                MelangerListe(listCar);
                AfficherListe(listCar);

                Random random = new Random();

                for (int i = 0; i < longueur; i++)
                {
                    for (int j = 0; j < hauteur; j++)
                    {
                        plateau[i, j] = listCar[i*longueur + j];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la lecture du fichier : {ex.Message}");
            }
        }

        static void MelangerListe<T>(List<T> liste)
        {
            Random rand = new Random();
            int n = liste.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);

                T temp = liste[i];
                liste[i] = liste[j];
                liste[j] = temp;
            }
        }

        static void AfficherListe<T>(List<T> liste)
        {
            foreach (var element in liste)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }

        public string toString()
        {
            string desplateau = "";
            for (int i = 0; i < this.longueur; i++)
            {
                for (int j = 0; j < this.hauteur; j++)
                {
                    desplateau += "|" + this.plateau[i, j];
                }
                desplateau += "|\n";
            }
            return desplateau;
        }
        public void ToFile(string nomfile)
        {

        }

        // Classe pour stocker les informations associées à une lettre
        public class LetterInfo
        {
            public int OccMax { get; set; }
            public int Poids { get; set; }

            // Constructeur
            public LetterInfo(int occMax, int poids)
            {
                OccMax = occMax;
                Poids = poids;
            }
        }
    }
}

