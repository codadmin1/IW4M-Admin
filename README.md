# IW4MAdmin
### Quick Start Guide
### Version 2.2
_______
### About
**IW4MAdmin** is an administration tool for [IW4x](https://iw4xcachep26muba.onion.link/), [Pluto T6](https://forum.plutonium.pw/category/33/plutonium-t6), [Pluto IW5](https://forum.plutonium.pw/category/5/plutonium-iw5), and most Call of Duty� dedicated servers. It allows complete control of your server; from changing maps, to banning players, **IW4MAdmin** monitors and records activity on your server(s). With plugin support, extending its functionality is a breeze.
### Download
Latest binary builds are always available at https://raidmax.org/IW4MAdmin  

---
### Setup
**IW4MAdmin** requires minimal effort to get up and running.
#### Prerequisites
* [.NET Core 2.1 Runtime](https://www.microsoft.com/net/download) *or newer*  
#### Installation
1. Install .NET Core Runtime
2.  Extract `IW4MAdmin-<version>.zip`  
#### Launching
1. Run `StartIW4MAdmin.cmd` (Windows)
2. Run `StartIW4MAdmin.sh` (Linux)
2. Configure **IW4MAdmin**
### Updating
1. Extract newer version of **IW4MAdmin** into pre-existing **IW4MAdmin** folder and overwrite existing files
- _Your configuration and database will be saved_
---
### Help
Feel free to join the **IW4MAdmin** [Discord](https://discord.gg/ZZFK5p3)  
If you come across an issue,  bug, or feature request please post an [issue](https://github.com/RaidMax/IW4M-Admin/issues)
___

### Configuration
#### Initial Configuration
When **IW4MAdmin** is launched for the _first time_, you will be prompted to setup your configuration.

`Enable webfront`
* Enables you to monitor and control your server(s) through a web interface
* Default &mdash; `http://0.0.0.0:1624`

`Enable multiple owners`
* Enables more than one client to be promoted to level of `Owner`
* Default &mdash; `false`

`Enable stepped privilege hierarchy`
* Allows privileged clients to promote other clients to the level below their current level
* Default &mdash; `false`

`Enable custom say name`
* Shows a prefix to every message send by **IW4MAdmin** -- `[Admin] message`
* _This feature requires you specify a custom say name_
* Default &mdash; `false`

`Enable social link`
* Shows a link to your community's social media/website on the webfront
* Default &mdash; `false`

`Use Custom Encoding Parser`
* Allows alternative encodings to be used for parsing game information and events
* **Russian users should use this and then specify** `windows-1251` **as the encoding string**
* Default &mdash; `false`

#### Server Configuration
After initial configuration is finished, you will be prompted to configure your servers for **IW4MAdmin**.

`Enter server IP Address`
* For almost all scenarios `127.0.0.1` is sufficient
* Default &mdash; `n/a`

`Enter server port`
* The port that your server is listening on (can be obtained via `net_port`)
* Default &mdash; `n/a`

`Enter server RCon password`
* The *\(R\)emote (Con)sole* password set in your server configuration (can be obtained via `rcon_password`)
* Default &mdash; `n/a`
 
`Use Pluto T6 parser`
* Used if setting up a server for Plutonium T6 (BO2)
* Default &mdash; `false`
 
`Use Pluto IW5 parser`
* Used if setting a server for Plutonium IW5 (MW3)
* Default &mdash; `false`
 
`Enter number of reserved slots`
* The number of client slots reserver for privileged players (unavailable for regular users to occupy)
* Default &mdash; `0`

#### Advanced Configuration
If you wish to further customize your experience of **IW4MAdmin**, the following configuration file(s) will allow you to changes core options using any text-editor.

#### `IW4MAdminSettings.json`-- _this file is created after initial setup_
* This file uses the [JSON](https://en.wikipedia.org/wiki/JSON#JSON_sample) specification, so please validate it before running **IW4MAdmin**

`WebfrontBindUrl`
* Specifies the address and port the webfront will listen on.
* The value can be an [IP Address](https://en.wikipedia.org/wiki/IP_address):port or [Domain Name](https://en.wikipedia.org/wiki/Domain_name):port
* Example http://gameserver.com:8080
* Default &mdash; `http://0.0.0.0:1624`

`CustomLocale`
* Specifies a [locale name](https://msdn.microsoft.com/en-us/library/39cwe7zf.aspx) to use instead of system default
* Locale must be from the `Equivalent Locale Name` column
* Default &mdash; `windows-1252`

`ConnectionString`
* Specifies the [connection string](https://www.connectionstrings.com/mysql/) to a MySQL server that is used instead of SQLite
* Default &mdash; `null`
 
`RConPollRate`
* Specifies (in milliseconds) how often to poll each server for updates
* Default &mdash; `5000`

`Servers`
* Specifies the list of servers **IW4MAdmin** will monitor
* Default &mdash; `[]`
* `IPAddress`
	* Specifies the IP Address of the particular server
	* Default &mdash; `n/a`
* `Port`
	* Specifies the port of the particular server
	* Default &mdash; `n/a`
* `Password`
	* Specifies the `rcon_password` of the particular server
	* Default &mdash; `n/a`
* `ManualLogPath`
    * Specifies the log path to be used instead of the automatically generated one
    * To use the `GameLogServer`, this should be set to the http address that the `GameLogServer` is listening on
    * Example &mdash; http://gamelogserver.com/
* `AutoMessages`
	* Specifies the list of messages that are broadcasted to the particular server
	* Default &mdash; `null`
* `Rules`
	* Specifies the list of rules that apply to the particular server
	* Default &mdash; `null`
* `ReservedSlotNumber`
    * Specifies the number of client slots to reserve for privileged users 
    * Default &mdash; `0`

`AutoMessagePeriod`
* Specifies (in seconds) how often messages should be broadcasted to each server
* Default &mdash; `60`

`AutoMessages`
* Specifies the list of messages that are broadcasted to **all** servers
* Specially formatted **tokens** can be used to broadcast dynamic information  
* `{{TOTALPLAYERS}}` &mdash; displays how many players have connected
* `{{TOPSTATS}}` &mdash; displays the top 5 players on the server based on performance
* `{{MOSTPLAYED}}` &mdash; displays the top 5 players based on number of kills
* `{{TOTALPLAYTIME}}` &mdash; displays the cumulative play time (in man-hours) on all monitored servers
* `{{VERSION}}` &mdash; displays the version of **IW4MAdmin**
* `{{ADMINS}}` &mdash; displays the currently connected and *unmasked* privileged users online
* `{{NEXTMAP}}` &mdash; displays the next map and gametype in rotation

`GlobalRules`
* Specifies the list of rules that apply to **all** servers`

`Maps`
* Specifies the list of maps for each supported game
* `Name`
	* Specifies the name of the map as returned by the game (usually the file name sans the file extension)
* `Alias`
	* Specifies the display name of the map (as seen while loading in)
___

### Commands
|Name              |Alias|Description                                                                               |Requires Target|Syntax           |Required Level|
|--------------| -----| --------------------------------------------------------| -----------------| -------------| ----------------|
|prune|pa|demote any privileged clients that have not connected recently (defaults to 30 days)|False|!pa \<optional inactive days\>|Owner|
|quit|q|quit IW4MAdmin|False|!q |Owner|
|rcon|rcon|send rcon command to server|False|!rcon \<commands\>|Owner|
|ban|b|permanently ban a client from the server|True|!b \<player\> \<reason\>|SeniorAdmin|
|unban|ub|unban client by client id|True|!ub \<client id\> \<reason\>|SeniorAdmin|
|find|f|find client in database|False|!f \<player\>|Administrator|
|killserver|kill|kill the game server|False|!kill |Administrator|
|map|m|change to specified map|False|!m \<map\>|Administrator|
|maprotate|mr|cycle to the next map in rotation|False|!mr |Administrator|
|plugins|p|view all loaded plugins|False|!p |Administrator|
|tempban|tb|temporarily ban a client for specified time (defaults to 1 hour)|True|!tb \<player\> \<duration (m\|h\|d\|w\|y)\> \<reason\>|Administrator|
|alias|known|get past aliases and ips of a client|True|!known \<player\>|Moderator|
|baninfo|bi|get information about a ban for a client|True|!bi \<player\>|Moderator|
|fastrestart|fr|fast restart current map|False|!fr |Moderator|
|flag|fp|flag a suspicious client and announce to admins on join|True|!fp \<player\> \<reason\>|Moderator|
|kick|k|kick a client by name|True|!k \<player\> \<reason\>|Moderator|
|list|l|list active clients|False|!l |Moderator|
|mask|hide|hide your presence as a privileged client|False|!hide |Moderator|
|reports|reps|get or clear recent reports|False|!reps \<optional clear\>|Moderator|
|say|s|broadcast message to all clients|False|!s \<message\>|Moderator|
|setlevel|sl|set client to specified privilege level|True|!sl \<player\> \<level\>|Moderator|
|setpassword|sp|set your authentication password|False|!sp \<password\>|Moderator|
|unflag|uf|Remove flag for client|True|!uf \<player\>|Moderator|
|uptime|up|get current application running time|False|!up |Moderator|
|usage|us|get application memory usage|False|!us |Moderator|
|balance|bal|balance teams|False|!bal |Trusted|
|login|li|login using password|False|!li \<password\>|Trusted|
|warn|w|warn client for infringing rules|True|!w \<player\> \<reason\>|Trusted|
|warnclear|wc|remove all warnings for a client|True|!wc \<player\>|Trusted|
|admins|a|list currently connected privileged clients|False|!a |User|
|getexternalip|ip|view your external IP address|False|!ip |User|
|help|h|list all available commands|False|!h \<optional commands\>|User|
|mostplayed|mp|view the top 5 dedicated players on the server|False|!mp |User|
|owner|iamgod|claim ownership of the server|False|!iamgod |User|
|ping|pi|get client's latency|False|!pi \<optional player\>|User|
|privatemessage|pm|send message to other client|True|!pm \<player\> \<message\>|User|
|report|rep|report a client for suspicious behavior|True|!rep \<player\> \<reason\>|User|
|resetstats|rs|reset your stats to factory-new|False|!rs |User|
|rules|r|list server rules|False|!r |User|
|stats|xlrstats|view your stats|False|!xlrstats \<optional player\>|User|
|topstats|ts|view the top 5 players in this server|False|!ts |User|
|whoami|who|give information about yourself|False|!who |User|

_These commands include all shipped plugin commands._

---

#### Player Identification
All players are identified 5 separate ways   
1. `npID/GUID/XUID` - The ID corresponding to the player's hardware or forum account   
2. `IP` - The player's IP Address   
3. `Client ID` - The internal reference to a player, generated by **IW4MAdmin**   
4. `Name` - The visible player name as it appears in game   
5. `Client Number` - The slot the client occupies on a server. (The number ranges between 0 and the max number of clients allowed on the server)

For most commands players are identified by their `Name`  
However, if they are currently offline, or their name contains un-typable characters, their `Client ID` must be used   

The `Client ID` is specified by prefixing a player's reference number with `@`.  
For example, `@123` would reference the player with a `Client ID` of 123.  

**All commands that require a `target` look at the `first argument` for a form of player identification**

---

#### Additional Command Examples
`setlevel`
- _shortcut_ - `sl`
- _Parameter 1_ - Player to modify level of
- _Parameter 2_ - Level to set the player to ```[ User, Trusted, Moderator, Administrator, SeniorAdmin, Owner ]```
- _Example_ - `!setlevel Player1 SeniorAdmin`, `!sl @123 Moderator`
- **NOTE** - An `owner` cannot set another player's level to `owner` unless the configuration option is enabled during setup

`ban`
- _Shortcut_ - `b`
- _Parameter 1_ - Player to ban
- _Parameter 2_ - Reason for ban
- _Example_ - `!ban Player1 caught cheating`, `!b @123 GUID Spoofing`

`tempban`
- _Shortcut_ - `tb`
- _Parameter 1_ - Player to ban
- _Parameter 2_ - Ban length (minutes|hours|days|weeks|years)
- _Parameter 3_ - Reason for ban
- _Example_ - `!tempban Player1 3w racism`, `!tb @123 8h Abusive behaivor`

`reports`  
- _Shortcut_ - `reps`
- _Optional Parameter 1_ - `clear` (erases reports for current server)

___
### Plugins

#### Welcome   
- This plugin uses geo-location data to welcome a player based on their country of origin
- All privileged users ( Trusted or higher ) receive a specialized welcome message as well 
- Welcome messages can be customized in `WelcomePluginSettings.json`

#### Stats
- This plugin calculates basic player performance, skill approximation, and kill/death ratio 
- Skill is an number derived from an algorithmic processing of a player's Kill Death Ratio (KDR) and Score per Minute (SPM).
- Elo Rating is based off of the number of encounters a player wins.
- Performance is the average of Skill + Elo Rating

**Commands added by this plugin** 

|Name              |Alias|Description                                                                               |Requires Target|Syntax           |Required Level|
|--------------| -----| --------------------------------------------------------| -----------------| -------------| ----------------|
|resetstats|rs|reset your stats to factory-new|False|!rs |User|
|stats|xlrstats|view your stats|False|!xlrstats \<optional player\>|User|
|topstats|ts|view the top 5 players on this server|False|!ts |User|
|mostplayed|mp|view the top 5 dedicated players on the server|False|!mp |User|

- To qualify for top stats, a client must have played for at least `3 hours` and connected within the past `15 days`.

#### Login
- This plugin deters GUID spoofing by requiring privileged users to login with their password before executing commands
- A password must be set using the `setpassword` command before logging in

 **Commands added by this plugin**  

|Name              |Alias|Description                                                                               |Requires Target|Syntax           |Required Level|
|--------------| -----| --------------------------------------------------------| -----------------| -------------| ----------------|
|login|l|login using password|False|!l \<password\>|Trusted|

#### Profanity Determent
- This plugin warns and kicks players for using profanity
- Profane words and warning message can be specified in `ProfanityDetermentSettings.json`
- If a client's name contains a word listed in the settings, they will immediately be kicked

#### IW4 Script Commands
- This plugin provides additional integration to IW4x
- In order to take advantage of it, copy the `userraw` folder into your IW4x server directory

#### VPN Detection [Script Plugin]
- This plugin detects if a client is using a VPN and kicks them if they are
- To disable this plugin, delete `Plugins\VPNDetection.js`
___
### Webfront
`Home`
* Shows an overview of the monitored server(s)

`Penalties`
* Shows a chronological ordered list of client penalties (scrolling down loads older penalties)

`Admins`
* Shows a list of privileged clients

`Login`
* Allows privileged users to login using their `Client ID` and password set via `setpassword`
* `ClientID` is a number that can be found by using `!find <client name>` or find the client on the webfront and copy the ID following `ProfileAsync/`

`Profile`
* Shows a client's information and history 

`Web Console`
* Allows logged in privileged users to execute commands as if they are in-game
---
### Game Log Server
The game log server provides a way to remotely host your server's log over a http rest api. 
This server is useful if you plan on running IW4MAdmin on a different machine than the game server
#### Requirements
- [Python 3.6](https://www.python.org/downloads/) or newer
- The following [PIP](https://pypi.org/project/pip/) packages (provided in `requirements.txt`)
 ```Flask>=1.0.2
aniso8601>=3.0.2
click>=6.7
Flask-RESTful>=0.3.6
itsdangerous>=0.24
Jinja2>=2.10
MarkupSafe>=1.0
pip>=9.0.3
pytz>=2018.5
setuptools>=39.0.1
six>=1.11.0
Werkzeug>=0.14.1
``` 
#### Installation
1. With Python 3 installed, open up a terminal/command prompt window in the `GameLogServer` folder and execute:
    ```console
    pip install -r requirements.txt
    ```
2. Allow TCP port 1625 through firewall  
    * [Windows Instructions](https://www.tomshardware.com/news/how-to-open-firewall-ports-in-windows-10,36451.html)
    * [Linux Instructions (iptables)](https://www.digitalocean.com/community/tutorials/how-to-set-up-a-basic-iptables-firewall-on-centos-6#open-up-ports-for-selected-services)
#### Launching  
With Python 3 installed, open a terminal/command prompt window open in the `GameServerLog`  folder and execute:
```console
python runserver.py
```
---
### Extending Plugins
#### Code
IW4Madmin functionality can be extended by writing additional plugins in C#.  
Each class library must implement the `IPlugin` interface.   
See the existing plugins for examples.
#### JavaScript
IW4MAdmin functionality can be extended using JavaScript.
The JavaScript parser supports [ECMA 5.1](https://ecma-international.org/ecma-262/5.1/) standards.
#### Plugin Object Template
In order to be properly parsed by the JavaScript engine, every plugin must conform to the following template.
```js
var plugin = {
    author: 'YourHandle',
    version: 1.0,
    name: 'Sample JavaScript Plugin',

    onEventAsync: function (gameEvent, server) {
    },

    onLoadAsync: function (manager) {
    },

    onUnloadAsync: function () {
    },

    onTickAsync: function (server) {
    }
};
```
#### Required Properties
- `author` &mdash; [string] Author of the plugin (usually your name or online name/alias)
- `version` &mdash; [float] Version number of your plugin (useful if you release several different versions)
- `name` &mdash; [string] Name of your plugin (be descriptive!)
- `onEventAsync` &mdash; [function] Handler executed when an event occurs
    - `gameEvent` &mdash; [parameter object] Object containing event type, origin, target, and other info (see the GameEvent class declaration)
    - `server` &mdash; [parameter object] Object containing information and methods about the server the event occured on (see the Server class declaration)  
- `onLoadAsync` &mdash; [function] Handler executed when the plugin is loaded by code
    - `manager` &mdash; [parameter object] Object reference to the application manager (see the IManager interface definition) 
- `onUnloadAsync` &mdash; [function] Handler executed when the plugin is unloaded by code (see live reloading)
- `onTickAsync` &mdash; [function] Handler executed approximately once per second by code *(unimplemented as of version 2.1)*
    - `server` &mdash; [parameter object] Object containing information and methods about the server the event occured on (see the Server class declaration) 
### Live Reloading
Thanks to JavaScript's flexibility and parsability, the plugin importer scans the plugins folder and reloads the JavaScript plugins on demand as they're modified. This allows faster development/testing/debugging.

---
### Discord Webhook
If you'd like to receive notifications on your Discord guild, configure and start `DiscordWebhook.py`
#### Requirements
- [Python 3.6](https://www.python.org/downloads/) or newer
- The following [PIP](https://pypi.org/project/pip/) packages (provided in `requirements.txt`)
 ```certifi>=2018.4.16
chardet>=3.0.4
idna>=2.7
pip>=18.0
requests>=2.19.1
setuptools>=39.0.1
urllib3>=1.23
``` 
#### Configuration Options
- `IW4MAdminUrl` &mdash; Base url corresponding to your IW4MAdmin `WebfrontBindUrl`.  
Example http://127.0.0.1
- `DiscordWebhookNotificationUrl` &mdash; [required] Discord generated URL to send notifications/alerts to; this includes **Reports** and **Bans**  
Example https://discordapp.com/api/webhooks/id/token
- `DiscordWebhookInformationUrl` &mdash; [optional] Discord generated URL to send information to; this includes information such as player messages
- `NotifyRoleIds` &mdash; [optional] List of [discord role ids](https://discordhelp.net/role-id) to mention when notification hook is sent
#### Launching
With Python installed, open a terminal/command prompt window open in the `Webhook`  folder and execute:
```console
python DiscordWebhook.py
```

---
### Misc
#### Anti-cheat
This is an [IW4x](https://iw4xcachep26muba.onion.link/) only feature (wider game support planned), that uses analytics to detect aimbots and aim-assist tools.  
To utilize anti-cheat, enable it during setup **and** copy `_customcallbacks.gsc` from `userraw` into your `IW4x Server\userraw\scripts` folder.  
The anti-cheat feature is a work in progress and as such will be constantly tweaked and may not be 100% accurate, however the goal is to deter as many cheaters as possible from IW4x.
#### Database Storage
By default, all **IW4MAdmin** information is stored in `Database.db`.  
Should you need to reset your database, this file can simply be deleted.  
Additionally, this file should be preserved during updates to retain client information.   
Setting the `ConnectionString` property in `IW4MAdminSettings.json` will cause **IW4MAdmin** to attempt to use a MySQL connection for database storage. 