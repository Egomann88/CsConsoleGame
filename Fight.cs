using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RpgGame
{
    internal class Fight
    {
        // Klassenvariablen
        private short[] CHARACTERCOOLDOWN = new short[] { 0, 0 };    // Heal, Ult
        private short[] ENEMYCOOLDOWN = new short[] { 0, 0 }; // Heal, Ult
        private const byte HEALCOOLDOWN = 3;    // Default cooldown of both sides for the healpotion
        private const byte ULTIMATECOOLDOWN = 4; // Default cooldown for both sides on the ultimate
        private const int SHORTTIMEOUT = 700;
        private const int TIMEOUT = 1200;
        private const int LONGTIMEOUT = 2000;


        // Membervariablen

        // Konstruktor
        /// <summary>
        /// Need an Character and Enemy to fight.
        /// </summary>
        /// <param name="c">Character Object</param>
        /// <param name="e">Enemy Object</param>
        public Fight(Character c, Enemy e) {
            Character = c;
            Enemy = e;
            RoundCount = 0;
        }

        // Methoden (Funktionen)
        public Character Character { get; set; }

        public Enemy Enemy { get; set; }

        private byte RoundCount { get; set; }


        public Character FightIn() {

            bool isPlayerFirst = GetFirstMove();

            if(isPlayerFirst) PlayerTurn();

            EnemyTurn();

            if(!isPlayerFirst) PlayerTurn();
            
            RoundCount++;
            
            return Character;
        }

        private bool PlayerTurn() {
            short[] coolDown = GetCoolDown(true);   // cooldown of abilitys
            string ultimateName = UltimateName();
            string actionText = ""; // what player will do
            ushort damage = 0;  // players dmg
            char input = '0';   // player input
            bool flee = false;

            do {
                Console.Clear();
                Console.WriteLine("{0}, was wollt ihr machen?", Character.Name);
                Console.Write("1) Angreifen\n2) Heilen (Abklingzeit: {0} Runden)\n3) {1} (Abklingzeit: {2} Runden)\n4) Fliehen"
                    , coolDown[0], ultimateName, coolDown[1]);
                input = Console.ReadKey(false).KeyChar; // do not show input in console
                switch (input) {
                    case '1':
                        damage = Character.Strength;

                        actionText = $"{Character.Name} greift an.\n";

                        if (IsCrit(Character.CritChance)) { 
                            damage = Convert.ToUInt16(Math.Round(damage * Character.CritDmg));
                            actionText += "Kritischer Treffer!";
                        }

                        actionText += $"{damage} Schaden!";

                        Console.WriteLine(actionText);

                        Character.ChangeCurrentHealth(Convert.ToInt16(-damage));

                        Thread.Sleep(TIMEOUT);
                        break;
                    case '2':
                        // if abilty is still on cooldown, go back to start
                        if (IsCharacterOnCoolDown(coolDown[0])) continue;

                        damage = Character.Intelligents;

                        actionText = $"{Character.Name} heilt sich.\n{damage} Leben wiederhergestellt";
                        Console.WriteLine(actionText);

                        Character.ChangeCurrentHealth(Convert.ToInt16(damage));

                        coolDown[0] = HEALCOOLDOWN;    // set heal cooldown

                        Thread.Sleep(TIMEOUT);
                        break;
                    case '3':
                        // if abilty is still on cooldown, go back to start
                        if (IsCharacterOnCoolDown(coolDown[1])) continue;

                        damage = GetCharacterUltimate();

                        actionText = $"{Character.Name} nutzt seine Ultimatie Fähigkeit \"{UltimateName()}\".\n";
                        
                        if (IsCrit(Character.CritChance)) {
                            damage = Convert.ToUInt16(Math.Round(damage * Character.CritDmg));
                            actionText += "Kritischer Treffer!";
                        }

                        actionText += $"{damage} Schaden";

                        Console.WriteLine(actionText);

                        Enemy.ChangeCurrentHealth(Convert.ToInt16(-damage));
                        coolDown[1] = ULTIMATECOOLDOWN;    // set ulti cooldown

                        Thread.Sleep(TIMEOUT);
                        break;
                    case '4':
                        actionText = $"{Character.Name} versucht zu fliehen.\n";

                        if (IsFled()) {
                            actionText += $"{Character.Name} ist geflohen!";
                            flee = true;
                        } else actionText += "Fehlgeschalgen!";

                        Console.WriteLine(actionText);
                        
                        Thread.Sleep(TIMEOUT);
                        break;
                    default: continue;  // must give new input
                }
                break;  // break out of loop
            } while (true);

            coolDown = coolDown.Select(x => --x).ToArray();   // decrease cooldowns by one

            CHARACTERCOOLDOWN = coolDown;  // save cooldowns for next round

            return flee;
        }

        /// <summary>
        /// Gives Ultimate diffrent name per class
        /// </summary>
        /// <returns>name -> string</returns>
        private string UltimateName() {
            string name = "";

            switch (Character.Class) {
                case 1: name = "Bodenspalter";  break;
                case 2: name = "Meteorschauer"; break;
                case 3: name = "Exitus"; break;
            }

            return name;
        }

        private void EnemyTurn() {
            Random r = new Random();
            byte numberPool = 3;    // Attack, Heal, Ultimate
            short[] enemyCoolDown = GetCoolDown(false);  // apply current cooldowns
            ushort damage = 0;
            byte rnd = 0;

            if (enemyCoolDown[0] > 0) numberPool--; // if move is on cooldown, dont try to use it
            if (enemyCoolDown[1] > 0) numberPool--; // if move is on cooldown, dont try to use it

            rnd = Convert.ToByte(r.Next(1, numberPool + 1));    // roll next move

            enemyCoolDown = enemyCoolDown.Select(x => --x).ToArray();   // decrease cooldowns by one

            switch (rnd) {
                case 1:
                    damage = Enemy.Strength;

                    if (IsCrit(Enemy.CritChance)) damage = Convert.ToUInt16(Math.Round(damage * Enemy.CritDmg));

                    Character.ChangeCurrentHealth(Convert.ToInt16(-damage));
                    break;
                case 2:
                    damage = Enemy.Intelligents;

                    Enemy.ChangeCurrentHealth(Convert.ToInt16(damage));

                    enemyCoolDown[0] = HEALCOOLDOWN;    // set ability cooldown
                    break;
                case 3:
                    if (Enemy.IsDmgUlt) {
                        // increase dmg with all possible variables
                        damage = Convert.ToUInt16(Math.Round(Enemy.Strength + Enemy.Dexterity + Enemy.Intelligents * 1.5));

                        if (IsCrit(Enemy.CritChance)) damage = Convert.ToUInt16(Math.Round(damage * Enemy.CritDmg));

                        Character.ChangeCurrentHealth(Convert.ToInt16(-damage));
                    } else {
                        // Heals himself with 1.2 of his Intelligents + 20 % of his max Health
                        damage = Convert.ToUInt16(Math.Round(Enemy.Intelligents * 1.2 + Enemy.Health[1] / 5));
                        Enemy.ChangeCurrentHealth(Convert.ToInt16(damage));
                    }

                    enemyCoolDown[1] = ULTIMATECOOLDOWN;    // set ability cooldown
                    break;
            }

            ENEMYCOOLDOWN = enemyCoolDown;  // save Enemycooldown for next round


        }

        private bool GetFirstMove() {
            Random r = new Random();

            if (Character.Dexterity == Enemy.Dexterity) return r.Next(1, 3) == 1 ? true : false;
            else if (Character.Dexterity > Enemy.Dexterity) return true;
            else return false;
        }

        /// <summary>
        /// Every Class has a diffrent Ult, damage wil also be diffrenty calculated.
        /// </summary>
        /// <returns>damage -> uhsort</returns>
        private ushort GetCharacterUltimate() {
            ushort damage = 0;
            switch (Character.Class)
            {
                case 1: // warrior
                    damage = Convert.ToUInt16(Character.Strength * 2 + (Character.Intelligents / 2) - Math.Round(RoundCount * 1.2));
                    break;
                case 2: // mage
                    // if number of shot metors is lesser than 1, just use 1
                    int countMetores = Character.Intelligents * 0.2 < 1 ? 1 : Convert.ToInt32(Character.Intelligents * 0.2);
                    damage = Convert.ToUInt16(Math.Round(Character.Intelligents * 0.6 * countMetores));
                    break;
                case 3: // thief
                    // if hp is overheal, zero dmg, instead of -dmg
                    int hpDmg = Character.Health[1] - Character.Health[0] < 0 ? 0 : Character.Health[1] - Character.Health[0];
                    damage = Convert.ToUInt16(hpDmg + Character.Dexterity + RoundCount);

                    // heals character with a quater of dealt dmg (no decimal number + no round + overheal allowed)
                    Character.ChangeCurrentHealth((short)(damage / 4), true); // send copy of dmg, cuz its needed for return
                    break;
            }

            return damage;
        }

        /// <summary>
        /// Get Enemy or Character Ability cooldowns<br />
        /// If cooldown ist under 0, it will be overwritten with it.
        /// </summary>
        /// <param name="isCharacter">true if its the Characters cooldown. If not -> false</param>
        /// <returns>coolodown -> short[]</returns>
        private short[] GetCoolDown(bool isCharacter) {
            short[] coolDown = isCharacter ? CHARACTERCOOLDOWN : ENEMYCOOLDOWN;

            if (coolDown[0] <= 0) coolDown[0] = 0;  // does not allow the counter to go under 0
            if (coolDown[1] <= 0) coolDown[1] = 0;

            return coolDown;
        }

        /// <summary>
        /// Checks if was able to flee
        /// </summary>
        /// <returns>true - succeeded / false - failed</returns>
        private bool IsFled() {
            Random r = new Random();
            int rnd = r.Next(1, 5); // 25 %

            if (rnd > 1) return false;

            return true;
        }

        /// <summary>
        /// Player method only.<br />
        /// Checks if abilty is on cooldown
        /// </summary>
        /// <param name="coolDown">cooldown of current abilty</param>
        /// <returns>bool -> true - Cooldown / false - no Cooldown</returns>
        private bool IsCharacterOnCoolDown(short coolDown) {
            if(coolDown > 0) return true;

            return false;
        }

        /// <summary>
        /// Checks if a cirt is rolled
        /// </summary>
        /// <param name="cChance">Crit Chance of Player / Enemy</param>
        /// <returns>bool -> Cirt or no Crit</returns>
        private bool IsCrit(float cChance) {
            Random r = new Random();
            byte i = Convert.ToByte(r.Next(1, 101));

            if (i > cChance) return false;

            return true;
        }
    }
}
