# Elite Dangerous Virtual Wing

## Idea
ED has an in-game wing limit of 4 players on the same platform, version and same game mode.  
A virtual wing allows players to see a live status of more players which are not part of their in-game wing.  
This targets squadrons and other groups which regularly exceed the 4 player limit but still want to participate in activities together.

- Web based application. Unfortunately only possible for PC based users. Everything will be updated live.
- Users share the local folder which contains their game journal with the web based application using the [File System Access API](https://developer.mozilla.org/en-US/docs/Web/API/File_System_Access_API). Journal is shared with the application server.
- Application server keeps track of the location, status, activities etc. of the CMDR.
- CMDR can then join one or multiple virtual wings. Their location, status, current activity etc. is then shared with all the members of the virtual wing.
- The CMDR can leave any virtual wing at any time or stop sharing their activities by closing the browser window.
- The CMDR can use multiple devices to see their wings activities.
- Information which will be shared with their virtual wings:
  - System
  - Station/Settlement/Signal Source (as far as possible)
  - Planet/Body
  - Coordinates if around or landed on a planet
  - Current activity / status: Hardpoints deployed, under attack, on foot, in srv, in supercruise etc...
  - Travel destination
  - Target information
  - Game: Horizon or Odyssey
  - Mode: Open, Private, Solo
  - If in vehicle: Basic information (Type, Hull, Shields)
