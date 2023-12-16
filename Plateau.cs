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
        public Dictionary<char, LetterInfo> lettre;
        private List<char> listCar;
        public static Dictionnaire dictionnaire;

        public Plateau(int longueur, int hauteur)
        {
            this.longueur = longueur;
            this.hauteur = hauteur;
            this.plateau = new char[longueur, hauteur];
            this.lettre = new Dictionary<char, LetterInfo>();
            this.listCar = new List<char>();


            // Créer une instance de la classe Random
            if ((longueur <= 0) || (hauteur <= 0))
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
                        plateau[i, j] = listCar[i * longueur + j];
                    }
                }
            dictionnaire = new Dictionnaire("Mots_Français.txt");
            Console.WriteLine("Dictionnaire:" + dictionnaire.toString());
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
            for (int j = (this.hauteur - 1); j >= 0; j--)
            {
                for (int i = 0; i < this.longueur; i++)
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
                writer.Write(longueur + "," + hauteur);
                writer.WriteLine();
                for (int j = (this.hauteur - 1); j >= 0; j--)
                {
                    for (int i = 0; i < this.longueur; i++)
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
                for (int j = (hauteurLu - 1); j >= 0; j--)
                {
                    string ligne = reader.ReadLine();
                    for (int i = 0; i < longueurLu; i++)
                    {
                        nouveauPlateau[i, j] = ligne[i];
                    }
                }

                // Mettre à jour le plateau actuel avec les nouvelles valeurs lues
                this.plateau = nouveauPlateau;
                this.longueur = longueurLu;
                this.hauteur = hauteurLu;


                // Console.WriteLine("Plateau chargé depuis le fichier " + nomfile + "\n");
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

        public List<int[]> Recherche_Mot(string mot)
        {
            if (mot.Length > 0)
            {
                if (dictionnaire.RechDichoRecursif(mot))
                {
                    for (int i = 0; i < longueur; i++)
                    {
                        if (this.plateau[i, 0] == mot[0])
                        {
                            List<int[]> chemin1 = RechercheMotRecursif(i - 1, 0, mot.Substring(1));
                            if (chemin1.Count > 0) { chemin1.Add(new int[] { i, 0 }); return chemin1;  }
                            List<int[]> chemin2 = RechercheMotRecursif(i - 1, 1, mot.Substring(1));
                            if (chemin2.Count > 0) { chemin2.Add(new int[] { i, 0 }); return chemin2; }
                            List<int[]> chemin3 = RechercheMotRecursif(i, 1, mot.Substring(1));
                            if (chemin3.Count > 0) { chemin3.Add(new int[] { i, 0 }); return chemin3; }
                            List<int[]> chemin4 = RechercheMotRecursif(i + 1, 1, mot.Substring(1));
                            if (chemin4.Count > 0) { chemin4.Add(new int[] { i , 0 }); return chemin4; }
                            List<int[]> chemin5 = RechercheMotRecursif(i + 1, 0, mot.Substring(1));
                            if (chemin5.Count > 0) { chemin5.Add(new int[] { i , 0 }); return chemin5; }
                        }
                    }

                }
                else 
                    { 
                        Console.WriteLine("Mot n'existe pas");
                    }
                
            }
            return new List<int[]>();
        }



        private List<int[]> RechercheMotRecursif(int col, int lig, string mot)
        {
            if ((col >= 0) && (lig >= 0) && (col < longueur) && (lig < hauteur)) 
            {
                // Console.WriteLine("?2: " + col + "," + lig + "," + plateau[col, lig] + " ?? " + mot[0]+" longueur"+longueur+" hauteur"+hauteur);
                if (plateau[col, lig] == mot[0])
                {
                    // Console.WriteLine("OK: "+ col+","+ lig+","+plateau[col, lig]+ " == "+ mot[0]);
                    if (mot.Length > 1)
                    {
                        List<int[]> chemin1 = RechercheMotRecursif(col - 1, lig, mot.Substring(1));
                        if (chemin1.Count > 0) { chemin1.Add(new int[] { col , lig }); return chemin1; }
                        List<int[]> chemin2 = RechercheMotRecursif(col + 1, lig, mot.Substring(1));
                        if (chemin2.Count > 0) { chemin2.Add(new int[] { col, lig }); return chemin2; }
                        List<int[]> chemin3 = RechercheMotRecursif(col, lig - 1, mot.Substring(1));
                        if (chemin3.Count > 0) { chemin3.Add(new int[] { col, lig }); return chemin3; }
                        List<int[]> chemin4 = RechercheMotRecursif(col, lig + 1, mot.Substring(1));
                        if (chemin4.Count > 0) { chemin4.Add(new int[] { col, lig }); return chemin4; }
                        List<int[]> chemin5 = RechercheMotRecursif(col - 1, lig + 1, mot.Substring(1));
                        if (chemin5.Count > 0) { chemin5.Add(new int[] { col, lig }); return chemin5; }
                        List<int[]> chemin6 = RechercheMotRecursif(col + 1, lig + 1, mot.Substring(1));
                        if (chemin6.Count > 0) { chemin6.Add(new int[] { col, lig }); return chemin6; }
                        List<int[]> chemin7 = RechercheMotRecursif(col - 1, lig - 1, mot.Substring(1));
                        if (chemin7.Count > 0) { chemin7.Add(new int[] { col, lig }); return chemin7; }
                        List<int[]> chemin8 = RechercheMotRecursif(col + 1, lig - 1, mot.Substring(1));
                        if (chemin8.Count > 0) { chemin8.Add(new int[] { col, lig}); return chemin8; }
                    }
                    else
                    {
                        List<int[]> chemin0 = new List<int[]> { new int[] { col, lig } };
                        return chemin0;
                    }
                }
            }
            return new List<int[]>();

        }

        public void Affiche_Chemin(List<int[]> chemin)
        {
            string message = "";

            if (chemin.Count > 0)
            {
                for (int i = (chemin.Count-1); i>=0; i--)
                {
                    message = message + "(" + chemin[i][0] + "," + chemin[i][1] + ") , ";
                }
            Console.WriteLine("Chemin trouvé : "+message+"\n");
            }

        }

            


        public void Maj_Plateau(List<int[]> chemin)
        {

            if (chemin.Count > 0)
            {
                for (int i = (chemin.Count - 1); i >= 0; i--)
                {
                    plateau[chemin[i][0], chemin[i][1]] = ' ';
                }
                for (int i = 0; i < longueur; i++)
                {
                    int j = hauteur -1;
                    while (j >=0)
                    {
                        if (plateau[i, j] == ' ')
                        {
                            if (j < (hauteur - 1))
                            {
                                for (int k = j; k < (hauteur - 1); k++)
                                {
                                    plateau[i, k] = plateau[i, k + 1];
                                }
                                plateau[i, hauteur - 1] = ' ';
                            }
                        }
                        j--;
                    }
                }
                Affiche_Chemin(chemin);
            }

        }
    }
}

