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
        Tri_Fusion(this.dictionnaire['A']);
        Tri_Fusion(this.dictionnaire['B']);
        // foreach (var entry in this.dictionnaire)
        // {
        //     Tri_Fusion(entry.Value);
        // }
    }

    private void Tri_Fusion(List<string> liste)
    {
        foreach (string element in liste)
        {
            Console.WriteLine("Passe 2 : "+element);
        }
        if (liste.Count <= 1)
            return;
        else
        {
            int millieu = liste.Count / 2;
            List<string> listegauche = new List<string>(liste.GetRange(0, millieu));
            List<string> listedroite = new List<string>(liste.GetRange(millieu, liste.Count - millieu));

            Tri_Fusion(listegauche);
            Tri_Fusion(listedroite);
            Tri_Fusion2(listegauche, listedroite, liste);
        }
    }

    private void Tri_Fusion2(List<string> listegauche, List<string> listedroite, List<string> listeFinale)
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