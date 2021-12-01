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
        /**
         * use the following to store and control the next movement of the yser
         */
        public enum PlayerActions {NOTHING, NORTH, EAST, SOUTH, WEST, PICKUP, ATTACK, QUIT };
        private PlayerActions action = PlayerActions.NOTHING;

        /**
         * tracks if the game is running
         */
        private bool active = false;

        /// <summary>
        /// Use this object member to store the loaded map.
        /// </summary>
        private char[][] originalMap = new char[0][];                                  //This is the y axis for the simple map
        private char[][] Map = new char[0][];
        private int[] position = { 0, 0 };


        /**
         * Reads user input from the Console
         * 
         * Please use and implement this method to read the user input.
         * 
         * Return the input as string to be further processed
         * 
         */
        private string ReadUserInput()
        {
            string inputRead = string.Empty;

            // Your code here

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
            // Your Code here
            input = input.ToLower();
            if (input == "load simple.map")
                InitializeMap("Simple.Map");
            if (input == "play")
                active = true;
            if (input == "w")
                action = PlayerActions.NORTH;
            if (input == "a")
                action = PlayerActions.WEST;
            if (input == "s")
                action = PlayerActions.SOUTH;
            if (input == "d")
                action = PlayerActions.EAST;


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
        public bool Update(bool active)
        {
            bool working = false;

            // Your code here
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
        public bool PrintMap()
        {


            // Your code here


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

        /**
        * Map and GameState get initialized
        * mapName references a file name 
        * Do not use abosolute paths but use the files which are relative to the executable.
        * 
        * Create a private object variable for storing the map in Crawler and using it in the game.
        */
        public bool InitializeMap(String mapName)
        {
            bool initSuccess = false;

            // Your code here
            string[] Text;
            string path = Environment.CurrentDirectory + @"\maps\" + mapName;
            Text = File.ReadAllLines(path);
            char[][] newMap = new char[Text.Length][];
            for (int y = 0; y < Text.Length; y++)
            {
                    newMap[y] = Text[y].ToCharArray();
            }

            initSuccess = true;
            originalMap = newMap;
            Map = newMap;
            return initSuccess;
        }

        /**
         * Returns a representation of the currently loaded map
         * before any move was made.
         * This map should not change when the player moves
         */
        public char[][] GetOriginalMap()
        {
            // Your code here
            ////int length = 0;
            ////try
            ////{
            ////    length = originalMap.Length;
            ////}
            ////catch
            ////{
            ////    InitializeMap("Simple.Map");
            ////}
            ////char[][] map = new char[length][];

            //////This new char array is not needed as the map is read in the Initialize Map method.
            ////for (int i = 0; i < length; i++)
            ////{
            ////    map[i] = originalMap[i];
            ////}

            char[][] map = originalMap;
            return map;
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
        public int[] GetPlayerPosition()
        {
            
            bool Found = false;
            try
            {
                for (int x = 0; x < Map.Length-1; x++)            // this is brute forced and will be needed for the advanced map
                {
                    for (int y = 0; y < Map[x].Length-1; y++)
                    {
                        if (originalMap[y][x] == '@')
                        {
                            position[0] = y;
                            position[1] = x;
                            Found = true;
                            y = 10;
                            x = 31;
                        }
                        if (Found == false)
                        {
                            if (originalMap[y][x] == 'S')
                            {
                                position[0] = y;
                                position[1] = x;
                                y = 10;
                                x = 31;
                            }
                        }
                    }
                }
            }
            catch 
            {
                int[] position = { 0, 0 };
                return position;
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
            int action = 0;

            // Your code here
            //if (action = ((int)PlayerActions.NOTHING))
                //action = 0;

            return action;
        }


        public bool GameIsRunning()
        {
            bool running = false;
            // Your code here 
            if (active == true)
                running = true;

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