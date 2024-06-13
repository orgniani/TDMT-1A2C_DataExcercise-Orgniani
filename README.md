# TDMT-1A2C_DataExcercise
What's asked for this task:
- Game Boot and additive scene control.
- NavigationManager.
- Event system (centralized/decentralized).
- Reference system (refManager/DataSource).
### Clarifications
- The code delivered code can be based on the one done in class, but it cannot be a simple copy. If it does not add anything to what was covered in class, the work will automatically fail.
- Logic equivalent to the difficulty of the systems seen in class will result in a passing grade of 4, with the grade potentially increasing based on the complexity of the systems, their extra contributions, and the polish given to them.
### Context
- We have a basic game with a character that needs to move and an enemy that needs to chase them.
    - The character's movement logic and the enemy's chasing logic will be provided in the base architecture.
    - A reference system must be implemented to allow the enemy to get a reference to the player.
    - An event system must be implemented to communicate the input logic with the character's movement logic.
    - When the enemy catches the character, an event must be triggered in the system to show the defeat screen and a menu to restart.
- We have a set of scenes, mainly consisting of a root or boot scene (*Boot*), a menu scene (*Menus*), and a game scene (*World* and *Level1*, *Level2*).
    - A scene control system must be provided that contemplates the possibility of loading multiple scenes at once, with a level equivalent to what was worked on in class.
 
- The tasks meant for this repository are the following:
- The game must start at the boot scene and load all needed scenes aditively.
    - First, the menus scene must be loaded, where a navigation manager must control all menus, like it was shown in class [more info](https://github.com/jvarelaaloisio/TDMT-1A2C_Navigation).
    - From there, an options panel should be available, a credits panel, and a button to play the game.
    - When pressing the play button, the world scene must be loaded, along with the Level 1 scene.
    - When the player reaches the objective in the level 1, level 2 must be loaded.
    - When the player reaches the objective in the level 2, all world related scenes must be unloaded and a win screen must be shown in the menus scene.
    - If the player is touched by an enemy, it will die immediately and a lose screen will be show. This follows the same behaviour as the win screen.
    - All gameplay is provided with the project and no code from the student (aside from the TODOs) is necessary to acomplish this task.
- All TODOs must be completed successfully.
    -  To know where to find the TODO list, you can follow these instructions for [visual studio](https://learn.microsoft.com/en-us/visualstudio/ide/using-the-task-list?view=vs-2022) or [Rider](https://www.jetbrains.com/help/rider/Navigation_and_Search__Navigating_Between_To_do_Items.html).
- All provided architecture must not be modified.
