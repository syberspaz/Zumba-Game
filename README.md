# Zumba-Game
 Zumba-Game MHSc thesis Ontario Shores 

## Project Backup Link:
I recommend making a backup of this just in case.
Contains:
- Full Project Backup
- Zumba Dependencies
- Builds

[Full Google Drive Folder](https://drive.google.com/drive/folders/1AukuDvS9npfJWukepsk6qU5TNqTcMhy0?usp=sharing)

Setup:
- Clone this repository
- Download Data (Not-In-Repo) from [GoogleDrive](https://drive.google.com/drive/folders/1n_O-T5-DuQffEQpL_IYX3dzj7yEDTTIy?usp=sharing)
- unzip this and copy them into the main repo directory
- ensure the .dll's and the .onnx file are in the main project direcoty (same folder as the Zumba-Excergame.sln)
- Add the repo directory to your Unity Hub, I've tested with Editor 2021.3.20f1
- Load the project

If you are having troubles, you may need to install:
- the Azure Kinect SDK from [Link](https://github.com/microsoft/Azure-Kinect-Sensor-SDK/blob/develop/docs/usage.md)
- the Azure Kinect Bodytracking SDK from [Link](https://www.microsoft.com/en-us/download/details.aspx?id=101454)


Debugging Kinect Scenes:
- testScene1 - Kinect bodytracking with collisionZones
- testScene2 - Kinect UI tests. Note that the **Kinect4AzureTracker** and **Main** gameobjects are deactivated because the button from testScene1 will use the Kinect objects from that scene (testing the scene loader)

Main Game Scenes:
- Menu - the main menu
- Minigames/Jigsaw
- Minigames/Matching Game
- Minigames/Word Scramble
- Minigames/Find Me/Find Me Hub
- Minigames/Find Me/Harbour Find Me
- Minigames/Find Me/Island Find Me
- Minigames/Find Me/New City
- Minigames/Find Me/Park
- Minigames/Find Me/Camping
- Minigames/Find Me/HighWayCountry
- Zumba/Beach
- Zumba/Park Zumba
- Zumba/SnowMountains
- Zumba/Beach2
- Zumba/Park Zumba2
- Zumba/SnowMountains2

## Notes of Zumba Capstone 2025:
- As April, 2025. Soar Capture Suite can no longer be found on their webpage.
- [Soar Capture Suite](https://streamsoar.com/) only having a contact email.
- In addition to the Azure Kinect dropping support in 2023, the documentation and support for this technology is very limited.
- New Playlist 2, has been added with new recordings. However due to several technical issues: the new recordings are lower quality than last year.

### iPad mode: 
Due to incompatibility with the Volumetric Models and packages like [Unity Render Streaming](https://docs.unity3d.com/Packages/com.unity.renderstreaming@3.1/manual/index.html)
We are currently using AnyDesk as a 3rd party solution.

For future iPad development with current **Volumetric Models**.
We would need to split the game into two seperate unity projects: 
- Zumba Dance with Volumtric Models
- Minigames

Suggested Solution for remote play "mirroring" with iPad:
I highly suggest we upgrade from using current Azure Kinect's Volumetric Models.
Due to Azure Kinect's Volumetric Models and how they are rendered in Unity.
This causes some incompatibility issues and will crash the Unity Editor.
No solution has been found yet for this problem.

## Zumba Game Notes
Notes for Zumba minigames
1. These use volumetric video assets from the Soar Capture Suite.  You can find the documentation [Dead Link](https://www.streamsoar.com/documentation/unity-package)
2. you should ensure the following settings:
* Select the **ModelMesh** object (this is the volumetric video asset) in the various "Songs"
* in the **Volumetric Render (Script)** Component, Turn on **Enable Re Lighting ** and **Per Pixel Lighting**
* If the textures are too dark, change the **ColorBiasMultiplier** on the Material/Shader settings.  I've added this to boost the color properly

Notes for Kinect UI (testScene2):
1. You should add the **Kinect4AzureTracker** and the **Main** object's to the Menu scene
2. Ensure the **DontDestroy** scripts are on both of these, this will make sure the Kinect is initialized ONCE and never destroyed, your other scenes should do a GameObject.Find("Kinect4AzureTracker") and GameObject.Find("Main") in their Awake() to get access to the actual objects if a script needs them, see the **KinectHand2D.cs** as an example (this looks specifically for the rightHand, rightFingerTip, and rightThumb objects from the Tracker)
3. Kinect Hand 2D script:
* The Canvas object MUST be set to the canvas you want to use for raycasting
* The **THRESHOLD** determines the distance between the Thumb and the Fingertip that is considered "Closed" and "Open", 0.12 seems to be "ok", you might want to tweak this
* Selection Duration isn't used at the moment, you can modify if needed.
4. Objects that can be selected with the Kinect Hand 2D selector **REQUIRE** the **Select Object (Script)** component.  
* the Hand slot should just be the object that has the **Kinect Hand 2D** component attached to it
* Modify this to perform an action you need to when selecting/deselecting

Notes for Kinect Body Tracking (testScene1):
1. I have added an object **COLLISION ZONES** as a child of the **pelvis** bone in the Kinect4AzureTracker object. This moves the collision zones with the object and let's you ensure they are relative to the body.  This can be modified if needed.
* I have set up 4 layers of collision boxes as children.
* Each box is labelled by CXYZ denoting a consistent grid index.  These are the same as the value of the **Grid index** vector. 
* Each box can be a different shape/size 
* Each box has a **Collision Zone (Script)** component, this is the script that determines if something is colliding with it and it is activated or not
* In the **Collision Zone** script, you will see that the OnTriggerEnter() function sets the name of the object hitting the box.  This is only the "last" collision so you may want to add to a List of objects in the Zone Info and remove them as they exit the box to keep track of multiple limbs if needed.

