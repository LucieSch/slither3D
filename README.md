# 3D Slither.io Clone (Offline)

This project is a 3D Slither.io-inspired game developed in Unity. It was created during a 5-month internship **before the start of my university studies**, representing my first experience with programming and game development.

## Features

- 3D snake gameplay inspired by Slither.io
- Player-controlled snake with smooth movement
- NPC snakes with simple autonomous behavior
- Food collection and growth mechanics
- Game over screen with restart options

## Technologies

- Unity 2020.3.27f1 (C#)
- Basic physics and collision handling

## How to Run

1. Open the project in Unity Hub
2. Load the scene: `StartMenu`
3. Press Play in the Unity Editor

## Controls

- **Arrow Keys** to move the snake
- **Spacebar** to boost

## Gameplay

- Enter your player name in the start menu and begin the game
- Collect food to grow your snake
- Avoid collisions with other snakes
- When the player dies, a game over screen appears with options to:
  - Restart
  - Start as a new player
  - Quit the game

## NPC Behavior

The NPC snakes are based on simple rule-based logic:

- Each snake has an extended invisible collider in front of its head
- If food enters this area, the snake moves toward it
- If another snake is detected, it attempts to move away

This approach creates basic but effective autonomous movement.

## Notes

This project reflects my early programming experience. Today, I would approach structure and AI design differently, but it demonstrates my initial understanding of game mechanics and logic implementation.

## Context

Internship before Bachelor's degree<br>
2021
