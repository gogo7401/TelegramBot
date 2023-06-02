using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using TL;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace WTelegramClientTestWF
{
	public partial class MainForm : Form
	{
		private readonly ManualResetEventSlim _codeReady = new();
		private WTelegram.Client _client;
		private User _user;

		private string path = null;
		private string FILENAME = null;

		public MainForm()
		{
			InitializeComponent();
			WTelegram.Helpers.Log = (l, s) => Debug.WriteLine(s);
			
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			_client?.Dispose();
			Properties.Settings.Default.Save();
		}

		private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(((LinkLabel)sender).Tag as string);
		}

		private async void buttonLogin_Click(object sender, EventArgs e)
		{
			buttonLogin.Enabled = false;
			listBox.Items.Add($"Connecting & login into Telegram servers...");
			_client = new WTelegram.Client(Config);
			_user = await _client.LoginUserIfNeeded();
			panelActions.Visible = true;
			listBox.Items.Add($"We are now connected as {_user}");
		}

		private async void buttonSendMsg_Click(object sender, EventArgs e)
		{
			var msg = Interaction.InputBox("Type some text to send to ourselves\n(Saved Messages)", "Send to self");
			if (!string.IsNullOrEmpty(msg))
			{
				msg = "_Here is your *saved message*:_\n" + Markdown.Escape(msg);
				var entities = _client.MarkdownToEntities(ref msg);
				await _client.SendMessageAsync(InputPeer.Self, msg, entities: entities);

			}
		}


        private void MainForm_Load(object sender, EventArgs e)
        {
			path = Directory.GetCurrentDirectory();
			FILENAME = $"{path}\\logo.gif";

			label7.Text = "от " + trackBar1.Value.ToString() + " до 5 мин.";
			Random rnd = new Random();
			int m = rnd.Next(trackBar1.Value, 5);  // creates a number between 1 and 5
			label9.Text = m.ToString() + " мин.";
			timer2.Interval = m * 60000;
			progressBar1.Maximum = m * 60;
		}

    }
}
