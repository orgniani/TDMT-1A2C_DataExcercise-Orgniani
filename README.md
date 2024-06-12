# TDMT-1A2C_DataExcercise
What's asked for this task:
- Game Boot and additive scene control.
- NavigationManager.
- Event system (centralized/decentralized).
- Reference system (refManager/DataSource).
### Clarifications
- El código entregado puede estar basado en el dado en clase, pero debe ser más que una simple copia. De no aportar nada a lo visto en clase, el trabajo quedará automaticamente desaprobado.
- Una lógica equivalente a la dificultad de los sistemas vistos en clase tendrá como resultado una nota aprobada de 4, pudiendo incrementar la nota resultado a partir de la complejidad de los sitemas, sus aportes extra y el pulido que se le otorgue.
### Contexto
- Tenemos un juego básico, con un personaje que debe moverse y un enemigo que debe perseguirlo.
    - La lógica de movimiento del personaje y la lógica de persecución del enemigo serán provistas en la arquitectura base.
    - Se debe implementar un sistema de referencias que permita al enemigo conseguir la referencia al jugador.
    - Se debe implementar un sistema de eventos que comunique la lógica de input con la lógica de movimiento del personaje.
    - Al enemigo atrapar al personaje, se debe levantar un evento en el sistema a implementar que permita mostrar la pantalla de derrota con el menú para volver.
- Tenemos un conjunto de escenas, formado principalmente por una escena raíz o boot, una escena de menus y una escena de juego.
    - Se debe proveer un sistema de control de escenas que contemple la posibilidad de cargar multiples escenas a la vez, con un nivel equivalente al trabajado en clase.
 
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
