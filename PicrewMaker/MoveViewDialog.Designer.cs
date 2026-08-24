/*
 * 由SharpDevelop创建。
 * 用户： pc
 * 日期: 2024/11/26
 * 时间: 19:41
 * let's all love lain!
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace PicrewCreator
{
	partial class MoveController
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MoveController));
			this.buttonUp = new System.Windows.Forms.Button();
			this.buttonLeft = new System.Windows.Forms.Button();
			this.buttonRight = new System.Windows.Forms.Button();
			this.buttonDown = new System.Windows.Forms.Button();
			this.buttonReset = new System.Windows.Forms.Button();
			this.positionTips = new System.Windows.Forms.Label();
			this.stepChoiceList = new System.Windows.Forms.ListBox();
			this.buttonAntiClockwise = new System.Windows.Forms.Button();
			this.buttonClockwise = new System.Windows.Forms.Button();
			this.infoView = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonUp
			// 
			//~ this.buttonUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonUp.Image")));
			this.buttonUp.Location = new System.Drawing.Point(70, 7);
			this.buttonUp.Name = "buttonUp";
			this.buttonUp.Size = new System.Drawing.Size(45, 45);
			this.buttonUp.TabIndex = 0;
			this.buttonUp.UseVisualStyleBackColor = true;
			// 
			// buttonLeft
			// 
			//~ this.buttonLeft.Image = ((System.Drawing.Image)(resources.GetObject("buttonLeft.Image")));
			this.buttonLeft.Location = new System.Drawing.Point(19, 58);
			this.buttonLeft.Name = "buttonLeft";
			this.buttonLeft.Size = new System.Drawing.Size(45, 45);
			this.buttonLeft.TabIndex = 1;
			this.buttonLeft.UseVisualStyleBackColor = true;
			// 
			// buttonRight
			// 
			//~ this.buttonRight.Image = ((System.Drawing.Image)(resources.GetObject("buttonRight.Image")));
			this.buttonRight.Location = new System.Drawing.Point(121, 58);
			this.buttonRight.Name = "buttonRight";
			this.buttonRight.Size = new System.Drawing.Size(45, 45);
			this.buttonRight.TabIndex = 2;
			this.buttonRight.UseVisualStyleBackColor = true;
			// 
			// buttonDown
			// 
			//~ this.buttonDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonDown.Image")));
			this.buttonDown.Location = new System.Drawing.Point(70, 109);
			this.buttonDown.Name = "buttonDown";
			this.buttonDown.Size = new System.Drawing.Size(45, 45);
			this.buttonDown.TabIndex = 3;
			this.buttonDown.UseVisualStyleBackColor = true;
			// 
			// buttonReset
			// 
			this.buttonReset.Location = new System.Drawing.Point(70, 58);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(45, 45);
			this.buttonReset.TabIndex = 4;
			this.buttonReset.Text = "Reset";
			this.buttonReset.UseVisualStyleBackColor = true;
			// 
			// positionTips
			// 
			this.positionTips.Location = new System.Drawing.Point(121, 7);
			this.positionTips.Name = "positionTips";
			this.positionTips.Size = new System.Drawing.Size(60, 13);
			this.positionTips.TabIndex = 7;
			this.positionTips.Text = "Info:";
			// 
			// stepChoiceList
			// 
			this.stepChoiceList.FormattingEnabled = true;
			this.stepChoiceList.ItemHeight = 12;
			this.stepChoiceList.Items.AddRange(new object[] {
									"Large",
									"Normal",
									"Small"});
			this.stepChoiceList.Location = new System.Drawing.Point(19, 10);
			this.stepChoiceList.Name = "stepChoiceList";
			this.stepChoiceList.Size = new System.Drawing.Size(45, 40);
			this.stepChoiceList.TabIndex = 10;
			this.stepChoiceList.SelectedIndexChanged += new System.EventHandler(this.StepChoiceListSelectedIndexChanged);
			// 
			// buttonAntiClockwise
			// 
			//~ this.buttonAntiClockwise.Image = ((System.Drawing.Image)(resources.GetObject("buttonAntiClockwise.Image")));
			this.buttonAntiClockwise.Location = new System.Drawing.Point(19, 109);
			this.buttonAntiClockwise.Name = "buttonAntiClockwise";
			this.buttonAntiClockwise.Size = new System.Drawing.Size(45, 45);
			this.buttonAntiClockwise.TabIndex = 11;
			this.buttonAntiClockwise.UseVisualStyleBackColor = true;
			// 
			// buttonClockwise
			// 
			//~ this.buttonClockwise.Image = ((System.Drawing.Image)(resources.GetObject("buttonClockwise.Image")));
			this.buttonClockwise.Location = new System.Drawing.Point(121, 109);
			this.buttonClockwise.Name = "buttonClockwise";
			this.buttonClockwise.Size = new System.Drawing.Size(45, 45);
			this.buttonClockwise.TabIndex = 12;
			this.buttonClockwise.UseVisualStyleBackColor = true;
			// 
			// infoView
			// 
			this.infoView.Location = new System.Drawing.Point(121, 24);
			this.infoView.Name = "infoView";
			this.infoView.Size = new System.Drawing.Size(60, 28);
			this.infoView.TabIndex = 13;
			// 
			// MoveController
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(184, 162);
			this.Controls.Add(this.infoView);
			this.Controls.Add(this.buttonClockwise);
			this.Controls.Add(this.buttonAntiClockwise);
			this.Controls.Add(this.stepChoiceList);
			this.Controls.Add(this.positionTips);
			this.Controls.Add(this.buttonReset);
			this.Controls.Add(this.buttonDown);
			this.Controls.Add(this.buttonRight);
			this.Controls.Add(this.buttonLeft);
			this.Controls.Add(this.buttonUp);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MoveController";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Move Controller";
			this.Shown += new System.EventHandler(this.MoveControllerShown);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label infoView;
		public System.Windows.Forms.Button buttonClockwise;
		public System.Windows.Forms.Button buttonAntiClockwise;
		private System.Windows.Forms.ListBox stepChoiceList;
		private System.Windows.Forms.Label positionTips;
		public System.Windows.Forms.Button buttonReset;
		public System.Windows.Forms.Button buttonDown;
		public System.Windows.Forms.Button buttonRight;
		public System.Windows.Forms.Button buttonLeft;
		public System.Windows.Forms.Button buttonUp;
	}
}
