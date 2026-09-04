# 💡 Lux AEterna

> **A 48-Hour Game Jam Entry built with NimbleFox AI for the "First Light" Theme.**

[![Game Jam](https://img.shields.io/badge/Game%20Jam-48%20Hours-orange.svg)](#)
[![Theme](https://img.shields.io/badge/Theme-First%20Light-yellow.svg)](#)
[![Engine](https://img.shields.io/badge/AI%20Engine-NimbleFox%20AAI-blue.svg)](#)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](#)

---

## 🌌 Overview

In **Lux AEterna**, you embody an ancient, god-like celestial machine—**The Solar Core**—awakening in a primordial, dark universe. Your awakening beam is the **"First Light"** this realm has seen in millennia.

You must control a rotating beacon of light to defend your core against creeping shadow entities while powering up autonomous repair drones. Balance offence and defense in a high-stakes, light-driven arcade survival experience!

---

## 🎮 Gameplay & Mechanics

* **☀️ The Solar Core (Player):** Positioned at the center of the arena. Control a 360° rotating light beam using your mouse or right joystick.
* **🤖 Builder Bots (Friendly AI):** Inactive in the darkness. When illuminated by your beam, NimbleFox AAI activates their repair routines, prompting them to move toward damaged sections of the Core.
* **👥 Shadow Stalkers (Enemy AI):** Dark creatures spawning from the surrounding pitch black. They swarm towards the Core to destroy it, but dissolve when caught directly in your beam of light.
* **⚖️ The Core Dilemma:** You cannot shine your beam everywhere at once! 
  * Focus too much on destroying enemies $
ightarrow$ your Builder Bots remain dormant and cannot repair the Core.
  * Spend too much time powering Builder Bots $
ightarrow$ Shadow Stalkers will flank you from the dark angles.

---

## 🛠️ Built With

* **NimbleFox AI System:** Powers agentic pathfinding, state machine transitions, and dynamic light-reaction behaviors.
* **2D Top-Down Physics Engine:** Efficient 2D raycasting and line-of-sight detection.
* **Dynamic 2D Lighting & Emissive Shaders:** Striking high-contrast neon visual aesthetic against pitch-black arena fog.

---

## 🚀 Getting Started

### Prerequisites
* Game Engine supported by NimbleFox AAI (Unity 2022.3+ / Unreal Engine 5.x)
* NimbleFox AAI Plugin / SDK installed

### Installation & Execution

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/your-username/lux-automata.git
   cd lux-automata
   ```

2. **Open the Project:**
   Launch your editor and load the project folder. Ensure the NimbleFox AAI plugin is enabled under project settings.

3. **Run the Main Scene:**
   Navigate to `Assets/Scenes/MainArena.unity` (or equivalent) and press **Play**.

---

## 🤖 NimbleFox AAI Integration

This project heavily leverages **NimbleFox AI** to manage dual-faction AI state logic based on environmental triggers:

```
                  ┌──────────────────────┐
                  │   Solar Core Beam    │
                  └──────────┬───────────┘
                             │
            ┌────────────────┴────────────────┐
            ▼                                 ▼
   [ Friendly Bot Tag ]             [ Shadow Enemy Tag ]
            │                                 │
   Hits Beam?                       Hits Beam?
  ┌─────────┴─────────┐            ┌──────────┴──────────┐
  │ YES               │ NO         │ YES                 │ NO
  ▼                   ▼            ▼                     ▼
[ State: REPAIR ]  [ State: IDLE ] [ State: DISSOLVE ] [ State: ATTACK ]
 (Path to Core)     (Stop/Standby)  (Take Damage/Die)   (Path to Core)
```

---

## 🕹️ Controls

| Action | Input (Mouse & Keyboard) | Input (Gamepad) |
| :--- | :--- | :--- |
| **Rotate Light Beam** | Aim with Mouse Cursor | Right Analog Stick |
| **Pulse/Focus Beam** | Left Mouse Button / Space | Right Trigger (RT) |
| **Pause Game** | Esc / P | Start Button |

---

## 🏆 Game Jam Details

* **Jam Duration:** 48 Hours
* **Theme:** *First Light*
* **Design Philosophy:** Non-religious deity concept based on cosmic architecture and light mechanics.

---

## 📜 License

Distributed under the MIT License. See `LICENSE` for more information.