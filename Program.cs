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

      while (name == "") {
        Console.Clear();
        Console.WriteLine("Geben Sie den Namen ihres Charakters ein:");
        name = Console.ReadLine();

        if (name == "") {
          Console.WriteLine("\nDer Name darf nicht leer sein!");
          Thread.Sleep(500);
          continue;
        }

        // convert to char array of the string
        char[] letters = name.ToCharArray();
        // upper case the first char
        letters[0] = char.ToUpper(letters[0]);
        // put array back together
        name = new string(letters);
      }

      do {
        Console.Clear();
        Console.WriteLine("Was ist die Klasse ihres Charakters?\n1) Krieger\n2) Magier\n3) Schurke");
        cl = Convert.ToByte(Console.ReadKey(false).KeyChar - 48);
      } while (cl < 1 || cl > 4);

      return new Character(name, cl);
    }

    /// <summary>
    /// Check and returns if Character is alive
    /// </summary>
    /// <param name="c">character</param>
    /// <returns>true, if yes - false, if not</returns>
    private static bool IsCharacterAlive(Character c) {
      if (c.Health[0] <= 0)
        return false;

      return true;
    }

    /// <summary>
    /// Generates rnd Enemy and let it be fought by the player
    /// </summary>
    /// <param name="c"></param>
    /// <returns>character with new stats</returns>
    private static Character Dungeon(Character c) {
      Random r = new Random();
      byte rnd = 0;

      if (c.Lvl < 5) rnd = Convert.ToByte(r.Next(1, 3)); // only picks the easy enemies
      else if (c.Lvl < 8) rnd = Convert.ToByte(r.Next(1, 4)); // only picks the easy enemies
      else if (c.Lvl < 10) rnd = Convert.ToByte(r.Next(1, 5)); // only picks the easy enemies
      else if (c.Lvl < 14) rnd = Convert.ToByte(r.Next(1, 6)); // only picks the easy enemies
      else {
        rnd = Convert.ToByte(r.Next(1, 101));

        if (rnd <= 20) rnd = 1; // 20 %
        else if (rnd <= 39) rnd = 2; // 19 %
        else if (rnd <= 55) rnd = 3; // 16 %
        else if (rnd <= 67) rnd = 4; // 12 %
        else if (rnd <= 77) rnd = 5; // 10 %
        else if (rnd <= 85) rnd = 6; // 8 %
        else if (rnd <= 91) rnd = 7; // 6 %
        else if (rnd <= 95) rnd = 8; // 4 %
        else if (rnd <= 98) rnd = 9; // 3 %
        else rnd = 10; // 2 %
      }

      Enemy e = new Enemy(c.Lvl, rnd, false); // generate enemy
      Fight f = new Fight(c, e);  // generate fight

      return f.FightIn();
    }

    static void Main(string[] args) {
      char input = '0';
      bool chAlive = false;
      Character character;
      Marketplace marketplace;

      do {
        // create character
        character = CharCreator();
        chAlive = true;
        marketplace = new Marketplace(character);

        do {
          Console.Clear();
          Console.WriteLine("Hauptmenü\n{0}, bei Ihnen liegt die Wahl.", character.Name);
          Console.WriteLine("1) Dungeon\n2) Charakter betrachten\n3) Marktplatz\n8) neuen Charakter erstellen\n9) Spiel beenden");
          input = Console.ReadKey(true).KeyChar;

          switch (input) {
            case '1':
              character = Dungeon(character);
              chAlive = IsCharacterAlive(character);
              break;
            case '2': character.ShowCharacter(); break;
            case '3': character = marketplace.OnMarket(); break;
            case '4': character.Gold += 9999; character.Lvl += 42; break;
            case '8': chAlive = false; break;
            case '9': Environment.Exit(0); break;   // stops appligation -> add button to aggre, before end
            default: break; // nothing happens
          }
        } while (chAlive);  // if char still alive, start new opt

      } while (true);

    }
  }
}
