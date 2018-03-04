using System;
using System.Collections.Generic;
using System.Text;
namespace test_20170920
{
	public class MailAddress
	{
		// メールアドレス (1文字から20文字)
		private readonly string[] ACOUNT_STRING = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
		private const int ACOUNT_LENGTH_MIN = 1;
		private const int ACOUNT_LENGTH_MAX = 20;

		// ドメイン
		private readonly string[] DOMAIN = { "@aaa.co.jp", "@bbb.jp", "@ccc.jp" };
		private readonly string[] DOMAIN_STRING = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
		private const int DOMAIN_LENGTH_MIN = 3;
		private const int DOMAIN_LENGTH_MAX = 10;

		// 登録済みリスト
		private HashSet<string> lst = new HashSet<string>();

		private Dictionary<string, int> lstOtherDomain = new Dictionary<string, int>();
		private HashSet<string> lstOtherDomainCountFew = new HashSet<string>();
		private int lstOtherCountMax = 0; // その他ドメインで100件に到達件数
										  //private HashSet<string> lstOtherDomainComplete = new HashSet<string>(); // 100回出現済

		//コンストラクタ
		public MailAddress()
		{
			this.lst = new HashSet<string>();
			this.lstOtherDomain = new Dictionary<string, int>();
		}

		// 値取得
		public string GetValue(Random rnd)
		{
			// メールアドレス生成
			while (true)
			{
				string str = Generate(rnd);
				if (!this.lst.Contains(str))
				{
					this.lst.Add((str));
					return str;
				}
			}
		}

		// 生成
		private string Generate(Random rnd)
		{
			return GenerateAccont(rnd) + GenerateDomain(rnd);
		}

		// アカウント作成
		private string GenerateAccont(Random rnd)
		{
			StringBuilder sb = new StringBuilder();
			int length;

			// アカウント文字数
			length = rnd.Next(ACOUNT_LENGTH_MIN, ACOUNT_LENGTH_MAX);

			for (int i = 1; i <= length; i++)
			{
				sb.Append(ACOUNT_STRING[rnd.Next(0, ACOUNT_STRING.Length)]);
			}
			return sb.ToString();
		}

		// ドメイン作成
		private string GenerateDomain(Random rnd)
		{
			string str = string.Empty;
			// 固定ドメイン(30%,20%,10%)、その他ドメイン(40%)
			switch (rnd.Next(1, 10) % 10)
			{
				case 1:
				case 2:
				case 3:
					str = DOMAIN[0];
					break;
				case 4:
				case 5:
					str = DOMAIN[1];
					break;
				case 6:
					str = DOMAIN[2];
					break;
				default:
					str = GenerateNewDomain(rnd);
					if (this.lstOtherDomain.ContainsKey(str))
					{
						this.lstOtherDomain[str] += 1;
						int cnt = this.lstOtherDomain[str];

						if (cnt == 100)
						{
							lstOtherCountMax++; //100回出現数
						}
						else if (cnt > 10)
						{
							// 10未満リストから削除
							this.lstOtherDomainCountFew.Remove(str);
						}
					}
					else
					{
						//新規ドメイン
						this.lstOtherDomain.Add(str, 1);
						this.lstOtherDomainCountFew.Add(str);
					}
					break;
			}
			return str;
		}

		//
		private string GenerateNewDomain(Random rnd)
		{

			// その他ドメインが全て最大数出現済み
			if (lstOtherCountMax == this.lstOtherDomain.Count)
			{

			}
			StringBuilder sb = new StringBuilder();
			int length;

			// アカウント文字数
			length = rnd.Next(DOMAIN_LENGTH_MIN, DOMAIN_LENGTH_MAX);

			sb.Append("@");
			for (int i = 1; i <= length; i++)
			{
				sb.Append(DOMAIN_STRING[rnd.Next(0, DOMAIN_STRING.Length)]);
			}
			sb.Append(".jp");

			return sb.ToString();
		}

		public void output()
		{
			foreach (KeyValuePair<string, int> pair in this.lstOtherDomain)
			{
				Console.WriteLine(" その他ドメイン-- ");
				Console.WriteLine(pair.Key.ToString() + pair.Value);
			}

		}
	}
}
