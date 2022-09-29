using System;
using System.Threading;

namespace RpgGame
{
  internal class Marketplace
  {
    // Klassenvariabeln
    private const byte WEAKHEALERPRICE = 25;
    private const byte NORMALHEALERPRICE = 45;
    private const byte STRONGHEALERPRICE = 80;
    private const byte LVLFORHIGHUSES = 8;
    private const int SHORTTIMEOUT = 800;
    private const int TIMEOUT = 1200;
    private const int LONGTIMEOUT = 2000;
    private const ushort STRPRICE = 220;
    private const ushort INTPRICE = 200;
    private const ushort DEXPRICE = 240;
    private const ushort HELPRICE = 210;
    private const ushort CCHPRICE = 460;
    private const ushort CDMPRICE = 440;

    // Membervariabeln

    // Konstruktoren
    public Marketplace(Character c) {
      Character = c;
    }

    // Methoden (funktionen)

    private Character Character { get; set; }

    /// <summary>
    /// Overview of the entire market -> mainspot<br />
    /// The player can go anywhere or leave
    /// </summary>
    /// <returns>current Character</returns>
    public Character OnMarket() {
      bool onMarket = true;
      char input = '0';

      while (onMarket) {
        Console.Clear();
        Console.WriteLine("Ihr befindet Euch auf dem Marktplatz.\nWohin wollt Ihr gehen?");
        Console.WriteLine("1) Heiler\n2) Glückspiel\n3) Arena\n4) Verstärkungsmagier\n9) Marktplatz verlassen");
        input = Console.ReadKey(true).KeyChar;

        switch (input) {
          case '1': HealerOverView(); break;
          case '2': GamblingOverView(); break;
          case '3': ArenaOverView(); break;
          case '4': StatPushOverView(); break;
          case '9': onMarket = false; break;
          default: continue;
        }
      }

      return Character;
    }

    /// <summary>
    /// Shows all the Healers and gives choice to go to one or to leave<br />
    /// The best Healer won't heal the player, if his level is to low
    /// </summary>
    private void HealerOverView() {
      char input = '0';

      while (true) {
        Console.Clear();
        Console.WriteLine("Es gibt drei Heiler auf dem Markt:");
        Console.WriteLine("1) den Anfänger, er kann 25 % eures Lebens wiederherstellen (Preis: {0})\n" +
            "2) den Erfaherenen, er kann 45 % eures Lebens wiederherstellen (Preis: {1})\n" +
            "3) die Meisterin, sie kann eurere Leben komplett wiederherstellen (Preis: {2})\n" +
            "9) Zurück zum Marktplatz\n\nEurer Leben: {3} / {4}", WEAKHEALERPRICE, NORMALHEALERPRICE,
            STRONGHEALERPRICE, Character.Health[0], Character.Health[1]);
        input = Console.ReadKey(true).KeyChar;

        switch (input) {
          case '1':
            if (Character.Gold >= WEAKHEALERPRICE) {
              Healer(0.25);
              Character.Gold -= WEAKHEALERPRICE;
            } else NotEnoughMoney();

            break;
          case '2':
            if (Character.Gold >= NORMALHEALERPRICE) {
              Healer(0.45);
              Character.Gold -= NORMALHEALERPRICE;
            } else NotEnoughMoney();

            break;
          case '3':
            if (Character.Lvl < LVLFORHIGHUSES) {
              Console.WriteLine("Die Heilerin lässt euch nicht hinein. Euer Level ist zu tief.");
              Thread.Sleep(TIMEOUT);
              continue;
            } else if (Character.Gold >= STRONGHEALERPRICE) {
              Character.FullHeal();
              Console.WriteLine("Komplettes Leben wurde wiederhergestellt.");
              Character.Gold -= STRONGHEALERPRICE;
            } else NotEnoughMoney();

            break;
          case '9': return;
          default: continue;
        }
        Thread.Sleep(SHORTTIMEOUT);
        return;
      }
    }

    /// <summary>
    /// heals the player by %
    /// </summary>
    /// <param name="healPerzent">% value the player will be healed by</param>
    private void Healer(double healPerzent) {
      double healValue = Math.Round(Character.Health[1] * healPerzent);
      Character.ChangeCurrentHealth((short)healValue);
      Console.WriteLine("{0} HP wurden wiederhergestellt.", healValue);
    }

    /// <summary>
    /// Shows the games one can play -> gives the choice to play one of them or to leave
    /// </summary>
    private void GamblingOverView() {
      char input = '0';

      while (true) {
        Console.Clear();
        Console.WriteLine("Ihr könnt \"Rot oder Schwarz\" oder \"Höher oder Tiefer\" spielen.");
        Console.WriteLine("Bei \"Rot oder Schwarz\" könnte Ihr selbst Euren Einsatz bestimmen.\n" +
          "Bei \"Höher oder Tiefer\" ist der Einsatz festgelegt.");
        Console.WriteLine("1) Rot oder Schwarz\n2) Höher oder Tiefer\n9) Zurück zum Marktplatz");
        input = Console.ReadKey(true).KeyChar;

        switch (input) {
          case '1': RedBlack(); break;
          case '2': HighLess(); break;
          case '9': return;
          default: continue;
        }
      }
    }

