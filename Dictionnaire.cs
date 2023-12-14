using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Dictionnaire
{
    private Dictionary<char, List<string>> dictionnaire;

    public Dictionnaire(string cheminFichier)
    {
        this.dictionnaire = new Dictionary<char, List<string>>();
        ChargerDictionnaire(cheminFichier);
        this.Tri_Fusion();

    }

    private void ChargerDictionnaire(string cheminFichier)
    {
        string repertoireCourant = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        if (repertoireCourant.Contains("Debug"))
        {
            repertoireCourant = repertoireCourant+ "/../../../";
        }
        else
        {
            repertoireCourant = repertoireCourant + "/../../../";
        }
        Console.WriteLine(repertoireCourant);
        using (StreamReader lecteur = new StreamReader(repertoireCourant+cheminFichier))
        {
            string ligne;
            while ((ligne = lecteur.ReadLine()) != null)
            {
                char premiereLettre = ligne[0];
                if (!this.dictionnaire.ContainsKey(premiereLettre))
                {
                    this.dictionnaire[premiereLettre] = new List<string>();
                }
                // Diviser la ligne en mots en utilisant l'espace comme délimiteur
                string[] mots = ligne.Split(' ');

                // Ajouter les mots à la liste associée à la première lettre
                this.dictionnaire[premiereLettre].AddRange(mots);
            }
        }
    }


    public string toString()
    {
        string msg = "Langue = fr : ";
        for (char c = 'A'; c <= 'Z'; c++)
        {
            msg += c + ":" + this.dictionnaire[c].Count + ",";
            /*
            if ((c == 'A') || (c == 'B'))
            {
                foreach (var entry in this.dictionnaire[c])
                {
                    Console.WriteLine(c+" - "+entry);
                }
            }
            */
            
        }

        return msg.TrimEnd(','); // Supprime la virgule à la fin de la chaîne
    }

    public bool RechDichoRecursif(string mot)
    {
        char premiereLettre = mot[0];
        if (this.dictionnaire.ContainsKey(premiereLettre))
        {
            List<string> mots = this.dictionnaire[premiereLettre];
            int debut = 0;
            int fin = mots.Count - 1;
            return RechRec(mot, debut, fin, mots);
        }
        return false;
    }

    private bool RechRec(string mot, int debut, int fin, List<string> mots)
    {
        if (debut <= fin)
        {
            int milieu = (debut + fin) / 2;
            int comparaison = mots[milieu].CompareTo(mot);
            if (comparaison == 0)
            {
                return true;
            }
            else if (comparaison < 0)
            {
                return RechRec(mot, milieu + 1, fin, mots);
            }
            else
            {
                return RechRec(mot, debut, milieu - 1, mots);
            }
        }
        return false;
    }

    public void Tri_Fusion()
    {
        for (char c = 'A'; c <= 'Z'; c++)
        {
            this.dictionnaire[c] = Tri(this.dictionnaire[c]);
        }
    }

    private List<string> Tri(List<string> listeMots)
    {
        if (listeMots.Count <= 1)
            return listeMots;
        else
        {
            int millieu = listeMots.Count / 2;
            List<string> listeGauche = new List<string>(listeMots.GetRange(0, millieu));
            List<string> listeDroite = new List<string>(listeMots.GetRange(millieu, listeMots.Count - millieu));

            List<string> listeGaucheTrie = Tri(listeGauche);
            List<string> listeDroiteTrie = Tri(listeDroite);
            return Fusion(listeGaucheTrie, listeDroiteTrie);
        }
    }

    private List<string> Fusion(List<string> listeGauche, List<string> listeDroite )
    {
        List<string> listeFinale = new List<string>(); 
        int indiGauche = 0;
        int indiDroite = 0;

        while ((indiGauche < listeGauche.Count) && (indiDroite < listeDroite.Count))
        {
            if (listeGauche[indiGauche].CompareTo(listeDroite[indiDroite]) <= 0)
            {
                listeFinale.Add(listeGauche[indiGauche]);
                indiGauche++;
            }
            else
            {
                listeFinale.Add(listeDroite[indiDroite]);
                indiDroite++;
            }
        }

        while (indiGauche < listeGauche.Count)
        {
            listeFinale.Add(listeGauche[indiGauche]);
            indiGauche++;
        }

        while (indiDroite < listeDroite.Count)
        {
            listeFinale.Add(listeDroite[indiDroite]);
            indiDroite++;
        }

        return listeFinale;
    }
}