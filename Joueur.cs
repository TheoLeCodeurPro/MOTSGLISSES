using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motsglisses
{
    internal class Joueur
    {
        public string nom;
        public List<string> mots;
        public int score;


        /// <summary>
        /// Constructeur de la classe Joueur
        /// </summary>
        /// <param name="nom"> Nom du joueur </param>
        public Joueur(string nom)
        {
            this.nom = nom;
            this.mots = new List<string>();
            this.score = 0;
        }


        /// <summary>
        /// Permet d'ajouter un mot à liste des mots trouvés
        /// </summary>
        /// <param name="mot"></param>
        public void Add_Mot(string mot)
        {
            mot = mot.ToUpper();
            this.mots.Add(mot);
        }


        /// <summary>
        /// Permet de créer une chaîne de caractère qui décris un joueur
        /// </summary>
        /// <returns> Retourne une chaîne de caractère </returns>
        public string toString()
        {
            string motss = "";
            if (this.mots.Count > 0)
            {
                foreach (string m in this.mots)
                {
                    motss += m + ", ";
                }
                motss = motss.Substring(0, motss.Length - 2); ;
            }

            return "Nom : " + this.nom + "\nMots déjà trouvés : " + motss + "\nScore : " + this.score;
        }


        /// <summary>
        /// Permet de d'ajouter une valeur au score d'un joueur
        /// </summary>
        /// <param name="valeur"> Score qui doit être ajouté </param>
        public void Add_Score(int valeur)
        {
            this.score += valeur;
        }


        /// <summary>
        /// Permet de savoir si un mot est contenu dans liste 
        /// </summary>
        /// <param name="mot"> Mot qui doit être testé </param>
        /// <returns> Retourne true si le mot est dans la liste et false si il ne l'est pas </returns>
        public bool Contient(string mot)
        {
            mot = mot.ToUpper();
            for (int i = 0; i < this.mots.Count; i++)
            {
                if (this.mots[i] == mot)
                {
                    return true;
                }
            }
            return false;
        }

    }
}