/*
 * 由SharpDevelop创建。
 * 用户： pc
 * 日期: 2024/10/29
 * 时间: 15:05
 * let's all love lain!
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Xml;
//using ImageHandler;
using ClassExtensions;
using System.Windows.Forms;

namespace StackHandler
{
	/// <summary>
	/// Handle the Picrew downloader OpenRaster index file.
	/// </summary>
	/// 
	public class Stack
	{
		public string dir;
		public string file="stack.xml";
		public XmlDocument doc;
		public XmlNodeList stacks;
		
		public int width;
		public int height;
		public string version;
		public int thumbnailSize=256;
		
		public Image[] imgs;
		public Point[] poses;
		public int[] angles;
		public Image img;
		/// <summary>
		/// Too late to write this attribute,
		/// so only used to select in position array.
		/// </summary>
		//public int selectedStackIndex=-1;
		//public Point pos;
		
		public Stack()
		{
			//doc=new XmlDocument();
		}
		public Stack(string stackDir)
		{
			dir=stackDir;
			//doc=new XmlDocument();
		}
		public void Close()
		{
			//Array.Clear(imgs,0,imgs.Length);
			for (int i = 0; i < imgs.Length; i++)
			{
				if(imgs[i]!=null)
				{
					imgs[i].Dispose();
					imgs[i]=null;
				}
			}
			imgs=null;
			img=null;
			Array.Clear(poses,0,poses.Length);
			Array.Clear(angles,0,angles.Length);
			//selectedStackIndex=-1;
			doc=null;
			stacks=null;
		}
		public void Save(string stackFile)
		{
			if(doc!=null)
				doc.Save(Path.Combine(dir,stackFile));
		}
		public void Save()
		{
			if(doc!=null)
				doc.Save(Path.Combine(dir,file));
		}
		public void SaveThumbnails()
		{
			var thumb=img.GetThumbnailImage(thumbnailSize,thumbnailSize,(()=>{return true;}),new IntPtr());
			thumb.Save(Path.Combine(dir,@"Thumbnails\thumbnail.png"),ImageFormat.Png);
			thumb.Dispose();
		}
		public void Load(string stackFile)
		{
			file=stackFile;
			Load();
		}
		public void Load()
		{
			doc=new XmlDocument();
			doc.Load(Path.Combine(dir,file));
			var rootElm=doc.DocumentElement;//<img>
			int.TryParse(rootElm.GetAttribute("w"),out width);
			int.TryParse(rootElm.GetAttribute("h"),out height);
			ImageExt.imgSize=new Size(width,height);
			version=rootElm.GetAttribute("version");
			
			var rootStack=rootElm.GetElementsByTagName("stack")[0];//<stack>
			stacks=rootStack.SelectNodes("stack");//<stack name=...>...
			imgs=new Image[stacks.Count];
			poses=new Point[stacks.Count];
			angles=new int[stacks.Count];
		}
		
		/*
		public ImageList GetLayerIcons(int stackIndex,int iconSize=16)
		{
			var allImages=new ImageList();
			allImages.ImageSize=new Size(iconSize,iconSize);
			//var allImgLists=new ImageList[stacks.Count];
			//for (int i = 0; i < stacks.Count; i++)
			var stack=stacks[stackIndex];
			var layerList=stack.SelectNodes("layer");//<stack name=...>...
			
			//for (int i = 0; i < layerList.Count; i++)
			foreach (XmlNode layer in layerList)
			{
				string src=layer.GetAttribute("src");
				//allImages=new ImageList();
				allImages.Images.Add(Image.FromFile(Path.Combine(dir,src)));
			}
			//MessageBox.Show(i.ToString());.CropBlank()
			
			//Array.Reverse(allImgLists);
			return allImages;
		}*/
		public ImageList GetLayerIcons(Image baseImg, int stackIndex,int iconSize=16)
		{
			var allImages=new ImageList();
			var size=new Size(iconSize,iconSize);
			allImages.ImageSize=size;
			allImages.Images.Add(baseImg);
			
			var stack=stacks[stackIndex];
			var layerList=stack.SelectNodes("layer");//<stack name=...>...
			
			//for (int i = 0; i < layerList.Count; i++)
			foreach (XmlNode layer in layerList)
			{
				string src=layer.GetAttribute("src");
				//allImages=new ImageList();
				var img=Bitmap.FromFile(Path.Combine(dir,src));
				var bmp=img.CropBlankSquare();
				//new Bitmap(img,size);
				//var icon=Icon.FromHandle(bmp.GetHicon());
				allImages.Images.Add(bmp);
				//bmp.Dispose();
				img.Dispose();
				//img.Dispose();
			}
			//MessageBox.Show(i.ToString());.CropBlank()
			
			//Array.Reverse(allImgLists);
			return allImages;
		}
		
		
		
		
		
		public void SaveImage(string fileName="mergedimage.png")
		{
			if(img!=null)LoadImage();
			img.Save(Path.Combine(dir,fileName));
		}
		
		public Image CombineImages()
		{
			//set ImageExt.imgSize to Size(width,height) in Load()
			img=imgs.CombineImages(poses,angles);
			return img;
		}
		
		public Image LoadImage()
		{
			//set ImageExt.imgSize to Size(width,height) in Load()
			img=LoadImages().CombineImages(poses,angles);
			return img;//.Save("test.png");
		}
		
		public Image[] LoadImages()
		{
			//foreach (XmlNode stack in stacks)
			for (int i = 0; i < stacks.Count; i++)
			{var stack=stacks[i];
				var layerList=stack.SelectNodes("layer");//<stack name=...>...
				
				//for (int i = 0; i < layerList.Count; i++)
				foreach (XmlNode layer in layerList)
				{
					if(layer.GetAttribute("visibility")!="hidden")
					{
						string src=layer.GetAttribute("src");
						//MessageBox.Show(Path.Combine(dir,src));
						imgs[i]=Image.FromFile(Path.Combine(dir,src));
						int x,y;
						int.TryParse(layer.GetAttribute("x"),out x);
						int.TryParse(layer.GetAttribute("y"),out y);
						poses[i]=new Point(x,y);
						int.TryParse(layer.GetAttribute("angle"),out angles[i]);
						//imgList.Add(Path.Combine(dir,src));
						break;
					}
					else
					{
						//Array.Clear(imgs,i,
						if(imgs[i]!=null)
						{
							imgs[i].Dispose();
							imgs[i]=null;
						}
						if(!poses[i].IsEmpty)
						{
							poses[i].X=0;
							poses[i].Y=0;
						}
						//if(angles[i]!=null)
						angles[i]=0;
					}
				}
			}
			Array.Reverse(angles);
			Array.Reverse(poses);
			Array.Reverse(imgs);
			return imgs;
		}
		
		public void ChangeImage(int index,Image img,Point pos,int angle)
		{
			imgs[index]=img;
			poses[index]=pos;
			angles[index]=angle;
			//selectedStackIndex=index;
			CombineImages();
		}
		public void ChangeImage(int index,string imgDataPath,Point pos,int angle)
		{
			ChangeImage(index,Image.FromFile(Path.Combine(dir,imgDataPath)),pos,angle);
		}
		/*
		public void ChangeImageFullPath(int index,string imgPath,Point pos)
		{
			ChangeImage(index,Image.FromFile(imgPath),pos);
		}
		*/
		public void SelectLayer(int stackIndex,int layerIndex)
		{
			var stack=stacks[stackIndex];
			var layerList=stack.SelectNodes("layer");
			//foreach (XmlNode layer in layerList)
			for (int i = 0; i < layerList.Count; i++)
			{var layer=layerList[i];
				var layerAttr=layer.Attributes;
				if(i==layerIndex)
				{
					layerAttr.RemoveNamedItem("visibility");
				}
				else if(layerAttr["visibility"]==null)
				{
					XmlAttribute attr=doc.CreateAttribute("visibility");//new XmlAttribute();
					attr.Value="hidden";
					layerAttr.InsertAfter(attr,layerAttr["src"]);
				}
			}
			
			//selectedStackIndex=stackIndex;
			int imgIndex=stacks.Count-1-stackIndex;
			string selectedSrc=layerList[layerIndex].GetAttribute("src");
			int x,y,angle;
			int.TryParse(layerList[layerIndex].GetAttribute("x"),out x);
			int.TryParse(layerList[layerIndex].GetAttribute("y"),out y);
			int.TryParse(layerList[layerIndex].GetAttribute("angle"),out angle);
			ChangeImage(imgIndex,selectedSrc,new Point(x,y),angle);
		}
		
		public void CloseStack(int stackIndex)
		{
			var stack=stacks[stackIndex];
			foreach (XmlNode layer in stack.SelectNodes("layer"))
			{
				var layerAttr=layer.Attributes;
				if(layerAttr["visibility"]==null)
				{
					XmlAttribute attr=doc.CreateAttribute("visibility");//new XmlAttribute();
					attr.Value="hidden";
					layerAttr.InsertAfter(attr,layerAttr["src"]);
				}
			}
			
			//selectedStackIndex=-1;
			int imgIndex=stacks.Count-1-stackIndex;
			ChangeImage(imgIndex,(Image)null,new Point(0,0),0);
		}
		public void SavePosition(int stackIndex,int layerIndex)
		{
			var stack=stacks[stackIndex];
			var layer=stack.SelectNodes("layer")[layerIndex];
			var layerAttr=layer.Attributes;
			
			int imgIndex=stacks.Count-1-stackIndex;
			layerAttr["x"].Value=poses[imgIndex].X.ToString();
			layerAttr["y"].Value=poses[imgIndex].Y.ToString();
		}
		
		public void Move(int stackIndex,int layerIndex,int dx,int dy)
		{
			int imgIndex=stacks.Count-1-stackIndex;
			poses[imgIndex].X+=dx;
			poses[imgIndex].Y+=dy;
			
			//SavePosition(stackIndex,layerIndex);
		}
		public void SaveRotation(int stackIndex,int layerIndex)
		{
			var stack=stacks[stackIndex];
			var layer=stack.SelectNodes("layer")[layerIndex];
			var layerAttr=layer.Attributes;
			
			int imgIndex=stacks.Count-1-stackIndex;
			
			string astr=layer.GetAttribute("angle");
			//int a;int.TryParse(astr,out a);
			if(angles[imgIndex]==0)
			{//remove angle attr
				if(!string.IsNullOrEmpty(astr))
					layerAttr.RemoveNamedItem("angle");
			}
			else
			{//(add and) change angle attr
				if(string.IsNullOrEmpty(astr))
				{//add
					XmlAttribute attr=doc.CreateAttribute("angle");//new XmlAttribute();
					attr.Value=angles[imgIndex].ToString();
					layerAttr.Append(attr);//.InsertAfter(attr,layerAttr["src"]);
				}
				//change
				else layerAttr["angle"].Value=angles[imgIndex].ToString();
			}
			
		}
		
		public void Rotate(int stackIndex,int layerIndex,int dAngle)
		{
			int imgIndex=stacks.Count-1-stackIndex;
			angles[imgIndex]+=dAngle;
			
			if(angles[imgIndex]>=360)angles[imgIndex]-=360;
			else if(angles[imgIndex]<0)angles[imgIndex]+=360;
			//SavePosition(stackIndex,layerIndex);
		}
		
	}
	
	
	
	
}
