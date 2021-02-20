﻿// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using GTA.UI;
using NLog;
using NoArtifactLights.Engine.Entities.Enums;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Resources;
using PlayerCompanion;
using System;

namespace NoArtifactLights
{
	internal static class Common
	{
		internal static int counter = 0;
		internal static Difficulty difficulty = Difficulty.Initial;
		internal static bool blackout;
		internal static NLog.Logger logger = LogManager.GetLogger("Common");
		internal static event EventHandler Unload;

		//internal static int intervalToRespawn;

		// internal static event EventHandler CashChanged;

		[Obsolete]
		public static int Cash { get; set; } = 0;
		
		[Obsolete]
		public static int Bank { get; set; } = 0;

		public static bool IsCheatEnabled { get; internal set; }

		internal static void OnLaunch(object you)
		{
			Start(you, new EventArgs());
		}

		internal static event EventHandler Start;

		internal static void UnloadMod(object you)
		{
			Notification.Show(Strings.Unload);
			Unload(you, new EventArgs());
		}

		public static bool Cost(int amount)
		{
			var wallet = Companion.Wallet;

			if (wallet.Money < amount)
			{
				Screen.ShowSubtitle(Strings.BuyNoMoney);
				return false;
			}

			wallet.Money -= amount;

			return true;
		}

		public static bool Earn(int amount)
		{
			if (Companion.Wallet.Money == int.MaxValue)
			{
				logger.Info("Player's cash has reached int limit");
				Notification.Show(NotificationIcon.Blocked, "", "", Strings.CashMaximum);
				return false;
			}
			Companion.Wallet.Money += amount;
			return true;
		}
	}
}
