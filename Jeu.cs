using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
// using System.Timers;

namespace motsglisses
{
    class Jeu
    {
        public Dictionnaire dictionnaire;
        public Plateau plateau;
        private List<Joueur> joueurs; // Liste des 2 joueurs
        static Timer timer;
        static int duree; // duree de la partie
        static int tempsRestant;  // duree du tour
        private int tourActuel; // Numero du tour

        private List<int[]> cheminMot;

        public Jeu(int tempsParTour = 120, int tempsTotal = 600)
        {
            this.tourActuel = 1;
            this.joueurs = new List<Joueur>();
        }

        public void AjouterJoueur()
        {
            string nom = "";
            Console.Write("\nNom du 1er joueur  : ");
            //int positionDebutSaisie = Console.CursorLeft;
            while (nom == "")
                nom = Console.ReadLine();
            joueurs.Add(new Joueur(nom));
            nom = "";
            Console.Write("Nom du 2eme joueur : ");
            while (nom == "")
                nom = Console.ReadLine();
            joueurs.Add(new Joueur(nom));
            Console.WriteLine("");
        }

        public void Jouer(Plateau p)
        {
            this.plateau = p;
            AjouterJoueur();

            // Initialiser le timer pour le tour du joueur et pour la partie avec la méthode à appeler et l'intervalle en millisecondes
            duree = 600;
            Console.WriteLine($"Vous avez {duree} secondes pour jouer. Tapez ENTER pour commencer");
            Console.ReadLine();
            Console.Clear();
            Console.SetCursorPosition(0, 2);
            timer = new Timer(TimerCallback, null, 0, 1000); // 1000 ms = 1 seconde


            while (( duree >= 0) && plateau.LettresRestantes())
            {

                Console.WriteLine($"Tour {tourActuel}");
                Joueur joueurCourant = joueurs[(tourActuel - 1) % 2];

                // Afficher l'état du plateau
                Console.WriteLine("Plateau actuel :\n" + plateau.toString());

                // Afficher l'état du joueur
                Console.WriteLine(joueurCourant.toString());

                // Initialiser le temps restant (60 secondes / tour)
                tempsRestant = 40;
                // Demander au joueur de saisir un mot
                Console.Write("\nSaisissez un mot : ");
                string mot = Console.ReadLine().ToUpper();
                Console.Clear();
                Console.SetCursorPosition(0, 0);

                if (tempsRestant >= 0)
                {
                    cheminMot = plateau.Recherche_Mot(mot);
                    // Vérifier si le mot est valide
                    if (!joueurCourant.Contient(mot) && (cheminMot.Count>0))
                    {
                        // Mettre à jour le score du joueur et le plateau
                        int score = CalculerScore(mot);
                        joueurCourant.Add_Mot(mot);
                        joueurCourant.Add_Score(score);
                        plateau.Maj_Plateau(cheminMot);
                        Console.WriteLine($"Mot valide ! Score du tour : {score}, Nouveau Score de {joueurCourant.nom} : {joueurCourant.score}\n");
                    }
                    else
                    {
                        Console.WriteLine("Mot invalide ou déjà utilisé. Au joueur suivant...\n");
                    }

                    // Passer au joueur suivant
                    tourActuel++;
                }
                else
                {
                    //timer.Dispose();
                    tourActuel++;
                }
            }

            // Afficher le résultat final
            AfficherResultatFinal();
        }
        private static void TimerCallback(object state)
        {
            // Cette méthode est appelée à chaque intervalle du timer (toutes les secondes dans cet exemple)
            tempsRestant--;
            duree--;

            // Obtenir la position du curseur
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.SetCursorPosition(0, 20);
            Console.WriteLine($"Temps restant: Pour le tour = {tempsRestant} secondes, Pour la partie = {duree} secondes");
            Console.SetCursorPosition(left, top);

            if (tempsRestant <= 0)
            // Afficher le message que le temps est écoulé
            {
                if (tempsRestant == 0)
                    Console.WriteLine("Temps écoulé pour ce tour ! Pressez sur ENTER pour continuer");
            }
            if (duree <= 0)
            // Afficher le message que le temps est écoulé
            {
                if (duree == 0)
                    Console.WriteLine("Temps écoulé pour la partie ! Pressez sur ENTER pour continuer");
            }
        }
 
        private int CalculerScore(string mot)
        {
            //  Score = Longueur du mot + somme des poids des lettres que le compose
            int score = mot.Length;
            foreach (char lettre in mot)
            {
                score += plateau.lettre[lettre].Poids;
            }
            return score;
        }

        private void AfficherResultatFinal()
        {
            Console.WriteLine("Partie terminée ! Résultats finaux :");
            foreach (Joueur joueur in joueurs)
            {
                Console.WriteLine($"{joueur.toString()}\n");
            }
            if (joueurs[0].score > joueurs[1].score) Console.WriteLine("Vainqueur : " + joueurs[0].nom);
            else Console.WriteLine("Vainqueur : " + joueurs[1].nom);
        }
    }
}