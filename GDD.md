# Fantasy Snake

Fantasy Snake is a Snake game with Fantasy RPG mixed in. The player controls a growing line of Heroes to fight against Monster.

## Gameplay

- The game will be played on a 16x16 grid board.
- At the start of the game, spawn a player-controlled Hero, a number of Obstacles, a number of collectable Heroes, and a number of Monsters.
- The player can do the following actions:
  - To move
    - Press WASD on the keyboard to move in the up, left, right, and down directions.
    - Pressing the D-Pad on the gamepad
    - The player cannot move in the opposite direction. If the player character is facing up, they can go left, up, or right, but not down.
    - The Hero beside the front character will move to occupy the same space as the previous Hero.
  - To rotate characters in the line
    - Pressing Q on the keyboard or the left shoulder button on the gamepad will rotate the second character in line to be the front character and the front character to be the last.
    - Pressing E on the keyboard or the right shoulder button on the gamepad will switch the last character in line to be the front character and the front character to be the second.
    - The rest of the line will be rotated accordingly.
    - Rotating does not make a move. The line of Heroes will be in the same position.
- Unlike Snake, there is no fixed interval for movement. This game uses a turn-based system, in which a turn is passed only when the player makes a move.
- Every character (both Heroes and Monsters) has two stats: Health and Attack.
  - There is a UI showing its Health and Attack for each character.
  - Stats are growing according to the time passed.
- Collision occurs when player character move in a direction that will occupy the space of other entity (Heroes line, Hero, Monster or Obstacle)
  - Colliding with the Hero line will result in the game being over immediately.
  - Colliding with a collectable Hero will collect that hero. The collected hero will be at the end of the line. The front character will occupy the space of that hero. Also, spawn new heroes according to the chance configuration.
  - Colliding with Obstacle will remove the front character. Move the rest of the line normally.
  - Colliding with the Monster will result in battle.
    - When a battle occurs, reduce Monster Health equal to Hero Attack and reduce Hero Health equal to Monster Attack at the same time.
    - If the Health of the Monster is 0 or lower, remove that enemy from the game and spawn new Monsters according to the chance configuration.
    - If the Health of the Hero is 0 or lower, remove that hero from the game and move the rest of the line normally.
- An obstacle can be either 1x1, 1x2, 2x1, or 2x2 in size.
- In the game over screen, display how many Monsters eliminated and a restart button. Pressing the restart button will restart the game.
- There is no win condition. The player can play this game endlessly.
- At least these values must be able to be configured.
  - Start number of entity spawn
  - Min and max stats
  - Growing coefficient (how fast the stats grow)
  - The chance of spawning (how many Heroes or Monsters will be spawned when removed)
