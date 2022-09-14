using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame
{
    internal class Character
    {
        // Klassenvariabeln


        // Membervariablen

        // Konstruktoren
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">playername</param>
        /// <param name="cl">class of player</param>
        public Character(string name, byte cl) {
            Name = name;
            Class = cl;
            Exp = new uint[] { 0, 500 };
            Lvl = 1;

            switch (cl) {
                case 1: // warrior
                    Strength = 3;
                    Intelligents = 1;
                    Dexterity = 2;
                    CritChance = 2.4F;
                    CritDmg = 1.25F;
                    Health = new short[] { 30, 30 };
                    Gold = 52;
                    break;
                case 2: // mage
                    Strength = 1;
                    Intelligents = 3;
                    Dexterity = 2;
                    CritChance = 4.8F;
                    CritDmg = 1.5F;
                    Health = new short[] { 16, 16 };
                    Gold = 21;
                    break;
                case 3: // thief
                    Strength = 2;
                    Intelligents = 2;
                    Dexterity = 2;
                    CritChance = 3.2F;
                    CritDmg = 1.75F;
                    Health = new short[] { 20, 20 };
                    Gold = 72;
                    break;
                default: Environment.Exit(1); break;
            }
        }

        // Methoden 

        public string Name { get; set; }

        public byte Class { get; set; }

        public ushort Strength { get; set; }

        public ushort Intelligents { get; set; }

        public ushort Dexterity { get; set; }

        public float CritChance { get; set; }

        public float CritDmg { get; set; }

        public short[] Health { get; set; }

        public int Gold { get; set; }

        public uint[] Exp { get; set; }

        public byte Lvl { get; set; }

        public void ShowCharacter() {
            string cl = "";
            if (Class == 1) cl = "Krieger";
            else if (Class == 2) cl = "Magier";
            else cl = "Schurke";

            do {
                Console.Clear();
                Console.WriteLine("Name:\t\t\t{0}", Name);
                Console.WriteLine("Klasse:\t\t\t{0}", cl);
                Console.WriteLine("Level:\t\t\t{0}", Lvl);
                Console.WriteLine("Exp:\t\t\t{0} / {1}", Exp[0], Exp[1]);
                Console.WriteLine("Leben:\t\t\t{0} / {1}", Health[0], Health[1]);
                Console.WriteLine("Gold:\t\t\t{0}", Gold);
                Console.WriteLine("Stärke:\t\t\t{0}", Strength);
                Console.WriteLine("Inteligents:\t\t{0}", Intelligents);
                Console.WriteLine("Geschwindigkeit:\t{0}", Dexterity);
                Console.WriteLine("Krit. Chance:\t\t{0} %", CritChance);
                Console.WriteLine("Krit. Schaden:\t\t{0} %", (CritDmg - 1.0F) * 100);
                Console.WriteLine("\nDrücken Sie <Enter> um zurückzukehren...");
            } while (Console.ReadKey(false).Key != ConsoleKey.Enter);
        }

        public void ChangeMaximumHealth(short health) {
            Health[1] += health;
            ChangeCurrentHealth(health, false);
        }

        public bool ChangeCurrentHealth(short health, bool overheal = false) {
            Health[0] += health;
            if (Health[0] > Health[1] && !overheal) Health[0] = Health[1];

            if (Health[0] <= 0) return false;
            else return true;
        }
        
        public void IncreaseLvl() {
            if(Lvl >= 100) {    // if lvl 100 is reached, no more leveling
                Exp[1] = 0;
                return;
            } else if (Exp[0] >= Exp[1]) {
                Lvl++;
                Exp[0] = 0;
                Exp[1] += 500;

                IncreaseStats();

                if (Lvl % 10 == 0) Exp[1] += 500;    // increases exp need all 10 lvls by 1000
            }
        }

        private void IncreaseStats() {
            Strength++;
            Intelligents++;
            Dexterity++;
            ChangeMaximumHealth(2);
            if (Lvl % 10 == 0) {
                CritChance += 0.2F;
                CritDmg += 0.5F;
            }

            switch (Class) {
                case 1: Strength++; break;
                case 2: Intelligents++; break;
                case 3: Dexterity++; break;
            }
        }
    }
}
