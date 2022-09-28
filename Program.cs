using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; // for timeout
using System.Text.Json; // has to be installed in nuget Package
using System.IO;  // to create and read files

namespace RpgGame
{
  internal class Program {

    /// <summary>
    /// Saves Json File with current Character Stats<br />
    /// https://www.nuget.org/packages/System.Text.Json<br />
    /// </summary>
    /// <param name="c">current character</param>
    private static void SaveCharacter(Character c) {
      Character _characterData = new Character(c.Name, 0) {
        Name = c.Name,
        Class = c.Class,
        Strength = c.Strength,
        Intelligents = c.Intelligents,
        Dexterity = c.Dexterity,
        CritChance = c.CritChance,
        CritDmg = c.CritDmg,
        Health = c.Health,
        Gold = c.Gold,
        Exp = c.Exp,
        Lvl = c.Lvl,
      };

      string path = Directory.GetCurrentDirectory();  // current Path
      string json = JsonSerializer.Serialize(_characterData);

      try {
        File.WriteAllText(path + @"\character_saves\" + c.Name + @".json", json);
        Console.Clear();
        Console.WriteLine("Speichern erfolgreich.");
      } catch (InvalidCastException e) { }

      Thread.Sleep(600);
    }

    private static void GetCharacters() {
      // https://www.geeksforgeeks.org/c-sharp-program-for-listing-the-files-in-a-directory/
      // https://www.tutorialsteacher.com/articles/convert-json-string-to-object-in-csharp
      string path = Directory.GetCurrentDirectory() + @"\character_saves\";  // current Path
      DirectoryInfo characterSaves = new DirectoryInfo(path);
      Character[] characters;
      FileInfo[] Files = characterSaves.GetFiles();
      foreach (FileInfo i in Files) {
        string jsonCharacterData = File.ReadAllText(path + i);
        var b = JsonSerializer.Deserialize<Character>(jsonCharacterData);
      }

      Console.ReadKey();
    }

    private static void LoadCharacter() {
      string path = Directory.GetCurrentDirectory();

    /// <summary>
    /// Checks if in Character Name is an InValid Sign
    /// </summary>
    /// <param name="input"></param>
    /// <returns>true if wrong sign / false - if all correct</returns>
    public static bool IsInValidSign(string input) {
      Regex regex = new Regex("[\\\\/:\\*\\?\"<>\\|]", RegexOptions.IgnoreCase);
      return regex.IsMatch(input);
    }

    /// <summary>
    /// Creates an new Character with Name and Class.<br />
    /// </summary>
    /// <returns>Character</returns>
    private static Character CreateCharacter() {
      string name = "";
      byte cl = 0;

      while (name == "") {
        Console.Clear();
        Console.WriteLine("Geben Sie den Namen ihres Charakters ein:");
        name = Console.ReadLine();

        if (IsInValidSign(name)) {
          Console.WriteLine("\nIm Namen ist ein unerlaubtes Zeichen enthalten!");
          Thread.Sleep(500);
          continue;
        } else if(name == "" || name == " ") {
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
      if (c.Health[0] <= 0) return false;

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
        character = CreateCharacter();
        chAlive = true;
        marketplace = new Marketplace(character);

        do {
          Console.Clear();
          Console.WriteLine("Hauptmenü\n{0}, bei Ihnen liegt die Wahl.", character.Name);
          Console.WriteLine("1) Dungeon\n2) Charakter betrachten\n3) Marktplatz\n7) Charakter speichern\n" +
            "8) neuen Charakter erstellen\n9) Spiel beenden");
          input = Console.ReadKey(true).KeyChar;

          switch (input) {
            case '1':
              character = Dungeon(character);
              chAlive = IsCharacterAlive(character);
              break;
            case '2': character.ShowCharacter(); break;
            case '3': character = marketplace.OnMarket(); break;
            case '4': character.Gold += 9999; character.Lvl += 42; character.Dexterity += 80; break;
            case '7': SaveCharacter(character); break;
            case '8': chAlive = false; break;
            case '9':
              do {
                Console.WriteLine("Wirklich beenden? [j/n]");
                input = Console.ReadKey(true).KeyChar;
              } while (input != 'j' && input != 'n');

              if (input == 'j') {
                do {
                  Console.WriteLine("Charakter speichern? [j/n]");
                  input = Console.ReadKey(true).KeyChar;
                } while (input != 'j' && input != 'n');

                if(input == 'j') SaveCharacter(character);
                Environment.Exit(0); // stops appligation
              }

              continue;
            default: break; // nothing happens
          }
          SaveCharacter(character);
        } while (chAlive);  // if char still alive, start new opt

      } while (true);

    }
  }
}
