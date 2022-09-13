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
        private int[] _Health = new int[2];
        private byte _Lvl = 0;
        private uint[] _Exp = new uint[] { };

        // Konstruktoren
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cl">class of player</param>
        public Character(string name, byte cl) {
            Name = name;

            switch (cl)
            {
                case 1: // warrior
                    Class = 1;
                    Strength = 3;
                    Intelligents = 1;
                    Dexterity = 2;
                    CritChance = 2.4F;
                    CritDmg = 1.25F;
                    Health = new Int32[] { 30, 30 };
                    Gold = 52;
                    break;
                case 2: // mage
                    Class = 2;
                    Strength = 1;
                    Intelligents = 3;
                    Dexterity = 2;
                    CritChance = 4.8F;
                    CritDmg = 1.5F;
                    Health = new Int32[] { 16, 16 };
                    Gold = 21;
                    break;
                case 3: // thief
                    Class = 3;
                    Strength = 2;
                    Intelligents = 2;
                    Dexterity = 2;
                    CritChance = 3.2F;
                    CritDmg = 1.75F;
                    Health = new Int32[] { 20, 20 };
                    Gold = 72;
                    break;
                default:
                    if(cl > 3 || cl < 1) Console.WriteLine("Keine gültige Eingabe.");
                    break;
            }
            Exp = new uint[] { 1, 500 };
            Lvl = 1;
            
        }

        // Methoden 

        public string Name { get; set; }

        public byte Class { get; set; }

        public uint Strength { get; set; }

        public uint Intelligents { get; set; }

        public uint Dexterity { get; set; }

        public float CritChance { get; set; }

        public float CritDmg { get; set; }

        public int[] Health { 
            get {
                return _Health;
            }
            set {
                if (value[0] >= _Health[1]) {   // if hp now is higher than max hp
                    _Health[0] = _Health[1];    // hp now = max hp (full heal)
                } else {
                    _Health = value;    // heal (and overwrites max hp on lvl up)
                }
            }
        }

        public uint Gold { get; set; }

        public uint[] Exp {
            get {
                return _Exp;
            } set {
                if(Lvl > 100) _Exp = new uint[] { 0, 0 };

                _Exp = value;
            }
        }

        public byte Lvl {
            get {
                return _Lvl;
            }
            set {
                if (_Lvl > 100) return;
                _Lvl = value;
            }
        }
    }
}
