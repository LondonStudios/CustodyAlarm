![London Studios](https://i.ibb.co/1mwSS1q/Untitled-design.png)

# London Studios - Update
Since forming London Studios in April 2020 we've created a number of **high quality** and **premium** resources for the FiveM project, focusing on the emergency services and aiming to bring your server to the next level.

Although we made a number of free resources such as this one in the first year, we've now switched to creating paid content, keeping them constantly updated and working along with providing the best possible support to our customers.

Our **most popular** resources now include *Smart Fires, Police Grappler* and *Smart Hose*.

With **thousands** of **happy customers** we are confident you'll love our resources and our active support team are on hand to help if you have any questions!

# Our store: https://store.londonstudios.net/github
# Our discord: https://discord.gg/nC2krpN

Therefore, this resource is now likely *out of date* and is *no longer supported by us*. The full source code is available should you wish to make any changes. All of our paid resources however are constantly updated and we invite you to take a look!

# CustodyAlarm - London Studios
**CustodyAlarm** is a **FiveM** resource coded in **C#** allowing you to activate custody alarms at custody stations. The locations can be fully customised in the config.ini file, allowing for easy setup of multiple stations.

The plugin includes an external, realistic British police custody alarm, sourced from a real life station.

This plugin is made by **LondonStudios**, we have created a variety of releases including TaserFramework, SearchHandler, ActivateAlarm, SmartTester, SmartSounds and more!

Join our discord [here](https://discord.gg/BbjwbGQ).

<a href="https://www.buymeacoffee.com/londonstudios" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" style="height: 51px !important;width: 217px !important;" ></a>

![CustodyAlarm](https://i.imgur.com/CJ48aJj.png)

## Usage
**/ca** - Activates the custody alarm (if you are near a station).
**/stopca** - Stops a custody alarm in the area, or one you started.

All sounds heard by nearby players reduce in volume the further away they are, the initial sound volume and the radius can all be configured in the "config.ini" file.
The command names can also be configured in this file.

You are not able to activate a custody alarm if it is already enabled, you may also not activate it if you are too far away from any of the stations created in config.ini.

## Installation
Download the resource from GitHub [here](https://github.com/LondonStudios/CustodyAlarm).
 1.  Create a new **resource folder** on your server.
 2.  Add the contents of **"resource"** inside it. This includes:
"Client.net.dll", "Server.net.dll", "fxmanifest.lua", "index.html", "sounds", "SharpConfig.dll", "config.ini"
3. In **server.cfg**, "ensure" CustodyAlarm, to make it load with your server startup.

## Configuration - Adding stations
This plugin can be fully configured in the "config.ini" file. This includes the command names, custody locations and more, such as the volume and sound radius.

    [CustodyAlarm]
    CustodyNames = { Walworth, Brixton, Wembley }
    AlarmRadius = 45.0
    AlarmVolume = 0.6 # Maximum 1.0
    Command = ca
    StopCommand = stopca

1. Open up "config.ini".
2. On **line 2** you can add stations here. They must be in the list otherwise they will be disabled. (See further steps required below for adding stations).
3. On **line 3** you can adjust the alarm radius, we recommend keeping this at 45.0.
4. On **line 4** you can adjust the alarm volume, this must be bekow 1.

**Adding custody stations:**
After you have added the station name to line 2, you must specify the location for each station, by creating a new section. This must be done like this:

    [Walworth]
    Location = { 714.81, 4157.46, 38.31 }

The **[Walworth]** is the name of the command, this must match the station you entered on **line 2**.
The **Location** = { X, Y, Z } is the station location, this must be setup and all values must end eg "1.0" and can not be an integer/whole number. The coordinates can be retrieved through most in-game menus.

This must be done for each station, in the example config.ini you'll see a station created already.

## Source Code
Please find the source code in the **"src"** folder. Please ensure you follow the licence in **"LICENCE.md"**.

## Feedback
We appreciate feedback, bugs and suggestions related to CustodyAlarm and future plugins. We hope you enjoy using the resource and look forward to hearing from people!
