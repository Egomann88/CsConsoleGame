using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RpgGame
{
    internal class Program
    {
        /// <summary>
        /// Creates an new Character with Name and Class.<br />
        /// </summary>
        /// <returns>Character</returns>
        private static Character CharCreator() {
            string name = "";
            byte cl = 0;

            do {
                Console.Clear();
                Console.WriteLine("Geben Sie den Namen ihres Charakters ein:");
                name = Console.ReadLine();

                if (name == "") {
                    Console.WriteLine("\nDer Name darf nicht leer sein!");
                    Thread.Sleep(500);
                    continue;
                }

                // convert to char array of the string
                char[] letters = name.ToCharArray();
                // upper case the first char
                letters[0] = char.ToUpper(letters[0]);
                // put array back together
                name = new string(letters);
            } while (name == "");

            do {
                Console.Clear();
                Console.WriteLine("Was ist die Klasse ihres Charakters?\n1) Krieger\n2) Magier\n3) Schurke");
                cl = Convert.ToByte(Console.ReadKey(false).KeyChar - 48);
            } while (cl < 1 && cl > 4);

            return new Character(name, cl);
        }

        /// <summary>
        /// Check and returns if Character is alive
        /// </summary>
        /// <param name="c">character</param>
        /// <returns>true, if yes - false, if not</returns>
        private static bool IsCharacterAlive(Character c) {
            if (c.Health[0] <= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Generates rnd Enemy and let it be fought by the player
        /// </summary>
        /// <param name="c"></param>
        /// <returns>character with new stats</returns>
        private static Character Dungeon(Character c) {
            Random r = new Random();
            byte rnd = 0;

            if (c.Lvl < 5) rnd = Convert.ToByte(r.Next(1, 4)); // only picks the easy enemys
            else if (c.Lvl < 8) rnd = Convert.ToByte(r.Next(1, 5)); // only picks the easy enemys
            else if (c.Lvl < 12) rnd = Convert.ToByte(r.Next(1, 6)); // only picks the easy enemys
            else {
                double rndDouble = r.NextDouble();

                if (rndDouble >= 0.26) rnd = 1; // 26 %
                else if (rndDouble >= 0.2) rnd = 2; // 20 %
                else if (rndDouble >= 0.16) rnd = 3; // 16 %
                else if (rndDouble >= 0.12) rnd = 4; // 12 %
                else if (rndDouble >= 0.10) rnd = 5; // 10 %
                else if (rndDouble >= 0.075) rnd = 6; // 7.5 %
                else if (rndDouble >= 0.06) rnd = 7; // 6 %
                else if (rndDouble >= 0.015) rnd = 8; // 1.5 %
                else if (rndDouble >= 0.006) rnd = 9; // 0.6 %
                else if (rndDouble >= 0.004) rnd = 10; // 0.4 %
            }

            Enemy e = new Enemy(c.Lvl, rnd, false); // generate enemy
            Fight f = new Fight(c, e);  // generate fight

            return f.FightIn();
        }

        static void Main(string[] args) {
            char input = '0';
            bool chAlive = false;
            Character character;

            do {
                // create character
                character = CharCreator();
                chAlive = true;

                do {
                    Console.Clear();
                    Console.WriteLine("Hauptmenü\n{0}, bei Ihnen liegt die Wahl.", character.Name);
                    Console.WriteLine("1) Dungeon\n2) Charakter betrachten\n8) neuen Charakter erstellen\n9) Spiel beenden");
                    input = Console.ReadKey(true).KeyChar;

                    switch(input) {
                        case '1':
                            character = Dungeon(character);
                            chAlive = IsCharacterAlive(character);
                            break;
                        case '2': character.ShowCharacter(); break;
                        case '8': chAlive = false; break;
                        case '9': Environment.Exit(0); break;   // stops appligation
                        default: break; // nothing happens
                    }
                } while (chAlive);  // if char still alive, start new opt

            } while (true);

        }
    }
}
