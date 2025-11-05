Make It POP – Unity Test Project
Project Overview

This is a 1-minute gameplay loop prototype built in Unity to showcase game feel and “POP factor” in mobile combat. The goal is to make every movement, hit, and effect feel satisfying, juicy, and fun.

The prototype supports both mobile touch and desktop controls for testing in the editor.

Unity Version: 6000.0.060f1

Main Scene Used: GameScene.unity

Chosen Path

Path: Hack & Slash Dungeon

Weapon Type: Melee (Sword)

Gameplay Goal: Defeat as many enemies as possible to score points within 1 minute

Features
Core Gameplay

Single playable character with sword attacks

Movement:

Touch: Swipe to move or tap to attack/dash

Desktop: Keyboard (WASD) for movement, Space for attack, Left Shift for dash

Dash mechanic: Quick movement in swipe direction or forward

Lock-On System: Player can lock onto nearest enemy

Auto-move: Player can automatically move toward a locked target

Enemy Waves: Small arena, multiple enemies spawned

Input System

Mobile Touch:

Swipe for movement (TouchInputManager)

Tap for attacks

Flick above threshold triggers dash

UI buttons for Attack, Dash, Lock-On, Move

Desktop/Editor:

WASD/Arrow keys for movement

Mouse click simulates touch swipe/tap

Space key triggers attack, Left Shift triggers dash

UI

Simple UI with timer and score

Buttons for movement, attack, dash, and lock-on

Buttons interactable depending on game state (e.g., move button only active when a target is locked)

Feedback & Juiciness

Camera shake on attack and dash (CameraShake3D)

Sword swing hitboxes trigger feedback on hits

Smooth rotation and movement for satisfying control feel

How to Play

Open the project in Unity 6000.0.060f1

Open the scene: Scenes/GameScene.unity

Use the movement swipe (mobile) or WASD (desktop) to move the player

Tap Attack or press Space to swing the sword

Swipe/flick to dash or press Left Shift

Lock-on to enemies for auto movement toward target

Defeat as many enemies as possible to increase your score before the timer runs out

Technical Notes
Scripts

TouchInputManager.cs – Handles mobile touch and editor input

PlayerController3D.cs – Player movement, attack, dash, lock-on, and auto-move logic

GameManager.cs – Game flow (start, end, reset), player/enemy spawning

GameUIScreen.cs – Handles UI, buttons, and timer updates

CameraFollow.cs, SwordSwingPivot.cs, SwordHitbox.cs, EnemySpawner3D.cs, Enemy3D.cs – Supporting gameplay components

Touch vs Desktop Input

Input system automatically switches based on platform:

UNITY_EDITOR / STANDALONE → keyboard/mouse input

Mobile → touch input

UI buttons work on all platforms and respect input flags to prevent conflicting actions

Setup Instructions

Ensure TouchInputManager, GameManager, PlayerPrefab, EnemySpawner3D, and GameUIScreen are present in the scene

Press Play in the editor or build for mobile

Focus on “POP Factor”

Snappy movement with responsive rotation

Juicy attack feedback: camera shake, hit flashes, and sword hit effects

Smooth dash and swipe mechanics for satisfying motion

Button feedback on mobile for clear player action feedback
