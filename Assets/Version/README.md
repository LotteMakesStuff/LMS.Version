# LMS.Version    <a href='https://ko-fi.com/A08215TT' target='_blank'><img height='46' style='border:0px;height:46px;' src='https://az743702.vo.msecnd.net/cdn/kofi3.png?v=0' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a> <a href='https://www.patreon.com/bePatron?u=7061709' target='_blank'><img height='46' style='border:0px;height:46px;' src='https://c5.patreon.com/external/logo/become_a_patron_button@2x.png' border='0' alt='Become a Patron!' /></a>
![Maintenance](https://img.shields.io/maintenance/yes/2022) [![GitHub license](https://img.shields.io/github/license/LotteMakesStuff/LMS.Version)](https://github.com/LotteMakesStuff/SimplePhysicsDemo/blob/master/LICENSE)
[![Twitter Follow](https://img.shields.io/twitter/follow/LotteMakesStuff?label=follow&style=social)](https://twitter.com/LotteMakesStuff) ![UnityVersion](https://img.shields.io/badge/made%20with%20unity-2019.4-blue?logo=unity)

### A SUPER simple auto build version tool
Managing a videogames version information can be a MASSIVE pain in the butt - especially in the final days close to release when you are juggling release candidates and bug fixes. Sometimes the most helpful thing in the world is just knowing exactly which version of a game you have running on a devkit. This package helps you solve a lot of these common versioning problems and is pretty much ripped right out of the codebase we used to ship Gunsport @ Necrosoft. 

## What does it do?
* Keep your games version information in one central, easy to edit asset
* Automatically increments the games build number each time you run a build
* Stores a timestamp for each build
* If your project uses Git for version control, it captures the most recent commit hash during a build too
* Prints all this info right at the top of a games [log file](https://docs.unity3d.com/Manual/LogFiles.html) making it much easier to match bug reports to game versions
* And provides easy ways to expose the game version in game - super handy for displaying the version in your UI or more feeding into multiplayer APIs for server filtering


![Example showing version infomation embeded into Gunsports main menu](/Documentation~/IngameVersionExample.png)

## How do i install it?
![Install](/Documentation~/Install.png)

Installation is hyper simple! Open up Unity's Package Manager view (Window → Package Manager) and then select the Add Package icon in the top left of the window to add a git package.

The current git install URL is  https://github.com/LotteMakesStuff/LMS.Version.git#0.6.1

Version information is kept in a Version asset, which needs to be created in your project after installing the package. Theres two simple ways to make the asset

### 1. Just build your game!
the easiest way is to just hit build in Unity. If no version asset is found in your project at build time a new one is created for you.

### 2. Open your project settings.
You can also manually trigger creating the asset in the Project Settings window (Edit → Project Settings → Version). If no Version asset is found in your project, youll see a button here for creating one. As soon as the asset is created you may edit version information from this screen.

![Creating a new Version Asset via the project settings screen](/Documentation~/ProjectSettingsCreate.png)

Both methods of creating the asset will try and set the initial version number to whatever the value of the version field in Player Settings.

## How does it work?
At its core, this package implements a Unity build preprocessor, a script that is executed before Unity builds your game. Every time you build your  game, this script opens the version asset and does the following modifications
- Increments the Build number. For example version 1.5.2 will become version 1.5.3
- Calls into Git to fetch the hash of the last commit
- Update the versions Timestamp field

The version asset is also added to the project [Preloaded asset list](https://docs.unity3d.com/Manual/class-PlayerSettingsStandalone.html#Optimization). This ensures it is one of the first objects to be loaded by your game. This is important because it needs an opportunity to write into the player log as early as possible. When the version information is loaded in at runtime it is immediately written into your games log file, which is SUPER useful when taking bug reports with logs as you can tell what version of the game the log is from within the first few lines of the log.

![Example showing version infomation embeded into a player log file](/Documentation~/PlayerLogVersionExample.png)

Version information can be edited directly on the version asset itself or via the Version page in the Project Settings window (Edit → Project Settings → Version). As well as being able to directly set the games Major & Minor version number, you can specify Extra Versions. Sometimes it's useful to specify versions for subsystems within your game. Some more common use cases might save data format version or netcode version. We used this extensively in Gunsport on Google Stadia to filter multiplayer lobbies based on netcode version, rather than game version. 

![Editing version information](/Documentation~/ProjectSettingsEdit.png)

## Whats the API?

API | Usage
------------ | -------------
Version.GetGameVersion() | Returns the version of the game as a string
Version.GetGameVersion(VersionDeliniator delineator) | Returns the version of the game as a string. Allows you to specify if you want the numbers separated with dots or underscores
Version.GetExtraVersion(string name) | Returns an extra version as a string. Name is case insensite.
Version.Instance | Singleton access to the Version Assets data 

## How can i show my games version information in my UI?
LMS.Versions ships with two UI components, VersionDisplay and VersionDisplayTextMeshPro that help with displaying version information on a uGUI canvas. These components let you specify a format string that automatically binds data from the version asset and give you a lot of flexibility over how it's displayed. The version string in this example shows  how [Gunsports version was displayed on the games main menu.](/Documentation~/IngameVersionExample.png)

![VersionDisplay inspector](/Documentation~/VersionDisplay.png)

## Wheres the code?

[Version.cs](/Assets/Version/Version.cs) - this class implements the actual Version asset as a Unity [ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html) It provides a bunch of static accessors that make grabbing version information easy at runtime. Its [OnEnable()](https://github.com/LotteMakesStuff/LMS.Version/blob/main/Assets/Version/Version.cs#L18) and [Initialize()](https://github.com/LotteMakesStuff/LMS.Version/blob/main/Assets/Version/Version.cs#L26) methods are responsible for printing version information into a Player.log file

[VersionDisplay.cs](/Assets/Version/VersionDisplay.cs) & [VersionDisplayTextMeshPro.cs](/Assets/Version/VersionDisplayTextMeshPro.cs) - these components can be used to display version information in a Unity GUI, using either the [inbult text renderer](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/script-Text.html) or [Textmesh Pro](https://docs.unity3d.com/Packages/com.unity.textmeshpro@2.2/manual/index.html). The most interesting feature here is the version format string, which can be used to customize how the version text is rendered without changing the underlying code.

[BuildNumberTool.cs](/Assets/Version/Editor/BuildNumberTool.cs) - Manages all the pre-build magic, including creating a version asset if one does not exist, incrementing the version number and saving changes to the asset as well as calling into git to find the most recent commit hash.

## As shipped in
[![Gunsport](/Documentation~/gunsport.png)](https://gunsport.tv/)

### Coffee
I hope you find this as useful in your projects as i have in mine! If you find this at all useful, please consider sending me a coffee <3
<img height='46' style='border:0px;height:46px;' src='https://az743702.vo.msecnd.net/cdn/kofi3.png?v=0' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>
