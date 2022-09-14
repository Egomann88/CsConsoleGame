﻿using System;
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
            } while (cl < 1 && cl > 4);

            return new Character(name, cl);
        }

        private static void ShowCharacter(Character c) {
            string cl = "";
            if (c.Class == 1) cl = "Krieger";
            else if (c.Class == 2) cl = "Magier";
            else cl = "Schurke";

            do {
                Console.Clear();
                Console.WriteLine("Name:\t\t\t{0}", c.Name);
                Console.WriteLine("Klasse:\t\t\t{0}", cl);
                Console.WriteLine("Level:\t\t\t{0}", c.Lvl);
                Console.WriteLine("Exp:\t\t\t{0} / {1}", c.Exp[0], c.Exp[1]);
                Console.WriteLine("Leben:\t\t\t{0} / {1}", c.Health[0], c.Health[1]);
                Console.WriteLine("Gold:\t\t\t{0}", c.Gold);
                Console.WriteLine("Stärke:\t\t\t{0}", c.Strength);
                Console.WriteLine("Inteligents:\t\t{0}", c.Intelligents);
                Console.WriteLine("Geschwindigkeit:\t{0}", c.Dexterity);
                Console.WriteLine("Krit. Chance:\t\t{0} %", c.CritChance);
                Console.WriteLine("Krit. Schaden:\t\t{0} %", (c.CritDmg - 1.0F) * 100);
                Console.WriteLine("\nDrücken Sie <Enter> um zurückzukehren...");
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
                    Console.WriteLine("Hauptmenü\n{0}, bei Ihnen liegt die Wahl.", character.Name);
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
