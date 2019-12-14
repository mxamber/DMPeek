using System;
using System.Drawing;
using System.Windows.Forms;

namespace MessageInspector
{
	public enum Corner {
		Top_Left,
		Top_Right,
		Bottom_Left,
		Bottom_Right
	}
	
	public enum MColor {
		Grey,
		Blue
	}
	
	public static class Graphing
	{
		public static Font MsgFont = new Font("Segoe UI", 12);
		public static Font DateFont = new Font("Segoe UI", 8);
		
		public static Label[] AddMessage(String text, DateTime date, Control parent, MColor color, bool attach = false) {
			int y_plus = 5;
			
			if(parent.Controls.Count > 1) {
				Control prev = parent.Controls[parent.Controls.Count -1];
				y_plus += prev.Location.Y + prev.Height;
			} else if(parent.Controls.Count > 0) {
				y_plus += parent.Controls[0].Height;
			} else if (parent.Controls.Count == 0) {
				y_plus = 15;
			}
			
			if(text.Length > 10000) {
				text = text.Substring(0,10000);
			}
			
			int max_width = (int)((parent.ClientSize.Width - 25) / 2);
			int min_width = (int)(max_width / 4);
			
			if(max_width < 0)
				max_width = 0;
			if(min_width <0)
				min_width = 0;
			
			Label label = new Label();
			label.Text = text;
			label.Font = new Font("Segoe UI", 12);
			label.MaximumSize = new Size(max_width, 2048);
			label.MinimumSize = new Size(min_width, 15);
			label.AutoSize = true;
			
			
			label.BackColor = Graphing.MsgColor(color);
			
			Label label2 = new Label();
			label2.AutoSize = true;
			
			label2.Font = new Font("Segoe UI", 8);
			label2.Text = date.ToShortTimeString();
			
			
			int x = 10;
			int x_sm = 15;
			
			if(color == MColor.Blue) {
				label.ForeColor = Color.White;
				x = parent.Width - 30 - label.PreferredWidth;
				x_sm = parent.Width - 35 - label2.PreferredWidth;
			}
			
			
			
			label.Location = new Point(x, y_plus);
			label2.Location = new Point(x_sm, label.Location.Y + 5 + label.PreferredHeight);
			
			if(attach) {
				parent.Controls.AddRange(new [] {label, label2});
			}
			
			return new Label[] {label, label2};
		}
		
		public static Color MsgColor(MColor type) {
			switch(type) {
				case MColor.Blue:
					return Color.FromArgb(29,161,242);
				case MColor.Grey:
					return Color.FromArgb(230,236,240);
			}
			
			return Color.White;
		}
		
		
		public static Bitmap indic_green() {
			return new Bitmap(System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream ("indic_green"));
		}
		public static Bitmap indic_red() {
			return new Bitmap(System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream ("indic_red"));
		}
		public static Bitmap indic_white() {
			return new Bitmap(System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream ("indic_white"));
		}
		public static Bitmap indic_yellow() {
			return new Bitmap(System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream ("indic_yellow"));
		}
		
		public static void Move(Control control, int x, int y, Corner pos, Control parent) {
			
			int menu = 0;
			if(control.FindForm().Menu != null) {
				menu = SystemInformation.MenuHeight;
			}
			
			int x_n = 0;
			int y_n = 0;
			
			switch(pos) {
				case Corner.Top_Left:
					break;
				case Corner.Top_Right:
					x_n = parent.ClientSize.Width - x - control.Width;
					break;
				case Corner.Bottom_Right:
					x_n = parent.ClientSize.Width - x - control.Width;
					y_n = parent.ClientSize.Height - y - control.Height;
					break;
				case Corner.Bottom_Left:
					y_n = parent.ClientSize.Height - y - control.Height;
					break;
			}
			
			control.Location = new Point(x_n, y_n - menu);
			
		}
		
		public static void AlignCenter(Control control) {
			
			
			
			
			if(control.Parent == null) {
				throw new ArgumentException("control has no parent");
			}
			
			int menu = 0;
			if(control.FindForm().Menu != null) {
				menu = SystemInformation.MenuHeight;
			}
			
			int w = control.Bounds.Width;
			int h = control.Bounds.Height;
			
			int p_w;
			int p_h;
			
			try {
				p_w = control.Parent.ClientSize.Width;
				p_h = control.Parent.ClientSize.Height;
			} catch {
				p_w = control.Parent.Width;
				p_h = control.Parent.Height;
			}
			
			double x_d = (p_w - w) / 2;
			double y_d = (p_h - h - menu) / 2;
			
			int x = Convert.ToInt32(x_d);
			int y = Convert.ToInt32(y_d);
			
			control.Location = new Point(x, y);
		}
	}
}
