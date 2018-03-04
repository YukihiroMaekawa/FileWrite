using System;
namespace test_20170920
{
	public class DateTime
	{
		// 日付フォーマット
		private const string DATE_FORMAT = "yyyy/MM/dd HH:mm:ss";

		// 日付範囲
		private const string MIN_DATE = "1999/01/01 00:00:00";
		private const string MAX_DATE = "2000/12/31 23:59:59";

		// コンストラクタ
		public DateTime() { }

		// 値取得
		public string GetValue(Random rnd)
		{
			// 生成
			return this.Generate(rnd);
		}

		// 生成
		private string Generate(Random rnd)
		{
			// 日付範囲の差(秒)から0の乱数を日付(min)に加算
			System.DateTime dt = System.DateTime.Parse(MIN_DATE);
			TimeSpan ts = System.DateTime.Parse(MAX_DATE) - System.DateTime.Parse(MIN_DATE);
			return dt.AddSeconds(rnd.Next(0, Convert.ToInt32(ts.TotalSeconds))).ToString(DATE_FORMAT);
		}
	}
}
