# Elite Dangerous Virtual Wing
Elite Dangerous has an in-game wing limit of 4 players on the same platform, version and same game mode.   
A virtual wing allows players to see a live status of more players which are not part of their in-game wing.  
This targets squadrons and other groups which regularly exceed the 4 player limit but still want to participate in activities together.


# Public instance
A public (beta stage) instance is available on [virtual-wing.edct.dev](https://virtual-wing.edct.dev/)


# Host your own instance
Feel free to host your own instance. The application runs on both Windows and Linux.
More information will follow here about how to host this on your own.

### Configuration
`EDVW__ConnectionString`: MariaDB instance connection string.  
`EDVW__HttpOrigin`: Allowed origin for CORS and the WebSocket client.  
`EDVW__FDevClientId`: FDev OAuth2 Client ID. Request here: [user.frontierstore.net/developer](https://user.frontierstore.net/developer)  
`EDVW__FDevAuthReturnUrl`: Return from FDev OAuth2

### Dev - MariaDB
The application is built to use a MariaDB instance to store its data.
For development and testing purposes, run a local MariaDB instance using Docker, e.g.  
`docker run --detach --name edvw-mariadb --env MARIADB_USER=dev --env MARIADB_PASSWORD=1234 --env MARIADB_ROOT_PASSWORD=1234 -p 3306:3306 mariadb:latest`

## Assets
[edassets.org](https://edassets.org/): Ships, icons  
CMDR Arithon [elite-dangerous-blog.co.uk](https://www.elite-dangerous-blog.co.uk/post/2017/10/26/Vehicle-icons): SRV image  
m-scha1337: Wanted icon.  
[Font Awesome](https://fontawesome.com/license)  
[Angular Material](https://material.angular.io)  

Virtual Wing was created using assets and imagery from Elite Dangerous, with the permission of Frontier Developments plc, for non-commercial purposes. It is not endorsed by nor reflects the views or opinions of Frontier Developments and no employee of Frontier Developments was involved in the making of it. 

## Frameworks
![Angular](https://angular.io/assets/images/logos/angular/angular.png) ![.NET](https://i.imgur.com/RYRYKhH.png)
