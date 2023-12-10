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
        List<string> mots;
        int score;
        public Joueur(string nom)
        {
            this.nom = nom;
            this.mots = new List<string>();
            this.score = 0;
        }
        public void Add_Mot(string mot) 
        {
            mot = mot.ToUpper();
            this.mots.Add(mot);
        }
        public string toString()
        {
            string motss = "";
            if (this.mots.Count > 0) 
            {
                foreach (string m in this.mots)
                {
                    motss += m + ", ";
                }
            }
            
            return "Nom : " + this.nom + "\nMots trouvés : " + motss + "\nScore : " + this.score; 
        }
        public void Add_Score(int valeur)
        {
            this.score += valeur;
        }
        public bool Contient(string mot) 
        {
            mot = mot.ToUpper();
            for (int i = 0; i<this.mots.Count; i++)
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
