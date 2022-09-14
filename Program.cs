using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char input = '0';
            bool chAlive = false;
            //Character character = new Character();

            do {
                // create character

                do {
                    Console.Clear();
                    Console.WriteLine("Hauptmenü\n");
                    Console.WriteLine("1) Dungeon\n2) Charakter betrachten\n8) neuen Charakter erstellen\n9) Spiel beenden");
                    input = Console.ReadKey(true).KeyChar;

                    switch(input) {
                        case '1': break;
                        case '2': break;
                        case '8': chAlive = false; break;
                        case '9': Environment.Exit(0); break;   // stops appligation
                        default: break; // nothing happens
                    }
                } while (chAlive);  // if char still alive, start neu opt

            } while (true);

            Console.WriteLine("\n\n" + input);

            Console.ReadKey(true);  // endl
        }
    }
}
