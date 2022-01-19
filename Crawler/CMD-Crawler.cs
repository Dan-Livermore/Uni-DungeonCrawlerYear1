using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Crawler
{
    /**
     * The main class of the Dungeon Crawler Application
     * 
     * You may add to your project other classes which are referenced.
     * Complete the templated methods and fill in your code where it says "Your code here".
     * Do not rename methods or variables which already exist or change the method parameters.
     * You can do some checks if your project still aligns with the spec by running the tests in UnitTest1
     * 
     * For Questions do contact us!
     */
    public class CMDCrawler
    {
        ///<summary>
        ///These variables hold the player's current action so it can be used in the Update() method.
        ///Clearly shows the action rather than storing an integer that is assigned the same value.
        ///</summary>
        public enum PlayerActions { NOTHING, NORTH, EAST, SOUTH, WEST, PICKUP, ATTACK, QUIT };
        private PlayerActions action = PlayerActions.NOTHING;

        /// <summary>
        /// These variables prevent the game from running until it is correctly initalised.
        /// </summary>
        private bool active = true;
        // Active stores the current status of the program. Used to make sure the tests pass correctly / complete the cycle in main();
        private bool started = false;
        // This one makes sure the game has started running so in advanced mode the game knows to take single character inputs rather than whole words.
        private bool working = false;
        // Working is used to prevent the user for entering play twice.
        private bool advanced = false;
        // This variable is used to enable the advanced features of the program.

        /// <summary>
        /// These objects will store the important global variables needed for the game to process. 
        /// It stores a copy of the map before any changes have been made,
        /// the  current map that gets updated when the player moves ,
        /// the current position of the player on the map
        /// and the tile that the player character is currently over.
        /// </summary>
        private char[][] originalMap = new char[0][];
        private char[][] Map = new char[0][];
        private int[] position = { 0, 0 };
        private char current = '-';

        ///<summary>
        /// Creates globals used in advanced functionality used to present the players stats in PrintMapAdvanced();
        /// </summary>

        private int playerhealth = 2;
        // Player health is stored as 2 so the player can take 2 hits from the monster before losing the game.
        private int playermoves = 0;
        // This counter increments after the player completes an action, showing how many turns it takes to complete the map.
        private int playercoins = 0;
        // This stores the number of coins that the player has picked up when pressing the "P" button.
        private int playerkills = 0;
        // This stores the amount of monsters that the player has defeated.
        private int playerdeaths = 0;
        // This stores the number of times the player has been defeated by the monsters.

        /// <summary>
        /// These variables store the same data that is required for the player movement but it is used for moving the single monster on Simple.map
        /// </summary>
        private int[] monsterposition = { 0, 0 };
        private int monsterhealth = 1;
        private char current2 = '-';

        ///<summary> 
        ///Takes the users input from keyboard and returns it as a string. 
        ///If the game hasn't started sentences can be enterred to initalise the game.
        ///but once it has started it only accepts single inputs to improve the flow of the game.
        ///</summary>
        private string ReadUserInput()
        {
            // Flavour text to help the player start the game
            if (started == false && working == false)
                Console.WriteLine("Select Mode, Load Map then Type Play.");
            string inputRead = string.Empty;
            if (advanced == true && started == true)
            {   // Used to set up the game and if playing in advanced mode, the player does not need to press enter for their movement to register.
                inputRead = Console.ReadKey().Key.ToString();
            }
            else
                // Requires the enter key to be pressed when the game is loaded.
                inputRead = Console.ReadLine();
            return inputRead;
        }

        /**
         * Processed the user input string
         * 
         * takes apart the user input and does control the information flow
         *  * initializes the map ( you must call InitializeMap)
         *  * starts the game when user types in Play
         *  * sets the correct playeraction which you will use in the Update
         *  
         *  DO NOT read any information from command line in here but only act upon what the method receives.
         */
        public void ProcessUserInput(string input)
        {
            // Basic validation of input
            // Starts advanced functionality
            if (input == "advanced")
            {
                advanced = true;
                Console.WriteLine("Started Advanced Mode");
            }
            // Returns to the basic functionality
            if (input == "basic")
            {
                advanced = false;
                Console.WriteLine("Returned to Basic Functionality");
            }

            // Needs to load map before game loop can start
            if (input == "load Simple.map")
            {
                InitializeMap("Simple.map");
                Console.WriteLine("Loaded Simple.Map");
            }

            // Starts game
            if (input == "play")
            {
                // Prevents the game from crashing when entering play twice / again once the game has loaded.
                if (started == true && working == true)
                    action = PlayerActions.NOTHING;
                // Starts the game.
                if (started == false && working == false)
                {
                    active = true;
                    started = true;
                    working = true;
                }
            }

            if (started == true)
            {
                input = input.ToUpper();
                // If the game has been initalized, take the users next input
                if (input == "W")
                    action = PlayerActions.NORTH;
                if (input == "A")
                    action = PlayerActions.WEST;
                if (input == "S")
                    action = PlayerActions.SOUTH;
                if (input == "D")
                    action = PlayerActions.EAST;

                // Advanced functionality
                if (advanced == true && input == "P")
                    action = PlayerActions.PICKUP;
                if (advanced == true && input == "Spacebar")
                    action = PlayerActions.ATTACK;
            }
        }

        /**
         * The Main Game Loop. 
         * It updates the game state.
         * 
         * This is the method where you implement your game logic and alter the state of the map/game
         * use playeraction to determine how the character should move/act
         * the input should tell the loop if the game is active and the state should advance
         * 
         * Returns true if the game could be updated and is ongoing
         */
        ///<summary>
        ///If the game has started, move the player character by 1 in the direction of the PLAYERACTION enum.
        ///</summary>
        public bool Update(bool active)
        {
            if (started == true)
            {
                if (action == PlayerActions.NORTH)
                {
                    // If the next space is a wall or a monster, don't move the player
                    if (Map[position[0]-1][position[1]] != '#' && Map[position[0] - 1][position[1]] != 'M')
                    {
                        // Places previous tile
                        Map[position[0]][position[1]] = current;
                        // Updates position
                        position[0] -= 1;
                        // Stores the tile that the player will be placed upon
                        current = Map[position[0]][position[1]];
                        // If the next position is the goal, end the game
                        if (Map[position[0]][position[1]] == 'X')
                            if (advanced == true)
                                // Gives the user the option to replay the same map again or quit
                                TerminateGame();
                            else
                            {
                                // In basic mode, the user is forced to quit
                                this.active = false;
                                started = false;
                            }
                        else
                            // Places the player character
                            Map[position[0]][position[1]] = '@';
                    }
                }
                if (action == PlayerActions.SOUTH)
                {
                    if (Map[position[0]+1][position[1]] != '#' && Map[position[0]+1][position[1]] != 'M')
                    {
                        Map[position[0]][position[1]] = current;
                        position[0] += 1;
                        current = Map[position[0]][position[1]];
                        if (Map[position[0]][position[1]] == 'X')
                            if (advanced == true)
                                TerminateGame();
                            else
                            {
                                this.active = false;
                                started = false;
                            }
                        else
                            Map[position[0]][position[1]] = '@';
                    }
                }
                if (action == PlayerActions.WEST)
                {
                    if (Map[position[0]][position[1]-1] != '#' && Map[position[0]][position[1]-1] != 'M')
                    {
                        Map[position[0]][position[1]] = current;
                        position[1] -= 1;
                        current = Map[position[0]][position[1]];
                        if (Map[position[0]][position[1]] == 'X')
                            if (advanced == true)
                                TerminateGame();
                            else
                            {
                                this.active = false;
                                started = false;
                            }
                        else
                            Map[position[0]][position[1]] = '@';
                    }
                }
                if (action == PlayerActions.EAST)
                {
                    if (Map[position[0]][position[1]+1] != '#' && Map[position[0]][position[1]+1] != 'M')
                    {
                        Map[position[0]][position[1]] = current;
                        position[1] += 1;
                        current = Map[position[0]][position[1]];
                        if (Map[position[0]][position[1]] == 'X')
                            if (advanced == true)
                                TerminateGame();
                            else
                            {
                                this.active = false;
                                started = false;
                            }
                        else
                            Map[position[0]][position[1]] = '@';
                    }
                }
                // In advanced mode, the player can pick up coins when standing on the same tile of the map.
                if (action == PlayerActions.PICKUP)
                {
                    // if the current position is a coin, replace the tile on the map and increment the coin counter.
                    if (current == 'C')
                    {
                        current = '-';
                        playercoins += 1;
                    }
                }
                if (action == PlayerActions.ATTACK)
                {
                    Console.WriteLine(monsterhealth);
                    // Checks that the postions one tile adjacent of the player contains a monster.
                    if (Map[position[0] - 1][position[1]] == Map[monsterposition[0]][monsterposition[1]] || Map[position[0] + 1][position[1]] == Map[monsterposition[0]][monsterposition[1]] || Map[position[0]][position[1] - 1] == Map[monsterposition[0]][monsterposition[1]] || Map[position[0]][position[1] + 1] == Map[monsterposition[0]][monsterposition[1]])
                    {
                        // Decrement monster's health and if it goes to 0, the monster dies and is removed from the map.
                        monsterhealth -= 1;
                        Console.WriteLine("attack" + monsterhealth);
                        if (monsterhealth < 1)
                        {
                            Map[monsterposition[0]][monsterposition[1]] = '-';
                            playerkills += 1;

                        }
                    }
                }
                // Used to show how many actions it took the player to complete the map, show in PrintMapAdvanced();
                playermoves += 1;
            }
            return working;
        }

        /**
         * The Main Visual Output element. 
         * It draws the new map after the player did something onto the screen.
         * 
         * This is the method where you implement your the code to draw the map ontop the screen
         * and show the move to the user. 
         * 
         * The method returns true if the game is running and it can draw something, false otherwise.
        */
        ///<summary>
        ///Outputs the map onto the window.
        /// </summary>
        public bool PrintMap()
        {
            // If advanced mode is enabled, print the map in colour
            if (advanced == true)
                PrintMapAdvanced();
            else
            {
                // Displays the map line by line onto the window.
                for (int i = 0; i < Map.Length; i++)
                    Console.WriteLine(Map[i]);
            }
            return true;
        }

        /**
         * Additional Visual Output element. 
         * It draws the flavour texts and additional information after the map has been printed.
         * 
         * This is the method does not need to be used unless you want to output somethign else after the map onto the screen.
         * 
         */
        public bool PrintExtraInfo()
        {
            // Your code here
            // If the advanced mode is enabled and the game has started, let the monster move or attack and print the updated map.
            if (advanced == true && started == true)
            {
                DoMonsterMovement();
                PrintMapAdvanced();
            }
            return true;
        }

        /// <summary>
        /// Replaces the print map method for advanced mode to display the map with colours to improve the UX.
        /// </summary>
        /// <returns></returns>
        public bool PrintMapAdvanced()
        {
            // Gets colour of the current item
            ConsoleColor colour = Console.ForegroundColor;
            for (int j = 0; j < Map.Length; j++)
            {
                for (int i = 0; i < Map[j].Length; i++)
                {
                    // Iterates through the list and changes the colours of each element.
                    if (Map[j][i] == '#')
                        colour = ConsoleColor.White;
                    else if (Map[j][i] == '-')
                        colour = ConsoleColor.DarkGray;
                    else if (Map[j][i] == '@')
                        colour = ConsoleColor.Cyan;
                    else if (Map[j][i] == 'C')
                        colour = ConsoleColor.Yellow;
                    else if (Map[j][i] == 'M')
                        colour = ConsoleColor.Red;
                    else if (Map[j][i] == 'X')
                        colour = ConsoleColor.Green;
                    // Print the element with the new colour.
                    Console.ForegroundColor = colour;
                    Console.Write(Map[j][i]);
                } 
                // Starts new line
                Console.Write("\n");
            }

            // If the game has started, display controls and the variables that are updated as the game continues.
            if (started == true)
            {
                Console.WriteLine("  Controls.");
                Console.WriteLine("    W - Up              Moves:  " + playermoves);
                Console.WriteLine("    A - Left            Health: " + playerhealth + " / 2");
                Console.WriteLine("    S - Down            Coins Collected: " + playercoins);
                Console.WriteLine("    D - Right           Monsters Defeated: " + playerkills);
                Console.WriteLine("    P - Collect Coins   Deaths: " + playerdeaths);
                Console.WriteLine("    SPACE - Attack      ");
            }
            return true;
        }

        /**
        * Map and GameState get initialized
        * mapName references a file name 
        * Do not use abosolute paths but use the files which are relative to the executable.
        * 
        * Create a private object variable for storing the map in Crawler and using it in the game.
        */

        ///<summary>
        ///Converts the map from the string input in the txt file into a 2-dimensional char array.
        /// </summary>
        public bool InitializeMap(String mapName)
        {
            bool initSuccess = false;
            string[] Text;
            // Finds correct directory
            string path = Environment.CurrentDirectory + "/maps/" + mapName;
            // No need to have a try loop needed as there should always be the folder of maps.
            // Reads all of the .map file.
            Text = File.ReadAllLines(path);

            // Creates local map that it can be stored in.
            char[][] newMap = new char[Text.Length][];

            for (int y = 0; y < Text.Length; y++)
            {
                // For every line of the map, and stores it in the local map.
                newMap[y] = Text[y].ToCharArray();
            }
            // The local map is stored into the object variables
            // Map becomes the version that gets updated when the player moves and originalMap is a copy of the original.
            originalMap = newMap;
            Map =  newMap;

            // Updates the object variables so the player is initialized and can move around the map.
            GetPlayerPosition();
            if (advanced == true)
                // The same thing is needed for the monster to move in advanced mode.
                GetMonsterPosition();
            initSuccess = true;

            return initSuccess;
        }

        /**
         * Returns a representation of the currently loaded map
         * before any move was made.
         * This map should not change when the player moves
         */
        public char[][] GetOriginalMap()
        {
            return originalMap;
        }

        /*
         * Returns the current map state and contains the player's move
         * without altering it 
         */
        public char[][] GetCurrentMapState()
        {
            // the map should be map[y][x]
            char[][] map = new char[0][];

            // Your code here
            return originalMap;
        }

        /**
         * Returns the current position of the player on the map
         * 
         * The first value is the y coordinate and the second is the x coordinate on the map
         */
        ///<summary>
        ///Finds the position of the player character on the map and updates the position object varaiable.
        /// </summary>
        public int[] 
            GetPlayerPosition()
        {
            for (int y = 0; y < Map.Length; y++)
            {
                for (int x = 0; x < 31; x++) // Fails if put Map[y].Length-1 because sometimes its 35 not 31, despite not existing
                {
                    // Iterates through the map and if there is the player or starter token, update the position object variables
                    if (Map[y][x] == '@' || Map[y][x] == 'S')
                    {
                        position[0] = y;
                        position[1] = x;
                        Map[y][x] = '@';
                        // Ends the for loops
                        y = Map.Length;
                        x = Map[Map.Length-1].Length;
                    }
                }
            }

            return position;
        }

        /// <summary>
        /// Advanced functionality that finds the position of the monsters so they can move / attack / pick up coins
        /// </summary>
        /// <returns></returns>
        public int[]
            GetMonsterPosition()
        {
                for (int y = 0; y < Map.Length - 1; y++)
                {
                    for (int x = 0; x < 31; x++) 
                    {
                        // Searches throughout the map and if its the monster token and if it is there update the global variable.
                        if (Map[y][x] == 'M')
                        {
                            monsterposition[0] = y;
                            monsterposition[1] = x;
                            y = Map.Length;
                            x = Map[Map.Length - 1].Length;
                        }
                    }
                }
            return monsterposition;
        }

        public bool DoMonsterMovement()
        {
            // If monster is not defeated, generate a random integer that is used to decide how the monster will move.
            //if (monsterhealth > 0)
            //{
            //    Random random = new Random();
            //    int randint = random.Next(8);

            //    //// If the player is adjacent to the monster, the monster will attack the player.
            //    //if (Map[monsterposition[0] - 1][monsterposition[1]] == Map[position[0]][position[1]] || Map[monsterposition[0] + 1][monsterposition[1]] == Map[position[0]][position[1]] || Map[monsterposition[0]][monsterposition[1] - 1] == Map[position[0]][position[1]] || Map[monsterposition[0]][monsterposition[1] + 1] == Map[position[0]][position[1]])
            //    //{
            //    //    // Each attack removes one point of health from the player and if the player's health is reduced to 0, remove the player from the map.
            //    //    // Increment the death counter and start the function to end the game in advanced mode.
            //    //    playerhealth -= 1;
            //    //    if (playerhealth < 1)
            //    //    {
            //    //        Map[position[0]][position[1]] = '-';
            //    //        Console.WriteLine("Get rekt you died. what a noob");
            //    //        playerdeaths += 1;
            //    //        TerminateGame();
            //    //        randint = 8;
            //    //    }
            //    //}
            //    // The random input decides which direction the monster moves in
            //    if (randint == 0)
            //    {
            //        // As long as it is not a wall or the player the monster can move
            //        if (Map[monsterposition[0] - 1][monsterposition[1]] != '#' && Map[monsterposition[0] - 1][monsterposition[1]] != '@')
            //        {
            //            // the previous position is replaced, the monsters co-ordinates are replaced and the monster token is placed back on the map.
            //            Map[monsterposition[0]][monsterposition[1]] = current2;
            //            monsterposition[0] -= 1;
            //            current2 = Map[monsterposition[0]][monsterposition[1]];
            //            // If the next position is the goal, end the game
            //            Map[monsterposition[0]][monsterposition[1]] = 'M';
            //        }
            //    }
            //    if (randint == 1)
            //    {
            //        if (Map[monsterposition[0] + 1][monsterposition[1]] != '#' && Map[monsterposition[0] + 1][monsterposition[1]] != '@')
            //        {
            //            Map[monsterposition[0]][monsterposition[1]] = current2;
            //            monsterposition[0] += 1;
            //            current2 = Map[monsterposition[0]][monsterposition[1]];
            //            Map[monsterposition[0]][monsterposition[1]] = 'M';
            //        }
            //    }
            //    if (randint == 2)
            //    {
            //        if (Map[monsterposition[0]][monsterposition[1] - 1] != '#' && Map[monsterposition[0]][monsterposition[1] - 1] != '@')
            //        {
            //            Map[monsterposition[0]][monsterposition[1]] = current2;
            //            monsterposition[1] -= 1;
            //            current2 = Map[monsterposition[0]][monsterposition[1]];
            //            Map[monsterposition[0]][monsterposition[1]] = 'M';
            //        }
            //    }
            //    if (randint == 3)
            //    {
            //        if (Map[monsterposition[0]][monsterposition[1] + 1] != '#' && Map[monsterposition[0]][monsterposition[1] + 1] != '@')
            //        {
            //            Map[monsterposition[0]][monsterposition[1]] = current2;
            //            monsterposition[1] += 1;
            //            current2 = Map[monsterposition[0]][monsterposition[1]];
            //            Map[monsterposition[0]][monsterposition[1]] = 'M';
            //        }
            //    }
            //}
            return true;
        }

        /**
        * Returns the next player action
        * 
        * This method does not alter any internal state
        */
        public int GetPlayerAction()
        {
            int currentaction = (int)action;
            return currentaction;
        }

        /// <summary>
        /// Checks if the game has started.
        /// </summary>
        /// <returns></returns>
        public bool GameIsRunning()
        {
            bool running = false;
            if (started == true)
                running = true;
            else if (started == false && working == true)
                running = false;

            return running;
        }

        /// <summary>
        /// This loop is for advanced mode only and is used so the user has the option to replay the map.
        /// </summary>
        /// <returns></returns>
        public bool TerminateGame()
        {
            Console.WriteLine("Game Complete");
            Console.WriteLine("Replay Map or Quit");

            string input = Console.ReadLine();
            if (input == "Replay" || input == "replay")
            {
                // If replay is chosen the variables are reset and the maps re-initialized.
                current = '-';
                current2 = '-';
                playerhealth = 2;
                monsterhealth = 1;
                ProcessUserInput("load Simple.map");
                ProcessUserInput("play");
            }
            else
                // Otherwise the game ends as normal
                this.active = false;
            return false;
        }

        /**
         * Main method and Entry point to the program
         * ####
         * Do not change! 
        */
        static void Main(string[] args)
        {
            CMDCrawler crawler = new CMDCrawler();

            string input = string.Empty;
            Console.WriteLine("Welcome to the Commandline Dungeon!" +Environment.NewLine+ 
                "May your Quest be filled with riches!"+Environment.NewLine);
            
            // Loops through the input and determines when the game should quit
            while (crawler.active && crawler.action != PlayerActions.QUIT)
            {
                Console.WriteLine("Your Command: ");
                input = crawler.ReadUserInput();
                Console.WriteLine(Environment.NewLine);

                crawler.ProcessUserInput(input);
            
                crawler.Update(crawler.active);
                crawler.PrintMap();
                crawler.PrintExtraInfo();
            }

            Console.WriteLine("See you again" +Environment.NewLine+ 
                "In the CMD Dungeon! ");


        }


    }
}