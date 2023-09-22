# PayCheck3
PayDay 3 (Live/Master Branch) Server emulator

## Disclaimer
 - You must own a legitimate copy of PAYDAY 3 to use this software, which can be purchased
at https://www.paydaythegame.com/payday3/#buy
 - The contributors of PayCheck3 in no way support piracy, and actively ensure that PayCheck3
does not work with an illegitimate copy. We're sorry to have caused any confusion that
this could be used for piracy.
 - This project is in no way affliated or endorsed by Starbreeze Entertainment.

## CURRENTLY MATCHMAKING IS NOT WORKING!
Thank you for understanding! I working on a solution, no ETA yet.

## Current State
Left to do (& Working on it):
- WSS Response
- Party and GameSessions
- New Responses

Need more Interogation:\
UDP Connection

How to run:
1. Go To Cert folder and follow readme.txt
2. Copy crt & pfx files and paste next to PayCheck3ServerApp.exe
3. Start the server.
4. Go to your c:\Windows\System32\Drivers\etc\hosts and open it.
5. Copy all stuff from hosts_edit.txt
6. Run your Client & Have fun.

If you want your friends to join you, they need to edit the 127.0.0.1 to your IP (Can be Lan or your External)

# Where can I get the built server?
In [Github Actions](https://github.com/SlejmUr/PayCheck3/actions) you find most recent pushes/request Build version.

# Configurate your server.
You see a config.json where you can edit some things that can edit server stuff.

### Saves
SaveRequest: In here you can save each request when client send any change in the inventory. (Usefull for debug)\
Extension: It basicly a json but with a funny extension

### Hosting
IP: Currently is its 127.0.0.1 but can be changed to your IP or anything as IP (IT HOST GSTATIC,WSS,UDP too!!!)\
WSS: Can enable/disable to run the HTTPS/WSS Server\
UDP: Can enable/disable to run the UDP Server (GameServer?) [NOT USED]\
GSTATIC: Can enable/disable to run the GSTATIC server (Enable you to play without internet)\
UDP_PORT: Can set Port for UDP connection

### InDevFeatures
Can enable/disable certain features that could land you something now work perfectly.

# Soon stuff's
Aka just ideas

### How to add your server
Open servers.json\
Duplicate the eu-central-1 alias server\
Edit the Status, Region, Port, IP to your UDP one
