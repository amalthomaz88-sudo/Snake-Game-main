# Snake Game – README

## 1. How to Run the Game

1. Open the project in **Unity 6 LTS**.
2. Open the `MainScene` scene from `Assets/ProjectMain/Scenes`.
3. Make sure the required ScriptableObject assets and prefab references are assigned in the Inspector.
4. Press the **Play** button in the Unity Editor.
5. Select **Start** from the main menu to begin playing.

## 2. Controls

| Input | Action |
|---|---|
| Arrow Keys | Move the snake |
| W, A, S, D | Move the snake |
| Gamepad D-Pad | Move the snake |
| Gamepad Left Stick | Move the snake |
| Escape | Pause the game |

The snake cannot immediately move in the opposite direction.

## 3. Architecture Overview

The project uses a modular, data-driven architecture.

- **Grid System:** Handles grid dimensions, cell positions, and grid configuration.
- **Snake System:** Handles snake movement, body positions, growth, and shrinking.
- **Input System:** Reads keyboard and gamepad input through a separate input reader.
- **Food System:** Uses ScriptableObjects to define normal, bonus, and shrink food types.
- **Level System:** Uses LevelData ScriptableObjects to define different obstacle layouts.
- **Game State System:** Manages Main Menu, Playing, Paused, and Game Over states.
- **UI System:** Handles menus, score display, high score, pause, and restart.
- **Save System:** Uses an `ISaveService` interface and PlayerPrefs implementation to save the high score.

ScriptableObjects make it possible to configure grid settings, food properties, and level layouts without changing the core gameplay code.

## 4. Assumptions Made

- The game is designed primarily for landscape orientation.
- The snake moves on a fixed grid.
- The snake starts with two segments.
- The minimum snake length is three when shrink food is collected.
- Bonus food disappears after five seconds if not collected.
- The game uses a single-player scoring system.
- Levels and food types are configured through ScriptableObject assets.

## 5. What Was Skipped and Why

- Online multiplayer and leaderboards were not implemented because they were outside the core assignment scope.
- Sound effects and background music were not prioritized because the focus was on gameplay functionality.
- Advanced animations and visual effects were kept minimal to prioritize core mechanics.
- Automated testing coverage is limited; additional tests would be useful for movement, collision, food effects, and saving.

## 6. Future Improvements

With more development time, I would improve:

- Add more levels and obstacle layouts.
- Add sound effects, music, and improved animations.
- Add mobile touch controls.
- Improve the snake movement and visual transitions.
- Expand automated Edit Mode tests.
- Improve save-data validation and add more robust storage.
- Add a level selection screen and difficulty settings.

## 7. Total Time Spent

**Estimated development time:** [48hr]

This includes project setup, gameplay implementation, UI development, testing, debugging, and documentation.

## 8. AI Tools Used

**ChatGPT (OpenAI):**
- Helped plan the project architecture.
- Assisted with C# script development and debugging.
- Provided guidance for Unity Inspector setup.
- Helped implement gameplay systems, food types, and level data.
- Assisted with documentation and explanations.

All AI-generated suggestions and code were reviewed and integrated into the Unity project during development.