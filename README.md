# Unity Ghost Replay System

![Recording](https://github.com/MaikelHendrikx1/Unity-Ghost-Replay-System/assets/84376526/0f2fe66c-51e9-4aa0-af91-55eec77ff0a8)


  * [Introduction](#introduction)
  * [How to implement](#how-to-implement)
  * [Performance impact](#performance-impact)
  * [Planned features](#planned-features)

## Introduction

Unity Ghost Replay System is a simple Unity Package that allows you to easily add a ghost replay system to your game. A ghost replay system is a system where all your inputs or movements are recorded, and then they are played back the next time you play the same level.
This is particularly useful in racing games where players might want to race against themselves to improve their best time, but there are also many use-cases for other types of games!

The system handles all the hard parts of a replay system for you (recording, playing back, writing & reading the recording to a file), so you just need to attach some scripts and call their methods and you're finished!

I have created a simple implementation of this system in the [Unity learn Karting microgame](https://assetstore.unity.com/packages/templates/unity-learn-karting-microgame-urp-150956). 
A small demo (in video format) can be found [here](https://www.youtube.com/watch?v=GKyJadbZ9KA).

A playable demo of this implementation can be downloaded [here](https://drive.google.com/file/d/1x-S__PTNgi36afmh1kk8dVm3Efq-BywO/view?usp=sharing).


## How to implement

Below is a step-by-step guide to implementing this system in your own projects. Note that these steps may be altered over time as the system gets updated.

### 1. Install the Unity Package
   
The latest release of the Unity Package can be found [here](https://github.com/MaikelHendrikx1/Unity-Ghost-Replay-System/releases), or in the releases section of this GitHub repository.
Once downloaded, with your project open, simply run GhostReplay.unitypackage and it will import into your project. If a menu pops up asking you which files to include, ensure to include all files. 
The 'ghost' material may be skipped if you want to create a custom material for your ghost object, however.


### (Optional) Configure the settings

If you want to change the floating point accuracy for the transform recorder, you can do so in TransformState.cs on line 13 (although I recommend keeping it as is).
If you want to change the location to which the recording files are saved, you can do so in RecordingStore.cs on line 14.


### 2. Add the 'ObjectRecorder' script to the gameobject that you want to record.

Make sure to add it to the correct gameobject; so whichever gameobjects' transform will be changing during gameplay. Then set the "recording id" in the inspector to an identifier unique to whatever map or level you are adding the implementation to.
A good example would be lvl01 or simply 01.


### 3. Create the object that will be playing back the recording

Create an object with the same model as the object from step 2. This object should have no physics, rigidbody or colliders. Ideally, it should contain nothing except for the visuals. 
Then I recommend making the ghost object visually distinctive from the player, and transparent. The easiest way is to use the ghost material provided by the package, but you can ofcourse also create a separate model or material.
Make sure to put this object in the same starting position as the player object, and to have it enabled at the start. This Unity package will automatically disable & hide the object if no recording is present.


### 4. Add the 'RecordingPlayer' script to the gameobject you created in step 3.

Set the "recording id" to the same identifier you used in step 2.


### 5. Call the right methods at the right time.

All that is left to do now is to call the methods exposed by the system, from your gameplay scripts!

#### ObjectRecorder (script you added in step 2):
- Whenever you want to start recording the movements (for example when the race starts), call StartRecording().
- Whenever you want to end the recording (and optionally save it to a file), call StopRecording(bool).
- If you want to momentarily pause/resume the recording, call PauseRecording() and ResumeRecording() respectively.


#### RecordingPlayer (script you added in step 4):
- Whenever you want to start the replay (usually at the same time as you called StartRecording()), call StartReplay()
- If you want to momentarily pause/resume the replaying, call PauseReplay() and ResumeReplay() respectively.


### 6. Time to test!

That was everything required to implement the ghost replay system. You can now test to see if everything works as expected.



## Performance impact

I ran some tests to measure the impact on performance while using this system. I implemented this system into the [Unity learn Karting microgame](https://assetstore.unity.com/packages/templates/unity-learn-karting-microgame-urp-150956) and measured the average framerate throughout a race. The hardware this test was ran on is as follows:
- CPU: Intel Core i7-10750H
- GPU: NVIDIA GeForce GTX 1650 Ti
- RAM: 16GB at 2933 MHz

Below are the results:
| Test nr | Without ghost system | With ghost system |
| ----- | ----- | ----- |
| 1 | 712 fps | 706 fps |
| 2 | 694 fps | 702 fps |
| 3 | 733 fps | 728 fps |
| 4 | 726 fps | 720 fps |
| 5 | 705 fps | 698 fps |
| All rounded | 714 fps | 711 fps  |

From this (admittedly small-scoped) test, it is now clear that this system does not have a significant effect on the framerate of the game.


## Planned features
These are the currently planned features. Once all these features are implemented & tested on a larger scale, the 1.0.0 version of this project will be released.

- Animation support (Recording & replaying of animations)
- The ability to easily add your own states to record (For example to allow you to record & replay particles, sprite renderers, custom components etc.), along with a template & guide to show you how.
- Options menu to allow you to change some settings such as state record interval, floating point accuracy, etc.
