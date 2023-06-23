<a name="br1"></a> 

Introduction

This ancillary document serves as a supplementary resource for the Unity DOTS (Data-Oriented

Technology Stack) Asteroids project. It provides additional information, guidelines, and

resources to support the understanding, implementation, and customization of the project.

System Requirements

●

●

●

●

●

Unity version - 202.3.34f1

Unity Entities package - [Version 0.50.1-preview.2 ]

Unity Burst package - [Version 1.6.4]

Unity Hybrid Renderer - [Version 0.50.0-preview.44]

Unity Jobs package - [Version 0.50.0-preview.9]

Project Structure

●

●

●

●

●

Assets/Scripts:

○

Contains the primary script files, systems, and components for the game.

Assets/Prefabs:

○

Stores prefabricated GameObjects for easy instantiation.

Assets/Scenes

○

Holds the main game scene

Assets/Sprite

○

Contains 2D assets used in the project

Assets/Audio

○

Stores audio files for sound effects and background music

DOTS Implementation

The project utilizes Unity's DOTS framework for high-performance and scalable gameplay. The

key DOTS concepts and their usage within the project are as follows:

●

●

●

Entities: Represent game objects and are composed of components.

Components: Hold data and behavior, representing various attributes of entities.

Systems: Process entities and their components, implementing the game logic and

behavior.



<a name="br2"></a> 

●

●

Jobs: Jobs are used to execute code in parallel, leveraging multi-threading capabilities

for performance optimization.

Burst Compiler: Used to optimize code for specific platforms, improving runtime

performance.

References

To further enhance your understanding and gain insights into Unity DOTS and game

development, consider referring to the following resources:

●

●

Unity DOTS Documentation: [[Link](https://unity.com/dots)]

Entities Overview Documentation: [[Link](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/index.html)]

