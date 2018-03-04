using System;
namespace test_20170920
{
	// ID
	public class Id
	{
		private int id;

		// コンストラクタ
		public Id()
		{
			this.id = 0;
		}

		// 
		public int GetValue()
		{
			return ++this.id;
		}
	}
}
