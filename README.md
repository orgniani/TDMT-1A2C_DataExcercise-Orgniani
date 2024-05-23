# TDMT-1A2C_DataExcercise-

### Aclaraciones
- El código entregado puede estar basado en el dado en clase, pero debe ser más que una simple copia. De no aportar nada a lo visto en clase, el trabajo quedará automaticamente desaprobado.
- Una lógica equivalente a la dificultad de los sistemas vistos en clase tendrá como resultado una nota aprobada de 4, pudiendo incrementar la nota resultado a partir de la complejidad de los sitemas, sus aportes extra y el pulido que se le otorgue.
### Contexto
- Tenemos un juego básico, con un personaje que debe moverse y un enemigo que debe perseguirlo.
    - La lógica de movimiento del personaje y la lógica de persecución del enemigo serán provistas en la arquitectura base.
    - Se debe implementar un sistema de referencias que permita al enemigo conseguir la referencia al jugador.
    - Se debe implementar un sistema de eventos que comunique la lógica de input con la lógica de movimiento del personaje.
    - Al enemigo atrapar al personaje, se debe levantar un evento en el sistema a implementar que permita mostrar la pantalla de derrota con el menú para volver.
- Al presionar el boton de ataque, el personaje debe atacar a su alrededor.
    - La lógica para el ataque y el input serán provistas en la arquitectura base.
- Tenemos un conjunto de escenas, formado principalmente por una escena raíz o boot, una escena de menus y una escena de juego.
    - Se debe proveer un sistema de control de escenas que contemple la posibilidad de cargar multiples escenas a la vez, con un nivel equivalente al trabajado en clase.
 
- The tasks meant for this repository are the following:
- All provided architecture must not be modified.
