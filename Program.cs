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

        private static void Dungeon(Character c) {
            Random r = new Random();
            byte rnd = 0;

            if (c.Lvl < 10) rnd = Convert.ToByte(r.Next(1, 5));
            else {
                double rndDouble = r.NextDouble();

                if (rndDouble >= 0.26) rnd = 1;
                else if (rndDouble >= 0.2) rnd = 2;
                else if (rndDouble >= 0.16) rnd = 3;
                else if (rndDouble >= 0.12) rnd = 4;
                else if (rndDouble >= 0.10) rnd = 5;
                else if (rndDouble >= 0.075) rnd = 6;
                else if (rndDouble >= 0.06) rnd = 7;
                else if (rndDouble >= 0.015) rnd = 8;
                else if (rndDouble >= 0.006) rnd = 9;
                else if (rndDouble >= 0.004) rnd = 10;
            }

            Enemy e = new Enemy(c.Lvl, rnd, false);

            Fight();
        }

        private static void Fight() {
            // whos first?

            // player move


            // enemy move

            // check if one if dead

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
                        case '1': Dungeon(character); break;
                        case '2': character.ShowCharacter(); break;
                        case '8': chAlive = false; break;
                        case '9': Environment.Exit(0); break;   // stops appligation
                        default: break; // nothing happens
                    }
                } while (chAlive);  // if char still alive, start neu opt

            } while (true);

        }
    }
}
