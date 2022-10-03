using System;

namespace RpgGame
{
  public class Program {
    /// <summary>
    /// Check and returns if Character is alive.<br />
    /// If not, character save file will be deleted
    /// </summary>
    /// <param name="c">character</param>
    /// <returns>true, if yes - false, if not</returns>
    private static bool IsCharacterAlive(Character c) {
      if (c.Health[0] <= 0) {
        Character.DeleteCharacer(c.Name);
        return false;
      }

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

      if (c.Lvl < 3) rnd = Convert.ToByte(r.Next(1, 3)); // only picks the easy enemies
      else if (c.Lvl < 5) rnd = Convert.ToByte(r.Next(1, 4)); // only picks the easy enemies
      else if (c.Lvl < 8) rnd = Convert.ToByte(r.Next(1, 5)); // only picks the easy enemies
      else if (c.Lvl < 10) rnd = Convert.ToByte(r.Next(1, 6)); // only picks the easy enemies
      else {
        rnd = Convert.ToByte(r.Next(1, 101));

        if (rnd <= 6) rnd = 1; // 6 %
        else if (rnd <= 12) rnd = 2; // 6 %
        else if (rnd <= 20) rnd = 3; // 8 %
        else if (rnd <= 34) rnd = 4; // 14 %
        else if (rnd <= 50) rnd = 5; // 16 %
        else if (rnd <= 68) rnd = 6; // 18 %
        else if (rnd <= 86) rnd = 7; // 18 %
        else if (rnd <= 91) rnd = 8; // 5 %
        else if (rnd <= 96) rnd = 9; // 5 %
        else rnd = 10; // 4 %
      }

      Enemy e = new Enemy(c.Lvl, rnd, false); // generate enemy
      Fight f = new Fight(c, e);  // generate fight

      return f.FightIn();
    }

    static Character MainMenu() {
      Character instanceCharacter = new Character();  // instans for static method
      string mainMenuText = "1) Charakter erstellen\n";

      if (Character.HasCharacters()) mainMenuText += "2) Charakter laden\n3) Charakter löschen\n";
      while (true) {
        var instans = new Character();
        Console.Clear();
        Console.WriteLine("Hauptmenü\n{0}9) Spiel beenden", mainMenuText);
        char input = Console.ReadKey(true).KeyChar;
        Console.Clear();

        switch (input) {
          case '1': return instanceCharacter.CreateCharacter();
          case '2':
            if (Character.HasCharacters()) return Character.GetCharacters(false);
            else break;
          case '3':
            if (Character.HasCharacters()) Character.GetCharacters(true);
            break;
          case '9': Environment.Exit(-1); break;
        }
      }
    }

    static void Main(string[] args) {
      char input = '0';
      bool chAlive = false;
      Character character;
      Marketplace marketplace;

      do {
        character = MainMenu();

        chAlive = true;
        marketplace = new Marketplace(character);
        
        do {
          Console.Clear();
          Console.WriteLine("{0}, bei Ihnen liegt die Wahl.", character.Name);
          Console.WriteLine("1) Dungeon\n2) Charakter betrachten\n3) Marktplatz\n6) Charakter umbenennen\n" +
            "7) Charakter speichern\n8) Zurück zum Hauptmenü\n9) Spiel beenden");
          input = Console.ReadKey(true).KeyChar;

          switch (input) {
            case '1':
              character = Dungeon(character);
              chAlive = IsCharacterAlive(character);
              break;
            case '2': character.ShowCharacter(); continue;
            case '3': character = marketplace.OnMarket(); break;
            case '4': character.Gold += 9999; character.Lvl += 42; character.Dexterity += 80; break;
            case '6': character.Name = character.ChangeName(); break;
            case '7': Character.SaveCharacter(character); continue;  // call sensitive methods with classname
            case '8': chAlive = false; continue;
            case '9':
              do {
                Console.WriteLine("Charakter speichern? [j/n]");
                input = Console.ReadKey(true).KeyChar;
              } while (input != 'j' && input != 'n');

              if (input == 'j') Character.SaveCharacter(character);  // call sensitive methods with classname
              Environment.Exit(0); // stops appligation

              continue;
            default: break; // nothing happens
          }
          Character.SaveCharacter(character); // auto-save
        } while (chAlive);  // if char still alive, start new opt
      } while (true);
    }
  }
}
