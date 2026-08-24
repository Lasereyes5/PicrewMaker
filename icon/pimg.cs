using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using System.Web;
using System.Windows.Forms;

public static class Program
{
	public static void Main(string[] args)
	{
		var img=Image.FromFile("test.png");
		var bmp=new Bitmap(img);//GetNoImageBitmap();
		//ImageDialog(bmp);
		//~ MessageBox.Show(
		/*Console.WriteLine(
			bmp.GetNonBlankRectSquare().ToString()
		);*/
		bmp.ProcessRaw(
			(rawData,width,height,stride,pixelFormat)
			=>{
				unsafe{
					bool find = false;
					byte* ptr=(byte*)rawData;
					
					
					
					
					// test
					Console.WriteLine(pixelFormat.ToString());
					var sw=new StreamWriter("testPixels.txt");
					for (int y = 0; y < height; y++)//行
					{
						find = false;
						byte* row = ptr + (y * stride);
						for (int x = 0; x < width; x++)//列
						{
							byte* pixel = row + (x * 4);//*a r g b
							sw.Write(string.Format("[{0},{1},{2},{3}] ",pixel[0],pixel[1],pixel[2],pixel[3]));
						}
						sw.WriteLine();
					}
					sw.Dispose();
					
					
				}
			}
		);
		//bmp.Save("NoImage.png");
		img.Dispose();
		bmp.Dispose();
	}
	public static Bitmap GetNoImageBitmap()
	{
		var bmp=new Bitmap(160,160,PixelFormat.Format32bppArgb);
		var g=Graphics.FromImage(bmp);
		var bs=new SolidBrush(Color.FromArgb(70,Color.Black));
		var bc=new SolidBrush(Color.FromArgb(25,Color.Black));
		var f=new Font("Arial",32);
		//~ var f=new Font("Microsoft YaHei UI",32);
		
		g.TextRenderingHint=System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
		g.FillEllipse(bc,20,20,120,120);
		g.DrawString("No",f,bs,48,32);
		g.DrawString("Image",f,bs,13,70);
		
		g.Dispose();
		bs.Dispose();
		bc.Dispose();
		f.Dispose();
		
		return bmp;
	}
	public static void ImageDialog(Image img)
	{
		Form imgViewDialog=new Form();
		imgViewDialog.Text="Picture Dialog";
		
		imgViewDialog.Size=img.Size;
		imgViewDialog.StartPosition=FormStartPosition.CenterParent;
		//~ imgViewDialog.FormBorderStyle=FormBorderStyle.FixedDialog;
		
		PictureBox imgView=new PictureBox();
		//~ imgView.Size=size;
		imgView.SizeMode= PictureBoxSizeMode.AutoSize;
		imgView.Dock=DockStyle.Fill;
		imgView.Image=img;
		
		imgViewDialog.Controls.Add(imgView);
		
		imgViewDialog.ShowDialog();
	}
	
	
	/// <summary>
	/// deepseek generated 
	/// 高性能处理位图 - 不安全代码版本（最快）
	/// </summary>
	/// <param name="this Bitmap">
	/// Bitmap to process.
	/// </param>
	/// <param name="processAction(rawData,width,height,stride,pixelFormat)">
	/// Action to process raw data from bitmap,with bitmap info param.
	/// </param>
	public static unsafe void ProcessRaw(this Bitmap bitmap, Action<IntPtr, int, int, int, PixelFormat> processAction)
	{
		if (bitmap == null) throw new ArgumentNullException("bitmap");
		if (processAction == null) throw new ArgumentNullException("processAction");
		
		BitmapData bmpData = null;
		try
		{
			bmpData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadWrite,
				bitmap.PixelFormat
			);
			
			// 直接传递内存指针，避免数据复制
			processAction(bmpData.Scan0, bitmap.Width, bitmap.Height, bmpData.Stride, bitmap.PixelFormat);
		}
		finally
		{
			if (bmpData != null)
				bitmap.UnlockBits(bmpData);
		}
	}
	public static Rectangle GetNonBlankRect(this Bitmap bmp)
	{
		int top=0,
			bottom=0,
			left=0,
			right=0;
		
		bmp.ProcessRaw(
			(rawData,width,height,stride,pixelFormat)
			=>{
				unsafe{
					bool find = false;
					byte* ptr=(byte*)rawData;
					
					
					
					/*
					// test
					Console.WriteLine(pixelFormat.ToString());
					var sw=new StreamWriter("NoImagePixels.txt");
					for (int y = 0; y < height; y++)//行
					{
						find = false;
						byte* row = ptr + (y * stride);
						for (int x = 0; x < width; x++)//列
						{
							byte* pixel = row + (x * 4);//*a r g b
							sw.Write(string.Format("[{0},{1},{2},{3}] ",pixel[0],pixel[1],pixel[2],pixel[3]));
						}
						sw.WriteLine();
					}
					sw.Dispose();
					*/
					
					
					//~ Top
					for (int y = 0; y < height; y++)//行
					{
						find = false;
						byte* row = ptr + (y * stride);
						for (int x = 0; x < width; x++)//列
						{
							//~ 0,0 1,0 2,0 3,0 4,0 ...
							//~ 0,1 1,1 2,1 3,1 4,1 ...
							byte* pixel = row + (x * 4);//*a r g b
							if (pixel[3]!=0)//[a] r g b
							{
								top=y;
								find = true;
								break;
							}
						}
						if (find) break;
					}
					//~ Bottom
					for (int y = height-1; y >=0 ; y--)//行
					{
						find = false;
						byte* row = ptr + (y * stride);
						for (int x = 0; x < width; x++)//列
						{
							//~ 0,600 1,600 2,600 3,600 4,600 ...
							//~ 0,599 1,599 2,599 3,599 4,599 ...
							byte* pixel = row + (x * 4);//*b g r a
							if (pixel[3]!=0)// b g r[a]
							{
								bottom=y;
								find = true;
								break;
							}
						}
						if (find) break;
					}
					//~ Left
					for (int x = 0; x < width; x++)//列
					{
						find = false;
						int column = x * 4;
						for (int y = 0; y < height; y++)//行
						{
							//~ 0,0 0,1 0,2 0,3 0,4 ...
							//~ 1,0 1,1 1,2 1,3 1,4 ...
							byte* pixel = ptr + (column + y * stride );//*b g r a
							if (pixel[3]!=0)// b g r[a]
							{
								left=x;
								find = true;
								break;
							}
						}
						if (find) break;
					}
					//~ Right
					for (int x = width-1; x >=0 ; x--)//列
					{
						find = false;
						int column = x * 4;
						for (int y = 0; y < height; y++)//行
						{
							//~ 600,0 600,1 600,2 600,3 600,4 ...
							//~ 599,0 599,1 599,2 599,3 599,4 ...
							byte* pixel = ptr + (column + y * stride );//*b g r a
							if (pixel[3]!=0)// b g r[a]
							{
								right=x;
								find = true;
								break;
							}
						}
						if (find) break;
					}
					
				}//unsafe
			}//action
		);//process func
		
		return new Rectangle(left,top,right-left,bottom-top);
	}
	public static Rectangle GetNonBlankRectSquare(this Bitmap bmp)
	{
		var nonBlankRect=bmp.GetNonBlankRect();
		int left=nonBlankRect.Left,
			top=nonBlankRect.Top,
			width=nonBlankRect.Width,
			height=nonBlankRect.Height;
		
		if(width<height)
			return new Rectangle(left-(height-width)/2,top,height,height);
		else
			return new Rectangle(left,top-(width-height)/2,width,width);
	}
	public static Bitmap CropBlankSquare(this Image img)
	{
		Bitmap bmpIn = new Bitmap(img);
		Rectangle rect=bmpIn.GetNonBlankRectSquare();
		int iWidth=rect.Width;
		int iHeight=rect.Height;
		Bitmap bmpOut = new Bitmap(iWidth, iHeight, bmpIn.PixelFormat);//.Format32bppArgb
		Graphics g = Graphics.FromImage(bmpOut);
		g.DrawImage(img, new Rectangle(0, 0, iWidth, iHeight), rect, GraphicsUnit.Pixel);
		g.Dispose();
		bmpIn.Dispose();
		return bmpOut;
		
		//var bmpin=new Bitmap(img);
		//var bmpout=bmpin.CropBlankSquare();
		//bmpin.Dispose();
		//return bmpout;
	}
	
}