using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motsglisses
{
    internal class Dictionnaire
    {
        private Dictionary<char, List<string>> motsParLettre;
        public Dictionnaire(string cheminFichier)
        {
            this.motsParLettre = new Dictionary<char, List<string>>();
            ChargerDictionnaire(cheminFichier);
        }
        private void ChargerDictionnaire(string cheminFichier)
        {
            using (StreamReader lecteur = new StreamReader(cheminFichier))
            {
                string ligne;
                while ((ligne = lecteur.ReadLine()) != null)
                {
                    char premiereLettre = ligne[0];
                    if (!this.motsParLettre.ContainsKey(premiereLettre))
                    {
                        this.motsParLettre[premiereLettre] = new List<string>();
                    }
                    this.motsParLettre[premiereLettre].Add(ligne);
                }
            }
        }
        public string toString()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                //Console.WriteLine(c);
                Console.WriteLine(c + " : " + this.motsParLettre[c][0]); 
            }
            
            return "";
        }
        
        public bool RechercherMot(string mot)
        {
            char premiereLettre = mot[0];
            if (this.motsParLettre.ContainsKey(premiereLettre))
            {
                List<string> mots = this.motsParLettre[premiereLettre];
                int debut = 0;
                int fin = mots.Count - 1;
                while (debut <= fin)
                {
                    int milieu = (debut + fin) / 2;
                    int comparaison = mots[milieu].CompareTo(mot);
                    if (comparaison == 0)
                    {
                        return true; // Mot trouvé
                    }
                    else if (comparaison < 0)
                    {
                        debut = milieu + 1;
                    }
                    else
                    {
                        fin = milieu - 1;
                    }
                }
            }
            return false; // Mot non trouvé
        }
        public void Tri_XXX()
        {

        }


    }
}
