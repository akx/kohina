using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Kohina.Nodes
{
	/// <summary>
	/// Description of BitmapViewer.
	/// </summary>
	public class BitmapViewer: Node
	{
		[PinAttribs("Input", PinDirection.Input, DataType.Bitmap)]
		Pin inputPin = null;
		
		Form bitmapWin;
		PictureBox bitmapPb;
		Timer updateTimer;
		DateTime startTime;
		Stopwatch sw, fpsSw;
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
			updateTimer = new Timer();
			updateTimer.Interval = updateInterval;
			updateTimer.Tick += new EventHandler(updateTimer_Tick);
			sw = new Stopwatch();
			fpsSw = new Stopwatch();
			frames = 0;
		}

		void updateTimer_Tick(object sender, EventArgs e)
		{
			if(bitmapPb != null && bitmapWin.Visible) {
				if(!fpsSw.IsRunning) fpsSw.Start();
				BitmapPinRequest req = new BitmapPinRequest();
				req.Time = (DateTime.Now - startTime).TotalSeconds;
				req.DesiredSize = desiredSize;
				sw.Reset();
				sw.Start();
				Bitmap bmp = inputPin.Read<Bitmap>(req);
				sw.Stop();
				renderTime = sw.ElapsedTicks / (double)Stopwatch.Frequency * 1000;
				if(bitmapPb.Image != bmp && bitmapPb.Image != null) {
					bitmapPb.Image.Dispose();
				}
				bitmapPb.Image = bmp;
				frames++;
				if(fpsSw.ElapsedMilliseconds > 1000) {
					fps = frames / (fpsSw.ElapsedMilliseconds / 1000.0);
					frames = 0;
					fpsSw.Reset();
				}
				bitmapWin.Text = string.Format("FPS: {0} | T = {1} (rt = {2:2} msec)", fps, req.Time.ToString(), renderTime);
			} else {
				fpsSw.Stop();
			}
		}
		
		public override void Interact() {
			if(bitmapWin == null) {
				bitmapWin = new Form() {
					Size = new Size(480, 320)
				};
				bitmapWin.SuspendLayout();
				bitmapPb = new PictureBox() {
					Dock = DockStyle.Fill,
					Parent = bitmapWin,
					Visible = true,
					Size = bitmapWin.Size
				};
				bitmapWin.ResumeLayout(true);
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
