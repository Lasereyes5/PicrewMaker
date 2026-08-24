using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

public static class GetArrowBitmap
{
	public static Bitmap Up()
	{
		var bmp=new Bitmap(45,45,PixelFormat.Format32bppArgb);
		var g=Graphics.FromImage(bmp);
		var b=new SolidBrush(Color.Black);
		
		Point[] arrowHead={
			new Point(22,4),
			new Point(14,20),
			new Point(30,20)
		};
		Rectangle arrowBody=new Rectangle(20,19,5,21);
		
		g.FillRectangle(b,arrowBody);
		g.SmoothingMode=SmoothingMode.AntiAlias;
		g.FillPolygon(b,arrowHead);
		
		g.Dispose();
		
		return bmp;
	}
	public static Bitmap Down()
	{
		var bmp=Up();
		bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
		return bmp;
	}
	public static Bitmap Left()
	{
		var bmp=Up();
		bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
		return bmp;
	}
	public static Bitmap Right()
	{
		var bmp=Up();
		bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
		return bmp;
	}
	
	public static Bitmap Clockwise()
	{
		var bmp=AntiClockwise();
		bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
		return bmp;
	}
	public static Bitmap AntiClockwise()
	{
		var bmp=new Bitmap(45,45,PixelFormat.Format32bppArgb);
		var g=Graphics.FromImage(bmp);
		var b=new SolidBrush(Color.Black);
		
		PointF[] arrowHead={
			new PointF(10,6.5f),
			new PointF(8,19),//point
			new PointF(20.5f,17.5f)
		};
		
		g.SmoothingMode=SmoothingMode.AntiAlias;
		for(float i=0;i <5;i+=0.4f)
		{
			g.DrawArc(new Pen(Color.Black),7f+i,7f+i, 30f-2*i,30f-2*i, -145,290);
		}
		g.FillPolygon(b,arrowHead);
		
		g.Dispose();
		
		return bmp;
	}
}


