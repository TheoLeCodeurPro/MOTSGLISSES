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
                return RechRec(mot, debut, fin);
            }
            return false;
        }
        public bool RechRec(string mot, int debut, int fin)
        {
            int milieu = (debut + fin) / 2;
            int comparaison = mot[milieu].CompareTo(mot);
            if (comparaison == 0)
            {
                return true; // Mot trouvÃ©
            }
            else if (comparaison < 0)
            {
                return RechRec(mot, milieu + 1, fin);
            }
            else
            {
                return RechRec(mot, debut, milieu - 1);
            }
        }

        public void Tri_Fusion()
        {
            if (this.motsParLettre.Count <= 1)
                return;
            else
            {
                int millieu = this.motsParLettre.Count / 2;
                List<string> listegauche = new List<string>();
                List<string> listedroite = new List<string>();
                for (int i = 0; i+millieu <= this.motsParLettre.Count; i++) 
                {
                    listegauche.Add(this.motsParLettre[i]);
                    listedroite.Add(this.motsParLettre[i+millieu]);
                }
                Tri_Fusion(listegauche);
                Tri_Fusion(listedroite);
                Tri_Fusion(listegauche, listedroite, this.motsParLettre)
            }
        }
        public void Tri_Fusion(List<string> listegauche, List<string> listedroite, List<string> listeFinal)
        {

        }


    }
}