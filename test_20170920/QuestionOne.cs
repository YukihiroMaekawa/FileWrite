using System;
using System.Text;
using System.IO;

namespace test_20170920
{
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
		public QuestionOne()
		{
			// 初期化
			this.id = new Id();
			this.mailAddress = new MailAddress();
			this.smtpCd = new SmtpCode();
			this.dateTime = new DateTime();
			this.loginId = new LoginId();
			this.randomString = new RandomString();
		}

		// メイン処理
		public void Execute()
		{
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
		private bool IsWriting()
		{
			return idx % FILE_OUTPUT_CNT == 0 ? true : false;
		}

		// 規定サイズ超えたか
		private bool IsSizeOver()
		{
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
}
