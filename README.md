



# MonopolyGo

A 3D single-player board game prototype inspired by Monopoly GO / Dice Dreams.
The player rolls dice, hops along a linear board, and collects fruit rewards into
a persistent inventory.

Built as a case study: linear (not square) board, JSON-driven map, dice that land
on the values the player enters (the result is the input, not physics RNG), and an
inventory that survives between sessions.

## Requirements

- Unity **6000.3.14f1** (Unity 6)
- No third-party packages. Only Unity's own UI (uGUI + TextMeshPro) and built-in
  systems are used.

## Running

1. Open the project in Unity.
2. Open `Assets/MonopolyGo/Scenes/GameScene.unity`.
3. Press Play.

## How to Play

- Enter a value (1–6) into each of the two dice fields in the top-left.
- Press **Roll**. The dice animate in 3D and land on the values you entered.
- The pawn advances by the **sum** of the dice along the board.
- Landing on a tile with a reward adds those items to the inventory (top-right).
- Moving past the last tile wraps back to the start; a tile keeps its reward, so
  it can be collected again on a later pass.
- Throws are unlimited.

## Features

**Inventory** — apples, pears and strawberries. Counts are saved to a JSON file on
every change and restored on the next launch. The UI lives in the top-right corner.

**Map** — built at runtime from `Assets/MonopolyGo/Data/BoardMap.json`. Each tile
is spawned in a line, showing its number and its reward. The tile count comes from
the array length, not a separate field.

**Dice** — two dice, values entered by the player. The roll is a hand-authored 3D
animation (no physics, no random result) that lands on the chosen face and reports
the sum.

**Movement** — the pawn hops tile by tile, wraps past the end via modulo, and
collects the landed tile's reward.

**Feedback** — particle bursts and sound on landing / collecting a reward.

## Architecture

The systems are decoupled and each is independently testable.

- **Composition root** — `GameBootstrap` is a single MonoBehaviour in the scene
  that constructs the systems and injects their dependencies (`Init()` for
  MonoBehaviours, constructor injection for plain classes). No DI framework, no
  service locator, no singletons.
- **Events** — a single static `GameEvents` hub with plain C# `Action` events,
  used only where real decoupling matters (e.g. inventory → UI, landing →
  feedback). Subscribers attach in `OnEnable`, detach in `OnDisable`.
- **Persistence** — the inventory is saved as one JSON blob behind a small
  `ISaveStorage` interface, so the storage backend is swappable. The default
  writes to `Application.persistentDataPath`.
- **Configuration as data** — both the board layout and the item-icon lookup
  (`ItemDatabase` ScriptableObject) are data, not code. Icon lookup is kept out of
  the inventory class.
- **Pooling** — dice and landing particle effects are object-pooled instead of
  being instantiated and destroyed each use.

## Project Structure

```
Assets/MonopolyGo/
  Art/            Materials, UI icons
  Data/           BoardMap.json, ItemDatabase asset
  FX/Particles/   Reward / hop particle prefabs
  Scenes/         GameScene.unity
  Scripts/Runtime/
    Core/         GameBootstrap, GameEvents, ItemType, ItemDatabase
    Dice/         Dice, DiceRoller, DicePool
    Map/          Board, tile data
    Player/       PlayerMover
    Inventory/    Inventory, controllers, ISaveStorage, JsonSaveStorage
    Feedback/     AudioController, VFXController, ParticlePool
    UI/           DiceInputUI
  Sound/SFX/      Sound effects
```

## Map Format

`BoardMap.json` is a flat list of tiles. Each entry is an item and an amount;
`"None"` is an empty tile.

```json
{
  "tiles": [
    { "item": "Apple", "amount": 5 },
    { "item": "None", "amount": 0 },
    { "item": "Strawberry", "amount": 15 }
  ]
}
```

## Gameplay Video

https://github.com/user-attachments/assets/d5f6105f-d202-477e-a3df-3de92052ba18
