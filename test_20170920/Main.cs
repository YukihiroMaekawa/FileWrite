using System;

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
}
