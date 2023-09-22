Here you make your custom .crt & pfx file (Req. by Server)

1. Run the "run.bat" file.
2. Install both the PFX and CRT files (Windows calls these "Security Certificate" and "Personal Information Exchange" files.  See below for how to do this in Windows.

--HOW TO INSTALL CRT FILE (Security Certificate)--

1. Double click the file
2. Click Install Certificate...
3. Select Local Machine and click Next (you need admin privileges)
4. Select Place all certificates in the following store, and click Browse
5. Click Trusted Root Certification Authorities
6. Keep clicking Next to finish up the installer.

--HOW TO INSTALL THE PERSONAL INFO EXCHANGE FILE (pfx)--

1. Double click the file
2. In the Certificate Import Wizard, select Local Machine and click Next (you need admin privileges)
3. Leave the default file name and click Next
4. In the Password field type "cert" and click Next
5. Select Place all certificates in the following store, and click Browse
6. Click Trusted Root Certification Authorities
7. Keep clicking Next to finish up the installer.
