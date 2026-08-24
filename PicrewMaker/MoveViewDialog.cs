/*
 * 由SharpDevelop创建。
 * 用户： pc
 * 日期: 2024/11/26
 * 时间: 19:41
 * let's all love lain!
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PicrewCreator
{
	/// <summary>
	/// Description of MoveViewDialog.
	/// </summary>
	public partial class MoveController : Form
	{
		public int step=10;
		//for buttons,change private to public in Designer.cs
		public MoveController()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			buttonUp.Image=GetArrowBitmap.Up();
			buttonDown.Image=GetArrowBitmap.Down();
			buttonLeft.Image=GetArrowBitmap.Left();
			buttonRight.Image=GetArrowBitmap.Right();
			buttonClockwise.Image=GetArrowBitmap.Clockwise();
			buttonAntiClockwise.Image=GetArrowBitmap.AntiClockwise();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public void UpdateInfoView(int x,int y,int angle)
		{
			//MessageBox.Show("x="+x.ToString()+"\ny="+y.ToString());
			infoView.Text=string.Format("({0},{1})\n {2}°",x,y,angle);
		}
		
		public delegate void DoMove();
		public event DoMove MoveUp,MoveDown,MoveLeft,MoveRight,RotateClockwise,RotateAntiClockwise,Reset;
		
		//public delegate void ViewUpdater();
		public event DoMove ViewUpdate;
		
		void StepChoiceListSelectedIndexChanged(object sender, EventArgs e)
		{
			int index=this.stepChoiceList.SelectedIndex;
			switch (index)
			{
				case 0:
					step=100;
					break;
				case 1:
					step=10;
					break;
				case 2:
					step=1;
					break;
				default:
					break;
			}
		}
		public void ButtonClicked(object sender, EventArgs e)
		{
			ViewUpdate.Invoke();
		}
		
		
		void MoveControllerShown(object sender, EventArgs e)
		{
			ViewUpdate.Invoke();
		}
		
		public void FinalizeMoveViewUpdate()
		{
			MoveUp		+=ViewUpdate;
			MoveDown	+=ViewUpdate;
			MoveLeft	+=ViewUpdate;
			MoveRight	+=ViewUpdate;
			RotateAntiClockwise	+=ViewUpdate;
			RotateClockwise		+=ViewUpdate;
			Reset		+=ViewUpdate;
			
			buttonUp.Click		+= (s,e)=>MoveUp.Invoke();
			buttonDown.Click	+= (s,e)=>MoveDown.Invoke();
			buttonLeft.Click	+= (s,e)=>MoveLeft.Invoke();
			buttonRight.Click	+= (s,e)=>MoveRight.Invoke();
			
			buttonReset.Click	+= (s,e)=>Reset.Invoke();
			
			buttonAntiClockwise.Click	+= (s,e)=>RotateAntiClockwise.Invoke();
			buttonClockwise.Click		+= (s,e)=>RotateClockwise.Invoke();
		}
		
		protected override bool ProcessDialogKey(Keys key)
		{
			switch (key)
			{
				case Keys.A:goto case Keys.Left;
				case Keys.W:goto case Keys.Up;
				case Keys.S:goto case Keys.Down;
				case Keys.D:goto case Keys.Right;
				
				case Keys.Q:
					RotateAntiClockwise.Invoke();
					break;
				case Keys.E:
					RotateClockwise.Invoke();
					break;
				
				case Keys.Up:
					MoveUp.Invoke();
					break;
				case Keys.Down:
					MoveDown.Invoke();
					break;
				case Keys.Left:
					MoveLeft.Invoke();
					break;
				case Keys.Right:
					MoveRight.Invoke();
					break;
					
				case Keys.R:
					Reset.Invoke();
					break;
				default:
					break;
					//throw new Exception("Invalid value for Keys");
			}
			return false;
		}
	}
}
