# SteamVR-Spotify
SteamVR-Spotify makes the Spotify Web Player available as a dashboard app. Please read **all** of the following instructions
## Installation & Startup
Download the latest version from the [Releases](https://github.com/Martin-Oehler/SteamVR-WebApps/releases) page. Unpack the archive to a location, where you have write-access (for example the user folder). Open the file `SteamVR-Spotify.exe.config` with a text editor and enter your Spotify credentials by replacing the placeholders:
```
<setting name="user" serializeAs="String">
    <value>[USER HERE]</value>
</setting>
<setting name="password" serializeAs="String">
    <value>[PASSWORD HERE]</value>
</setting>
```
This is required because keyboard emulation doesn't work yet and it is faster. Now start `SteamVR-Spotify.exe` while SteamVR is running. The dashboard app will now appear, when you click your System Button. At first startup, you also need to solve the Google Captcha to log in. SteamVR-Spotify will now automatically start each time you start up SteamVR. To uninstall simply remove or move the application folder.
      
## Usage & Features
**IMPORTANT!** Due to DRM reasons, the dashboard app can't play music itself yet. Instead you need to start up Spotify on your PC separatly (Browser or Desktop app). You can then select your PC as a playback device in the dashboard app (should be default). After this, you can use Spotify normally by using your trigger to do a left-click.
A Steam notification will show the current song.

Please provide feedback if you encounter bugs or have a suggestion. You can do so in the [issues](https://github.com/Martin-Oehler/SteamVR-WebApps/issues) section .

## Known bugs
* Notifications reappear when clicking on links in the website
* Keyboard doesn't show when clicking on text fields.