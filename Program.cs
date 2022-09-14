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
                }
            } while (name == "");

            do {
                Console.Clear();
                Console.WriteLine("Was ist die Klasse ihres Charakters?\n1) Krieger\n2) Magier\n3) Schurke");
                cl = Convert.ToByte(Console.ReadKey(false).KeyChar - 48);
            } while (cl < 0 && cl > 4);

            return new Character(name, cl);
        }

        private static void ShowCharacter(Character c) {
            do {
                Console.WriteLine("");

            } while (Console.ReadKey(false).Key != ConsoleKey.Enter);
        }

        static void Main(string[] args)
        {
            char input = '0';
            bool chAlive = false;
            Character character;

            do {
                // create character
                character = CharCreator();
                chAlive = true;

                do {
                    Console.Clear();
                    Console.WriteLine("Hauptmenü\n{0} bei Ihnen liegt die Wahl.", character.Name);
                    Console.WriteLine("1) Dungeon\n2) Charakter betrachten\n8) neuen Charakter erstellen\n9) Spiel beenden");
                    input = Console.ReadKey(true).KeyChar;

                    switch(input) {
                        case '1': break;
                        case '2': ShowCharacter(character); break;
                        case '8': chAlive = false; break;
                        case '9': Environment.Exit(0); break;   // stops appligation
                        default: break; // nothing happens
                    }
                } while (chAlive);  // if char still alive, start neu opt

            } while (true);

        }
    }
}
