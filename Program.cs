namespace motsglisses
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Joueur Joueur1 = new Joueur("Théo");
            
            Joueur1.Add_Mot("Bonjour");
            Joueur1.Add_Mot("Adieu");
            Joueur1.Add_Score(15);
            Console.WriteLine(Joueur1.toString());
            string MotTeste =  "theo";
            Console.WriteLine($"{MotTeste} déja utilisé? {Joueur1.Contient(MotTeste)}");

            //Dictionnaire Dico = new Dictionnaire("C:/Users/theor/source/repos/TheoLeCodeurPro/MOTSGLISSES/Mots_Français.txt");
            Dictionnaire Dico = new Dictionnaire("Mots_Français.txt");
            Console.WriteLine(Dico.toString());
            // Dico.Tri_Fusion();
            //Console.WriteLine(Dico.['A'][0]);
            //Console.WriteLine(Dico.RechDichoRecursif("AFFECTIONNEE"));
            
            //Console.WriteLine("Le programme est en pause. Appuyez sur Entrée pour continuer...");
            //Console.ReadLine();

        }
    }
}