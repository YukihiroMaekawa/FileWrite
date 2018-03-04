using System;
using System.Text;
using System.IO;

namespace test_20170920
{
	// ファイル
	public class FileWriter : IDisposable
	{
		private bool disposed = false;
		private StreamWriter sw;

		// コンストラクタ
		public FileWriter(string filePath, bool append)
		{
			sw = new StreamWriter(filePath, append);
		}

		// 書き込み
		public void Write(StringBuilder sb)
		{
			sw.Write(sb);
		}

		// 書き込み(改行あり)
		public void WriteLine(StringBuilder sb)
		{
			sw.WriteLine(sb);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.sw.Close();

				if (disposing)
				{
					// マネージから呼ばれた場合は、ここを通る
					GC.SuppressFinalize(this);
				}
			}
		}

		// ファイナライザ
		~FileWriter()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
