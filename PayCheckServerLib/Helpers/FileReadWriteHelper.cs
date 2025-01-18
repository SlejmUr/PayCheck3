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

		public static string ReadAllText(string path)
		{
			if (CachedFileContents.ContainsKey(path))
			{
				return CachedFileContents[path];
			}

			return File.ReadAllText(path);
		}

		public static void WriteAllText(string path, string contents)
		{
			CachedFileContents[path] = contents;
		}

		public static void SaveCachedFiles()
		{
			foreach (var file in CachedFileContents)
			{
				try
				{
					File.WriteAllText(file.Key, file.Value);
				}
				catch
				{
					System.Threading.Thread.Sleep(100);
					File.WriteAllText(file.Key, file.Value);
				}
			}
			CachedFileContents.Clear();
		}

		public static bool ShouldRunCachedFilesThread { get; set; }
		public static void SaveCachedFilesThread()
		{
			while(ShouldRunCachedFilesThread)
			{
				SaveCachedFiles();
				System.Threading.Thread.Sleep(1000);
			}
			SaveCachedFiles(); // save everything one last time on exit
		}
	}
}
