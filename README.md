
Contents:

- [How to run project](#how-to-run-project)
- [Extra features](#extra-features)
- [How to config game value](#how-to-config-game-value)
- [Class diagram](#class-diagram)
- [Techinal Decision](#techinal-decision)

## How to run project

- [Download and Play](https://drive.google.com/file/d/1Z__xrLDYNwXQD7PEeu7OCxx_P9n00tsq/view?usp=drive_link)
or
- Clone from github
- Open "StartScene" at Assets/Scenes/StartScene.unity
![Start Scene](document/img/startscene.png)

## Extra features

- Character's Level and EXP system.
   - Player will get more EXP if you have more allies when the turn ends.
   - Monster will get more EXP when the turn ends.
   - Character's Status increase when character's level is increase.
- Every period of turns. character will spawn (the turn can be config)
   - Character will spawn every 5 turns
   - monster will spawn with a random amount every 10 turns

## How to config game value
Config file path:
Assets/Bundles/Data/GameConfig.asset
You can Adjust on inspector.
![Config](document/img/config.jpg)

## Class diagram
![Class Diagram](document/img/class-diagram.png)

class diagram:
https://drive.google.com/file/d/1GS3m6tgXnBQ4GNG9-y2y1dQ9jqYQwCy_/view?usp=drive_link

## Techinal Decision
[Place Obstacle](document/place-obstacle.md)

[Status Formula](document/status-formula.md)

