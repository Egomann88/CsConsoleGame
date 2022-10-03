using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading; // timeout
using System.Text.Json; // has to be installed in nuget Package
using System.IO;  // to create and read files
using System.Text.RegularExpressions; // regex

namespace RpgGame
{
  internal class Character {
    // Klassenvariabeln
    const ushort MAXSTATSVALUE = 1000;
    const float MAXCRITCHANCE = 100;
    const float MAXCRITDAMAGE = 3;
    const short MAXHEALTH = 600;
    const int MAXGOLD = 100000;
    const byte MAXLVL = 100;

    // Membervariablen
    ushort _Strength = 0;
    ushort _Intelligents = 0;
    ushort _Dexterity = 0;
    float _CritChance = 0;
    float _CritDmg = 0;
    short[] _Health = new short[2];
    int _Gold = 0;
    uint[] _Exp = new uint[2];
    byte _Lvl = 0;


    // Konstruktoren
    public Character() {
      CreateDirectory();  // create saves directory, if not exsists
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">playername</param>
    /// <param name="cl">class of player</param>
    public Character(string name, byte cl) {
      CreateDirectory();  // create saves directory, if not exsists

      Name = name;
      Class = cl;
      Exp = new uint[2] { 0, 30 };
      Lvl = 1;

      switch (cl) {
        case 1: // warrior
          Strength = 4;
          Intelligents = 2;
          Dexterity = 2;
          CritChance = 2.4F;
          CritDmg = 1.25F;
          Health = new short[2] { 30, 30 };
          Gold = 29;
          break;
        case 2: // mage
          Strength = 2;
          Intelligents = 4;
          Dexterity = 2;
          CritChance = 4.8F;
          CritDmg = 1.5F;
          Health = new short[2] { 22, 22 };
          Gold = 21;
          break;
        case 3: // thief
          Strength = 2;
          Intelligents = 2;
          Dexterity = 4;
          CritChance = 3.2F;
          CritDmg = 1.75F;
          Health = new short[2] { 26, 26 };
          Gold = 36;
          break;
        default: break;  // should not get here -> check for wrong input MUST happen before
      }
    }

    // Methoden 

    public string Name { get; set; }

    public byte Class { get; set; }

    public ushort Strength {
      get {
        return _Strength;
      } set {
        if (_Strength + value >= MAXSTATSVALUE) _Strength = MAXSTATSVALUE;
        else _Strength = value;
      }
    }

    public ushort Intelligents {
      get {
        return _Intelligents;
      } set {
        if (_Intelligents + value >= MAXSTATSVALUE) _Intelligents = MAXSTATSVALUE;
        else _Intelligents = value;
      }
    }

    public ushort Dexterity {
      get {
        return _Dexterity;
      } set {
        if (_Dexterity + value >= MAXSTATSVALUE) _Dexterity = MAXSTATSVALUE;
        else _Dexterity = value;
      }
    }

    public float CritChance {
      get {
        return _CritChance;
      } set {
        if (_CritChance + value >= MAXCRITCHANCE) _CritChance = MAXCRITCHANCE;
        else _CritChance = value;
      }
    }

    public float CritDmg {
      get {
        return _CritDmg;
      } set {
        if (_CritDmg + value >= MAXCRITDAMAGE) _CritDmg = MAXCRITDAMAGE;
        else _CritDmg = value;
      }
    }

    public short[] Health {
      get {
        return _Health;
      } set {
        for(byte i = 0; i < _Health.Length; i++) {
          if (_Health[i] + value[i] >= MAXHEALTH) _Health[i] = MAXHEALTH;
          else _Health = value;
        }
      }
    }

    public int Gold {
      get {
        return _Gold;
      } set {
        if (_Gold + value >= MAXGOLD) _Gold = MAXGOLD;
        else _Gold = value;
      }
    }

    public uint[] Exp { 
      get {
        return _Exp;
      }
      set {
        if(Lvl == 100) _Exp = new uint[2] { 0, 0 };
        _Exp = value;
      }
    }

    public byte Lvl {
      get {
        return _Lvl;
      } set {
        if(_Lvl >= MAXLVL) {
          _Lvl = MAXLVL;
          Exp = Exp.Select(x => x = 0).ToArray();
        } else _Lvl = value;
      }
    }

    /// <summary>
    /// Creates an new Character with Name and Class.<br />
    /// </summary>
    /// <returns>Character</returns>
    public Character CreateCharacter() {
      string name = "";
      byte cl = 0;

      name = ChangeName();

      do {
        Console.Clear();
        Console.WriteLine("Was ist die Klasse ihres Charakters?\n1) Krieger\n2) Magier\n3) Schurke");
        cl = Convert.ToByte(Console.ReadKey(false).KeyChar - 48);
      } while (cl < 1 || cl > 4);

      return new Character(name, cl);
    }

    /// <summary>
    /// Saves Json File with current Character Stats<br />
    /// https://www.nuget.org/packages/System.Text.Json<br />
    /// </summary>
    /// <param name="c">current character</param>
    public static void SaveCharacter(Character c) {
      string path = Directory.GetCurrentDirectory() + @"\character_saves\";  // current Path
      string json = JsonSerializer.Serialize(c);

      File.WriteAllText(path + c.Name + @".json", json);
      Console.Clear();

      Thread.Sleep(600);
    }

    public static void DeleteCharacer(string name) {
      string path = Directory.GetCurrentDirectory() + @"\character_saves\";  // current Path
      File.Delete(path + name + ".json");
    }

    public static bool HasCharacters() {
      // https://www.geeksforgeeks.org/c-sharp-program-for-listing-the-files-in-a-directory/
      string path = Directory.GetCurrentDirectory() + @"\character_saves\";  // current Path
      DirectoryInfo characterSaves = new DirectoryInfo(path);
      FileInfo[] Files = characterSaves.GetFiles();

      // https://stackoverflow.com/questions/24518299/if-file-directory-is-empty-c-sharp
      if (Files.Length == 0) return false;
      return true;
    }

    public static List<Character> CharacterList() {
      // https://www.geeksforgeeks.org/c-sharp-program-for-listing-the-files-in-a-directory/
      string path = Directory.GetCurrentDirectory() + @"\character_saves\";  // current Path
      DirectoryInfo characterSaves = new DirectoryInfo(path);
      FileInfo[] Files = characterSaves.GetFiles();
      List<Character> charactersList = new List<Character>();
      

      // fill character list
      // https://www.tutorialsteacher.com/articles/convert-json-string-to-object-in-csharp
      foreach (FileInfo i in Files) {
        string jsonCharacterData = File.ReadAllText(path + i);
        charactersList.Add(JsonSerializer.Deserialize<Character>(jsonCharacterData));
      }

      return charactersList;
    }

    public static Character GetCharacters(bool delete) {
      List<Character> charactersList = CharacterList();
      byte choosenCharacterId = 0;

      // list all characters
      Character[] characters = charactersList.ToArray();  // convert list to array
      ListCharacters(characters); // lists all characters

      do {
        choosenCharacterId = ChooseCharacter(); // player input
      } while (choosenCharacterId > characters.Length || choosenCharacterId <= 0); // player can give much higher input than save are

      choosenCharacterId--; // decrease id by one to be sync with the array

      if (delete) {
        DeleteCharacer(characters[choosenCharacterId].Name);
        return new Character(); // useless, only for return value
      } else {
        return Prepare2Load(choosenCharacterId, characters);
      }
    }

    private static void ListCharacters(Character[] characters) {
      Console.WriteLine("Wählen Sie einen Charakter aus:");

      for (byte i = 0; i < characters.Length; i++) {
        if (i == 255) break;
        Console.WriteLine("{0}) {1}, {2} (Level: {3})",
          i + 1, characters[i].Name, characters[i].GetClassName(), characters[i].Lvl
        );
      }
    }

    private static byte ChooseCharacter() {
      byte input = 0;

      do {
        Console.Write("Eingabe: ");
      } while (!byte.TryParse(Console.ReadLine(), out input));

      return input;
    }

    /// <summary>
    /// checks if charactersave if correct and can be loaded
    /// </summary>
    /// <param name="character">Character which should be loaded</param>
    /// <returns></returns>
    private static bool CanLoadCharacter(Character character) {
      if (IsCharacterValid(character)) return true;

      return false;
    }

    /// <summary>
    /// Checks if the loaded save wasnt modified.<br />
    /// If it was modified, it cannot be loaded
    /// </summary>
    /// <param name="c">characer object</param>
    /// <returns>true - if character is valid / flase - if not</returns>
    private static bool IsCharacterValid(Character c) {
      bool nameVaild = false, classValid = false;

      if (c.Name == "" || !IsInValidSign(c.Name)) nameVaild = true;
      if (c.Class > 0 && c.Class < 4) classValid = true;
      c.Strength = c.Strength;
      c.Intelligents = c.Intelligents;
      c.Dexterity = c.Dexterity;
      c.CritChance = c.CritChance;
      c.CritDmg = c.CritDmg;
      c.Health = c.Health;
      c.Gold = c.Gold;
      c.Exp = c.Exp;
      c.Lvl = c.Lvl;

      if (nameVaild && classValid) return true;

      return false;
    }
    private static Character Prepare2Load(byte characterId, Character[] characters) {
      if (CanLoadCharacter(characters[characterId])) return LoadCharacter(characters[characterId]);
      else {  // save file is edited
        string error = "Die geladene Charakterdatei ist korrput.";
        Console.WriteLine(error);
        Thread.Sleep(800);
        throw new IndexOutOfRangeException(error);
      }
    }

    /// <summary>
    /// Return the character
    /// </summary>
    /// <param name="characters">loading Character</param>
    /// <returns></returns>
    private static Character LoadCharacter(Character character) {
      return character;
    }

    /// <summary>
    /// Checks if in Character Name is an InValid Sign
    /// </summary>
    /// <param name="input"></param>
    /// <returns>true if wrong sign / false - if all correct</returns>
    public static bool IsInValidSign(string input) {
      Regex regex = new Regex("[\\\\/:\\*\\?\"<>\\|]", RegexOptions.IgnoreCase);
      return regex.IsMatch(input);
    }

    public string ChangeName() {
      string name = "";

      while (name == "") {
        Console.Clear();
        Console.WriteLine("Geben Sie den Namen ihres Charakters ein:");
        name = Console.ReadLine();

        if (IsInValidSign(name)) {
          Console.WriteLine("\nIm Namen ist ein unerlaubtes Zeichen enthalten!");
          Thread.Sleep(500);
          continue;
        } else if (IsDoubleName(name)) {
          Console.WriteLine("\nIhr benutzt denselben Namen zweimal. Das ist nicht möglich.");
          Thread.Sleep(500);
          continue;
        } else if (name == "" || name == " ") {
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
        
        DeleteCharacer(Name);
      }


      return name;
    }

    /// <summary>
    /// creates the character_save directory is it not exsists
    /// </summary>
    public static void CreateDirectory() {
      string path = Directory.GetCurrentDirectory() + @"\character_saves\";

      Directory.CreateDirectory(path);
    }

    /// <summary>
    /// Checks if the new characters Name is already existing<br />
    /// If its the case, than a new name must be choosen for the character
    /// </summary>
    /// <param name="characterName">name of current created character</param>
    /// <returns>true if name is doubled / false if name is unique</returns>
    public static bool IsDoubleName(string characterName) {
      List<Character> list = CharacterList();
      Character[] characters = list.ToArray();  // convert list to array

      for (byte i = 0; i < characters.Length; i++) {
        if (characterName == characters[i].Name) return true;
      }

      return false;
    }

    /// <summary>
    /// Returns Classname
    /// </summary>
    /// <returns></returns>
    public string GetClassName() {
      string cl = "";
      if (Class == 1) cl = "Krieger";
      else if (Class == 2) cl = "Magier";
      else cl = "Schurke";

      return cl;
    }

    public void ShowCharacter() {
      string cl = GetClassName();

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
    /// Reduces current Exp with the Exp need for this level
    /// </summary>
    public void IncreaseLvl() {
      // if lvl 100 is reached, no more leveling
      if (Lvl >= MAXLVL - 1) return;
      while(Exp[0] >= Exp[1]) { // allows multiple lvl ups
        Console.WriteLine("{0} ist ein Level aufgestiegen.\n{0} ist nun Level {1}.", Name, ++Lvl);
        Console.ReadKey(true);
        Exp[0] -= Exp[1];
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
        CritDmg += 0.10F;
      }

      if(Lvl % 2 == 0) {
        switch (Class) {
          case 1: Strength++; break;
          case 2: Intelligents++; break;
          case 3: Dexterity++; break;
        }
      }
    }
  }
}
