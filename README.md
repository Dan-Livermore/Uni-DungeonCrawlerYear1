# 2021-COMP1000-Coursework 2
## Dan Livermore 10716150

This is the codebase for my dungeon crawler program.

The program is a game on the commandline, where the player journeys through a 2d map defeating monsters, collecting coins and going to the exit.

The user starts by entering: ``` load Simple.map ``` to load the map for the dungeon. <br />
And then enters: ``` play ``` to start their adventure. <br />

![](gif1.gif)

The user controls their character by using the WASD keys followed by ENTER on their keyboard. <br />

The elements on the map represent: <br />
> @ = The player
> X = The exit
> - = The floor
> # = Walls
> C = Coins to collect
> M = Monsters to defeat

To complete the game, the user must get to the X tile.

![](gif4.gif)

The user can also start the game in **ADVANCED MODE** by typing in ``` advanced ``` before loading the map <br />

![](gif2.gif)

This also enables other features. In this mode: <br />
- the user can now pick up coins by pressing the P key when their character is ontop of them
- the player can now attack the monsters that are 1 tile adjacent from them
- the user does not need to press ENTER after each input
- the monsters can move
- the user can quickly replay the game once they have escaped the dungeon.

The map is displayed with different colours for each element of the map
``` @ = Cyan ```
``` X = Green ```
```  - = Grey ```
``` # = White ```
> C = Yellow
> M = Red

![](gif3.gif)


Now try to escape the dungeon yourself!


### Resources used: <br />
[How to colour text](https://www.tutorialspoint.com/how-to-change-the-foreground-color-of-text-in-chash-console) <br />
[C# this parameter](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/this) <br />


## [Link to demo](https://youtu.be/tw-2qRdt0R0) 

