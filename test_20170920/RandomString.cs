using System;
using System.Text;

namespace test_20170920
{
	public class RandomString
	{
		private const int LENGTH = 100;
		private const int MINVALUE = 97;
		private const int MAXVALUE = 122;

		// コンストラクタ
		public RandomString() { }

		// 値取得
		public string GetValue(Random rnd)
		{
			// 生成
			return this.Generate(rnd);
		}

		// 生成
		private string Generate(Random rnd)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 1; i <= LENGTH; i++)
			{
				sb.Append(((char)rnd.Next(MINVALUE, MAXVALUE)).ToString());
			}
			return sb.ToString();
		}
	}
}
