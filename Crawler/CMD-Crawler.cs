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
        public enum PlayerActions {NOTHING, NORTH, EAST, SOUTH, WEST, PICKUP, ATTACK, QUIT };
        private PlayerActions action = PlayerActions.NOTHING;

        /// <summary>
        /// These variables prevent the game from running until it is correctly initalised.
        /// </summary>
        private bool active = false;
        private bool working = false;

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
        ///Takes the users input from keyboard and returns it as a string. 
        ///If the game hasn't started sentences can be enterred to initalise the game.
        ///but once it has started it only accepts single inputs to improve the flow of the game.
        ///</summary>
        private string ReadUserInput()
        {
            string inputRead = string.Empty;
            if (active == true && working == true)
                inputRead = Console.ReadKey().ToString();
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
            // Needs to load map before game loop can start
            if (input == "load simple.map")
                InitializeMap("Simple.Map");
            if (active == true)
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
            }
            // Starts game
            if (input == "play")
            {
                if (active == true && working == true)
                    action = PlayerActions.NOTHING;
                if (active == false && working == false)
                {
                    active = true;
                    working = true;
                }
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
            if (active == true)
            {
                if (action == PlayerActions.NORTH)
                {
                    // If the next space is a wall, don't move the player
                    if(Map[position[0]-1][position[1]] != '#')
                    {
                        Map[position[0]][position[1]] = '-';
                        position[0] -= 1;
                        // If the next position is the goal, end the game
                        if (Map[position[0]][position[1]] == 'X')
                            this.active = false;
                        else
                            Map[position[0]][position[1]] = '@';
                    }
                }
                if (action == PlayerActions.SOUTH)
                {
                    if (Map[position[0]+1][position[1]] != '#')
                    {
                        Map[position[0]][position[1]] = '-';
                        position[0] += 1;
                        if (Map[position[0]][position[1]] == 'X')
                            this.active = false;
                        else
                            Map[position[0]][position[1]] = '@';
                    }
                }
                if (action == PlayerActions.WEST)
                {
                    if (Map[position[0]][position[1]-1] != '#')
                    {
                        Map[position[0]][position[1]] = '-';
                        position[1] -= 1;
                        if (Map[position[0]][position[1]] == 'X')
                            this.active = false;
                        else
                            Map[position[0]][position[1]] = '@';
                    }
                }
                if (action == PlayerActions.EAST)
                {
                    if (Map[position[0]][position[1]+1] != '#')
                    {
                        Map[position[0]][position[1]] = '-';
                        position[1] += 1;
                        if (Map[position[0]][position[1]] == 'X')
                            this.active = false;
                        else
                            Map[position[0]][position[1]] = '@';
                        
                    }
                }
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
            for (int i = 0; i < Map.Length-1; i++)
            {
                Console.WriteLine(Map[i]);
                return true;
            }
            return false;
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
            string path = Environment.CurrentDirectory + @"\maps\" + mapName;
            // No try loop needed as there should always be the folder of maps.
            Text = File.ReadAllLines(path);
            char[][] newMap = new char[Text.Length][];

            for (int y = 0; y < Text.Length; y++)
                newMap[y] = Text[y].ToCharArray();
            
            // Map becomes the version that gets updated when the player moves and originalMap is a copy of the original.
            originalMap = newMap;
            Map =  newMap;
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
                    if (Map[y][x] == '@' || Map[y][x] == 'S')
                    {
                        position[0] = y;
                        position[1] = x;
                        Map[y][x] = '@';
                        y = Map.Length;
                        x = Map[Map.Length].Length;
                    }
                }
            }

            return position;
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
            if (active == true)
                running = true;
            else if (active == false)
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