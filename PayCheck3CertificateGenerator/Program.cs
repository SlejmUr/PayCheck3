using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PayCheck3CertificateGenerator
{
	internal class Program
	{
		static void Main(string[] args)
		{
			if(File.Exists("./cert.pfx") || File.Exists("./cert.crt"))
			{
				Console.Write("Certificate files already exists, are you sure you want to overwrite them? ");
				string? response = Console.ReadLine();
				if(response != null)
				{
					response = response.ToLower().Trim();
					if(response != "y" && response != "yes")
					{
						return;
					}
				}
				Console.WriteLine("Continuing generation of certificates.");
			}

			var nameBuilder = new X500DistinguishedNameBuilder();
			// CN=jib.starbreeze.accelbyte.io, CN=development.starbreeze.accelbyte.io, CN=connectivitycheck.gstatic.com, CN=api-nebula.starbreeze.com, CN=nebula.starbreeze.com
			nameBuilder.AddCommonName("PayCheck3 Self Signed Certificate");

			var subjectAlternativeNameBuilder = new SubjectAlternativeNameBuilder();

			// These are all the URLs that PAYDAY 3 can possibly contact Starbreeze Nebula with.
			// development.starbreeze.accelbyte.io is present because some of the "Development" beta builds with PDBs connect to it.
			subjectAlternativeNameBuilder.AddDnsName("127.0.0.1");
			subjectAlternativeNameBuilder.AddDnsName("nebula.starbreeze.com");
			subjectAlternativeNameBuilder.AddDnsName("development.starbreeze.accelbyte.io");
			subjectAlternativeNameBuilder.AddDnsName("connectivitycheck.gstatic.com"); // Added in case the user truly has no internet connection, they can reroute the check that PAYDAY 3 uses to connect to the internet in the first place.
			subjectAlternativeNameBuilder.AddDnsName("analytics.starbreeze.com");

			// https://stackoverflow.com/a/52535184
			var keyPair = RSA.Create(2048);

			var certificateRequest = new CertificateRequest(nameBuilder.Build(), keyPair, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

			certificateRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, true, 1, true));

			certificateRequest.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(certificateRequest.PublicKey, false));

			certificateRequest.CertificateExtensions.Add(subjectAlternativeNameBuilder.Build());

			certificateRequest.CertificateExtensions.Add(
				new X509KeyUsageExtension(
						X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment,
						true
					)
				);
			certificateRequest.CertificateExtensions.Add(
				new X509EnhancedKeyUsageExtension(
					new OidCollection()
					{
						new Oid("1.3.6.1.5.5.7.3.1"), // Server Authentication
						new Oid("1.3.6.1.5.5.7.3.2"), // Client Authentication
					},
					true)
				);

			var validFrom = DateTimeOffset.Now;
			var validTo = DateTimeOffset.Now.AddYears(1);

			var certificate = certificateRequest.CreateSelfSigned(validFrom, validTo);

			File.WriteAllText("cert.key", keyPair.ExportRSAPrivateKeyPem());

			File.WriteAllText("cert.csr", certificateRequest.CreateSigningRequestPem());

			Console.WriteLine("Writing cert.pfx...");
			File.WriteAllBytes("./cert.pfx", certificate.Export(X509ContentType.Pfx, "cert"));

			Console.WriteLine("Writing cert.crt...");
			File.WriteAllText("./cert.crt",
			"-----BEGIN CERTIFICATE-----\r\n"
			+ Convert.ToBase64String(certificate.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
			+ "\r\n-----END CERTIFICATE-----");

			Console.WriteLine("Certificate successfully generated, the generated certificate expires on " + validTo.ToString("F"));

			Console.Write("Press any key to exit...");
			Console.ReadKey();
		}
	}
}