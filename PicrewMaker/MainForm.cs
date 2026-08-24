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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using ClassExtensions;
using StackHandler;

//using ImageHandler;


namespace PicrewCreator
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		//string dir=@"F:\Projects\dotnet\Framework\c#\PicrewCreator\testimg";
		//string dir=@"F:\Projects\others\picrew\maker\1180183";
		Stack stack=null;
		bool opened=false;
		int lastTabIndex=0;
		int selectedStackIndex=0;
		int selectedLayerIndex=-1;
		
		static int iconSize=96;
		
		//static System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
		Image no_imgIcon=GetNoImageBitmap();
			//((Image)(resources.GetObject("no_img")));
		
		public MainForm(string path=null)
		{
			InitializeComponent();
			// 32bit Task.Run() fail for some reason
			if(Environment.Is64BitProcess)
				 tabControl1.SelectedIndexChanged += (s,e)=>Task.Run(()=>TabControl1SelectedIndexChanged(s,e));
			else tabControl1.SelectedIndexChanged += TabControl1SelectedIndexChanged;
			
			if(string.IsNullOrEmpty(path))return;
			LoadPath(path);
		}
		
		void LoadPath(string path)
		{
			try
			{
				//if(openingMaker)return;
				if(Directory.Exists(path))
				{
					LoadStack(path);
				}
				if(File.Exists(path))
				{
					LoadStack(
						Path.GetDirectoryName(path),
						Path.GetFileName(path)
					);
				}
				
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				Application.Exit();
				throw ex;
				//this.Load += (sender,e)=>{Application.Exit();};
				
			}
		}
		
		void LoadStack(string dir,string stackFile="stack.xml",bool doSave=true)
		{
			ShowLoading();
			if(stack!=null)
			{
				if(doSave)stack.Save();
				stack.Close();
				stack.dir=dir;
				//stack.file=stackFile;
				//stack=null;
				//GC.Collect();
				//GC.WaitForPendingFinalizers();
			}
			else
			{
				stack=new Stack(dir);
			}
			//MessageBox.Show(opened.ToString());
			if(File.Exists(
				Path.Combine(dir,"stack_backup.xml")
			) )opened=true;
			
			//MessageBox.Show(opened.ToString());
			if(!opened)
			{
				//MessageBox.Show("copying");
				File.Copy(
					Path.Combine(dir,"stack.xml"),
					Path.Combine(dir,"stack_backup.xml")
				);
			}
			
			
			stack.Load(stackFile);
			stack.LoadImage();
			
			//this.textBox1.Visible=false;
			//ImageDialog(stack.img);
			//test();
			//ImageDialog(stack.img);
			this.pictureBox1.Image=stack.img;
			stack.SaveThumbnails();
			//this.button1.Visible=true;
			//this.button2.Visible=true;
			//this.button3.Visible=true;
			
			if(!opened)CommentViewDialog();
			
			/*
			LoadTabPages();
		}
		void LoadTabPages()
		{
			*/
			//this.tabControl1.SelectedIndexChanged;
			if(this.tabControl1.TabPages.Count>0)
			{
				var tabEvent=new System.EventHandler(TabControl1SelectedIndexChanged);
				this.tabControl1.SelectedIndexChanged-=tabEvent;
				this.tabControl1.TabPages.Clear();
				this.tabControl1.SelectedIndexChanged+=tabEvent;
			}
			
			//ToolStrip strip=new ToolStrip();
			foreach (XmlNode layersStack in stack.stacks)
			{
				TabPage tab=new TabPage();
				ToolStrip strip=new ToolStrip();
				ListView view=new ListView();
				
				
				tab.Text=layersStack.GetAttribute("name");
				//tab.
				
				view.MultiSelect=false;
				view.View=View.LargeIcon;
				view.Location=new Point(0,strip.Size.Height);
				view.Dock=DockStyle.Fill;
				view.BackColor=Color.DarkGray;
				
				List<Color> colors=new List<Color>();
				foreach (XmlNode layer in layersStack)
				{
					string imgDataPath=layer.GetAttribute("src");
					string colorString=imgDataPath.Substring(imgDataPath.IndexOf('#')+1,6);
					Color color=ColorFix.FromRgb(colorString);
					if(!colors.Contains(color))
						colors.Add(color);
				}
				foreach (var color in colors)
				{
					ToolStripButton b=new ToolStripButton();
					b.BackColor=color;
					strip.Items.Add(b);
				}
				strip.ItemClicked+=new ToolStripItemClickedEventHandler(colorButtonClick);
				
				//MessageBox.Show(view.Items.Count.ToString());
				view.SelectedIndexChanged+= (sender,e)=>{
					if(view.SelectedItems.Count==0)return;
					//int stackIndex=this.tabControl1.SelectedIndex;
					selectedLayerIndex=view.SelectedItems[0].ImageIndex-1;
					
					if(selectedLayerIndex<0) stack.CloseStack(selectedStackIndex);
					else stack.SelectLayer(selectedStackIndex,selectedLayerIndex);
					
					this.pictureBox1.Image=stack.img;
					stack.SaveThumbnails();
					//MessageBox.Show("layer:"+layerIndex,"stack:"+stackIndex);
				};
				
				
				
				tab.Controls.Add(view);
				tab.Controls.Add(strip);
				this.tabControl1.TabPages.Add(tab);
			}
			
			/*if(Environment.Is64BitProcess)
			{
				tabControl1.Enabled=false;
				Task.Run(()=>LoadEnd());
				tabControl1.Enabled=true;
			}
			else */LoadEnd();
		}
		void LoadEnd()
		{
			var imgl=stack.GetLayerIcons(no_imgIcon,0,iconSize);
			var nowView=((ListView)
			 this.tabControl1.SelectedTab.Controls[0]
			);
			nowView.BeginUpdate();
			var iconList=stack.GetLayerIcons(no_imgIcon,0,iconSize);
			nowView.Invoke((MethodInvoker)(()=>{nowView.LargeImageList=iconList;}));
			
			initView();
			nowView.EndUpdate();
			
			lastTabIndex=0;
			selectedStackIndex=0;
			initView();
			
			ShowLoadDone();
		}
		
		void colorButtonClick(object sender, ToolStripItemClickedEventArgs e)
		{
			((ListView)
			 this.tabControl1.SelectedTab.Controls[0]
			).Items.Clear();
			
			((ListView)
			 this.tabControl1.SelectedTab.Controls[0]
			).Items.Add("",0);
			
			int stackIndex=this.tabControl1.SelectedIndex;
			//int layerIndex=
			var colorString="#"+e.ClickedItem.BackColor.ToStringRgb();
			//MessageBox.Show(colorString);
			//string s="";
			//foreach (XmlNode layer in stack.stacks[stackIndex].SelectNodes("layer"))
			var layers=stack.stacks[stackIndex].SelectNodes("layer");
			for (int i = 0; i < layers.Count; i++)
			{
				var src=layers[i].GetAttribute("src");
				//s+="\n"+src+"	";
				if(src.Contains(colorString) )
				{
					((ListView)
					 this.tabControl1.SelectedTab.Controls[0]
					).Items.Add("",i+1);
					//s+="Added";
				}
			}
			
			//MessageBox.Show(s,colorString);
			((ListView)
			 this.tabControl1.SelectedTab.Controls[0]
			).SetGap(iconSize);
				//.Items=view.Items;
		}
		
		void initView()
		{
			((ListView)
			 this.tabControl1.SelectedTab.Controls[0]
			).Items.Clear();
			
			((ListView)
			 this.tabControl1.SelectedTab.Controls[0]
			).Items.Add("",0);
			
			int stackIndex=this.tabControl1.SelectedIndex;
			selectedLayerIndex=-1;
			//int layerIndex=
			var colorString="#"+
				((ToolStrip)
				 this.tabControl1.SelectedTab.Controls[1]
				).Items[0].BackColor.ToStringRgb();
			//MessageBox.Show(colorString);
			//string s="";
			//foreach (XmlNode layer in stack.stacks[stackIndex].SelectNodes("layer"))
			var layers=stack.stacks[stackIndex].SelectNodes("layer");
			for (int i = 0; i < layers.Count; i++)
			{
				var src=layers[i].GetAttribute("src");
				//s+="\n"+src+"	";
				if(src.Contains(colorString) )
				{
					((ListView)
					 this.tabControl1.SelectedTab.Controls[0]
					).Items.Add("",i+1);
					//s+="Added";
				}
			}
			
			//MessageBox.Show(s,colorString);
			((ListView)
			 this.tabControl1.SelectedTab.Controls[0]
			).SetGap(iconSize);
				//.Items=view.Items;
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		void ShowLoading()
		{
			this.label1.Text="Loading...";
			this.label1.Visible=true;
			//this.pictureBox2.Visible=true;
			this.button1.Visible=false;
			this.button2.Visible=false;
			this.button3.Visible=false;
			this.button4.Visible=false;
		}
		void ShowLoadDone()
		{
			//this.pictureBox2.Visible=false;
			this.label1.Visible=false;
			this.button1.Visible=true;
			this.button2.Visible=true;
			this.button3.Visible=true;
			this.button4.Visible=true;
		}
		
		void ImageViewDialog()
		{
			stack.Save();
			Size size=new Size(stack.width,stack.height);
			
			Form imgViewDialog=new Form();
			imgViewDialog.Text="Click picture to save file";//"Your picture is done";//"Drag picture to save file";
			imgViewDialog.Size=size;
			imgViewDialog.StartPosition=FormStartPosition.CenterParent;
			imgViewDialog.FormBorderStyle=FormBorderStyle.FixedDialog;
			
			PictureBox imgView=new PictureBox();
			imgView.Size=size;
			imgView.SizeMode= PictureBoxSizeMode.Zoom;
			imgView.Dock=DockStyle.Fill;
			imgView.Image=stack.img;
			imgView.Click+= (sender,e)=>{
				using(var save=new SaveFileDialog())
				{
					//Stream file;
					/*
					ImageFormat
						.Bmp
						.Emf
						.Exif
						.Gif
						.Icon
						.Jpeg
						.Png
						.Tiff
						.Wmf
					*/
					save.Filter=
						"Portable NetWork Graphics (*.png)|*.png"+
						"|Joint Photographic Experts Group (*.jpg)|*.jpg"+
						"|Bitmap File (*.bmp)|*.bmp"+
						"|All Files (*.*)|*.*"
					;
					save.RestoreDirectory=true;
					save.FileName="mergedimage.png";
					if(save.ShowDialog()==DialogResult.OK)
					{
						//MessageBox.Show(save.FileName);
						try
						{
							stack.img.SaveAutoFormat(save.FileName);
							MessageBox.Show("Picture saved!","Save Picture");
						}
						catch (ImageExt.UnsupportedImageFormat ex)
						{
							MessageBox.Show(ex.Message,"Format Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
							//throw;
						}
						/*
						if((file=save.OpenFile())!=null)
						{
							var content=File.ReadAllBytes(Path.Combine(stack.dir,"mergedimage.png"));
							file.Write(content,0,content.Length);
							file.Close();
						}
						MessageBox.Show("Picture saved!","Save Picture");
						*/
					}
				}
			};
			
			imgViewDialog.Controls.Add(imgView);
			
			imgViewDialog.ShowDialog();
			imgViewDialog.Dispose();
		}
		
		void CommentViewDialog()
		{
			Size size=new Size(stack.width,stack.height);
			
			Form commentViewDialog=new Form();
			commentViewDialog.Text="Description";
			commentViewDialog.Size=size;
			commentViewDialog.StartPosition=FormStartPosition.CenterParent;
			
			RichTextBox commentView=new RichTextBox();
			commentView.ReadOnly=true;
			commentView.BorderStyle=BorderStyle.None;
			commentView.Dock=DockStyle.Fill;
			commentView.Lines=File.ReadAllLines(Path.Combine(stack.dir,"comment.txt"));
			
			commentViewDialog.Controls.Add(commentView);
			
			commentViewDialog.ShowDialog();
			commentViewDialog.Dispose();
		}
		
		
		void MoveLayer(int dx,int dy)
		{
			stack.Move(selectedStackIndex,selectedLayerIndex,dx,dy);
			this.pictureBox1.Image=stack.CombineImages();
		}
		void RotateLayer(int dAngle)
		{
			stack.Rotate(selectedStackIndex,selectedLayerIndex,dAngle);
			this.pictureBox1.Image=stack.CombineImages();
		}
		void MoveViewDialog()
		{
			//int stackIndex=this.tabControl1.SelectedIndex;
			//int layerIndex=stack.selectedLayerIndex;
			int stackIndex=stack.stacks.Count-1-selectedStackIndex;
			
			var moveViewDialog=new MoveController();
			var currentPositionBackup=stack.poses[stackIndex];
			int currentAngleBackup=stack.angles[stackIndex];
			
			moveViewDialog.MoveUp	+= ()=>MoveLayer(0,-moveViewDialog.step);
			moveViewDialog.MoveDown	+= ()=>MoveLayer(0,moveViewDialog.step);
			moveViewDialog.MoveLeft	+= ()=>MoveLayer(-moveViewDialog.step,0);
			moveViewDialog.MoveRight+= ()=>MoveLayer(moveViewDialog.step,0);
			
			moveViewDialog.RotateAntiClockwise	+=()=>RotateLayer(-moveViewDialog.step);
			moveViewDialog.RotateClockwise		+=()=>RotateLayer(moveViewDialog.step);
			
			moveViewDialog.Reset+= ()=>{
				stack.poses[stackIndex]=currentPositionBackup;
				stack.angles[stackIndex]=currentAngleBackup;
				stack.SavePosition(selectedStackIndex,selectedLayerIndex);
				stack.SaveRotation(selectedStackIndex,selectedLayerIndex);
				
				this.pictureBox1.Image=stack.LoadImage();
			};
			
			
			moveViewDialog.ViewUpdate+= ()=>{
				var pos=stack.poses[stackIndex];
				moveViewDialog.UpdateInfoView(pos.X,pos.Y,stack.angles[stackIndex]);
			};
			moveViewDialog.FinalizeMoveViewUpdate();
			
			//moveViewDialog.ViewUpdate.Invoke();
			moveViewDialog.ShowDialog();
			moveViewDialog.Dispose();
			stack.SavePosition(selectedStackIndex,selectedLayerIndex);
			stack.SaveRotation(selectedStackIndex,selectedLayerIndex);
			//stack.poses[stack.selectedLayerIndex]=currentSize;
		}
		
		public static Bitmap GetNoImageBitmap()
		{
			var bmp=new Bitmap(160,160,PixelFormat.Format32bppArgb);
			var g=Graphics.FromImage(bmp);
			var bs=new SolidBrush(Color.FromArgb(170,Color.Black));
			var bc=new SolidBrush(Color.FromArgb(125,Color.Black));
			var f=new Font("Arial",32);
			
			g.TextRenderingHint=System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
			g.FillEllipse(bc,20,20,120,120);
			g.DrawString("No",f,bs,45,32);
			g.DrawString("Image",f,bs,12,70);
			
			g.Dispose();
			bs.Dispose();
			bc.Dispose();
			f.Dispose();
			
			return bmp;
		}
		
		
		
		void Button1Click(object sender, EventArgs e)
		{
			stack.SaveImage();
			ImageViewDialog();
		}
		
		void MainFormDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.All;			//重要代码：表明是所有类型的数据，比如文件路径
			else
				e.Effect = DragDropEffects.None;
		}
		
		void MainFormDragDrop(object sender, DragEventArgs e)
		{
			/*
			string path = (
					(System.Array)e
					.Data
					.GetData(DataFormats.FileDrop)
				).GetValue(0)
				.ToString()
			;
			*/
			string path = (
					(string[])e
					.Data
					.GetData(DataFormats.FileDrop)
				)[0]
			;
			//MessageBox.Show(path);
			LoadPath(path);
		}
		
		
		void Button2Click(object sender, EventArgs e)
		{
			CommentViewDialog();
		}
		
		
		void Button3Click(object sender, EventArgs e)
		{
			if(MessageBox.Show(
				"Do you want to clear ALL states and reset to the default state?",
				"Reset Stack",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			)==DialogResult.Yes)
			{
				//stack.Close();
				File.Copy(
					Path.Combine(stack.dir,"stack_backup.xml"),
					Path.Combine(stack.dir,"stack.xml"),
					true
				);
				LoadStack(stack.dir,"stack.xml",false);
			}
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			if(selectedLayerIndex<0)
			{
				MessageBox.Show("No layer selected!","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}
			MoveViewDialog();
			stack.SaveThumbnails();
		}
		
		void TabControl1SelectedIndexChanged(object sender, EventArgs e)
		{
			this.tabControl1.Enabled=false;
			int nowTabIndex=this.tabControl1.SelectedIndex;
			selectedStackIndex=this.tabControl1.SelectedIndex;
			
			ListView lastView=(ListView)this.tabControl1.TabPages[lastTabIndex].Controls[0];
			ListView nowView=(ListView)this.tabControl1.SelectedTab.Controls[0];
			//ToolStrip nowStrip=(ToolStrip)this.tabControl1.SelectedTab.Controls[1];
			
			lastView.LargeImageList.Dispose();
			lastView.Clear();
			//GC.Collect();
			//nowStrip.Enabled=false;
			var iconList=stack.GetLayerIcons(no_imgIcon,nowTabIndex,iconSize);
			nowView.BeginUpdate();
			nowView.Invoke((MethodInvoker)(()=>{nowView.LargeImageList=iconList;}));
			
			initView();
			//nowStrip.Enabled=true;
			nowView.EndUpdate();
			//nowStrip.Items[0].Select();
			//MessageBox.Show(this.tabControl1.SelectedIndex.ToString());
			lastTabIndex=nowTabIndex;
			
			this.tabControl1.Enabled=true;
		}
		
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(stack!=null)if(stack.doc!=null)
			{
				stack.Save();
				stack.Close();
			}
		}
		
		void PictureBox1Paint(object sender, PaintEventArgs e)
		{
			stack.SaveThumbnails();
		}
		/*
		void ClickOpen()
		{
			using (OpenFileDialog open = new OpenFileDialog())
			{
				open.Filter="Stack File (*.xml)|*.xml|All Files (*.*)|*.*";
				open.RestoreDirectory=true;
				
				if(open.ShowDialog()==DialogResult.OK)
				{
					LoadPath(open.FileName);
				}
			}
		}
		
		void TextBox1Click(object sender, EventArgs e)
		{
			ClickOpen();
		}
		
		void PictureBox1Click(object sender, EventArgs e)
		{
			ClickOpen();
		}
		//*/
		
	}
	
	
}
