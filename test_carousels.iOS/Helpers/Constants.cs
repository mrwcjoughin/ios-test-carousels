﻿using System;
using UIKit;

namespace test_carousels.iOS
{
	public static class Constants
	{
		public static readonly string MainFont = "Arial";

		public static readonly nfloat HorizontalMargin = (UIApplication.SharedApplication.StatusBarFrame.Width * 0.01f);
		public static readonly nfloat VerticalMargin = (UIApplication.SharedApplication.StatusBarFrame.Width * 0.01f);

		public static nfloat Width = UIApplication.SharedApplication.StatusBarFrame.Width;

		public static nfloat WidthLessLeftAndRightMargins
		{
			get
			{
				return UIApplication.SharedApplication.StatusBarFrame.Width - HorizontalMargin - HorizontalMargin;
			}
		}

		public static nfloat WidthLessLeftMargins
		{
			get
			{
				return UIApplication.SharedApplication.StatusBarFrame.Width - HorizontalMargin;
			}
		}
	}
}