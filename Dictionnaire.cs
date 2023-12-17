using motsglisses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Dictionnaire
{
    public Dictionary<char, List<string>> dictionnaire;
    public static string relativePath;


    /// <summary>
    /// Permet de créer et de trier le dictionnaire que l'on va utiliser pour la partie
    /// </summary>
    /// <param name="cheminFichier"> Chemin pour ouvrir dictionnaire </param>
    public Dictionnaire(string cheminFichier)
    {
        this.dictionnaire = new Dictionary<char, List<string>>();
        ChargerDictionnaire(cheminFichier);
        this.Tri_Fusion();

    }


    /// <summary>
    /// Permet de charger le fichier comme un dictionnaire
    /// </summary>
    /// <param name="cheminFichier"> Chemin pour ouvrir dictionnaire </param>
    private void ChargerDictionnaire(string cheminFichier)
    {
        string repertoireCourant = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        repertoireCourant = repertoireCourant+ Program.relativePath;

        // Console.WriteLine(repertoireCourant);
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


    /// <summary>
    /// Permet de créer une chaine de caractère de la langue et le nombre de mot par lettre dans le dictionnaire
    /// </summary>
    /// <returns> Retourne la chaine de caractère décrivant le dictionnaire </returns>
    public string toString()
    {
        string msg = "Langue = fr : ";
        for (char c = 'A'; c <= 'Z'; c++)
        {
            msg += c + ":" + this.dictionnaire[c].Count + ",";
        }
        return msg.TrimEnd(','); // Supprime la virgule à la fin de la chaîne
    }


    /// <summary>
    /// Permet de rechercher un mot dans le dictionnaire pour vérifier si il existe
    /// </summary>
    /// <param name="mot"> C'est le mot que l'on essaye de chercher </param>
    /// <returns> Retourne true si le mot existe et retourne false si le mot n'est pas dans le dictionnaire </returns>
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


    /// <summary>
    /// Permet de chercher récursivement si le mot est dans le dictionnaire
    /// </summary>
    /// <param name="mot"> Mot que l'on cherche </param>
    /// <param name="debut"> Indice de l'endroit de début de la recherche </param>
    /// <param name="fin"> Indice de l'endroit où se fini la recherche </param>
    /// <param name="mots"> C'est la liste des mots commençant par la même lettre que le mot que l'on cherche </param>
    /// <returns> Retourne true si le mot est dans le dictionnaire </returns>
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


    /// <summary>
    /// Permet de trier le dictionnaire
    /// </summary>
    public void Tri_Fusion()
    {
        for (char c = 'A'; c <= 'Z'; c++)
        {
            this.dictionnaire[c] = Tri(this.dictionnaire[c]);
        }
    }


    /// <summary>
    /// Tri le dictionnaire grâce au tri fusion
    /// </summary>
    /// <param name="listeMots"></param>
    /// <returns></returns>
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


    /// <summary>
    /// Refusionne et tri deux liste pour n'en faire qu'une
    /// </summary>
    /// <param name="listeGauche"> Première liste à fusionner </param>
    /// <param name="listeDroite"> Deuxième liste à fusionner </param>
    /// <returns> Retourne la liste finale </returns>
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