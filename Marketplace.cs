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
        

        // arena
        private void ArenaOverView() {

        }
        
        // increase stats
        private void StatPushOverView() {

        }

    }
}
