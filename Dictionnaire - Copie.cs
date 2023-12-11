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
            string msg = "FR : ";
            for (char c = 'A'; c <= 'Z'; c++)
            {
                //Console.WriteLine(c);
                //Console.WriteLine(c + " : " + this.motsParLettre[c]);
                int nBDeMots = this.motsParLettre[c][0].Count(t => t == ' ');
                msg += c + ":" + nBDeMots.ToString() + ",";
            }

            return msg;
        }

        public bool RechercherMot(string mot)
        {
            char premiereLettre = mot[0];
            if (this.motsParLettre.ContainsKey(premiereLettre))
            {
                List<string> mots = this.motsParLettre[premiereLettre];
                int debut = 0;
                int fin = mots.Count - 1;
                return RechRec(mots);
            }
        }
        public bool RechRec(mot, debut = 0, fin = mot.Count - 1)
        {
            int milieu = (debut + fin) / 2;
            int comparaison = mots[milieu].CompareTo(mot);
            if (comparaison == 0)
            {
                return true; // Mot trouvé
            }
            else if (comparaison < 0)
            {
                RechRec(mot, milieu + 1, fin);
            }
            else
            {
                RechRec(mot, debut, milieu - 1);
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
