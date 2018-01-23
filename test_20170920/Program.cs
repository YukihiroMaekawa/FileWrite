using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace test_20170920
{
    class MainClass
    {
        public static void Main()
        {
            Console.WriteLine("-- START --");
            Console.WriteLine(System.DateTime.Now);

            // 問題１
            QuestionOne qOne = new QuestionOne();
			qOne.Execute();

			Console.WriteLine(System.DateTime.Now);
			Console.WriteLine("-- END --");
        }
    }

    public class QuestionOne
    {
        private const int FILE_SIZE = 1024 * 1024 * 5; // ファイルサイズ
		// ファイル件数
		private const int FILE_OUTPUT_CNT = 1000;
		// 出力先
		private const string OUTPUT_PATH = "/Users/Shared/Share/test";
		private const string OUTPUT_FILE_NAME = "test.csv";
		// 項目区切文字
		private const string CSV_DELIMITER = ",";

        private int idx = 0;

        // クラス定義
        Random rnd;
		Id id;
		MailAddress mailAddress;
		SmtpCode smtpCd;
		DateTime dateTime;
		LoginId loginId;
		RandomString randomString;

		// コンストラクタ
		public QuestionOne() {
            // 初期化
            this.id = new Id();
            this.mailAddress = new MailAddress();
            this.smtpCd = new SmtpCode();
            this.dateTime = new DateTime();
            this.loginId = new LoginId();
            this.randomString = new RandomString();
        }

		// メイン処理
		public void Execute(){
            // CSV作成
            this.GenerateCsvFile();

            this.mailAddress.output();
		}

        // CSV作成
		private void GenerateCsvFile()
		{
			StringBuilder sb = new StringBuilder();
            this.rnd = new Random();

            // 規定サイズを超えるまで
            while (true)
			{
				this.idx++;

                // レコード生成
                sb.Append(this.GenerateRecord()).Append((Environment.NewLine));

                // 書き込み判定
                if (!IsWriting()) continue;

				//Utility.OutPutMemory();

				// ファイル書き込み
				this.WriteFile(sb);

                // 規定サイズを超えたら終了
                if (this.IsSizeOver()) return;

				sb.Length = 0;
                //this.rnd = new Random();
			}
		}

		// レコード生成
		private StringBuilder GenerateRecord()
		{
            StringBuilder sb = new StringBuilder();
			sb.Append(this.id.GetValue());
            sb.Append(CSV_DELIMITER).Append(this.mailAddress.GetValue(this.rnd));
            sb.Append(CSV_DELIMITER).Append(this.smtpCd.GetValue(this.rnd));
            sb.Append(CSV_DELIMITER).Append(this.dateTime.GetValue(this.rnd));
			sb.Append(CSV_DELIMITER).Append(this.loginId.GetValue(this.rnd));
            sb.Append(CSV_DELIMITER).Append(this.randomString.GetValue(this.rnd));
            return sb;
		}

        // 書き込みタイミングか
        private bool IsWriting(){
            return idx % FILE_OUTPUT_CNT == 0 ? true : false;
        }

        // 規定サイズ超えたか
        private bool IsSizeOver(){
            return Utility.GetFileSize(Path.Combine(OUTPUT_PATH, OUTPUT_FILE_NAME)) > FILE_SIZE ? true : false;    
        }

		// ファイル書き込み
		private void WriteFile(StringBuilder sb)
		{
			using (FileWriter sw = new FileWriter(Path.Combine(OUTPUT_PATH, OUTPUT_FILE_NAME), true))
			{
                sw.Write(sb);
			}
		}
	}

    // ファイル
    public class FileWriter : IDisposable{
        private bool disposed = false;
        private StreamWriter sw;

        // コンストラクタ
        public FileWriter(string filePath, bool append){
            sw = new StreamWriter(filePath, append);
        }

        // 書き込み
        public void Write(StringBuilder sb){
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

    // ID
    public class Id{
        private int id;

        // コンストラクタ
        public Id(){
            this.id = 0;
        }

        // 
        public int GetValue(){
            return ++this.id;
        }
    }

    // MailAddress
    public class MailAddress{
		// メールアドレス (1文字から20文字)
        private readonly string[] ACOUNT_STRING = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u" ,"v" ,"w" ,"x" ,"y" ,"z" ,"0" ,"1" ,"2" ,"3" ,"4" ,"5" ,"6" ,"7" ,"8" ,"9"};
		private const int ACOUNT_LENGTH_MIN = 1;
		private const int ACOUNT_LENGTH_MAX = 20;

		// ドメイン
		private readonly string[] DOMAIN = { "@aaa.co.jp", "@bbb.jp", "@ccc.jp" };
		private readonly string[] DOMAIN_STRING = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
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
        private string Generate(Random rnd){
            return GenerateAccont(rnd) + GenerateDomain(rnd); 
		}

        // アカウント作成
        private string GenerateAccont(Random rnd){
			StringBuilder sb = new StringBuilder();
			int length;

            // アカウント文字数
            length = rnd.Next(ACOUNT_LENGTH_MIN, ACOUNT_LENGTH_MAX);

            for (int i = 1; i <= length;i++){
                sb.Append(ACOUNT_STRING[rnd.Next(0, ACOUNT_STRING.Length)]);
			}
            return sb.ToString();
        }

        // ドメイン作成
        private string GenerateDomain(Random rnd){
            string str = string.Empty;
            // 固定ドメイン(30%,20%,10%)、その他ドメイン(40%)
            switch  (rnd.Next(1,10) % 10){
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
                    if(this.lstOtherDomain.ContainsKey(str)){
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
                    }else{
                        //新規ドメイン
                        this.lstOtherDomain.Add(str, 1);
						this.lstOtherDomainCountFew.Add(str);
                    }
                    break;
			}
            return str;
        }

        //
        private string GenerateNewDomain(Random rnd){

            // その他ドメインが全て最大数出現済み
            if(lstOtherCountMax == this.lstOtherDomain.Count){
                
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

    // SmtpCode
    public class SmtpCode{
		private readonly string[] Code = { "211", "214", "220" ,"221" ,"250" ,"251" ,"354" ,"421" ,"450" ,"451" ,"452" ,"500" ,"501" ,"502" ,"503" ,"504" ,"550" , "551" , "552" , "553" , "554"};

		// コンストラクタ
		public SmtpCode(){}

        // 値取得
        public string GetValue(Random rnd)
        {
            // 生成
            return this.Generate(rnd);
        }

		// 生成
        private string Generate(Random rnd){
            return Code[rnd.Next(0,Code.Length)].ToString();
        }
    }

	// DateTime
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

	// ログインID
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

	// RandomString
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

    // Random
    public class Random
    {
        private System.Random rnd;
        // コンストラクタ
        public Random(){
            this.rnd = new System.Random();    
        }

        public Random(int seed){
            this.rnd = new System.Random(seed);
        }

		// Next
        public int Next(){
            return this.rnd.Next();
        }

		public int Next(int minValue, int maxValue){
            return this.rnd.Next(minValue, maxValue); 
        }
    }

    // Utility
    static public class Utility
    {
        // 使用メモリ
        static public void OutPutMemory(){
			System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
			p.Refresh();

            Console.WriteLine("メモリ使用量(MB): {0}", p.WorkingSet64 / 1024 / 1024);
        }
               
        // ファイルサイズ
        static public long GetFileSize(string filePath){
			FileInfo fi = new System.IO.FileInfo(filePath);
            return fi.Length;
        }
    }
}

