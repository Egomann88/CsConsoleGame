using System;
using System.Linq;
using System.Threading;

namespace RpgGame
{
  internal class FightArena :Fight {
    // Klassenvariabeln

    // Membervariabeln

    // Konstruktor
    public FightArena(Character c, Enemy e) :base(c,e) { }

    // Methoden (funktionen)
    override public Character FightIn() {
      bool fightOver = false;
      bool giveUp = false;
      bool isPlayerFirst = GetFirstMove();
      byte playerTurns = GetNumOfTurns(true);
      byte enemyTurns = GetNumOfTurns(false);

      Console.Clear();
      Console.WriteLine("Ein {0} seit auf der Hut.", Enemy.Name);
      Thread.Sleep(SHORTTIMEOUT);

      do {
        Console.Clear();    // clear all fighting texts

        if (isPlayerFirst) {
          for (byte i = 0; i < playerTurns; i++) {    // repeat as long as Player still has turns
            fightOver = giveUp = PlayerTurn();
            if (Enemy.Health[0] <= 0) {
              fightOver = true;
              break;
            }
          }
          if (fightOver) continue;    // go to end of while-loop -> enemy died
        }

        for (byte i = 0; i < enemyTurns; i++) { // repeat as long as Enemy still has turns
          EnemyTurn();
          if (Character.Health[0] <= 0) {
            Character.Health[0] = 1; // survive with 1 hp
            fightOver = true;
            break;  // break out of for-loop
          }
        }
        if (fightOver) continue;    // go to end of while-loop -> character died

        if (!isPlayerFirst) {
          for (byte i = 0; i < playerTurns; i++) {    // repeat as long as Player still has turns
            fightOver = giveUp = PlayerTurn(); // if player fled, jump direct to end
            if (Enemy.Health[0] <= 0) {
              fightOver = true;
              break;
            }
          }
          if (fightOver) continue;    // go to end of while-loop -> enemy died
        }

        RoundCount++;

      } while (!fightOver);

      Console.Clear();
      if (giveUp) Console.WriteLine("{0} hat aufgegeben!", Character.Name);
      else if (Character.Health[0] == 1) Console.WriteLine("{0} hat verloren.", Character.Name);
      else {  // defeated enemy
        Console.WriteLine("{0} war siegreich!\n{1} Exp erhalten.\n{2} Gold erhalten", Character.Name, Enemy.Exp, Enemy.Gold);
        // get enemy gold and exp
        Character.Exp[0] += Enemy.Exp;
        Character.ChangeAmoutOfGold(Enemy.Gold);

        // player lvl up
        Character.IncreaseLvl();
      }

      Console.WriteLine("\n\nDrücken Sie auf eine Taste, um fortzufahren...");
      Console.ReadKey(true);

      Character.SaveCharacter(Character); // autosave
      return Character;
    }

    override protected bool PlayerTurn() {
      short[] coolDown = GetCoolDown(true);   // cooldown of abilitys
      string ultimateName = GetUltimateName(), actionText = ""; // what player will do
      ushort damage = 0;  // players dmg
      ushort chance2Hit = (ushort)(75 + Character.Dexterity - Enemy.Dexterity); // 75 % base value + char dex - enemy dex (dodge chance)
      char input = '0';   // player input
      bool giveUp = false;

      do {
        Console.Clear();
        Console.WriteLine("{0}, was wollt ihr machen?\nLeben: {1} / {2}",
          Character.Name, Character.Health[0], Character.Health[1]);
        Console.Write("1) Angreifen\n2) Heilen (Abklingzeit: {0} Runden)\n" +
          "3) {1} (Abklingzeit: {2} Runden)\n4) Aufgeben" , coolDown[0], ultimateName, coolDown[1]);
        input = Console.ReadKey(true).KeyChar; // do not show input in console
        Console.Clear();
        switch (input) {
          case '1':
            damage = Character.Strength;

            actionText = $"{Character.Name} greift an.\n";

            if (IsCritDodge(Character.CritChance)) {
              damage = Convert.ToUInt16(Math.Round(damage * Character.CritDmg));
              actionText += "Kritischer Treffer!\n";
              chance2Hit = 100; // Crit is always an hit
            }

            if (!IsCritDodge(chance2Hit)) {
              actionText += $"{Enemy.Name} ist ausgewichen!\n";
              damage = 0;
            } else actionText += $"{damage} Schaden!";


            Enemy.ChangeCurrentHealth(Convert.ToInt16(-damage));
            break;
          case '2':
            // if abilty is still on cooldown, go back to start
            if (IsCharacterOnCoolDown(coolDown[0])) continue;

            damage = Character.Intelligents;

            actionText = $"{Character.Name} heilt sich.\n{damage} Leben wiederhergestellt";

            Character.ChangeCurrentHealth(Convert.ToInt16(damage));

            coolDown[0] = HEALCOOLDOWN;    // set heal cooldown
            break;
          case '3':
            // if abilty is still on cooldown, go back to start
            if (IsCharacterOnCoolDown(coolDown[1])) continue;

            damage = GetCharacterUltimate();

            actionText = $"{Character.Name} nutzt seine Ultimatie Fähigkeit \"{ultimateName}\".\n";

            if (IsCritDodge(Character.CritChance)) {
              damage = Convert.ToUInt16(Math.Round(damage * Character.CritDmg));
              actionText += "Kritischer Treffer!\n";
              chance2Hit = 100; // Crit is always an hit
            }

            if (!IsCritDodge(chance2Hit + ULTHITBONUS)) { // ultimate has extra hit chance
              actionText += $"{Enemy.Name} ist ausgewichen!\n";
              damage = 0;
            } else actionText += $"{damage} Schaden!";

            Enemy.ChangeCurrentHealth(Convert.ToInt16(-damage));
            coolDown[1] = ULTIMATECOOLDOWN;    // set ulti cooldown
            break;
          case '4':
            actionText = $"{Character.Name} gibt auf.\n";
            giveUp = true;
            break;
          default: continue;  // must give new input
        }

        Console.WriteLine(actionText);

        Thread.Sleep(TIMEOUT);

        break;  // break out of loop
      } while (true);

      coolDown = coolDown.Select(x => --x).ToArray();   // decrease cooldowns by one

      CHARACTERCOOLDOWN = coolDown;  // save cooldowns for next round

      return giveUp;
    }
  }
}
