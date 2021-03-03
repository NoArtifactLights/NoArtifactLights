// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using GTA;
using GTA.Native;
using GTA.UI;
using NLog;
using NoArtifactLights.Engine.Mod.API;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Resources;
using System;
using System.IO;
using Logger = NLog.Logger;
using NoArtifactLights.Engine.Entities;
using System.Collections.Generic;
using System.Threading;
using NoArtifactLights.Cilent;
using System.Reflection;
using PlayerCompanion;
using System.Drawing;
using Configuration = NoArtifactLights.Engine.Entities.Configuration;

namespace NoArtifactLights
{
	[ScriptAttributes(Author = "RelaperCrystal")]
	public class Entry : Script
	{
		internal static readonly HandleableList peds1 = new HandleableList();
		internal static readonly HandleableList killedPeds = new HandleableList();
		internal static readonly HandleableList weaponedPeds = new HandleableList();

		internal static readonly List<Blip> blips = new List<Blip>();

		private static readonly Logger logger = LogManager.GetLogger("Entry");

		public static GameProcess Process { get; private set; }

		public void LoadMod()
		{
			try
			{
				Configuration.LoadConfiguration(this.Settings);

				if (Configuration.FirstSetup)
				{
					Companion.Colors.Current = Color.SpringGreen;
					Configuration.CompleteFirstSetup();
					Configuration.SaveConfiguration(this.Settings);
				}

				Thread.CurrentThread.Name = "main";
				Screen.FadeOut(1000);
				logger.Info("Starting NAL");
				GameUI.DisplayHelp(Strings.Start);
				if (!File.Exists("scripts\\NoArtifactLights.pdb"))
				{
					logger.Warn("Attention: No debug database found in game scripts folder. This means logs cannot provide additional information related to exception.");
				}
				Process = new GameProcess(Assembly.GetExecutingAssembly().GetName().Version);
				this.Interval = 100;
				this.Tick += Entry_Tick;
				this.Aborted += Entry_Aborted;
			}
#pragma warning disable CA1031
			catch (Exception ex)
#pragma warning restore CA1031
			{
				Screen.FadeIn(500);
				logger.Fatal(ex, "Excepting during initial load process");
				Abort();
			}
		}

		public Entry()
		{
			this.KeyDown += Entry_KeyDown;
		}

		private void Entry_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == System.Windows.Forms.Keys.L && e.Alt)
			{
				this.KeyDown -= Entry_KeyDown;
				LoadMod();
			}
		}

		private void Entry_Tick(object sender, EventArgs e) => Process.Tick();

		private void Entry_Aborted(object sender, EventArgs e)
		{
			logger.Warn("Script is facing abort.");

			GameController.SetRelationshipBetGroupsUInt(Relationship.Pedestrians, 0x02B8FA80, 0x47033600);
			GameController.SetRelationshipBetGroupsUInt(Relationship.Pedestrians, 0x47033600, 0x02B8FA80);

			foreach (var blip in blips)
			{
				if (blip.Exists())
				{
					blip.Delete();
				}
			}
		}
	}
}
