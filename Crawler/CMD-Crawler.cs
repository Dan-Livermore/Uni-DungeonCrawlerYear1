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
        ///These variables are used to track the users next input and make the code more readable.
        ///</summary>
        public enum PlayerActions { NOTHING, NORTH, EAST, SOUTH, WEST, PICKUP, ATTACK, QUIT };
        private PlayerActions action = PlayerActions.NOTHING;

        /// <summary>
        /// These variables prevent the game from running until it is correctly initalised.
        /// </summary>
        private bool active = true;
        private bool started = false;
        private bool working = false;
        private bool advanced = false;

        /// <summary>
        /// These objects will store the important global variables needed for the game to process. 
        /// It stores a copy of the map before any changes have been made,
        /// the map that gets updated when the player moves 
        /// and the current position of the player.
        /// </summary>
        private char[][] originalMap = new char[0][];
        private char[][] Map = new char[0][];
        private int[] position = { 0, 0 };

        ///<summary>
        /// Creates globals used in advanced functionality
        /// </summary>
        private char current = 'S';

        private int playerhealth = 2;
        private int playermoves = 0;
        private int playercoins = 0;
        private int playerkills = 0;

        private int[] monsterposition = { 0, 0 };
        private int monsterhealth = 1;
        private int monstercoins = 0;



        ///<summary> 
        ///Takes the users input from keyboard and returns it as a string. 
        ///If the game hasn't started sentences can be enterred to initalise the game.
        ///but once it has started it only accepts single inputs to improve the flow of the game.
        ///</summary>
        private string ReadUserInput()
        {
            string inputRead = string.Empty;
            if (advanced == true && started == true)
            {
                
                inputRead = Console.ReadKey().Key.ToString();
                //if (inputRead == Key.Spacebar)
                //    inputRead = ConsoleKey.Spacebar.ToString();
            }
            else
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
            input = input.ToLower();
            // Starts advanced functionality
            if (input == "advanced")
            {
                advanced = true;
                Console.WriteLine("Started Advanced Mode");
            }
            if (input == "basic")
            {
                advanced = true;
                Console.WriteLine("Returned to Basic Functionality");
            }

            // Needs to load map before game loop can start
            if (input == "load simple.map")
            {
                InitializeMap("Simple.Map");
                Console.WriteLine("Loaded Simple.Map");
            }
            if (input == "load simple2.map")
            {
                InitializeMap("Simple2.Map");
                Console.WriteLine("Loaded Simple.Map 2");
            }
            if (input == "load advanced.map")
            {
                InitializeMap("Advanced.Map");
                Console.WriteLine("Loaded Advanced.Map");
            }

            // Starts game
            if (input == "play")
            {
                if (started == true && working == true)
                    action = PlayerActions.NOTHING;
                if (started == false && working == false)
                {
                    active = true;
                    started = true;
                    working = true;
                }
            }

            if (started == true)
            {
                // If the game has been initalized, take the users next input
                if (input == "w")
                    action = PlayerActions.NORTH;
                if (input == "a")
                    action = PlayerActions.WEST;
                if (input == "s")
                    action = PlayerActions.SOUTH;
                if (input == "d")
                    action = PlayerActions.EAST;

                // Advanced functionality
                if (advanced == true && input == "p")
                    action = PlayerActions.PICKUP;
                if (advanced == true && input == "e") //ConsoleKey.Spacebar == true)
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
                    // If the next space is a wall, don't move the player
                    if (Map[position[0]-1][position[1]] != '#' && Map[position[0] - 1][position[1]] != 'M')
                    {
                        Map[position[0]][position[1]] = current;
                        position[0] -= 1;
                        current = Map[position[0]][position[1]];
                        // If the next position is the goal, end the game
                        if (Map[position[0]][position[1]] == 'X')
                        {
                            this.active = false;
                            started = false;
                        }
                        else
                        {
                            Map[position[0]][position[1]] = '@';
                        }
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
                        {
                            this.active = false;
                            started = false;
                        }
                        else
                        {
                            Map[position[0]][position[1]] = '@';
                        }
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
                        {
                            this.active = false;
                            started = false;
                        }
                        else
                        {
                            Map[position[0]][position[1]] = '@';
                        }
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
                        {
                            this.active = false;
                            started = false;
                        }
                        else
                        {
                            Map[position[0]][position[1]] = '@';
                        }
                    }
                }
                if (action == PlayerActions.PICKUP)
                {
                    if (current == 'C')
                    {
                        current = '-';
                        playercoins += 1;
                    }
                }
                if (action == PlayerActions.ATTACK)
                {
                    if (Map[position[0] - 1][position[1]] == Map[monsterposition[0]][monsterposition[1]] || Map[position[0] + 1][position[1]] == Map[monsterposition[0]][monsterposition[1]] || Map[position[0]][position[1] - 1] == Map[monsterposition[0]][monsterposition[1]] || Map[position[0]][position[1] + 1] == Map[monsterposition[0]][monsterposition[1]])
                    {
                        monsterhealth -= 1;
                        if (monsterhealth < 1)
                        {
                            Map[monsterposition[0]][monsterposition[1]] = '-';
                            playerkills += 1;

                        }
                    }
                }
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
            if (advanced = true && started == true)
            {
                PrintMapAdvanced();
                return true;
            }
            try
            {
                //for (int i = 0; i < Map.Length; i++)
                //    Console.WriteLine(Map[i]);
                Console.WriteLine(Map[0]);
                Console.Write(Map[1]);
                Console.WriteLine(" Controls.");
                Console.Write(Map[2]);
                Console.WriteLine(" W - Up              Moves: " + playermoves);
                Console.Write(Map[3]);
                Console.WriteLine(" A - Left            Coins Collected: " + playercoins);
                Console.Write(Map[4]);
                Console.WriteLine(" S - Down            Monsters Defeated: " + playerkills);
                Console.Write(Map[5]);
                Console.WriteLine(" D - Right");
                Console.WriteLine(Map[6]);
                Console.Write(Map[7]);
                Console.WriteLine(" P - Collect Coins");
                Console.Write(Map[8]);
                Console.WriteLine(" SPACE - Attack");
                Console.WriteLine(Map[9]);
            }
            catch
            {
                Console.WriteLine("Choose Map");
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

            return true;
        }





        /// <summary>
        /// Replaces the print map method for advanced mode to display the map with colours to improve the UX.
        /// </summary>
        /// <returns></returns>
        public bool PrintMapAdvanced()
        {
            ConsoleColor colour = Console.ForegroundColor;
            for (int j = 0; j < Map.Length; j++)
            {
                for (int i = 0; i < 31; i++)
                {
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
                    Console.ForegroundColor = colour;
                    Console.Write(Map[j][i]);
                }
                
                Console.Write("\n");
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
            string path = Environment.CurrentDirectory + "/maps/Simple.map"; //" + mapName;
            // No try loop needed as there should always be the folder of maps.
            Text = File.ReadAllLines(path);
            Console.WriteLine(mapName);
            Console.WriteLine(path);
            char[][] newMap = new char[Text.Length][];

            for (int y = 0; y < Text.Length; y++)
            {
                newMap[y] = Text[y].ToCharArray();
            }
            
            // Map becomes the version that gets updated when the player moves and originalMap is a copy of the original.
            originalMap = newMap;
            Map =  newMap;
            GetPlayerPosition();
            if (advanced == true)
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

            //again unneccessary
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
            for (int y = 0; y < Map.Length - 1; y++)
            {
                for (int x = 0; x < 31; x++) // Fails if put Map[x].Length-1 because sometimes its 35 not 31, despite not existing
                {
                    if (Map[y][x] == '@' || Map[y][x] == 'S') //(Map[y][x] == '@' ||
                    {
                        position[0] = y;
                        position[1] = x;
                        Map[y][x] = '@';
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