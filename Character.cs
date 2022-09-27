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
      Exp = new uint[] { 0, 30 };
      Lvl = 1;

      switch (cl) {
        case 1: // warrior
          Strength = 4;
          Intelligents = 2;
          Dexterity = 2;
          CritChance = 2.4F;
          CritDmg = 1.25F;
          Health = new short[] { 30, 30 };
          Gold = 29;
          break;
        case 2: // mage
          Strength = 2;
          Intelligents = 4;
          Dexterity = 2;
          CritChance = 4.8F;
          CritDmg = 1.5F;
          Health = new short[] { 22, 22 };
          Gold = 21;
          break;
        case 3: // thief
          Strength = 2;
          Intelligents = 2;
          Dexterity = 4;
          CritChance = 3.2F;
          CritDmg = 1.75F;
          Health = new short[] { 26, 26 };
          Gold = 36;
          break;
        default: break;  // should not get here -> check for wrong input MUST happen before
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

    /// <summary>
    /// de- / increases max HP
    /// </summary>
    /// <param name="health">value with which max HP is increased or not</param>
    public void ChangeMaximumHealth(short health) {
      Health[1] += health;
    }

    /// <summary>
    /// de- / increases HP
    /// </summary>
    /// <param name="health">value with which HP is increased or not</param>
    /// <param name="overheal">allows to heal over max HP</param>
    public void ChangeCurrentHealth(short health, bool overheal = false) {
      Health[0] += health;
      if (Health[0] > Health[1] && !overheal) Health[0] = Health[1];
    }

    /// <summary>
    /// Sets Characters current HP to max
    /// </summary>
    public void FullHeal() {
      ChangeCurrentHealth(Health[1], false);
    }

    /// <summary>
    /// de- / increases amout of Gold
    /// </summary>
    /// <param name="gold">value with which Gold is increased or not</param>
    public void ChangeAmoutOfGold(int gold) {
      Gold += gold;
    }

    /// <summary>
    /// Increases Level and Exp, which is needed for next lvl<br />
    /// Sets current Exp back to 0
    /// </summary>
    public void IncreaseLvl() {
      // if lvl 100 is reached, no more leveling
      if (Lvl >= 100) Exp[1] = 0;
      else if (Exp[0] >= Exp[1]) {
        Console.WriteLine("{0} ist ein Level aufgestiegen.\n{0} ist nun Level {1}.", Name, ++Lvl);
        Console.ReadKey(true);
        Exp[0] = 0;
        Exp[1] += (byte)(20 + Lvl);

        if (Lvl % 10 == 0) Exp[1] += 50;    // increases exp need every 10 lvls a bit more

        IncreaseStats();
      }
    }

    /// <summary>
    /// Increases all stats by one (exept for class stat - increased by 2)<br />
    /// Heal the Character to max HP
    /// </summary>
    private void IncreaseStats() {
      Strength++;
      Intelligents++;
      Dexterity++;
      ChangeMaximumHealth(2);
      FullHeal();
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
