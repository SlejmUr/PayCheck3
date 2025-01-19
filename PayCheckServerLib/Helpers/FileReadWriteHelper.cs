using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCheckServerLib.Helpers
{
	public class FileReadWriteHelper
	{
		// TODO: Remove file caching, and switch everything over to an "atomic" file read write (writes will wait for active reads to finish, and reads will wait for active writes to finish)
		public static Dictionary<string, string> CachedFileContents = new Dictionary<string, string>();

		private static bool CachedFileContentsBeingModified = false;
		public static string ReadAllText(string path)
		{
			while (CachedFileContentsBeingModified) { }
			if (CachedFileContents.ContainsKey(path))
			{
				return CachedFileContents[path];
			}

			return File.ReadAllText(path);
		}

		public static bool Exists(string path)
		{
			while (CachedFileContentsBeingModified) { }
			if (CachedFileContents.ContainsKey(path))
				return true;

			return File.Exists(path);
		}

		public static void WriteAllText(string path, string contents)
		{
			CachedFileContentsBeingModified = true;
			CachedFileContents[path] = contents;
			CachedFileContentsBeingModified = false;
		}

		public static void SaveCachedFiles()
		{
			while (CachedFileContentsBeingModified) { }
			foreach (var file in CachedFileContents)
			{
				try
				{
					if(!Directory.Exists(Path.GetDirectoryName(file.Key)))
						Directory.CreateDirectory(Path.GetDirectoryName(file.Key));
					File.WriteAllText(file.Key, file.Value);
				}
				catch
				{
					System.Threading.Thread.Sleep(100);
					if (!Directory.Exists(Path.GetDirectoryName(file.Key)))
						Directory.CreateDirectory(Path.GetDirectoryName(file.Key));
					File.WriteAllText(file.Key, file.Value);
				}
			}
			CachedFileContentsBeingModified = true;
			CachedFileContents.Clear();
			CachedFileContentsBeingModified = false;
		}

		public static bool ShouldRunCachedFilesThread { get; set; }
		public static void SaveCachedFilesThread()
		{
			while(ShouldRunCachedFilesThread)
			{
				SaveCachedFiles();
				System.Threading.Thread.Sleep(500);
			}
			SaveCachedFiles(); // save everything one last time on exit
		}
	}
}