    /// <summary>
    /// The player has to give his bet (the game does not allow the p. to give higher bets than he possesses)<br />
    /// The player has to choose between red and black<br />
    /// If the player is right, he gets his bet paid out - if not, he has to pay his bet
    /// </summary>
    private void RedBlack() {
      Random r = new Random();
      string[] moods = { "grimmige", "gelangweilte", "fröhliche" };
      string dealerMood = moods[r.Next(1, moods.Length)];
      string dealer = "Der {0} Spielmeister ";
      bool gameIsRed = false, characterIsRed = false, ableToPlay = false;
      uint stake = 0;
      char input = '0';

      if(Character.Gold <= 0) {
        NotEnoughMoney();
        Thread.Sleep(TIMEOUT);
        return;
      }

      do {
        Console.Clear();
        Console.WriteLine(dealer + "fragt nach eurem Einsatz. (Euer Gold: {1})", dealerMood, Character.Gold);
        Console.Write("Euer Einsatz: ");
        if (!uint.TryParse(Console.ReadLine(), out stake)) continue;
        if(stake == 0) {
          Console.WriteLine("Ihr müsst eine Wette abschliessen!");
          Thread.Sleep(SHORTTIMEOUT);
        } else if (stake > Character.Gold) {
          Console.WriteLine("Ihr dürft nicht mehr wetten, als Ihr eigentlich besitzt!");
          Thread.Sleep(SHORTTIMEOUT);
        } else ableToPlay = true;
      } while (!ableToPlay);

      Console.WriteLine(dealer + "fragt, für das Ihr wettet.", dealerMood);
      while (true) {
        Console.WriteLine("\n1) Rot\n2) Schwarz");
        Console.Write("Eure Wahl: ");
        input = Console.ReadKey(false).KeyChar;

        if (input == '1') characterIsRed = true;
        else if (input == '2') characterIsRed = false;
        else continue;

        break;
      }

      Console.Write("\n\nDas Ergebnis ist");
      for (byte i = 0; i < 4; i++) {
        Console.Write(".");
        Thread.Sleep(SHORTTIMEOUT - 300); // 0.5s * 4 = 2s
      }

      if (r.Next(1, 3) == 1) {
        gameIsRed = true;
        Console.WriteLine("\nRot!");
      } else {
        gameIsRed = false;
        Console.WriteLine("\nSchwarz!");
      }

      if (characterIsRed == gameIsRed) {
        Console.WriteLine("Ihr habt gewonnen!");
        Character.Gold += Convert.ToInt32(stake);
      } else {
        Console.WriteLine("Ihr habt verloren...");
        Character.Gold -= Convert.ToInt32(stake);
      }

      Thread.Sleep(TIMEOUT);
    }

    /// <summary>
    /// rnd number between 1 and 10 is rolled<br />
    /// The player has to guess if the next number is smaller or bigger than the current one<br />
    /// If the player guest correct, he'll get gold - if not he losses gold<br />
    /// If the player can't afford to play -> he'll be thrown out
    /// </summary>
    private void HighLess() {
      Random r = new Random();
      char input = '0';
      bool playerIsHigher = false, gameIsHigher = false, wonGame = false;
      int lastGameNumber = 0, gameNumber = 0, stake = 40;

      Console.Clear();
      if (Character.Gold < stake) {
        NotEnoughMoney();
        Thread.Sleep(TIMEOUT);
        return;
      }

      Console.WriteLine("Eine Zahl zwischen 1 und 10 wird gewürfelt. " +
          "Ihr müsst sagen, ob die nächste Zahl höher oder tiefer, als die Jetzige sein wird.\n" +
          "Der Einsatz ist {0} Gold.", stake);

      while (true) {
        lastGameNumber = r.Next(1, 11);
        gameNumber = r.Next(1, 11);

        Console.WriteLine("Die 1. Zahl ist eine {0}", lastGameNumber);

        while (true) {
          Console.WriteLine("1) Höher\n2) Tiefer");
          input = Console.ReadKey(true).KeyChar;

          if (input == '1') { playerIsHigher = true; break; }
          else if (input == '2') { playerIsHigher = false; break; }
          else continue;
        }

        Console.WriteLine("Die 2. Zahl ist eine {0}", gameNumber);
        if (gameNumber > lastGameNumber) gameIsHigher = true;
        else gameIsHigher = false;

        if (playerIsHigher == gameIsHigher) {
          Console.WriteLine("Ihr hattet recht!");
          wonGame = true;
        } else {
          Console.WriteLine("Ihr lagt falsch...");
          wonGame = false;
        }
        PayPlayer(wonGame, stake);
        break;
      }
    }

    /// <summary>
    /// Checks and says if the player won / lost money and adds / reduces the amout of gold from the player
    /// </summary>
    /// <param name="pWon">has player won?</param>
    /// <param name="stake">sum the player won or lost</param>
    private void PayPlayer(bool pWon, int stake) {
      string wonMsg = pWon ? "erhaltet" : "bezahlt";
      Console.WriteLine("Ihr {0} {1} Gold.", wonMsg, stake);
      if (pWon) Character.Gold += stake;
      else Character.Gold -= stake;
      Thread.Sleep(TIMEOUT);
    }

