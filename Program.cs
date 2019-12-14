using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MessageInspector
{
	class Program
	{
		
		//Close console window
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FreeConsole();
		
		[STAThread]
		public static void Main(string[] args)
		{
			Twitter twt = new Twitter();
			
//			FreeConsole();
			Application.EnableVisualStyles();
			
			Icon icon = new Icon (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream ("icon"));
			
			Form window = new Form();
			window.Icon = icon;
			window.Text = "DMPeek";
			window.Width = 550;
			window.Height = 675;
			window.MaximizeBox = false;
			window.FormBorderStyle = FormBorderStyle.FixedSingle;
			
			MainMenu menu = new MainMenu();
			window.Menu = menu;
			
			MenuItem menu_file = new MenuItem("File");
			MenuItem menu_help = new MenuItem("Help");
			
			MenuItem item_ping = new MenuItem("Check connection");
			item_ping.Click += item_ping_Click;
			menu_help.MenuItems.Add(item_ping);
			
			MenuItem item_load = new MenuItem("Load file");
			item_load.Click += item_load_Click;
			menu_file.MenuItems.Add(item_load);
			
			
			menu.MenuItems.AddRange(new [] {
				menu_file,
				menu_help,
			});

			Panel cpanel = new Panel();
			cpanel.Name = "cpanel";
			cpanel.Width = 500;
			cpanel.Height = 600;
			cpanel.BorderStyle = BorderStyle.Fixed3D;
			cpanel.BackColor = Color.White;
			cpanel.VerticalScroll.Visible = true;
			cpanel.AutoScroll = true;
			
			window.Controls.Add(cpanel);
			Graphing.AlignCenter(cpanel);
			
			PictureBox indic = new PictureBox();
			indic.Name = "indic";
			indic.Image = Graphing.indic_white();
			indic.Width = 8;
			indic.Height = 8;
			indic.SizeMode = PictureBoxSizeMode.StretchImage;
			window.Controls.Add(indic);
			Graphing.Move(indic, 2, 2, Corner.Bottom_Right, window);
			
			Utils.PingUpdate(twt, window);
			
			window.Show();
			Application.Run(window);
		}
		
		static void item_ping_Click(object sender, EventArgs e) {
			MenuItem menu = sender as MenuItem;
			Form form = menu.GetMainMenu().GetForm();
			
			Utils.PingUpdate(new Twitter(), form);
		}
		
		static void item_load_Click(object sender, EventArgs e) {
			MenuItem mItem = sender as MenuItem;
			Form form = mItem.GetMainMenu().GetForm();
			Panel cpanel = (Panel)form.Controls["cpanel"];
			if(cpanel == null)
				return;
			
			OpenFileDialog dia = new OpenFileDialog();
			dia.Title = "Select JSON File";
			dia.Filter = "JSON files|*.json";
			if(dia.ShowDialog() == DialogResult.OK) {
				Stream fstream = dia.OpenFile();
				StreamReader sreader = new StreamReader(fstream);
				String content = sreader.ReadToEnd();
				sreader.Close();
				
				DmConversation convo = Twitter.ParseConversation(content);
				if(convo == null) {
					MessageBox.Show("The file you chose doesn't contain any conversation data.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				
				cpanel.BackColor = Color.LightGray;
				Console.Clear();
				
				while(cpanel.Controls.Count > 0) {
					cpanel.Controls.Remove(cpanel.Controls[0]);
				}
				
				foreach (Message msg in convo.messages) {
					MColor color;
					if(msg.sender.id == convo.userTwo.id) {
						color = MColor.Blue;
					} else {
						color = MColor.Grey;
					}
					
					Console.WriteLine("[{0}] {1}", msg.date.ToShortTimeString(), msg.text);
					Graphing.AddMessage(msg.text, msg.date, cpanel, color, true);
				}
				
				cpanel.BackColor = Color.White;	
			}
		}
	}
}