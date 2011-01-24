using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Kohina.Nodes
{
	class BitmapViewerWindow: Form {
		
		Bitmap pBitmap = null;
		
		public Bitmap PBitmap {
			get { return pBitmap; }
			set { 
				if(pBitmap != null) pBitmap.Dispose();
				pBitmap = value;
			}
		}
		
		public BitmapViewerWindow()
		{
			DoubleBuffered = true;
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			
		}
		
		
		
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Black);
			if(pBitmap != null) {
				e.Graphics.DrawImage(pBitmap, 0, 0);//DrawImageUnscaledAndClipped(pBitmap, e.ClipRectangle);
			}
		}
		
		protected override void OnPaintBackground(PaintEventArgs e)
		{

		}
		
	}
	
	
	public class BitmapViewer: Node
	{
		[PinAttribs("Input", PinDirection.Input, DataType.Bitmap)]
		Pin inputPin = null;
		
		BitmapViewerWindow bitmapWin;
		System.Windows.Forms.Timer updateTimer;
		DateTime startTime;
		Stopwatch fpsSw;
		double renderTime, fps;
		int frames;
		
		Size desiredSize = new Size(400, 400);
		
		int updateInterval = 30;
		
		public Size DesiredSize {
			get { return desiredSize; }
			set { desiredSize = value; }
		}
		
		public int UpdateInterval {
			get { return updateInterval; }
			set { updateTimer.Interval = updateInterval = value; }
		}
		
		
		public BitmapViewer()
		{
			updateTimer = new System.Windows.Forms.Timer();
			updateTimer.Interval = updateInterval;
			updateTimer.Tick += new EventHandler(updateTimer_Tick);
			fpsSw = new Stopwatch();
			frames = 0;
		}
		
		private Bitmap ReadInput(BitmapPinRequest req) {
			Bitmap bmp;
			Stopwatch sw = new Stopwatch();
			sw.Start();
			bmp = inputPin.Read<Bitmap>(req);
			sw.Stop();
			renderTime = sw.ElapsedTicks / (double)Stopwatch.Frequency * 1000;
			
			return bmp;
			
		}
		
		void DoUpdate(Object obj) {
			
			BitmapPinRequest req = new BitmapPinRequest();
			req.Time = (DateTime.Now - startTime).TotalSeconds;
			req.DesiredSize = desiredSize;
			Bitmap bmp = ReadInput(req);
			bitmapWin.PBitmap = bmp;
			bitmapWin.Invalidate();
			
		}

		void updateTimer_Tick(object sender, EventArgs e)
		{
			if(bitmapWin != null && bitmapWin.Visible) {
				if(!fpsSw.IsRunning) fpsSw.Start();
				frames++;
				if(fpsSw.ElapsedMilliseconds > 1000) {
					fps = frames / (fpsSw.ElapsedMilliseconds / 1000.0);
					frames = 0;
					fpsSw.Reset();
				}
				bitmapWin.Text = string.Format("FPS: {0:0.000} | Rt = {1:#.00}ms", fps, renderTime);
				lock(inputPin) {
					ThreadPool.QueueUserWorkItem(new WaitCallback(DoUpdate));
				}
			} else {
				fpsSw.Stop();
			}
		}
		
		public override void Interact() {
			if(bitmapWin == null) {
				bitmapWin = new BitmapViewerWindow() {
					Size = new Size(480, 320)
				};
				bitmapWin.Show();
				updateTimer.Enabled = true;
				startTime = DateTime.Now;
			} else {
				bitmapWin.Show();
			}
			bitmapWin.BringToFront();
			bitmapWin.ClientSize = desiredSize;
		}
		
		public override void Dispose()
		{
			if(bitmapWin != null) {
				updateTimer.Enabled = false;
				bitmapWin.Dispose();
				bitmapWin = null;
			}
			base.Dispose();
		}
		
	}
}

