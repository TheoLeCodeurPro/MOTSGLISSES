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
        private List<Joueur> joueurs;
        private int tempsParTour; // en secondes
        private int tempsTotal; // en secondes
        static Timer timer;
        static int tempsRestant;
        private int tourActuel;
        private List<int[]> cheminMot;

        public Jeu(int tempsParTour = 120, int tempsTotal = 600)
        {
            this.tempsParTour = tempsParTour;
            this.tempsTotal = tempsTotal;
            this.tourActuel = 1;
            this.joueurs = new List<Joueur>();
        }

        public void AjouterJoueur()
        {
            joueurs.Add(new Joueur("Théo"));
            joueurs.Add(new Joueur("Olivier"));
        }

        public void Jouer(Plateau p)
        {
            this.plateau = p;
            AjouterJoueur();

            while ( tempsTotal > 0) //&& (tourActuel <= joueurs.Count) & && plateau.LettresRestantes())
            {
                Console.WriteLine($"Tour {tourActuel}");
                Joueur joueurCourant = joueurs[(tourActuel - 1) % 2];

                // Afficher l'état du plateau
                Console.WriteLine("Plateau actuel :\n" + plateau.toString());

                // Afficher l'état du joueur
                Console.WriteLine(joueurCourant.toString());

                // Initialiser le timer pour le tour du joueur avec la méthode à appeler et l'intervalle en millisecondes
                timer = new Timer(TimerCallback, null, 0, 1000); // 1000 ms = 1 seconde

                // Initialiser le temps restant
                tempsRestant = 60;

                if (tempsRestant > 0)
                {
                    // Demander au joueur de saisir un mot
                    Console.Write("Saisissez un mot : ");
                    string mot = Console.ReadLine().ToUpper();
                    cheminMot = plateau.Recherche_Mot(mot);
                    // Vérifier si le mot est valide
                    if (!joueurCourant.Contient(mot) && (cheminMot.Count>0))
                    {
                        // Mettre à jour le score du joueur et le plateau
                        // Console.WriteLine("Poid de la lettre A:"+plateau.lettre['A'].Poids);
                        int score = CalculerScore(mot);
                        joueurCourant.Add_Mot(mot);
                        joueurCourant.Add_Score(score);
                        plateau.Maj_Plateau(cheminMot);
                        Console.WriteLine($"Mot valide ! Score du tour : {score}");
                    }
                    else
                    {
                        Console.WriteLine("Mot invalide ou déjà utilisé. Essayez à nouveau.");
                    }

                    // Passer au joueur suivant
                    tourActuel++;
                }
                else
                {
                    Console.WriteLine("Temps écoulé pour ce tour !");
                    tourActuel++;
                }
            }

            // Afficher le résultat final
            AfficherResultatFinal();
        }

        private bool LancerTimer(int duree)
        {
            Console.WriteLine($"Vous avez {duree} secondes pour jouer. Appuyez sur Entrée pour commencer le timer.");
            // Console.ReadLine();

            DateTime debut = DateTime.Now;
            DateTime fin = debut.AddSeconds(duree);

            while (DateTime.Now < fin)
            {
                Thread.Sleep(1000); // Attendre 1 seconde
            }

            return DateTime.Now >= fin; // Retourner vrai si le temps est écoulé
        }

        private static void TimerCallback(object state)
        {
            // Cette méthode est appelée à chaque intervalle du timer (toutes les secondes dans cet exemple)
            tempsRestant--;

            // Console.WriteLine($"Temps restant : {tempsRestant} secondes");

            if (tempsRestant <= 0)
            {
                // Arrêter le timer lorsque le temps est écoulé
                timer.Dispose();
                Console.WriteLine("Le temps est écoulé ! Pressez une touche pour que votre adversaire puisse jouer");
                Console.ReadLine();
            }
        }
 
        private int CalculerScore(string mot)
        {
            // Logique de calcul du score (à adapter selon les règles du jeu)
            int score = 0;
            Console.WriteLine("Poid de la lettre A:" + plateau.lettre['A'].Poids);
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
                Console.WriteLine($"{joueur.toString()} - Score total : {joueur.score}");
            }
        }
    }
}