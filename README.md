# PayCheck3
PayDay 3 (Live/Master Branch) Server emulator

## Disclaimer
 - You must own a legitimate copy of PAYDAY 3 to use this software, which can be purchased
at https://www.paydaythegame.com/payday3/#buy
 - The contributors of PayCheck3 in no way support piracy, and actively ensure that PayCheck3
does not work with an illegitimate copy. We're sorry to have caused any confusion that
this could be used for piracy.
 - This project is in no way affiliated or endorsed by Starbreeze Entertainment.

# How to use the server

## Hosting PayCheck3 locally
**The certificates created by `PayCheck3CertificateGenerator.exe` expire exactly 1 year after being generated.
If you use this software, and it stops working 1 year after you made the certificates, you need to regenerate them and reinstall them.**

1. Download the latest release of PayCheck3.
2. Unpack the latest release and run `PayCheck3CertificateGenerator.exe` to generate the required **self-signed** SSL certificates.
3. On Windows: Double-click on the `cert.crt` file and click the "Install Certificate..." button. In the dialog that appears
select "Current User" and click "Next". In the new dialog, select "Place all certificates in the following store" and click "Browse".
In the "Select Certificate Store" dialog, choose "Trusted Root Certification Authorities" and click "OK".
After choosing the certificate store, client "Next" and "Finish".
If a dialog appears asking if you want to install the certificate, click "Yes".
4. Once the certificate is installed, add ```127.0.0.1 nebula.starbreeze.com``` to your `C:\Windows\System32\Drivers\etc\hosts` file. **To stop the game from using PayCheck3, remove this line from your `hosts` file**.
5. Run `PayCheck3ServerApp.exe` and PayCheck3 will start.

## Hosting PayCheck3 on a public facing server 

1. Download the latest release of PayCheck3.
2. Edit `config.json` and change the `SSLCertificatePassword` value to the password of your server's SSL certificate.
   (If `config.json` does not exist, run `PayCheck3ServerApp.exe` at least once).
3. Put your server's SSL certificate, in PFX format, in PayCheck3's install folder and name it "cert.pfx".
4. Run `PayCheck3ServerApp.exe` and PayCheck3 will start.

### How can players connect to my server?

For players to be able to connect to your public-facing PayCheck3 server, they will need to make the following edits to their PAYDAY 3 `Engine.ini` config file.

**TODO**
