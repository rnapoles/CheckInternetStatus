/*
 * User: rnapoles
 * Date: 18/07/2023
 * Time: 21:07
 * 
 */
using System;
using System.Threading;
using System.Windows.Forms;

namespace CheckInternetStatus
{
    public sealed class Program
	{

		static NotificationIcon notificationIcon;

		#region Main - Program entry point
		/// <summary>Program entry point.</summary>
		/// <param name="args">Command Line Arguments</param>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex mtx = new Mutex(true, nameof(CheckInternetStatus), out isFirstInstance))
			{
				if (isFirstInstance)
				{
					notificationIcon = new NotificationIcon();
					Application.Run();
				}
			} // releases the Mutex
		}
		#endregion


	}
}
