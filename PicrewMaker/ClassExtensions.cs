/*
 * 由SharpDevelop创建。
 * 用户： pc
 * 日期: 2024/11/7
 * 时间: 20:18
 * let's all love lain!
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using System.Windows.Forms;

namespace ClassExtensions
{
	
	/// <summary>
	/// Extended to combine images
	/// </summary>
	public static class ImageExt
	{
		public static Size imgSize=new Size(0,0);
		/*
		public static Image CombineImages(this Image[] imgs,int width,int height)
		{
			var imgRect=new Rectangle(0, 0, width, height);
			
			Bitmap bitMap = new Bitmap(width,height);
			Graphics resultGraphic = Graphics.FromImage(bitMap);
			resultGraphic.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, width, height));
			//foreach(var img in imgs)
			for (int i = 0; i < imgs.Length; i++)
			{var img=imgs[i];
				if(img!=null)
					resultGraphic.DrawImageUnscaledAndClipped(img,imgRect);
			}
			Image resultImg = bitMap;
			return resultImg;
			
		}
		public static Image CombineImages(this Image[] imgs,Point[] poses)
		{
			//var imgRect=new Rectangle(0, 0, width, height);
			
			Bitmap bitMap = new Bitmap(imgSize.Width,imgSize.Height);
			Graphics resultGraphic = Graphics.FromImage(bitMap);
			resultGraphic.FillRectangle(Brushes.Transparent, new Rectangle(new Point(0,0), imgSize));
			//foreach(var img in imgs)
			for (int i = 0; i < imgs.Length; i++)
			{var img=imgs[i];
				
				if(img!=null)
					resultGraphic.DrawImageUnscaledAndClipped(img,new Rectangle(poses[i],img.Size));
			}
			Image resultImg = bitMap;
			return resultImg;
			
		}
		*/
		public static Image CombineImages(this Image[] imgs,Point[] poses,int[] angles)
		{
			//var imgRect=new Rectangle(0, 0, width, height);
			
			Bitmap bitMap = new Bitmap(imgSize.Width,imgSize.Height);
			Graphics resultGraphic = Graphics.FromImage(bitMap);
			resultGraphic.FillRectangle(Brushes.Transparent, new Rectangle(new Point(0,0), imgSize));
			//foreach(var img in imgs)
			for (int i = 0; i < imgs.Length; i++)
			{var img=imgs[i];
				
				if(img!=null)
				{
					int dx=poses[i].X+img.Width/2;
					int dy=poses[i].Y+img.Height/2;
					resultGraphic.TranslateTransform(dx,dy);
					//angle-=dAngle
					resultGraphic.RotateTransform(angles[i]);
					resultGraphic.DrawImageUnscaledAndClipped(img,new Rectangle(new Point(-img.Width/2,-img.Height/2),img.Size));
					resultGraphic.RotateTransform(-angles[i]);
					resultGraphic.TranslateTransform(-dx,-dy);
				}
			}
			resultGraphic.Dispose();
			Image resultImg = bitMap;
			return resultImg;
			
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
						//~ Top
						for (int y = 0; y < height; y++)//行
						{
							find = false;
							byte* row = ptr + (y * stride);
							for (int x = 0; x < width; x++)//列
							{
								//~ 0,0 1,0 2,0 3,0 4,0 ...
								//~ 0,1 1,1 2,1 3,1 4,1 ...
								byte* pixel = row + (x * 4);//*b g r a
								if (pixel[3]!=0)// b g r[a]
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
		/*
		
		void ImageDialog(Image img)
		{
			Size size=new Size(stack.width,stack.height);
			
			Form imgViewDialog=new Form();
			imgViewDialog.Text="Drag picture to save file";
			imgViewDialog.Size=size;
			imgViewDialog.StartPosition=FormStartPosition.CenterParent;
			imgViewDialog.FormBorderStyle=FormBorderStyle.FixedDialog;
			
			PictureBox imgView=new PictureBox();
			imgView.Size=size;
			imgView.SizeMode= PictureBoxSizeMode.Zoom;
			imgView.Dock=DockStyle.Fill;
			imgView.Image=img;
			
			imgViewDialog.Controls.Add(imgView);
			
			imgViewDialog.ShowDialog();
		}
		
		
		
		
		public static Rectangle GetNonBlankRectSquare(this Bitmap bmp)
		{
			int top=0,
				bottom=0,
				left=0,
				right=0;
			//~ Top
			for (int i = 0; i < bmp.Height; i++)//行
			{
				bool find = false;
				for (int j = 0; j < bmp.Width; j++)//列
				{
					//~ 0,0 1,0 2,0 3,0 4,0 ...
					//~ 0,1 1,1 2,1 3,1 4,1 ...
					Color c = bmp.GetPixel(j, i);
					if (c.A!=0)
					{
						top=i;
						find = true;
						break;
					}
				}
				if (find) break;
			}
			//~ Bottom
			for (int i = bmp.Height-1; i >=0 ; i--)//行
			{
				bool find = false;
				for (int j = 0; j < bmp.Width; j++)//列
				{
					//~ 0,600 1,600 2,600 3,600 4,600 ...
					//~ 0,599 1,599 2,599 3,599 4,599 ...
					Color c = bmp.GetPixel(j, i);
					if (c.A!=0)
					{
						bottom=i;
						find = true;
						break;
					}
				}
				if (find) break;
			}
			//~ Left
			for (int i = 0; i < bmp.Width; i++)//列
			{
				bool find = false;
				for (int j = 0; j < bmp.Height; j++)//行
				{
					//~ 0,0 0,1 0,2 0,3 0,4 ...
					//~ 1,0 1,1 1,2 1,3 1,4 ...
					Color c = bmp.GetPixel(i,j);
					if (c.A!=0)
					{
						left=i;
						find = true;
						break;
					}
				}
				if (find) break;
			}
			//~ Right
			for (int i = bmp.Width-1; i >=0 ; i--)//列
			{
				bool find = false;
				for (int j = 0; j < bmp.Height; j++)//行
				{
					//~ 600,0 600,1 600,2 600,3 600,4 ...
					//~ 599,0 599,1 599,2 599,3 599,4 ...
					Color c = bmp.GetPixel(i,j);
					if (c.A!=0)
					{
						right=i;
						find = true;
						break;
					}
				}
				if (find) break;
			}
			int width=right-left;
			int height=bottom-top;
			if(width<height)
				return new Rectangle(left-(height-width)/2,top,height,height);
			else
				return new Rectangle(left,top-(width-height)/2,width,width);
			//return new Rectangle(left,top,,);
		}
		public static Bitmap CropBlank(this Image img)
		{
			return (new Bitmap(img)).CropBlank();
		}
		public static Bitmap CropBlank(this Bitmap bmp)
		{
			return bmp.Clone(bmp.GetNonBlankRect(),bmp.PixelFormat);
		}
		public static Bitmap CropBlankSquare(this Image img)
		{
			Bitmap bmpIn = new Bitmap(img);
			Rectangle rect=bmpIn.GetNonBlankRectSquare();
			int iWidth=rect.Width;
			int iHeight=rect.Height;
			Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format32bppArgb);
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
		public static Bitmap CropBlankSquareSize(this Image img,Size size)
		{
			Bitmap bmpIn = new Bitmap(img);
			Rectangle rect=bmpIn.GetNonBlankRectSquare();
			int iWidth=rect.Width;
			int iHeight=rect.Height;
			Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmpOut);
			g.DrawImage(img, new Rectangle(0, 0, iWidth, iHeight), rect, GraphicsUnit.Pixel);
			g.Dispose();
			bmpIn.Dispose();
			Bitmap bmp = new Bitmap(bmpOut,size);
			bmpOut.Dispose();
			return bmp;
			//var outbmp=bmp.Clone(bmp.GetNonBlankRectSquare(),bmp.PixelFormat);
			//return outbmp;
		}
		public static Bitmap CropBlankSquare(this Bitmap bmpIn)
		{
			Rectangle rect=bmpIn.GetNonBlankRectSquare();
			int iWidth=rect.Width;
			int iHeight=rect.Height;
			Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmpOut);
			g.DrawImage(bmpIn, new Rectangle(0, 0, iWidth, iHeight), rect, GraphicsUnit.Pixel);
			g.Dispose();
			//bmpIn.Dispose();
			return bmpOut;
			//var outbmp=bmp.Clone(bmp.GetNonBlankRectSquare(),bmp.PixelFormat);
			//return outbmp;
			
		}
		public static Bitmap CropBlankSquareSize(this Bitmap bmpIn,Size size)
		{
			Rectangle rect=bmpIn.GetNonBlankRectSquare();
			int iWidth=rect.Width;
			int iHeight=rect.Height;
			Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format32bppArgb);
			Graphics g = Graphics.FromImage(bmpOut);
			g.DrawImage(bmpIn, new Rectangle(0, 0, iWidth, iHeight), rect, GraphicsUnit.Pixel);
			g.Dispose();
			
			Bitmap bmp = new Bitmap(bmpOut,size);
			bmpOut.Dispose();
			return bmp;
			//var outbmp=bmp.Clone(bmp.GetNonBlankRectSquare(),bmp.PixelFormat);
			//return outbmp;
		}
		//*/
		
		
		
		public static ImageFormat[] supportedFormats={
			ImageFormat.Bmp,ImageFormat.Emf,ImageFormat.Exif,
			ImageFormat.Gif,ImageFormat.Icon,ImageFormat.Jpeg,
			ImageFormat.MemoryBmp,ImageFormat.Png,ImageFormat.Tiff,
			ImageFormat.Wmf
		};
		public static string ToLowerExt(this string ext)
		{
			string lowerExt=ext.ToLower();
			switch (lowerExt)
			{
				case "jpg":
					return "jpeg";
				case "tif":
					return "tiff";
				case "ico":
					return "icon";
				default:
					return lowerExt;
					//break;
			}
		}
		public static void SaveAutoFormat(this Image img,string fileName)
		{
			string fileExt=Path.GetExtension(fileName).Substring(1);//remove '.'
			bool supported=false;
			foreach (var fmt in supportedFormats)
			{
				if(fileExt.ToLowerExt()==fmt.ToString().ToLower())
				{
					img.Save(fileName,fmt);
					supported=true;
					break;
				}
			}
			if(!supported)throw new UnsupportedImageFormat("Unsupported Image Format: "+fileExt);
		}
		public class UnsupportedImageFormat:Exception
		{
			public UnsupportedImageFormat(string message):base(message)
			{
				
			}
		}
		
		
		
		
		
	}
	
	/// <summary>
	/// Extended to set gap size of ListView
	/// </summary>
	public static class ListViewExt
	{
		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg,int wParam, int lParam);
		private static readonly int LVM_SETICONSPACING=0x1035;
		
		public static void SetGap(this ListView self, int rowGap,int lineGap)
		{
			SendMessage(self.Handle,LVM_SETICONSPACING,0,0x10000*rowGap+lineGap);
		}
		
		public static void SetGap(this ListView self, int gap)
		{
			SendMessage(self.Handle,LVM_SETICONSPACING,0,0x10000*gap+gap);
		}
		
		public static void CopyItemsBy(this ListView self, ListView src)
		{
			self.Items.Clear();
			foreach (ListViewItem item in src.Items)
			{
				self.Items.Add(item);
			}
		}
	}
	
	
	/// <summary>
	/// Extended to parse uint and string to Color
	/// </summary>
	public static partial class ColorFix
	{
		public static string ToStringRgb(this Color color)
		{
			return ToStringArgb(color).Substring(2);
		}
		public static string ToStringArgb(this Color color)
		{
			return ToInt(color).ToString("X");
		}
		public static int ToInt(this Color color)
		{
			return (
				color.A<<24 |
				color.R<<16 |
				color.G<<8 |
				color.B
			);
		}
		
		
		
		
		public static Color FromRgb(string colorHex)
		{
			return FromArgb("FF"+colorHex);
		}
		public static Color FromArgb(string alphaColorHex)
		{
			int alphaColorValue=int.Parse(alphaColorHex,System.Globalization.NumberStyles.AllowHexSpecifier);
			return Color.FromArgb(alphaColorValue);
		}
		public static Color FromArgb(uint alphaColorValue)
		{
			unchecked
			{
				return Color.FromArgb((int)alphaColorValue);
			}
		}
		public static Color FromArgb(long alphaColorValue)
		{
			unchecked
			{
				return Color.FromArgb((int)alphaColorValue);
			}
		}
	}
	
	
	
	/// <summary>
	/// Reimplement some XmlElement method on XmlNode
	/// </summary>
	public static class XmlExt
	{
		public static string GetAttribute(this XmlNode self,string attribute)
		{
			var a=self.Attributes[attribute];
			if(a==null)return null;
			return a.Value;
		}
		public static XmlNodeList GetNodesByTagName(this XmlNode self,string tagName)
		{
			return self.SelectNodes(tagName);
		}
	}
}
