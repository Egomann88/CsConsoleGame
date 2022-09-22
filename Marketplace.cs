using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RpgGame
{
    internal class Marketplace
    {
        // Klassenvariabeln
        private const byte WEAKHEALERPRICE = 25;
        private const byte NORMALHEALERPRICE = 45;
        private const byte STRONGHEALERPRICE = 80;
        private const int SHORTTIMEOUT = 700;
        private const int TIMEOUT = 1200;
        private const int LONGTIMEOUT = 2000;

        // Membervariabeln

        // Konstruktoren
        public Marketplace(Character c) {
            Character = c;
        }


        // Methoden (funktionen)

        private Character Character { get; set; }

        public Character OnMarket() {
            bool onMarket = true;
            char input = '0';

            while (onMarket) {
                Console.Clear();
                Console.WriteLine("Ihr befindet Euch auf dem Marktplatz.\nWohin wollt Ihr gehen?");
                Console.WriteLine("1) Heiler\n2) Glückspiel\n3) Arena\n4) Verstärkungsmagier\n9) Marktplatz verlassen");
                input = Console.ReadKey().KeyChar;

                switch (input) {
                    case '1': HealerOverView(); break;
                    case '2': GamblingOverView(); break;
                    case '3': ArenaOverView(); break;
                    case '4': StatPushOverView(); break;
                    case '9': onMarket = false; break;
                    default: continue;
                }
            }

            return Character;
        }

        private void HealerOverView() {
            char input = '0';
            float healValue = 0;

            while (true) {
                Console.Clear();
                Console.WriteLine("Es gibt drei Heiler auf dem Markt:");
                Console.WriteLine("1) den Anfänger, er kann 25 % eures Lebens wiederherstellen (Preis: {0})\n" +
                    "2) den Erfaherenen, er kann 45 % eures Lebens wiederherstellen (Preis: {1})\n" +
                    "3) die Meisterin, sie kann eurere Leben komplett wiederherstellen (Preis: {2})\n" +
                    "4) Zurück zum Marktplatz",
                    WEAKHEALERPRICE, NORMALHEALERPRICE, STRONGHEALERPRICE);
                input = Console.ReadKey().KeyChar;

                switch (input) {
                    case '1':
                        if(Character.Gold >= WEAKHEALERPRICE) {
                            healValue = Convert.ToSingle(Math.Round(Character.Health[1] * 0.25));

                            Character.ChangeCurrentHealth((short)healValue);
                            Console.WriteLine("{0} HP wurden wiederhergestellt.", healValue);
                            Character.Gold -= WEAKHEALERPRICE;
                        } else Console.WriteLine("Ihr habt nicht genügend Gold.");

                        break;
                    case '2':
                        if(Character.Gold >= NORMALHEALERPRICE) {
                            healValue = Convert.ToSingle(Math.Round(Character.Health[1] * 0.45));
                            Character.ChangeCurrentHealth((short)healValue);
                            Console.WriteLine("{0} HP wurden wiederhergestellt.", healValue);
                            Character.Gold -= NORMALHEALERPRICE;
                        } else Console.WriteLine("Ihr habt nicht genügend Gold.");

                        break;
                    case '3':
                        if(Character.Lvl < 8) {
                            Console.WriteLine("Die Heilerin lässt euch nicht hinein. Euer Level ist zu tief.");
                            Thread.Sleep(SHORTTIMEOUT);
                            continue;
                        } else if (Character.Gold >= STRONGHEALERPRICE) {
                            Character.FullHeal();
                            Console.WriteLine("Komplettes Leben wurde wiederhergestellt.");
                            Character.Gold -= STRONGHEALERPRICE;
                        } else Console.WriteLine("Ihr habt nicht genügend Gold.");

                        break;
                    case '4': return;
                    default: continue;
                }
                Thread.Sleep(SHORTTIMEOUT);
                return;
            }
        }

        // glücksspiel (rot o. schwarz, höher o. tiefer)
        private void GamblingOverView() {
            char input = '0';

            while (true) {
                Console.Clear();
                Console.WriteLine("Ihr könnt \"Rot oder Schwarz\" oder \"Höher oder Tiefer\" spielen.");
                Console.WriteLine("1) Rot oder Schwarz\n2) Höher oder Tiefer\n3) Zurück zum Marktplatz");
                input = Console.ReadKey().KeyChar;

                switch (input) {
                    case '1': RedBlack(); break;
                    case '2': HighLess(); break;
                    case '3': return;
                    default: continue;
                }
            }
        }

        private void RedBlack() {
            Random r = new Random();
            string[] moods = { "grimmige", "gelangweilte", "fröhliche" };
            string dealerMood = moods[r.Next(1, moods.Length)];
            string dealer = "Der {0} Spielmeister ";
            bool gameIsRed = false;
            bool characterIsRed = false;
            uint stake = 0;
            char input = '0';

            do {
                Console.Clear();
                Console.WriteLine(dealer + "fragt nach eurem Einsatz.", dealerMood);
                Console.Write("Euer Einsatz: ");
            } while (!uint.TryParse(Console.ReadLine(), out stake) && stake <= Character.Gold);

            Console.WriteLine(dealer + "fragt, für das Ihr wettet.");
            while(true) {
                Console.WriteLine("1) Rot\n 2) Schwarz");
                Console.Write("Eure Wahl: ");
                input = Console.ReadKey().KeyChar;

                if (input == '1') characterIsRed = true;
                else if (input == '2') characterIsRed = false;
                else continue;

                break;
            }

            Console.Write("Das Ergebnis ist");
            for(byte i = 0; i < 4; i++) {
                Console.Write(".");
                Thread.Sleep(SHORTTIMEOUT - 400);
            }

            if (r.Next(1, 3) == 1)
            {
                gameIsRed = true;
                Console.WriteLine("\nRot!");
            } else {
                gameIsRed = false;
                Console.WriteLine("\nSchwarz!");
            }
        
            if(characterIsRed == gameIsRed) {
                Console.WriteLine("Ihr habt gewonnen!");
                Character.Gold += Convert.ToInt32(stake);
            } else {
                Console.WriteLine("Ihr habt verloren...");
                Character.Gold -= Convert.ToInt32(stake);
            }

            Thread.Sleep(TIMEOUT);
        }

        private void HighLess() {
            
        }

        // arena
        private void ArenaOverView() {

        }
        
        // increase stats
        private void StatPushOverView() {

        }

    }
}
