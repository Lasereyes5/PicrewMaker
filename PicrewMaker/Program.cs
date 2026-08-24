/*
 * 由SharpDevelop创建。
 * 用户： pc
 * 日期: 2024/10/27
 * 时间: 17:08
 * let's all love lain!
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;

namespace PicrewCreator
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			string path=null;
			switch (args.Length)
			{
				case 1:
					path=args[0];
					break;
				default:
					break;
			}
			
			Application.Run(new MainForm(path));
		}
		
	}
}
