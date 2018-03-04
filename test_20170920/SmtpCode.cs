using System;
namespace test_20170920
{
	public class SmtpCode
	{
		private readonly string[] Code = { "211", "214", "220", "221", "250", "251", "354", "421", "450", "451", "452", "500", "501", "502", "503", "504", "550", "551", "552", "553", "554" };

		// コンストラクタ
		public SmtpCode() { }

		// 値取得
		public string GetValue(Random rnd)
		{
			// 生成
			return this.Generate(rnd);
		}

		// 生成
		private string Generate(Random rnd)
		{
			return Code[rnd.Next(0, Code.Length)].ToString();
		}
	}
}
