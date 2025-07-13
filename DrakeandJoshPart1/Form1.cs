using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DrakeandJoshPart1
{
    public partial class Form1 : Form
{
    /*
    TODO 
    Create a ResetGame() method that clears the player, cast, currentEnemy, and sets GamePhase = MainMenu
    - After Round_1 (player attacks), queue up a new GamePhase.EnemyTurn_1
    - Then use your existing EnemyTakesTurn() and randomize their move
    - Add dialog options and non-combat interactions later using new phases like GamePhase.Dialog
    - Multi-round fights
    - Abliity to Flee Combat and contrinue story.
    - Add "Victory" and "Defeat" states:
    */
    
    private Character player; //TODO: Might need to move this to its own class.
    private static Dictionary<string, Character> cast; 
    // TODO    - Create Dictionary<string, Func<string>> npcArtBank and use npcArtBank[currentEnemy.Name]() in updateStats

    private bool awaitingPlayChoice = true;
    private bool showingCredits = false;
    public enum GamePhase // create an enum for game phases
    {
        MainMenu,
        CharacterSelect,
        WeaponSelect,
        PlayerTurn_1,
        Round_1,
        EnemyTurn_1,
        Victory,
        Defeat,
        Credits,
        Exit
    }
    private GamePhase currentPhase = GamePhase.MainMenu; // create a variable for the current game phase.
    string titleArt = AsciiArtBank.GetTitleArt(); // create a variable for the title art.
    private List<Character> nearbyNPCs = new List<Character>(); // create a list for nearby NPCs.

    private void ResetGame()
    {
        player = null;
        cast = null;
        currentEnemy = null;
        nearbyNPCs.Clear();

        statTextBox.Clear();
        readTextBox.Clear();
        writeTextBox.Clear();

        currentPhase = GamePhase.MainMenu;

        statTextBox.Text = titleArt;
        statTextBox.Font = new Font("Consolas", 8);
        statTextBox.WordWrap = false;

        readTextBox.AppendText("Drake and Josh's Really Big Text Adventure!" + Environment.NewLine);
        readTextBox.AppendText("[1] Play" + Environment.NewLine);
        readTextBox.AppendText("[2] Credits" + Environment.NewLine);
        readTextBox.AppendText("[X] Exit" + Environment.NewLine);
    }
    public Form1() // Constructor.
    {
        InitializeComponent();
        writeTextBox.KeyPress += new KeyPressEventHandler(writeTextBox_KeyPress);     // Handles individual key input (e.g. filters what the user is allowed to type in the main menu).
        writeTextBox.KeyDown += new KeyEventHandler(writeTextBox_KeyDown); // Handles when the Enter key is pressed to process a full input line.
    }
    private void InitializeCast() //we add the character that is not chosen in the start up screen to the cast.
    {
        cast = new Dictionary<string, Character>
        {
            { "Megan", new NPCCharacter("Megan", 100) },
            { "Baaahhb", new NPCCharacter("Baaahhb", 100) },
            { "Helen", new NPCCharacter("Helen", 100) },
            { "Walter", new NPCCharacter("Walter", 100) },
            { "Craig", new NPCCharacter("Craig", 100) },
            { "Eric", new NPCCharacter("Eric", 100) },
            { "Albury", new NPCCharacter("Albury", 100) },
            { "Crazy Steve", new NPCCharacter("Crazy Steve", 100) },
            { "Gavin", new NPCCharacter("Gavin", 100) },
            { "Mindy", new NPCCharacter("Mindy", 100) },
            { "Mrs. Hayfer", new NPCCharacter("Mrs. Hayfer", 100) },
            { "Dr Jeff", new NPCCharacter("Dr Jeff", 100) }, //Resident doctor of 
            { "Bruce Wincil", new NPCCharacter("Bruce Wincil", 100) }, //Walters nemisis
            { "Oprah", new NPCCharacter("Oprah", 100) },
            { "The Great Henry Doheny", new NPCCharacter("The Great Henry Doheny", 100) }, //Joshes favorite magician
            { "Clayton", new NPCCharacter("Clayton", 100) }, //Retarded kid in class
            { "Bludge", new NPCCharacter("Bludge", 100) }, //Prison breaker turned ally.
            { "Dr. Adrian G. Favershim", new NPCCharacter("Dr. Adrian G. Favershim", 100) }, //Cooks animals and eats them.
            { "Papa Nicoles", new NPCCharacter("Papa Nicoles", 100) },
            { "Gary Coleman", new NPCCharacter("Gary Coleman", 100) },
            { "Tony Hawk", new NPCCharacter("Tony Hawk", 100) },
            { "The Theater Thug", new NPCCharacter("The Theater Thug", 100) },
            { "Wendy", new NPCCharacter("Wendy", 100) }, //Drakes obsesed underage fan.
            { "Tiberius ", new NPCCharacter("Tiberius", 100) },
            { "Animal Control Guy ", new NPCCharacter("Animal Control Guy", 100) },
            { "Sensei", new NPCCharacter("Sensei", 100) },
            { "Blaze ", new NPCCharacter("Blaze ", 100) }, //Criminal
            { "The Governor", new NPCCharacter("The Governor", 100) },
            { "Ashley Blake", new NPCCharacter("Ashley Blake", 100) }, //Famous movie star
            { "Trevor", new NPCCharacter("Trevor", 100) }, //Memeber of drakes band
            { "Scottie ", new NPCCharacter("Scottie", 100) }, //Memeber of drakes band
            { "Rina ", new NPCCharacter("Rina", 100) }, //Memeber of drakes band
            { "Vince ", new NPCCharacter("Vince", 100) }, //pilot instructor
            { "Sergeant Doty ", new NPCCharacter("Sergeant Doty", 100) }, //responds with his partner to investigate stolen furniture.
            { "Robbie", new NPCCharacter("Robbie ", 100) }, //gay next door neighbor.
            { "Jerry", new NPCCharacter("Jerry  ", 100) }, //Josh's look-a-like.
            { "Drew", new NPCCharacter("Drew ", 100) }, //Drake's look-a-like.
            { "Carly", new NPCCharacter("Carly", 100) }, //Drake's girl friend..
            { "Perry", new NPCCharacter("Perry", 100) }, //parole officer trying to stop drake and josh
            { "Bobo", new NPCCharacter("Bobo", 100) }, //Orangatan being cooked by Dr. Adrian G. Favershim
            { "Cookie", new NPCCharacter("Cookie", 100) }, //parrys monkey
            { "Zeke", new NPCCharacter("Zeke", 100) }, //School Janitor
            { "Jackie", new NPCCharacter("Jackie", 100) } //I love you! Bye!
            // Add more as needed
        };
        
        // Add the opposite character of the player's choice
        if (player != null)
        {
            if (player.Name == "Drake")
            {
                cast["Josh"] = new NPCCharacter("Josh", 120);
            }
            else if (player.Name == "Josh")
            {
                cast["Drake"] = new NPCCharacter("Drake", 100);
            }
        }
    }
    private void updateStats(Func<string> getAsciiArt)
    {
        statTextBox.Clear();
        statTextBox.AppendText(getAsciiArt() + Environment.NewLine);
        DisplayPlayerStats();
        DisplayEnemyStats();
    } //Updates art and statbox
    public class Character // create a new class called Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public string Goal { get; set; }
        public int PowerLevel { get; set; }
        public List<string> Inventory { get; set; }
        public Weapon EquippedWeapon { get; set; }
        public bool IsNPC { get; set; }


        public Character(string name, int health, string goal = "", bool isNPC = false)
        {
            Name = name;
            Health = health;
            Goal = goal;
            IsNPC = isNPC;
            PowerLevel = 1;
            Inventory = new List<string>();
        }

        public void EquipWeapon(Weapon weapon) // Creates a method for equipping weapons to the player.
        {
            EquippedWeapon = weapon;
            PowerLevel = weapon.PowerBoost;
            Inventory.Add(weapon.Name);
        }

        public void TakeDamage(int damage) //method to take damage to the player's health.
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public void Heal(int amount) // Method to heal the player.
        {
            Health += amount;
            if (Health > 100) Health = 100;
        }

        public void AddItem(string item) // Method to add an item to the player's inventory.'
        {
            Inventory.Add(item);
            MessageBox.Show($"{item} added to inventory.", "Item Collected");
        }
        public class Weapon //Creates a class for weapons.
        {
            public string Name { get; set; }
            public int Uses { get; set; }
            public int PowerBoost { get; set; }

            public Weapon(string name, int uses, int powerBoost) //constructor for weapons.
            {
                Name = name;
                Uses = uses;
                PowerBoost = powerBoost;
            }

            public bool Use()
            {
                Uses--;
                return Uses > 0;
            }
        }
        
        }
    public class NPCCharacter : Character // create a new class that inherits from Character
    {
        public NPCCharacter(string name, int health) : base(name, health, isNPC: true)
        {
            
        }
    }
    private Character currentEnemy;
    
    //    - Check in ApplyCombatChoice() or playersTurn() if currentEnemy.Health <= 0
    //    - If so, display a victory screen, reward the player, and change the phase to a new GamePhase like Victory or return to MainMenu
    private void playersTurn(int choice)
    {
        int roll = new Random().Next(1, 7); // Roll 1–6
        bool badOutcome = roll % 2 == 1; // 1, 3, 5 are bad

        switch (choice)
        {
            case 1: // Aggressive move
                if (badOutcome)
                {
                    player.TakeDamage(15);
                    updateStats(AsciiArtBank.getPaddelcrash);
                    readTextBox.Clear();
                    readTextBox.AppendText($"Roll: {roll} {(badOutcome ? "[BAD]" : "[GOOD]")}\n"+ Environment.NewLine);
                    readTextBox.AppendText("You miss and throw your weapon through the window. Baaahhb charges!\n"+ Environment.NewLine);
                    

                }
                else
                {
                    currentEnemy.TakeDamage(15);
                    updateStats(AsciiArtBank.getsuccessArt);
                    readTextBox.Clear();
                    readTextBox.AppendText($"Roll: {roll} {(badOutcome ? "[BAD]" : "[GOOD]")}\n" + Environment.NewLine);
                    readTextBox.AppendText("Direct hit! Baaahhb reels from the impact.\n" + Environment.NewLine);

                }
                break;


            case 2: // Defensive move
                if (badOutcome)
                {
                    player.TakeDamage(10);
                    updateStats(AsciiArtBank.GetDrakeArt);
                    readTextBox.Clear();
                    readTextBox.AppendText($"Roll: {roll} {(badOutcome ? "[BAD]" : "[GOOD]")}\n" + Environment.NewLine);
                    readTextBox.AppendText("You try to defend, but Baaahhb counters and hits you!\n" + Environment.NewLine);
                }
                else
                {
                    currentEnemy.TakeDamage(10);
                    updateStats(AsciiArtBank.GetPingpongArt);
                    readTextBox.Clear();
                    readTextBox.AppendText($"Roll: {roll} {(badOutcome ? "[BAD]" : "[GOOD]")}\n" + Environment.NewLine);
                    readTextBox.AppendText("You block and counter Baaahhb!\n" + Environment.NewLine);
                }
                break;

            case 3: // Run away
                if (badOutcome)
                {
                    player.TakeDamage(30);
                    updateStats(AsciiArtBank.GetBaabArt);
                    readTextBox.Clear();
                    readTextBox.AppendText("You try to run but trip — Baaahhb stomps you.\n" + Environment.NewLine);
                }
                else
                {
                    updateStats(AsciiArtBank.getJumpArt);
                    readTextBox.Clear();
                    readTextBox.AppendText("You get away safely... this time.\n" + Environment.NewLine);
                }
                break;

            default:
                updateStats(AsciiArtBank.getFailedArt);
                readTextBox.Clear();
                readTextBox.AppendText("Invalid move. Type 1, 2, or 3.\n" + Environment.NewLine);
                break;
            
        }
        
    }  //Players Roll
    
    private void EnemyTakesTurn(int choice)
    {
       
        int roll = new Random().Next(1, 7); // 1 to 6
        bool badForPlayer = roll % 2 == 1; // 1,3,5 = hit
        
        readTextBox.AppendText("\n--- Enemy's Turn ---\n");

        if (badForPlayer)
        {
            int damage = 10 + currentEnemy.PowerLevel;
            player.TakeDamage(damage);
            
            readTextBox.AppendText(currentEnemy.Name + " counters viciously and deals " + damage + " damage!\n");
        }
        else
        {
            
            readTextBox.AppendText(currentEnemy.Name + " stumbles and misses their attack!\n");
        }
        readTextBox.Clear();
        readTextBox.AppendText("Enemy rolled: " + roll + (badForPlayer ? " [Hit!]" : " [Miss!]") + "\n");
        updateStats(() => AsciiArtBank.GetBaabArt());
    } // Enemies Roll

    private void DisplayEnemyStats()
    {
        
        if (currentEnemy != null) //only visible if current enemy is not null.
        {
            statTextBox.AppendText("===== Enemy Stats=====" + Environment.NewLine); // Display Enemy Stats
            statTextBox.AppendText(currentEnemy.Name  + " | Health: " +currentEnemy.Health + Environment.NewLine);
        }
        
        foreach (var npc in nearbyNPCs) //Display nearby NPCs.
        {
            statTextBox.AppendText($"Nearby: {npc.Name} is watching...\n");
        }
    }
    private void DisplayPlayerStats() // want to display your starting health and inventory in some sort of table right after these messages
    {
        statTextBox.Font = new Font("Consolas", 12);
        statTextBox.WordWrap = false;
        statTextBox.ScrollBars = ScrollBars.None; // If you're trying to avoid scrollbars
        statTextBox.AppendText("===== PLAYER STATS =====" + Environment.NewLine);
        statTextBox.AppendText($"Player: {player.Name}" + Environment.NewLine);
        statTextBox.AppendText($"Health: {player.Health}" + Environment.NewLine);
        
        if (!player.IsNPC) 
            statTextBox.AppendText($"Goal: {player.Goal}" + Environment.NewLine);
        statTextBox.AppendText($"Power Level: {player.PowerLevel}" + Environment.NewLine);
        statTextBox.AppendText("Inventory: " + (player.Inventory.Count > 0 ? string.Join(", ", player.Inventory) : "(empty)") + Environment.NewLine);
        statTextBox.AppendText("=========================" + Environment.NewLine);
    }
    //waits for player to enter drake or josh and then displays the player stats.
    private void writeTextBox_KeyDown(object sender, KeyEventArgs e) // Serves as the core game loop.
    {
        if (e.KeyCode == Keys.Enter)
        {
            string input = writeTextBox.Text.ToLower().Trim();
            writeTextBox.Clear();

            switch (currentPhase)
            {
                case GamePhase.MainMenu: // changes gamephase to Main menu
                    if (input == "1" || input == "play")
                    {
                        currentPhase = GamePhase.CharacterSelect; // changes gamephase to Character select
                        readTextBox.Clear();
                        readTextBox.AppendText("Who do you want to play as? Type 'Drake' or 'Josh'." +
                                               Environment.NewLine);
                    }
                    else if (input == "2" || input == "credits")
                    {
                        readTextBox.Clear();
                        readTextBox.AppendText("Created by Nickelodeon Fan Club. Press Enter to return to menu." +
                                               Environment.NewLine);
                    }
                    else if (input == "x" || input == "exit")
                    {
                        Application.Exit();
                    }
                    else
                    {
                        readTextBox.AppendText("Invalid input. Please enter 1, 2, or X." + Environment.NewLine);
                    }

                    break;



                case GamePhase.CharacterSelect: // We switch game phases here. And we set the player chosen here.
                    if (input == "drake")
                    {
                        player = new Character("Drake", 100, "Get your drivers license");
                        InitializeCast();
                    }
                    else if (input == "josh")
                    {
                        player = new Character("Josh", 120, "Get the Golden Vest at the Premier");
                        InitializeCast();
                    }
                    else
                    {
                        readTextBox.AppendText("Invalid choice. Please type 'Drake' or 'Josh'." + Environment.NewLine);
                        return;
                    }

                    currentPhase = GamePhase.WeaponSelect;


                    // This displays art and current stats in the stat box for player
                    updateStats(AsciiArtBank.GetMeganArt);

                    // This displays the first message for the player in the read box.
                    readTextBox.Clear();
                    readTextBox.AppendText($"You are playing as {player.Name}. Goal: {player.Goal}\n" +
                                           Environment.NewLine);
                    readTextBox.AppendText(
                        "You roll out of bed and see three items: a Ping Pong Paddle, the Game Sphere, or a Banana." +
                        Environment.NewLine);
                    readTextBox.AppendText("Which do you pick up?" + " Type:" + Environment.NewLine +
                                           "Paddle" + Environment.NewLine + "Sphere" + Environment.NewLine + "Banana" +
                                           Environment.NewLine);
                    return;
                case GamePhase.WeaponSelect: // We switch game phases here.
                    Character.Weapon chosenWeapon = null;

                    if (input.Contains("paddle")) chosenWeapon = new Character.Weapon("Ping Pong Paddle", 3, 2);
                    else if (input.Contains("sphere")) chosenWeapon = new Character.Weapon("Game Sphere", 2, 3);
                    else if (input.Contains("banana")) chosenWeapon = new Character.Weapon("Banana", 1, 1);

                    if (chosenWeapon == null)
                    {
                        readTextBox.AppendText("Invalid weapon choice. Please type 'paddle', 'sphere', or 'banana'." +
                                               Environment.NewLine);
                        return;
                    }

                    player.EquipWeapon(chosenWeapon);


                    updateStats(AsciiArtBank.GetBaabArt);

                    readTextBox.Clear();
                    currentEnemy = cast["Baaahhb"]; //Adds Baaahhb to Combat Current Emeny NPC list.
                    readTextBox.AppendText("Megan unleashes " + currentEnemy.Name + " to attack you in her proxy." + Environment.NewLine);
                    readTextBox.AppendText($"The first battle begins: You vs " + currentEnemy.Name + " with your " + chosenWeapon.Name + Environment.NewLine);

                    currentPhase = GamePhase.PlayerTurn_1;
                    
                    nearbyNPCs.Clear(); // Clears nearby NPCs list.
                    nearbyNPCs.Add(cast["Megan"]); //Adds Megan to nearby NPC list.
                    
                    return;

                case GamePhase.PlayerTurn_1:
   
                    readTextBox.Clear();

                    if (player.EquippedWeapon.Name == "Ping Pong Paddle")
                    {
                        updateStats(AsciiArtBank.GetPingpongArt);
                        readTextBox.AppendText("Choose your move: " + Environment.NewLine +
                                               "1. Ping Pong Strike " + currentEnemy.Name + Environment.NewLine + // Offensive Move
                                               "2. Chuck the paddle at " + Environment.NewLine + // Defensive Move
                                               "3. Run Away"); // Run Away Move
                    } //Ping Pong Paddle set of attacks
                    else if (player.EquippedWeapon.Name == "Game Sphere")
                    {
                        updateStats(AsciiArtBank.GetGamesphereArt2);
                        readTextBox.AppendText("Choose your move: " + Environment.NewLine +
                                               "1. Spherical Attack!" + Environment.NewLine + // Offensive Move
                                               "2. Chuck the gamesphere at " + currentEnemy.Name + Environment.NewLine + // Defensive Move
                                               "3. Run Away"); // Run Away Move
                    } //Game Sphere set of attacks
                    else if (player.EquippedWeapon.Name == "Banana")
                    {
                        updateStats(AsciiArtBank.GetBananaArt);
                        readTextBox.AppendText("Choose your move: " + Environment.NewLine +
                                               "1. Slippery Smash" + Environment.NewLine + // Offensive Move
                                               "2. Peel the Banana and chuck it at " + currentEnemy.Name + Environment.NewLine + // Defensive Move
                                               "3. Run Away"); // Run Away Move.
                    } //Banana set of attacks

                    currentPhase = GamePhase.Round_1;
                    
                    return;
                    
                    case GamePhase.Round_1: // This is where the combat action happens.

                    if (input == "1" || input == "2" || input == "3")
                    {
                        playersTurn(int.Parse(input));

                        if (currentEnemy.Health <= 0)
                        {
                            MessageBox.Show($"You defeated {currentEnemy.Name}!", "Victory");
                            currentPhase = GamePhase.Victory;
                            ResetGame();
                            return;
                        }

                        currentPhase = GamePhase.EnemyTurn_1;
                        int enemyChoice = new Random().Next(1, 4);
                        EnemyTakesTurn(enemyChoice);

                        if (player.Health <= 0)
                        {
                            MessageBox.Show("You have been defeated!", "Defeat");
                            currentPhase = GamePhase.Defeat;
                            ResetGame();
                            return;
                        }

                        currentPhase = GamePhase.PlayerTurn_1;
                    }

                    return;

        }
        }
    }
    private void writeTextBox_KeyPress(object sender, KeyPressEventArgs e) // Handles individual key input (e.g. filters what the user is allowed to type in the main menu).
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            return; // Let KeyDown handle Enter fully
        }

        if (currentPhase == GamePhase.MainMenu)
        {
            char lowerChar = char.ToLower(e.KeyChar);
            string previewInput = (writeTextBox.Text + lowerChar).ToLower();

            if (!( "play".StartsWith(previewInput) ||
                   "credits".StartsWith(previewInput) ||
                   "exit".StartsWith(previewInput) ||
                   previewInput == "1" ||
                   previewInput == "2" ||
                   previewInput == "x"))
            {
                e.Handled = true;
                return;
            }

            e.Handled = false; // Allow valid typing
        }
    }
    private void Form1_Load(object sender, EventArgs e) // This is the load event, or Event Begin Play.
    
    {
        statTextBox.Text = titleArt;
        statTextBox.Font = new Font("Consolas", 8);
        statTextBox.WordWrap = false;
        statTextBox.ScrollBars = ScrollBars.None; // If you're trying to avoid scrollbars
        
        readTextBox.Clear();
        readTextBox.AppendText("Drake and Josh's Really Big Text Adventure!" + Environment.NewLine);
        readTextBox.AppendText("[1] Play" + Environment.NewLine);
        readTextBox.AppendText("[2] Credits" + Environment.NewLine);
        readTextBox.AppendText("[X] Exit" + Environment.NewLine);
        SoundLibrary.PlayTransitionSound();
        
        currentPhase = GamePhase.MainMenu;
    }
}
}