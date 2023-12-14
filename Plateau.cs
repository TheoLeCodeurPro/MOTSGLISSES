using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.Metadata;

namespace motsglisses
{
    public class Plateau
    {
        char[,] plateau;
        int longueur;
        int hauteur;
        private Dictionary<char, LetterInfo> lettre;
        private List<char> listCar;
        public Dictionnaire dico;

        public Plateau(int longueur, int hauteur)
        {
            this.longueur = longueur;
            this.hauteur = hauteur;
            this.plateau = new char[longueur, hauteur];
            this.lettre = new Dictionary<char, LetterInfo>();
            this.listCar = new List<char>();


            // Créer une instance de la classe Random
            if ((longueur<=0)||(hauteur<=0))
            {
                Console.WriteLine("Dimensions du plateau incohérente (longueur et hauteur doivent être > 0)");
            }
            try
            {
                string repertoireCourant = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                repertoireCourant = repertoireCourant + "/../../../Lettre.txt";

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
                // AfficherListe(listCar);

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

        // Melange du tableau selon l' algorithme de Fisher-Yates 
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
            string repertoireCourant = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            repertoireCourant = repertoireCourant + "/../../../" + nomfile;

            using (StreamWriter writer = new StreamWriter(repertoireCourant))
            {
                writer.Write(longueur+","+hauteur);
                writer.WriteLine();
                for (int i = 0; i < longueur; i++)
                {
                    for (int j = 0; j < hauteur; j++)
                    {
                        writer.Write(plateau[i, j]);
                    }
                    writer.WriteLine();
                }
            }
            Console.WriteLine("Plateau sauvé dans le fichier " + nomfile + "\n");
        }

        public void ToRead(string nomfile)
        {
            string repertoireCourant = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            repertoireCourant = repertoireCourant + "/../../../" + nomfile;

            using (StreamReader reader = new StreamReader(repertoireCourant))
            {
                // Lire les dimensions du plateau depuis la première ligne
                string dimensions = reader.ReadLine();
                string[] dimensionsArray = dimensions.Split(',');
                int longueurLu = int.Parse(dimensionsArray[0]);
                int hauteurLu = int.Parse(dimensionsArray[1]);

                // Initialiser un nouveau plateau avec les dimensions lues
                char[,] nouveauPlateau = new char[longueurLu, hauteurLu];

                // Lire le reste du fichier et remplir le plateau
                for (int i = 0; i < longueurLu; i++)
                {
                    string ligne = reader.ReadLine();
                    for (int j = 0; j < hauteurLu; j++)
                    {
                        nouveauPlateau[i, j] = ligne[j];
                    }
                }

                // Mettre à jour le plateau actuel avec les nouvelles valeurs lues
                this.plateau = nouveauPlateau;
                this.longueur = longueurLu;
                this.hauteur = hauteurLu;
                longueur = this.longueur;
                hauteur = this.hauteur;

                Console.WriteLine("Plateau chargé depuis le fichier " + nomfile+"\n");
            }
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

        public bool Recherche_Mot(string mot)
        {
            bool found;
            if (mot.Length > 0)
            {
                if (dico.RechDichoRecursif(mot))
                {
                    found = false;
                    for (int i = 0; i < longueur; i++)
                        if (this.plateau[i, hauteur - 1] == mot[0])
                        {
                            bool foundG = this.RechercheMotRecursif(i - 1, hauteur - 1, mot.Substring(1));
                            bool foundD = RechercheMotRecursif(i + 1, hauteur - 1, mot.Substring(1));
                            bool foundH = RechercheMotRecursif(i, hauteur - 2, mot.Substring(1));
                            found = found || foundG || foundD || foundH;
                        }
                    return found;
                }
                else return false;
            }
            else return false;


        }

        public bool RechercheMotRecursif(int col, int lig, string mot)
        {
            bool foundG, foundD, foundH, foundB;
            
            if (this.plateau[col-1,lig] == mot[0])
            {
                if (mot.Length > 1)
                {
                    foundG = false;
                    foundD = false;
                    foundH = false;
                    foundB = false;
                    if (col > 0)
                        foundG = RechercheMotRecursif(col - 1, lig, mot.Substring(1));
                    if (col < (longueur - 1))
                        foundD = RechercheMotRecursif(col + 1, lig, mot.Substring(1));
                    if (lig < (hauteur -1))
                        foundB = RechercheMotRecursif(col, lig + 1, mot.Substring(1));
                    if (lig > 0)
                        foundH = RechercheMotRecursif(col, lig - 1, mot.Substring(1));
                    return (foundG || foundD || foundH || foundB); 
                }
                else return true;

            }
            else return false;
        }
        
        public void Maj_Plateau(object objet)
        {

        }
    }
}

