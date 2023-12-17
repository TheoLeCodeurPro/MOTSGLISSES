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
        public static string relativePath;


        /// <summary>
        /// Constructeur de la classe Plateau
        /// </summary>
        /// <param name="longueur"> Longueur du plateau que l'on est entrain de créer </param>
        /// <param name="hauteur"> Hauteur du plateau </param>
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
                repertoireCourant = repertoireCourant + Program.relativePath +"Lettre.txt";

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
                // Console.WriteLine("Dictionnaire:" + dictionnaire.toString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la lecture du fichier Lettre.txt : {ex.Message}");
            }
        }


        /// <summary>
        /// Mélange du tableau selon l'algorithm de Fisher-Yates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="liste"> C'est la liste que l'on veut mélanger </param>
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


        /// <summary>
        /// Permet d'afficher une liste
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="liste"> C'est la liste que l'on veut afficher </param>
        static void AfficherListe<T>(List<T> liste)
        {
            foreach (var element in liste)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }


        /// <summary>
        /// Permet de retourner une chaîne de caractère décrivant le plateau
        /// </summary>
        /// <returns> La chaîne de caractère du plateau </returns>
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


        /// <summary>
        /// Permet de créer un fichier grâce au plateau de jeu
        /// </summary>
        /// <param name="nomfile"> Le nom du fichier que l'on veut créer </param>
        public void ToFile(string nomfile)
        {
            string repertoireCourant = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            repertoireCourant = repertoireCourant + Program.relativePath + nomfile;

            using (StreamWriter writer = new StreamWriter(repertoireCourant))
            {
                writer.Write(this.longueur + "," + this.hauteur);
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
            // Console.WriteLine("Plateau sauvé dans le fichier " + nomfile + "\n");
        }


        /// <summary>
        /// Permet de créer un plateau grâce a un fichier
        /// </summary>
        /// <param name="nomfile"> Le nom du fichier que l'on veut ouvrir </param>
        /// <returns></returns>
        public bool ToRead(string nomfile)
        {
            string repertoireCourant = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            repertoireCourant = repertoireCourant + Program.relativePath + nomfile;
            try
            {
                using (StreamReader reader = new StreamReader(repertoireCourant))
                {
                    // Lire les dimensions du plateau depuis la première ligne
                    string dimensions = reader.ReadLine();
                    string[] dimensionsArray = dimensions.Split(',');
                    int longueurLu = int.Parse(dimensionsArray[0]);
                    int hauteurLu = int.Parse(dimensionsArray[1]);
                    Console.WriteLine("Longueur:" + longueurLu + " Hauteur:" + hauteurLu);
                    // Console.ReadLine();

                    // Initialiser un nouveau plateau avec les dimensions lues
                    char[,] nouveauPlateau = new char[longueurLu, hauteurLu];

                    // Lire le reste du fichier et remplir le plateau
                    for (int j = (hauteurLu - 1); j >= 0; j--)
                    {
                        string ligne = reader.ReadLine();
                        for (int i = 0; i < longueurLu; i++)
                        {
                            nouveauPlateau[i, j] = char.ToUpper(ligne[i]);
                        }
                    }

                    // Mettre à jour le plateau actuel avec les nouvelles valeurs lues
                    this.plateau = nouveauPlateau;
                    this.longueur = longueurLu;
                    this.hauteur = hauteurLu;
                    // Console.WriteLine("Plateau chargé depuis le fichier " + nomfile + "\n");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la lecture du fichier : {ex.Message}");
                return false;
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


        /// <summary>
        /// Permet chercher si le mot entré existe bien et si il est sur le plateau
        /// </summary>
        /// <param name="mot"> C'est le mot entré par l'utilisateur </param>
        /// <returns> Retourne une liste des coordonnées du chemin qui decrive le mot </returns>
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
                            int[] caseActuelle = new int[] { i, 0 };
                            List<int[]> cheminForward = new List<int[]> { caseActuelle };
                            List<int[]> chemin1 = RechercheMotRecursif(i - 1, 0, mot.Substring(1), cheminForward);
                            if (chemin1.Count > 0) 
                                return cheminForward;
                            List<int[]> chemin2 = RechercheMotRecursif(i - 1, 1, mot.Substring(1), cheminForward);
                            if (chemin2.Count > 0) 
                                return cheminForward;
                            List<int[]> chemin3 = RechercheMotRecursif(i, 1, mot.Substring(1), cheminForward);
                            if (chemin3.Count > 0) 
                                return cheminForward;
                            List<int[]> chemin4 = RechercheMotRecursif(i + 1, 1, mot.Substring(1), cheminForward);
                            if (chemin4.Count > 0) 
                                return cheminForward;
                            List<int[]> chemin5 = RechercheMotRecursif(i + 1, 0, mot.Substring(1), cheminForward);
                            if (chemin5.Count > 0) 
                                return cheminForward;
                            cheminForward = new List<int[]>();
                        }
                    }
                }
            }
            return new List<int[]>();
        }


        /// <summary>
        /// Permet de la recherche récursive des lettres qui compose le mot
        /// </summary>
        /// <param name="col"> Indice de la colonne où se trouve la lettre </param>
        /// <param name="lig"> Indice de la ligne où se trouve la lettre </param>
        /// <param name="mot"> Mot que l'on essaye de trouver sur le plateau </param>
        /// <param name="cheminForward"> Liste des coordonnées du chemin que l'on a fais </param>
        /// <returns></returns>
        private List<int[]> RechercheMotRecursif(int col, int lig, string mot, List<int[]> cheminForward)
        {
            if ((col >= 0) && (lig >= 0) && (col < longueur) && (lig < hauteur)) 
            {
                int[] caseActuelle = new int[] { col, lig };
                if ((plateau[col, lig] == mot[0]) && (!cheminForward.Any(element => Enumerable.SequenceEqual(element, caseActuelle))))
                {
                    cheminForward.Add(caseActuelle);
                    if (mot.Length > 1)
                    {
                        List<int[]> chemin1 = RechercheMotRecursif(col - 1, lig, mot.Substring(1), cheminForward);
                        if (chemin1.Count > 0)
                        { 
                            chemin1.Add(caseActuelle); 
                            return chemin1; 
                        }
                        List<int[]> chemin2 = RechercheMotRecursif(col + 1, lig, mot.Substring(1), cheminForward);
                        if (chemin2.Count > 0)
                        { 
                            chemin2.Add(caseActuelle); 
                            return chemin2; 
                        }
                        List<int[]> chemin3 = RechercheMotRecursif(col, lig - 1, mot.Substring(1), cheminForward);
                        if (chemin3.Count > 0)
                        { 
                            chemin3.Add(caseActuelle); 
                            return chemin3; 
                        }
                        List<int[]> chemin4 = RechercheMotRecursif(col, lig + 1, mot.Substring(1), cheminForward);
                        if (chemin4.Count > 0) 
                        { 
                            chemin4.Add(caseActuelle); 
                            return chemin4; 
                        }
                        List<int[]> chemin5 = RechercheMotRecursif(col - 1, lig + 1, mot.Substring(1), cheminForward);
                        if (chemin5.Count > 0)
                        { 
                            chemin5.Add(caseActuelle); 
                            return chemin5; 
                        }
                        List<int[]> chemin6 = RechercheMotRecursif(col + 1, lig + 1, mot.Substring(1), cheminForward);
                        if (chemin6.Count > 0)
                        { 
                            chemin6.Add(caseActuelle); 
                            return chemin6; 
                        }
                        List<int[]> chemin7 = RechercheMotRecursif(col - 1, lig - 1, mot.Substring(1), cheminForward);
                        if (chemin7.Count > 0) 
                        { 
                            chemin7.Add(caseActuelle); 
                            return chemin7; 
                        }
                        List<int[]> chemin8 = RechercheMotRecursif(col + 1, lig - 1, mot.Substring(1), cheminForward);
                        if (chemin8.Count > 0) 
                        { 
                            chemin8.Add(caseActuelle); 
                            return chemin8; 
                        }
                        cheminForward.RemoveAt(cheminForward.Count - 1);
                    }
                    else
                    {
                        List<int[]> chemin0 = new List<int[]> { caseActuelle };
                        return chemin0;
                    }
                }
            }
            return new List<int[]>();

        }


        /// <summary>
        /// Permet d'afficher le chemin pour faire un mot composé de plusieurs couple de coordonnées
        /// </summary>
        /// <param name="chemin"> Liste des coordonnées nécessaire pour faire un mot </param>
        public void Affiche_Chemin(List<int[]> chemin)
        {
            string message = "";

            if (chemin.Count > 0)
            {
                for (int i = 0; i<= (chemin.Count - 1); i++)
                {
                    message = message + "(" + chemin[i][0] + "," + chemin[i][1] + ") , ";
                }
            Console.WriteLine("Chemin trouvé : "+message+"\n");
            }

        }



        /// <summary>
        /// Permet de mettre à jour le plateau en enlevant les lettres qui se trouve sur le chemin du mot
        /// </summary>
        /// <param name="chemin"> Liste des coordonnées nécessaire pour faire un mot </param>
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
                // Affiche_Chemin(chemin);
            }

        }


        /// <summary>
        /// Permet de vérifier si il reste des lettres sur le plateau
        /// </summary>
        /// <returns> Retourne vrai si il reste des lettres, faux si le plateau est vide </returns>
        public bool LettresRestantes()
        {
            bool lr = false;
            for (int i = 0;i < longueur;i++)
                for (int j = 0; j < hauteur;j++)
                    if (plateau[i, j] !=' ')
                        lr = true;
            return lr;
        }


        /// <summary>
        /// Permet de cloner le tableau
        /// </summary>
        /// <returns> Retourne un clone du tableau de jeu </returns>
        public Plateau Clone()
        {
            return new Plateau(longueur,hauteur)
            {
                longueur = this.longueur,
                hauteur = this.hauteur,
                plateau = (char[,])this.plateau.Clone() // Copie du tableau
            };
        }

    }
}

