# ConsoleSnake

🐍 Snake game in the console


This is still in development

This was originally written in VB.NET, but I am now converting, refactoring and improving it in C#

## Flags
can either be added to the command as --\<flagname> or -\<first letter of name>

- help: Displays info about the command, can also be run using ? as an arguement

- quickexit: Exits immediatley and clears the console buffer when the program ends

- basicscore: using a more basic score output. Useful if the console isnt able to update fast enough for the more visual score output

- pacifist: makes the snake invulnerable. It can cross over itslef and wrap around the map

- mute: mutes the sounds effects

- cheese: the snake has holes in it

## Parameters
given in the form --\<parameter name>=\<value>

- fruitcount (integer): sets the amount of fruit to be available at once

- speed (integer): sets the speed of the snake. Measured in tiles/second

- gridheight (integer): sets the height of the grid

- gridwidth (integer): sets the width of the grid