    /// <summary>
    /// Shows arena and rules -> gives choice to fight or leave
    /// </summary>
    private void ArenaOverView() {
      char input = '0';

      while (true) {
        Console.Clear();
        Console.WriteLine("In der Arena werdet ihr ausschliesslich starke Gegner treffen und wie der Zufall es will, " +
            "habt ihr die Möglichkeit gegen besonders starke Gegner zu kämpfen, mit höheren Belohnungen natürlich\n" +
            "In der Arena gelten nicht dieselben Regeln wie in der Wildnis. Hier könnt ihr nicht sterben.");
        Console.WriteLine("1) Normaler Arenakampf\n2) Kampf gegen starken Gegner\n9) Zurück zum Marktplatz.");
        input = Console.ReadKey(true).KeyChar;

        switch (input) {
          case '1': FightArena(false); break;
          case '2': FightArena(true); break;
          case '9': return;
          default: continue;
        }
      }
    }

    /// <summary>
    /// Evaluates and fights against enemy in arena
    /// </summary>
    /// <param name="ishard">Is Enemy strong?</param>
    private void FightArena(bool ishard) {
      Enemy e = EvalEnemy(ishard);
      FightArena fa = new FightArena(Character, e);

      fa.FightIn();
    }

    /// <summary>
    /// generates rnd enemy between 5 and 10<br/>
    /// enemy can be extra strong
    /// </summary>
    /// <param name="isHard">Is Enemy strong?</param>
    /// <returns>new Enemy</returns>
    private Enemy EvalEnemy(bool isHard) {
      Random r = new Random();
      byte enemyId = Convert.ToByte(r.Next(1, 101));

      if (enemyId <= 21) enemyId = 5; // 21 %
      else if (enemyId <= 41) enemyId = 6; // 20 %
      else if (enemyId <= 56) enemyId = 7; // 18 %
      else if (enemyId <= 75) enemyId = 8; // 16 %
      else if (enemyId <= 89) enemyId = 9; // 14 %
      else enemyId = 10; // 11 %

      return new Enemy(Character.Lvl, enemyId, isHard);
    }

    /// <summary>
    /// Gives the opportunity to increase stats permanently.<br />
    /// The Player can't use these service is its level is too low.<br />
    /// If the player can't pay, he won't get it
    /// </summary>
    private void StatPushOverView() {
      char input = '0';
      ushort price = 0;

      if (Character.Lvl < LVLFORHIGHUSES) {
        Console.Clear();
        Console.WriteLine("Die Verstärkungsmagier lässt euch nicht hinein. Euer Level ist zu tief.");
        Thread.Sleep(TIMEOUT);
        return;
      }

      while (true) {
        Console.Clear();
        Console.WriteLine("Der Verstärkungsmagier kann euch, auf eine neue Ebene der Macht bringen, " +
            "für einen kleinen Betrag natürlich.");
        Console.WriteLine("1) +1 Stärke (Preis: {0} Gold)\n2) +1 Inteligents (Preis: {1} Gold)\n3) +1 Geschicklichkeit (Preis: {2} Gold)\n" +
          "4) +5 Max Leben (Preis: {3} Gold)\n5) Krit. Chance + 2 % (Preis: {4} Gold)\n6) Krit. Schaden + 5 % (Preis: {5} Gold)\n" +
          "9) Zurück zum Marktplatzs", STRPRICE, INTPRICE, DEXPRICE, HELPRICE, CCHPRICE, CDMPRICE);
        input = Console.ReadKey().KeyChar;
                  
        switch (input) {
          case '1':
            if (Character.Gold >= STRPRICE) Character.Strength++;
            else NotEnoughMoney();
            price = STRPRICE;
            break;
          case '2':
            if (Character.Gold >= INTPRICE) Character.Intelligents++;
            else NotEnoughMoney();
            price = INTPRICE;
            break;
          case '3':
            if (Character.Gold >= DEXPRICE) Character.Dexterity++;
            else NotEnoughMoney();
            price = DEXPRICE;
            break;
          case '4':
            if (Character.Gold >= HELPRICE) Character.Health[1] += 5;
            else NotEnoughMoney();
            price = HELPRICE;
            break;
          case '5':
            if (Character.Gold >= CCHPRICE) Character.CritChance += 2;
            else NotEnoughMoney();
            price = CCHPRICE;
            break;
          case '6':
            if (Character.Gold >= CDMPRICE) Character.CritDmg += 0.05F;
            else NotEnoughMoney();
            price = CDMPRICE;
            break;
          case '9': return;
          default: continue;
        }
        Character.Gold -= price;
      }
    }

    /// <summary>
    /// Will trigger if the player can't pay for a service 
    /// </summary>
    private void NotEnoughMoney() {
      Console.WriteLine("Ihr habt nicht genügend Geld.");
    }
  }
}
