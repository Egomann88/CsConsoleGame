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
        private byte _Lvl = 0;
        private uint[] _Exp = new uint[] { };

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
                default:
                    Environment.Exit(1);
                    break;
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
