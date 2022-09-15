using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame
{
    internal class Fight
    {
        // Klassenvariablen
        private short[] CHARACTERCOOLDOWN = new short[] { 0, 0 };    // Heal, Ult
        private short[] ENEMYCOOLDOWN = new short[] { 0, 0 }; // Heal, Ult
        private const byte HEALCOOLDOWN = 3;
        private const byte ULTIMATECOOLDOWN = 4;

        // Membervariablen

        // Konstruktor
        public Fight(Character c, Enemy e) {
            Character = c;
            Enemy = e;
        }

        // Methoden (Funktionen)
        public Character Character { get; set; }

        public Enemy Enemy { get; set; }


        public Character Fightin() {

            EnemyTurn();

            return Character;
        }

        private void PlayerTurn() {
            short[] coolDown = GetCoolDown(true);
            string ultimateName = UltimateName();
            char input = '0';

            do {
                Console.WriteLine("{0}, was wollt ihr machen?", Character.Name);
                Console.WriteLine("1) Angreifen\n2) Heilen (Abklingzeit: {0} Runden)\n3) {1} (Abklingzeit: {2} Runden)\n4) Fliehen"
                    , coolDown[0], ultimateName, coolDown[1]);
                input = Console.ReadKey().KeyChar;
            } while (false);
        }

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

        /// <summary>
        /// Get Enemy or Character Ability cooldowns<br />
        /// If cooldown ist under 0, it will be overwritten with it.
        /// </summary>
        /// <param name="isCharacter">true if its the Characters cooldown. If not -> false</param>
        /// <returns>coolodown</returns>
        private short[] GetCoolDown(bool isCharacter) {
            short[] coolDown = isCharacter ? CHARACTERCOOLDOWN : ENEMYCOOLDOWN;

            if (coolDown[0] <= 0) coolDown[0] = 0;
            if (coolDown[1] <= 0) coolDown[1] = 0;

            return coolDown;
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
