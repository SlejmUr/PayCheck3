openssl genpkey -outform PEM -algorithm RSA -pkeyopt rsa_keygen_bits:2048 -out cert.key
openssl req -new -nodes -key cert.key -config cert.conf -nameopt utf8 -utf8 -out global.csr
openssl req -x509 -nodes -in cert.csr -days 365 -key global.key -config cert.conf -extensions req_ext -nameopt utf8 -utf8 -out cert.crt
certutil -p "cert,cert" -mergepfx cert.crt cert.pfx
pause