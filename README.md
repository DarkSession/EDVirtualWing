# Elite Dangerous Virtual Wing
Elite Dangerous has an in-game wing limit of 4 players on the same platform, version and same game mode.  
A virtual wing allows players to see a live status of more players which are not part of their in-game wing.  
This targets squadrons and other groups which regularly exceed the 4 player limit but still want to participate in activities together.

## Development
This application is currently under active development.
It uses ASP.NET 6 to build the server and Angular 13 as user frontend.
The application code is available under the MIT license. Feel free to host this application yourself (once completed) and/or contribute to it.
We will also provide a public instance of this application for everyone to participate.

### MariaDB
The application is built to use a MariaDB instance to store its data.
For development and testing purposes, run a local MariaDB instance using Docker, e.g.
`docker run --detach --name edvw-mariadb --env MARIADB_USER=dev --env MARIADB_PASSWORD=1234 --env MARIADB_ROOT_PASSWORD=1234 -p 3306:3306 mariadb:latest`

### Configuration
EDVW_MARIADB_CONNECTIONSTRING: MariaDB instance connection string
EDVW_HTTP_ORIGIN=https://localhost:44440

## Idea
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

## Credits
Assets used: [edassets.org](https://edassets.org/)