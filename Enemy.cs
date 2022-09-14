using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame
{
    internal class Enemy
    {
        // Klassenvariabeln
        private byte MAXENEMYID = 10;

        // Membervariablen

        // Konstruktoren
        public Enemy(byte pLvl, byte? eId = null, bool ishard = false) {
            if(eId == null) {
                Random r = new Random();
                eId = Convert.ToByte(r.Next(1, MAXENEMYID + 1));
                // throw new ArgumentNullException(nameof(eId));
            }

            SetEnemyStats(pLvl, Convert.ToByte(eId), ishard);
        }

        // Methoden

        public byte EnemyId { get; set; }

        public string Name { get; set; }

        public ushort Strength { get; set; }

        public ushort Intelligents { get; set; }

        public ushort Dexterity { get; set; }

        public float CritChance { get; set; }

        public float CritDmg { get; set; }

        public short[] Health { get; set; }

        public uint Gold { get; set; }

        public uint Exp { get; set; }

        private void SetEnemyStats(byte pLvl, byte eId, bool ishard) {
            float multiplier = 1.0F;

            // increase difficulty by increasing stats multiplier
            if (ishard) multiplier += 0.75F;
            while (pLvl - 10 >= 0) {
                multiplier += 0.2F;
                pLvl -= 10;
            }

            switch (eId) {
                // goblin
                case 1:
                    Name = "Golbin";
                    Strength = Convert.ToUInt16(Math.Round(2 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(0 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(1 * multiplier));
                    CritChance = 0.02F * multiplier;
                    CritDmg = MaxMultiplier(1.2F, multiplier);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(10 * multiplier)),
                        Convert.ToInt16(Math.Round(10 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(10 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(5 * multiplier));
                    break;
                // assasin
                case 2:
                    Name = "Assasin";
                    Strength = Convert.ToUInt16(Math.Round(2 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(2 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(4 * multiplier));
                    CritChance = 0.12F * multiplier;
                    CritDmg = MaxMultiplier(1.25F, multiplier);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(23 * multiplier)),
                        Convert.ToInt16(Math.Round(23 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(19 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(9 * multiplier));
                    break;
                // paladin
                case 3:
                    Name = "Paladin";
                    Strength = Convert.ToUInt16(Math.Round(2 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(5 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(3 * multiplier));
                    CritChance = 0.09F * multiplier;
                    CritDmg = MaxMultiplier(1.25F, multiplier);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(37 * multiplier)),
                        Convert.ToInt16(Math.Round(37 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(38 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(19 * multiplier));
                    break;
                // plantara
                case 4:
                    Name = "Plantara";
                    Strength = Convert.ToUInt16(Math.Round(6 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(1 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(2 * multiplier));
                    CritChance = 0.03F * multiplier;
                    CritDmg = MaxMultiplier(1.2F, multiplier);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(37 * multiplier)),
                        Convert.ToInt16(Math.Round(37 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(12 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(30 * multiplier));
                    break;
                // beserker
                case 5:
                    Name = "Beserker";
                    Strength = Convert.ToUInt16(Math.Round(16 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(5 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(6 * multiplier));
                    CritChance = 0.03F * multiplier;
                    CritDmg = MaxMultiplier(1.8F, multiplier, true);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(52 * multiplier)),
                        Convert.ToInt16(Math.Round(52 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(68 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(56 * multiplier));
                    break;
                // wizard
                case 6:
                    Name = "Wizard";
                    Strength = Convert.ToUInt16(Math.Round(3 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(22 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(4 * multiplier));
                    CritChance = 0.23F * multiplier;
                    CritDmg = MaxMultiplier(1.1F, multiplier);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(42 * multiplier)),
                        Convert.ToInt16(Math.Round(42 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(70 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(56 * multiplier));
                    break;
                // grifin
                case 7:
                    Name = "Grifin";
                    Strength = Convert.ToUInt16(Math.Round(16 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(15 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(15 * multiplier));
                    CritChance = 0.10F * multiplier;
                    CritDmg = MaxMultiplier(1.7F, multiplier, true);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(102 * multiplier)),
                        Convert.ToInt16(Math.Round(102 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(36 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(160 * multiplier));
                    break;
                // dragon
                case 8:
                    Name = "Dragon";
                    Strength = Convert.ToUInt16(Math.Round(13 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(9 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(11 * multiplier));
                    CritChance = 0.05F * multiplier;
                    CritDmg = MaxMultiplier(1.5F, multiplier);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(80 * multiplier)),
                        Convert.ToInt16(Math.Round(80 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(160 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(120 * multiplier));
                    break;
                // demon
                case 9:
                    Name = "Demon";
                    Strength = Convert.ToUInt16(Math.Round(15 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(9 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(13 * multiplier));
                    CritChance = 0.06F * multiplier;
                    CritDmg = MaxMultiplier(1.3F, multiplier);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(96 * multiplier)),
                        Convert.ToInt16(Math.Round(96 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(140 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(125 * multiplier));
                    break;
                // ashura
                case 10:
                    Name = "Ashura";
                    Strength = Convert.ToUInt16(Math.Round(17 * multiplier));
                    Intelligents = Convert.ToUInt16(Math.Round(14 * multiplier));
                    Dexterity = Convert.ToUInt16(Math.Round(20 * multiplier));
                    CritChance = 0.33F * multiplier;
                    CritDmg = MaxMultiplier(1.5F, multiplier, true);
                    Health = new short[] {
                        Convert.ToInt16(Math.Round(100 * multiplier)),
                        Convert.ToInt16(Math.Round(100 * multiplier))
                    };
                    Gold = Convert.ToUInt16(Math.Round(150 * multiplier));
                    Exp = Convert.ToUInt16(Math.Round(240 * multiplier));
                    break;
            }
        }
        private float MaxMultiplier(float mutliplicator, float multiplier, bool boss = false) {
            float maxMultiplier = boss ? 3F : 2.5F;
            float result = mutliplicator * multiplier;

            return result > maxMultiplier ? maxMultiplier : result;
        }
    }
}
