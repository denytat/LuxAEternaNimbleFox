# Lux AEterna

**Lux AEterna** is a top-down 2D action-arcade game built in Unity. Players control a central power core emitting a directed beam of light. By steering the beam, burning shadow entities, and sustaining power output, players defend the core against waves of encroaching hostiles while purifying fallen enemies into loyal builder bots.

---

## Core Gameplay Mechanics

* **Directional Beam Sweeping:** Rotate the light beam around the core in real time to sweep across approaching enemies.
* **Dual Light States:**
  * **Doomed Mode (Default):** A low-power, dimmed beam (10% opacity) that remains visible as a faint glow but deals no damage and cannot convert corpses.
  * **Active Mode (Powered):** Full opacity beam activated via user input, capable of burning shadow enemies and purifying fallen entities.
* **Corpse Purification:** When hostiles reach 0 HP, they stop moving and enter a corpse state. Holding the active beam continuously on a corpse for **1 second** transforms it into an **Ally Builder Bot**.
* **Core Repair Loop:** Converted allies automatically trek back to the central core upon purification to repair its integrity (+5 HP).

---

## Controls

* **Q:** Rotate Beam Left (Counter-Clockwise)
* **E:** Rotate Beam Right (Clockwise)
* **W (Hold):** Power Up Light Beam (Toggle Active Mode)

---

## Architecture & Unity Technical Setup

### Object Hierarchy
```text
PlayerCore (Root - Kinematic Rigidbody 2D, LightbeamController script)
├── Core (Sprite Renderer - Core Graphic & Trigger Collider)
└── LightBeam (Sprite Renderer - Beam Graphic, Offset Pivot, Trigger Collider)

```

### Script Overview

* **`LightbeamController.cs`:** Controls Q/E rotation around the core, handles W power state toggling, and updates sprite alpha dynamically between active (1.0) and dimmed (0.1) opacity.
* **`Enemy.cs`:** Manages hostile pathfinding to the core, damage state tracking, corpse decay timers, sustained light exposure calculation, and ally transformation logic.
* **`CoreHealth.cs`:** Tracks central core health, applies incoming hostile contact damage, and processes ally repair healing.

---

## Technical Requirements

* **Engine:** Unity (2D Template)
* **Input System:** Configured for **Both** (Legacy `Input.GetKey` and Unity **New Input System** `UnityEngine.InputSystem`)
* **Physics Setup:** Kinematic Rigidbodies on root objects with `Is Trigger` 2D Colliders for light-to-enemy interaction detection.

---

## License

This project is released under the MIT License.
  <Elicitation label="Build an Enemy Wave Spawner" query="How can I set up an Enemy Wave Spawner for Lux AEterna that spawns enemies around the screen edges?"/>
  <Elicitation label="Create the Core Health & Game Over UI" query="How can I create a Core Health system that shows UI health and triggers a Game Over when destroyed?"/>
</ElicitationsGroup>

```
