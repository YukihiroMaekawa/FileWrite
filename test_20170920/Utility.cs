using System;
using System.IO;

namespace test_20170920
{
	static public class Utility
	{
		// 使用メモリ
		static public void OutPutMemory()
		{
			System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
			p.Refresh();

			Console.WriteLine("メモリ使用量(MB): {0}", p.WorkingSet64 / 1024 / 1024);
		}

		// ファイルサイズ
		static public long GetFileSize(string filePath)
		{
			FileInfo fi = new System.IO.FileInfo(filePath);
			return fi.Length;
		}
	}
}
