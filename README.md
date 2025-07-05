# ℹ Happy Room Event
**A plugin for SCP: Secret Laboratory, that adds a new event (game mode):**
- 🏠 Special schematic - Happy Room spawns in the game.
- 🤵 All players spawn in the room and cannot leave it (at least alive).
- ✨ Players play different <s>deadly</s> cool games (SubEvents) in the room.
- 🤖 Plugin randomly selects next game (SubEvent).
- ⏱ The event ends when one or no players remain.

# ⭐ Features
- 🏠 **Schematic**: Room with many structured objects.
- ⚙ **Configurable**: change some event settings in the config file and schematic.
- 🤖 **Automatic**: you just need to start the event. That's all.
- ⌨ **Console commands**: you can start and end the event using Remote Admin Console commands.
- 🌐 **API**: access public types and their members in your code.

> [!NOTE]
> **You need `RoundEvents` permission to execute event commnads.**

# ✨ Starting and Ending event
To start or end the event you can use one of the methods below:
- Using commands in **Remote Admin Console**:
   - `start_hroom` command to start the event.
   - `end_hroom` command to end the event.
- Using **API**:
  - Call `HappyRoomEvent.Instance.StartEvent()` method to start the event.
  - Call `HappyRoomEvent.Instance.EndEvent()` method to end the event.
 
> [!NOTE]
> **Players whose role is `Class-D` are automatically event players. That means, if your respawn player with `Class-D` role during the event, he will become event player.**

# 📁Installation and Configuration
> [!IMPORTANT]
> **For this plugin to work correctly, you need to install (if you didn't) [ProjectMER](https://github.com/Michal78900/ProjectMER) plugin.**

- [Installation Guide](https://github.com/northwood-studios/LabAPI/wiki/Installing-Plugins)  
- [Configuration Guide](https://github.com/northwood-studios/LabAPI/wiki/Configuring-Plugins)

# 🎉 Games (SubEvents)
## 🕺 **Disco**
Floor has many different colors. At the beggining of a game round, floor randomly and quickly changes its colors, then all colors except one randomly selected begin to turn black. Players standing on the black floor will die.

https://github.com/user-attachments/assets/86c5c427-5d86-478a-9292-1f44244ac458

## 🔴 **Erythrophobia**
Random floor parts begin to turn red. In addition, the red stick rotates and accelerates. Players touching anything red will die.

https://github.com/user-attachments/assets/19e9c4fe-79fe-4b63-b729-2916d1c05257

## 🔥 **Lava**
Lava begins to fill the room. There is a path through different challenges to the top of the room. Players touching the lava will die.

https://github.com/user-attachments/assets/cf56822c-3ac2-4ffb-9931-6a82da8f5c7b

# 🖼 Gallery
![DiscoSubEvent](https://github.com/user-attachments/assets/ac5b1397-f6e1-4319-a5ab-5ec98b1e2620)
![ErythrophobiaSubEvent](https://github.com/user-attachments/assets/bdbe0151-8604-4ba1-815c-6174f1c47230)
![LavaSubEvent](https://github.com/user-attachments/assets/ce28737a-008a-4e80-adc0-3cdfbef8e4dc)
![HappyRoom_Unity](https://github.com/user-attachments/assets/3fc3eb5f-1356-4b7a-b74a-82cf5d7b94c0)

