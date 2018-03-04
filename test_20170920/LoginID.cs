using System;
using System.Collections.Generic;
using System.Text;

namespace test_20170920
{
	public class LoginId
	{
		private const int LENGTH = 6;
		private const int MINVALUE = 97;
		private const int MAXVALUE = 122;

		// 登録済みリスト
		private HashSet<string> lst = new HashSet<string>();

		//コンストラクタ
		public LoginId()
		{
			this.lst = new HashSet<string>();
		}

		// 値取得
		public string GetValue(Random rnd)
		{
			while (true)
			{
				string str = Generate(rnd);
				if (!lst.Contains(str))
				{
					lst.Add(str);
					return str;
				}
			}
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
