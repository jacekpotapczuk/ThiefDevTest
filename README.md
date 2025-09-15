# Theft Prototype

This project started as a **test assignment** for a game dev position.  
The task was to build a simple prototype of a theft system in Unity where the player steals objects from a secured area, avoids cameras, and sees progress in the UI.  

The base project was provided (Unity 6000.0.41f1) and already included:  
- a simple character controller (part of the task was to clean up this one)
- a prototyping scene  

I had to fix and extend it with object carrying, camera detection, UI feedback, and basic game flow.  

---

## Timebox

I gave myself a limit of **8 hours** to finish the prototype. The scope was big for such a short time, but I managed to get all the requested mechanics working within that limit.  

---

## Object Carrying

I implemented picking up, carrying, and dropping objects through the existing interaction system.  
I aimed for a bit of a "simulation feel" so when you drop an object it can inherit some momentum from the player movement or quick camera turns.  

---

## Camera Detection

Instead of going with the simplest ray or angle check, I tried a different approach.  
At runtime, each camera generates a **mesh-shaped detection zone** that checks if the player is inside.  

This allowed me to give clear **visual feedback** to the player, showing where they can be spotted.  
It could have been done more simply, but I liked the idea of making it visible and intuitive.  

---

## Reaction to Detection

Because of the timebox, I kept it minimal:  
- the camera vision area changes material when detection starts  
- a **detection progress bar** is shown  
- once filled, a very simple **Game Over** screen appears  

---

## Refactoring

I restructured a fair bit of the provided code to fit my own programming style.  
In a real production project I would of course follow the team’s existing codebase standards, but since this was meant to include refactoring, I leaned into cleaning it up.  

---

## Limitations

There are still rough edges. A few that stand out:  
- objects can currently be carried through walls and out of bounds  
- the UI is mostly "debug-level" quality since the main focus was mechanics and time management  

---

This is a **prototype**, not a polished game. The goal was to deliver a working system that demonstrates mechanics, flexibility for future growth, and a bit of creativity in how detection is visualized. 