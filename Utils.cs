using System;
using System.Windows.Forms;

namespace MessageInspector
{
	public static class Utils
	{
		public static void Wait(int msecs) {
			double time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond * 1;
			while((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond * 1) < time + msecs) {			}
			return;
		}
		
		public static void PingUpdate(Twitter twt, Form form) {
			PictureBox indic = (PictureBox) form.Controls["indic"];
			
			Wait(800);
			twt.Ping();
			if(twt.network) {
				indic.Image = Graphing.indic_green();
			} else {
				indic.Image = Graphing.indic_red();
			}
		}
	}
}
