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
        static int dureeTour; // duree du tour
        static int tempsRestant;  // temps restant dans le tour
        private int tourActuel; // Numero du tour
        private List<string> solutions; // Mots possibles
        private bool triche; // Si le joueur à utilisé la commande ?
        private List<int[]> cheminMot;


        /// <summary>
        /// Constructeur de la classe Jeu
        /// </summary>
        public Jeu()
        {
            this.tourActuel = 1;
            this.joueurs = new List<Joueur>();
        }


        /// <summary>
        /// Permet la création des deux joueurs
        /// </summary>
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


        /// <summary>
        /// Permet de lancer la partie 
        /// </summary>
        /// <param name="p"> C'est le plateau sur lequel la partie va se dérouler </param>
        public void Jouer(Plateau p)
        {
            this.plateau = p;
            AjouterJoueur();

            // Initialisation de la durée la partie
            duree = 600;
            Console.WriteLine($"Vous avez {duree} secondes pour jouer. Tapez ENTER pour commencer");
            Console.ReadLine();
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            // Initialiser le timer pour le tour du joueur et pour la partie, avec la méthode à appeler et l'intervalle en millisecondes
            timer = new Timer(TimerCallback, null, 0, 1000); // 1000 ms = 1 seconde
            string mot = "";
            // Initialiser le temps restant (30 secondes / tour)
            dureeTour = 30;
            tempsRestant = dureeTour;
            solutions = plateau.Recherche_Solutions();
            triche = false;

            while (( duree >= 0) && plateau.LettresRestantes() && (mot != "STOPJEU") && (solutions.Count > 0))
            {
                Console.SetCursorPosition(0, 4);
                Console.WriteLine($"Tour {tourActuel}");
                Joueur joueurCourant = joueurs[(tourActuel - 1) % 2];

                // Afficher l'état du plateau
                Console.SetCursorPosition(0, 5);
                Console.WriteLine("Plateau actuel :\n" + plateau.toString());

                // Afficher l'état du joueur
                Console.WriteLine(joueurCourant.toString()+"\n");

                solutions = plateau.Recherche_Solutions();

                // Demander au joueur de saisir un mot
                Console.Write("Saisissez un mot (? pour de l'aide => Score pour le joueur = 0, STOPJEU pour arreter le jeu en cours): ");
                mot = Console.ReadLine().ToUpper();
                Console.Clear();
                Console.SetCursorPosition(0, 0);

                if ((mot != "STOPJEU") && (mot != "?"))
                {
                    if (tempsRestant >= 0)
                    {
                        cheminMot = plateau.Recherche_Mot(mot);
                        // Vérifier si le mot est valide
                        if (!joueurCourant.Contient(mot) && (cheminMot.Count > 0))
                        {
                            // Mettre à jour le score du joueur et le plateau
                            int score = CalculerScore(mot);
                            joueurCourant.Add_Mot(mot);
                            if (!triche)
                                joueurCourant.Add_Score(score);
                            plateau.Maj_Plateau(cheminMot);
                            solutions = plateau.Recherche_Solutions();
                            triche = false;

                            Console.WriteLine($"{mot} est un mot valide ! Score du tour : {score}, Nouveau Score de {joueurCourant.nom} : {joueurCourant.score}\n");
                            // Passer au joueur suivant
                            tourActuel++;
                            tempsRestant = dureeTour;
                        }
                        else
                        {
                            Console.WriteLine("Mot invalide ou déjà utilisé. Veuillez rejouer...\n");
                        }

                    }
                    else
                    {
                        //timer.Dispose();
                        tourActuel++;
                        tempsRestant = dureeTour;
                    }
                }     
                else if (mot == "?")
                {
                    AfficherSolutions(solutions);
                    triche = true;
                }
            }

            // Afficher le résultat final
            AfficherResultatFinal();
        }


        /// <summary>
        /// Permet d'avoir un timer et de l'afficher 
        /// </summary>
        /// <param name="state"></param>
        private static void TimerCallback(object state)
        {
            // Cette méthode est appelée à chaque intervalle du timer (toutes les secondes dans cet exemple)
            tempsRestant--;
            duree--;

            // Obtenir la position du curseur
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.SetCursorPosition(0, Math.Min(40, Console.BufferHeight)-2);
            Console.WriteLine($"Temps restant: Pour le tour = {Math.Max(tempsRestant,0)} secondes, Pour la partie = {Math.Max(duree,0)} secondes");
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
 

        /// <summary>
        /// Calcule le score grâce au poid de la lettre et à la longueur du mot
        /// </summary>
        /// <param name="mot"> Le mot qui à été rentré par le joueur </param>
        /// <returns> Retourne le nouveau score du joueur </returns>
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


        /// <summary>
        /// Permet d'afficher à la fin de la partie le gagnant, le nombre de point de chacun et les mots qu'ils ont trouvés
        /// </summary>
        private void AfficherResultatFinal()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            if (solutions.Count == 0)
                Console.Write("Plus de solutions ! ");
            Console.WriteLine("Partie terminée ! Résultats finaux :");
            foreach (Joueur joueur in joueurs)
            {
                Console.WriteLine($"{joueur.toString()}\n");
            }
            if (joueurs[0].score > joueurs[1].score) Console.WriteLine($"Vainqueur : {joueurs[0].nom}");
            else if (joueurs[1].score > joueurs[0].score) Console.WriteLine($"Vainqueur : {joueurs[1].nom}");
            else Console.WriteLine($"{joueurs[0].nom} et {joueurs[1].nom} sont ex aequo !");
            timer.Dispose();
            Console.ReadLine(); 
        }

        /// <summary>
        /// Permet d'afficher les mots solutions (commande ?)
        /// </summary>
        /// <param name="solutions"> Listes des solutions possibles </param>
        private void AfficherSolutions(List<string> solutions)
        {
            if (solutions.Count == 0)
            {
                Console.WriteLine("\nPas de solutions ! (tapez ENTER pour terminer)");
                Console.ReadLine();
                return;
            }
            else
            {
                string message = "Solutions : ";
                foreach (string solution in solutions)
                {
                    message = message + solution+"("+ CalculerScore(solution) + ") ,";
                }
                Console.WriteLine(message.TrimEnd(','));
            }


        }
    }
}