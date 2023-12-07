using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motsglisses
{
    internal class Joueur
    {
        string nom;
        string[] mots;
        int score;
        public Joueur(string nom)
        {
            this.nom = nom;
            this.mots = new string[0];
            this.score = 0;
        }
        public void Add_Mot(string mot) 
        {
            this.mots[this.mots.Length + 1] = mot;
        }
        public string toString()
        {
            string motss = "";
            foreach (string m in this.mots) 
            {
                motss += ", " + m;
            }
            return "Nom : " + this.nom + "\nMots trouvés : " + motss + "\nScore : " + this.score; 
        }
        public void Add_Score(int valeur)
        {
            this.score += valeur;
        }
        public bool Contient(string mot) 
        {
            for (int i = 0; i<this.mots.Length; i++)
            {
                if (this.mots[i]==mot)
                { 
                    return true; 
                }
            }
            return false;
        }
    }
}
