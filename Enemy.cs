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

    // Membervariablen

    // Konstruktoren
    public Enemy(byte pLvl, byte eId, bool ishard = false) {
      SetEnemyStats(pLvl, eId, ishard);
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

    public int Gold { get; set; }

    public uint Exp { get; set; }

    public bool IsDmgUlt { get; set; }

    public void ChangeCurrentHealth(short health, bool overheal = false) {
      Health[0] += health;
      if (Health[0] > Health[1] && !overheal) Health[0] = Health[1];
    }

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
          Intelligents = Convert.ToUInt16(Math.Round(0.4 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(1 * multiplier));
          CritChance = 0.02F * multiplier;
          CritDmg = MaxMultiplier(1.2F, multiplier);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(10 * multiplier)),
                        Convert.ToInt16(Math.Round(10 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(10 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(8 * multiplier));
          IsDmgUlt = true;
          break;
        // assasin
        case 2:
          Name = "Assasine";
          Strength = Convert.ToUInt16(Math.Round(2 * multiplier));
          Intelligents = Convert.ToUInt16(Math.Round(2 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(4 * multiplier));
          CritChance = 0.12F * multiplier;
          CritDmg = MaxMultiplier(1.25F, multiplier);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(19 * multiplier)),
                        Convert.ToInt16(Math.Round(19 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(19 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(14 * multiplier));
          IsDmgUlt = true;
          break;
        // paladin
        case 3:
          Name = "Paladin";
          Strength = Convert.ToUInt16(Math.Round(2 * multiplier));
          Intelligents = Convert.ToUInt16(Math.Round(4 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(3 * multiplier));
          CritChance = 0.09F * multiplier;
          CritDmg = MaxMultiplier(1.25F, multiplier);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(29 * multiplier)),
                        Convert.ToInt16(Math.Round(29 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(38 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(26 * multiplier));
          IsDmgUlt = false;
          break;
        // plantara
        case 4:
          Name = "Plantara";
          Strength = Convert.ToUInt16(Math.Round(5 * multiplier));
          Intelligents = Convert.ToUInt16(Math.Round(1 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(2 * multiplier));
          CritChance = 0.03F * multiplier;
          CritDmg = MaxMultiplier(1.2F, multiplier);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(33 * multiplier)),
                        Convert.ToInt16(Math.Round(33 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(12 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(37 * multiplier));
          IsDmgUlt = false;
          break;
        // beserker
        case 5:
          Name = "Beserker";
          Strength = Convert.ToUInt16(Math.Round(14 * multiplier));
          Intelligents = Convert.ToUInt16(Math.Round(5 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(6 * multiplier));
          CritChance = 0.03F * multiplier;
          CritDmg = MaxMultiplier(1.8F, multiplier, true);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(48 * multiplier)),
                        Convert.ToInt16(Math.Round(48 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(68 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(72 * multiplier));
          IsDmgUlt = true;
          break;
        // wizard
        case 6:
          Name = "Wizard";
          Strength = Convert.ToUInt16(Math.Round(3 * multiplier));
          Intelligents = Convert.ToUInt16(Math.Round(20 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(4 * multiplier));
          CritChance = 0.23F * multiplier;
          CritDmg = MaxMultiplier(1.1F, multiplier);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(39 * multiplier)),
                        Convert.ToInt16(Math.Round(39 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(70 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(80 * multiplier));
          IsDmgUlt = false;
          break;
        // grifin
        case 7:
          Name = "Grifin";
          Strength = Convert.ToUInt16(Math.Round(15 * multiplier));
          Intelligents = Convert.ToUInt16(Math.Round(14 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(14 * multiplier));
          CritChance = 0.10F * multiplier;
          CritDmg = MaxMultiplier(1.7F, multiplier, true);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(98 * multiplier)),
                        Convert.ToInt16(Math.Round(98 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(36 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(190 * multiplier));
          IsDmgUlt = false;
          break;
        // dragon
        case 8:
          Name = "Dragon";
          Strength = Convert.ToUInt16(Math.Round(12 * multiplier));
          Intelligents = Convert.ToUInt16(Math.Round(9 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(10 * multiplier));
          CritChance = 0.05F * multiplier;
          CritDmg = MaxMultiplier(1.5F, multiplier);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(78 * multiplier)),
                        Convert.ToInt16(Math.Round(78 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(160 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(150 * multiplier));
          IsDmgUlt = true;
          break;
        // demon
        case 9:
          Name = "Demon";
          Strength = Convert.ToUInt16(Math.Round(14 * multiplier));
          Intelligents = Convert.ToUInt16(Math.Round(9 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(12 * multiplier));
          CritChance = 0.06F * multiplier;
          CritDmg = MaxMultiplier(1.3F, multiplier);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(86 * multiplier)),
                        Convert.ToInt16(Math.Round(86 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(140 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(180 * multiplier));
          IsDmgUlt = true;
          break;
        // ashura
        case 10:
          Name = "Ashura";
          Strength = Convert.ToUInt16(Math.Round(16 * multiplier));
          Intelligents = Convert.ToUInt16(Math.Round(14 * multiplier));
          Dexterity = Convert.ToUInt16(Math.Round(18 * multiplier));
          CritChance = 0.33F * multiplier;
          CritDmg = MaxMultiplier(1.5F, multiplier, true);
          Health = new short[] {
                        Convert.ToInt16(Math.Round(92 * multiplier)),
                        Convert.ToInt16(Math.Round(92 * multiplier))
                    };
          Gold = Convert.ToInt32(Math.Round(150 * multiplier));
          Exp = Convert.ToUInt16(Math.Round(240 * multiplier));
          IsDmgUlt = true;
          break;
      }
    }
    private float MaxMultiplier(float mutliplicator, float multiplier, bool boss = false) {
      float maxMultiplier = boss ? 3F : 2.5F; // if enemy is strong, use bigger multiplier for max crit
      float result = mutliplicator * multiplier;

      return result > maxMultiplier ? maxMultiplier : result;
    }
  }
}
