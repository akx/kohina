﻿using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kohina.Nodes
{
	[NodeCategoryAttribute(DataType.Bitmap, NodeCategoryKind.Modifier)]
	public class BitmapChannelRemap: Node
	{
		[PinAttribs("Input", PinDirection.Input, DataType.Bitmap)]
		Pin inputPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		UInt32 mask = 0x18100800;
		
		public uint Mask {
			get { return mask; }
			set { mask = value; }
		}
		
		
		
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Bitmap bmp = inputPin.Read<Bitmap>(request);
				if(bmp == null) return null;
				bmp = bmp.ResizeIfNeeded(
					(request is BitmapPinRequest) ? (request as BitmapPinRequest).DesiredSize : bmp.Size
				);
				Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);
				BitmapData bData = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				CBlend.CRemap(bData.Scan0, (UInt32)(bData.Stride * bData.Height), mask);
				bmp.UnlockBits(bData);
				return bmp;
			}
			return null;
		}
		
	}
}
