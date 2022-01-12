# Elite Dangerous Virtual Wing

## Idea
- Web based application. Only possible for PC based users.
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
