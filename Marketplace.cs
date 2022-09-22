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
            Random r = new Random();
            char input = '0';
            bool playerIsHigher = false, gameIsHigher = false;
            int lastGameNumber = 0, gameNumber = 0, stake = 0;
            const int stdPlyValue = 20; // standard Play Value

            if(Character.Gold < stdPlyValue) {
                Console.WriteLine("Ihr besitzt nicht genügend Geld, um zu spielen.");
                Thread.Sleep(SHORTTIMEOUT);
            }

            Console.WriteLine("Eine Zahl zwischen 1 und 10 wird gewürfelt." +
                "Ihr müsst sagen, ob die nächste Zahl höher oder tiefer, als die jetzige sein wird.\n" +
                "Der erste Einsatz ist 20 Gold, dieser wird jede Runde verdoppelt. Das Spiel geht max. 5 Runden.");

            while (true) {
                int i = 1;
                stake = stdPlyValue;
                lastGameNumber = r.Next(1, 11);

                Console.WriteLine("Die {0}. Zahl ist eine {1}", i, lastGameNumber);

                while (true) {
                    Console.WriteLine("1) Höher\n2) Tiefer\n3) Gewinn nehmen & gehen");
                    input = Console.ReadKey().KeyChar;

                    if (input == '1') { playerIsHigher = true; break; }
                    else if (input == '2') { playerIsHigher = false; break; }
                    else if (input == '3') { PayPlayer(true, stake); return; }    // end func with payout
                    else continue;
                }

                stake += 20;
                if (stake >= Character.Gold) {
                    Console.WriteLine("Wenn Ihr verliert, könnt ihr nicht bezahlen.\nDie Runde wird beendet.");
                    PayPlayer(true, stake - 20);
                    Thread.Sleep(SHORTTIMEOUT);
                    return;
                }

                gameNumber = r.Next(1, 11);

                Console.WriteLine("Die {0}. Zahl ist eine {1}", i+1, gameNumber);
                if (gameNumber > lastGameNumber) gameIsHigher = true;
                else gameIsHigher = false;

                if(playerIsHigher == gameIsHigher) {
                    Console.WriteLine("Ihr hattet recht!");
                    PayPlayer(true, stake);
                } else {
                    Console.WriteLine("Ihr lagt falsch...");
                    PayPlayer(true, -stake);
                }
                break;
            }
        }

        private void PayPlayer(bool pWon, int stake) {
            string wonMsg = pWon ? "erhaltet" : "bezahlt";
            Console.WriteLine("Ihr {0} {1} Gold.", wonMsg, stake);
            Character.Gold += stake;
            Thread.Sleep(TIMEOUT);
        }

        // arena
        private void ArenaOverView() {
            char input = '0';

            while(true) {
                Console.Clear();
                Console.WriteLine("In der Arena werdet ihr ausschliesslich starke Gegner treffen und wie der Zufall es will, " +
                    "habt ihr die Möglichkeit gegen besonders starke Gegner zu kämpfen, mit höheren Belohnungen natürlich\n" +
                    "In der Arena gelten nicht dieselben Regeln wie in der Wildnis. Hier könnt ihr nicht sterben.");
                Console.WriteLine("1) Normaler Arenakampf\n2) Kampf gegen starken Gegner\n3) Zurück zum Marktplatz.");
                input = Console.ReadKey().KeyChar;

                switch (input) {
                    case '1': EvalEnemy(false); break;
                    case '2': EvalEnemy(true); break;
                    case '3': return;
                    default: continue;
                }
            }
        }

        private void EvalEnemy(bool isHard) {
            Random r = new Random();
            byte enemyId = Convert.ToByte(r.Next(1, 101));

            if(enemyId <= 21) enemyId = 5; // 21 %
            else if (enemyId <= 42) enemyId = 6; // 21 %
            else if (enemyId <= 62) enemyId = 7; // 20 %
            else if (enemyId <= 80) enemyId = 8; // 18 %
            else if (enemyId <= 96) enemyId = 9; // 14 %
            else enemyId = 10; // 6 %

            Enemy e = new Enemy(Character.Lvl, enemyId, isHard);

            // how to start new fight?
            // new class, with inherits from fight?
        }

        // increase stats
        private void StatPushOverView() {

        }

    }
}
