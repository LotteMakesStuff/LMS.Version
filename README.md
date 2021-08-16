# LMS.Version    <a href='https://ko-fi.com/A08215TT' target='_blank'><img height='46' style='border:0px;height:46px;' src='https://az743702.vo.msecnd.net/cdn/kofi3.png?v=0' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a> <a href='https://www.patreon.com/bePatron?u=7061709' target='_blank'><img height='46' style='border:0px;height:46px;' src='https://c5.patreon.com/external/logo/become_a_patron_button@2x.png' border='0' alt='Become a Patron!' /></a>
![Maintenance](https://img.shields.io/maintenance/yes/2021) [![GitHub license](https://img.shields.io/github/license/LotteMakesStuff/LMS.Version)](https://github.com/LotteMakesStuff/SimplePhysicsDemo/blob/master/LICENSE)
[![Twitter Follow](https://img.shields.io/twitter/follow/LotteMakesStuff?label=follow&style=social)](https://twitter.com/LotteMakesStuff) ![UnityVersion](https://img.shields.io/badge/made%20with%20unity-2019.4-blue?logo=unity)

### A SUPER simple auto build version tool
Managing a videogames version information can be a MASSIVE pain in the butt - especially in the final days close to release when you are juggling release candidates and bug fixes. Sometimes the most helpful thing in the world is just knowing exactly which version of a game you have running on a devkit. This package helps solve a lot of those problems, and is pretty much ripped right out of the codebase we used to ship Gunsport @ Necrosoft. 

## What does it do?
* Keep your games version information in one central, easy to edit asset
* Automatically increments the games build number each time you run a build
* Stores a timestamp for each build
* If your project uses Git for version control, it captures the most recent commit hash during a build too
* Prints all this info right at the top of a games [log file](https://docs.unity3d.com/Manual/LogFiles.html) making it much easier to match bug reports to game versions
* And provides easy ways to expose the game version in game - super handy for displaying the version in your UI or more feeding into multiplayer APIs for server filtering


![Example showing version infomation embeded into Gunsports main menu](/Docs/IngameVersionExample.png)

## How do i install it?
![Install](/Docs/Install.png)

Installation is hyper simple! Open up Unity's Package Manager view (Window → Package Manager) and then select the Add Package icon in the top left of the window to add a git package.

The current git install URL is  https://github.com/LotteMakesStuff/LMS.Version.git#0.6.1

## How does it do it?
todo write this section!

![Example showing version infomation embeded into a player log file](/Docs/PlayerLogVersionExample.png)

## Wheres the code?

[Version.cs](/Assets/Version/Version.cs) - this class implements the actual Version asset as a Unity [ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html) It provides a bunch of static accessors that make grabbing version information easy at runtime. Its [OnEnable()](https://github.com/LotteMakesStuff/LMS.Version/blob/main/Assets/Version/Version.cs#L18) and [Initialize()](https://github.com/LotteMakesStuff/LMS.Version/blob/main/Assets/Version/Version.cs#L26) methods are responsible for printing version information into a Player.log file

[VersionDisplay.cs](/Assets/Version/VersionDisplay.cs) & [VersionDisplayTextMeshPro.cs](/Assets/Version/VersionDisplayTextMeshPro.cs) - these components can be used to display version information in a Unity GUI, using either the [inbult text renderer](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/script-Text.html) or [Textmesh Pro](https://docs.unity3d.com/Packages/com.unity.textmeshpro@2.2/manual/index.html). The most interesting feature here is the version format string, which can be used to customize how the version text is rendered without changing the underlying code.
![VersionDisplay inspector](/Docs/VersionDisplay.png)

[BuildNumberTool.cs](/Assets/Version/Editor/BuildNumberTool.cs) - Manages all the pre-build magic, including creating a version asset if one does not exist, incrementing the version number and saving changes to the asset as well as calling into git to find the most recent commit hash.

## As shipped in
[![Gunsport](https://github.com/LotteMakesStuff/LotteMakesStuff/blob/master/gunsport.png)](https://gunsport.tv/)
