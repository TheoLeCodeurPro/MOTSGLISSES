using System;
using System.Collections.Generic;
using System.IO;

namespace motsglisses
{
    internal class Dictionnaire
    {
        private Dictionary<char, List<string>> Dico;

        public Dictionnaire(string cheminFichier)
        {
            this.Dico = new Dictionary<char, List<string>>();
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
                    if (!this.Dico.ContainsKey(premiereLettre))
                    {
                        this.Dico[premiereLettre] = new List<string>();
                    }
                    this.Dico[premiereLettre].Add(ligne);
                }
            }
        }

        public string ToString()
        {
            string msg = "FR : ";
            for (char c = 'A'; c <= 'Z'; c++)
            {
                int nBDeMots = this.Dico[c][0].Count(t => t == ' ');
                msg += c + ":" + nBDeMots.ToString() + ",";
            }

            return msg;
        }

        public bool RechercherMot(string mot)
        {
            char premiereLettre = mot[0];
            if (this.Dico.ContainsKey(premiereLettre))
            {
                List<string> mots = this.Dico[premiereLettre];
                int debut = 0;
                int fin = mots.Count - 1;
                return RechRec(mot, debut, fin);
            }
            return false;
        }

        public bool RechRec(string mot, int debut, int fin)
        {
            if (debut <= fin)
            {
                int milieu = (debut + fin) / 2;
                int comparaison = mots[milieu].CompareTo(mot);
                if (comparaison == 0)
                {
                    return true; // Mot trouvé
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
            return false;
        }

        public void Tri_Fusion()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (this.Dico[c].Count <= 1)
                    return;
                else
                {
                    int millieu = this.Dico[c].Count / 2;
                    List<string> listegauche = new List<string>();
                    List<string> listedroite = new List<string>();
                    for (int i = 0; i < millieu; i++)
                    {
                        listegauche.Add(this.Dico[c][i]);
                    }
                    for (int i = millieu; i < this.Dico[c].Count; i++)
                    {
                        listedroite.Add(this.Dico[c]);
                    }
                    Tri_Fusion(listegauche);
                    Tri_Fusion(listedroite);
                    Tri_Fusion2(listegauche, listedroite, liste);
                }
            }

        public void Tri_Fusion2(List<string> listegauche, List<string> listedroite, List<string> listeFinale)
        {
            int indigauche = 0;
            int indidroite = 0;
            int indifusion = 0;

            while (indigauche < listegauche.Count && indidroite < listedroite.Count)
            {
                if (listegauche[indigauche].CompareTo(listedroite[indidroite]) <= 0)
                {
                    listeFinale[indifusion] = listegauche[indigauche];
                    indigauche++;
                }
                else
                {
                    listeFinale[indifusion] = listedroite[indidroite];
                    indidroite++;
                }
                indifusion++;
            }

            while (indigauche < listegauche.Count)
            {
                listeFinale[indifusion] = listegauche[indigauche];
                indigauche++;
                indifusion++;
            }

            while (indidroite < listedroite.Count)
            {
                listeFinale[indifusion] = listedroite[indidroite];
                indidroite++;
                indifusion++;
            }
        }
    }
}