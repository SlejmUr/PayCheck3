# PayCheck3
PayDay 3 (Beta Branch) Server emulator

Left to do (& Working on it):
- WSS Response
- Party and GameSessions

Need more Interogation:\
UDP Connection

How to run:
1. Go To Cert folder and follow readme.txt
2. Copy crt & pfx files and paste next to PayCheck3ServerApp.exe
3. Start the server.
4. Go to your c:\Windows\System32\Drivers\etc\hosts and open it.
5. Copy all stuff from hosts_edit.txt
6. Run your Beta Client & Have fun.

If you want your friends to join you, they need to edit the 127.0.0.1 to your IP (Can be Lan or your External)

# Configurate your server.
You see a config.json where you can edit some things that can edit server stuff.

### Saves
SaveRequest: In here you can save each request when client send any change in the inventory. (Usefull for debug)\
Extension: It basicly a json but with a funny extension

### Hosting
WSS: Can enable/disable to run the HTTPS/WSS Server\
UDP: Can enable/disable to run the UDP Server (GameServer?) [NOT USED]\
GSTATIC: Can enable/disable to run the GSTATIC server (Enable you to play without internet)
UDP_PORT: Can Set 

### InDevFeatures
Can enable/disable certain features that could land you something now work perfectly.

# Soon stuff's

### How to add your server
.

### How to 