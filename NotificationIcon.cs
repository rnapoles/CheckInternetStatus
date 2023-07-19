using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CheckInternetStatus
{
    internal class NotificationIcon
    {

		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;
		System.Timers.Timer aTimer;
		ComponentResourceManager resources;
		private Icon offlineIcon;
		private Icon onlineIcon;

		Queue<String> ips = new Queue<string>();

		#region Initialize icon and menu
		public NotificationIcon()
		{
			notifyIcon = new NotifyIcon();
			notificationMenu = new ContextMenu(InitializeMenu());
			notifyIcon.DoubleClick += IconDoubleClick;

			resources = new ComponentResourceManager(typeof(NotificationIcon));
			offlineIcon = (Icon) resources.GetObject("offline");
			onlineIcon = (Icon) resources.GetObject("online");

			notifyIcon.Icon = offlineIcon;
			notifyIcon.ContextMenu = notificationMenu;
			notifyIcon.BalloonTipTitle = "";
			notifyIcon.Visible = true;

			//Google DNS
			ips.Enqueue("8.8.8.8");
			ips.Enqueue("8.8.4.4");
			//Cloudflare: 1.1.1.1 & 1.0.0.1
			ips.Enqueue("1.1.1.1");
			ips.Enqueue("1.0.0.1");
			//Control D: 76.76.2.0 & 76.76.10.0
			ips.Enqueue("76.76.10.0");
			ips.Enqueue("76.76.2.0");
			//Quad9: 9.9.9.9 & 149.112.112.112
			ips.Enqueue("9.9.9.9");
			//Alternate DNS: 76.76.19.19 & 76.223.122.150
			ips.Enqueue("76.76.19.19");

			aTimer = new System.Timers.Timer();
			aTimer.Interval = 5000;
			aTimer.Elapsed += OnTimedEvent;
			aTimer.Enabled = true;

		}

		~NotificationIcon()  // finalizer
		{
			if(notifyIcon != null)
            {
				notifyIcon.Visible = false;
            }
		}

		void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
		{

			string ip = ips.Dequeue();
			ips.Enqueue(ip);

			bool result = PingChecker.PingHost(ip);
			string text = result ? $"{ip} is Online\n" : $"{ip} is Offline\n";
			notifyIcon.BalloonTipText = text;
			notifyIcon.Text = text;
			notifyIcon.Icon = result ? onlineIcon : offlineIcon;
			//notifyIcon.ShowBalloonTip(1000);

		}

		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("About", menuAboutClick),
				new MenuItem("Exit", menuExitClick)
			};
			return menu;
		}
		#endregion

		#region Event Handlers
		private void menuAboutClick(object sender, EventArgs e)
		{
			MessageBox.Show("About This Application");
		}

		private void menuExitClick(object sender, EventArgs e)
		{
			notifyIcon.Visible = false;
			Application.Exit();
		}

		private void IconDoubleClick(object sender, EventArgs e)
		{
			//MessageBox.Show("The icon was double clicked");
			notifyIcon.ShowBalloonTip(1000);
		}
		#endregion

	}
}
