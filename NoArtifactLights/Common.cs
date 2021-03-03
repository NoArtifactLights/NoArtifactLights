// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using GTA.UI;
using NLog;
using NoArtifactLights.Engine.Entities.Enums;
using NoArtifactLights.Resources;
using PlayerCompanion;
using System;

namespace NoArtifactLights
{
	internal static class Common
	{
		private static readonly Logger logger = LogManager.GetLogger("Common");
		internal static event EventHandler Unload;

		public static Difficulty CurrentDifficulty { get; internal set; }

		[Obsolete("Use Player Companion instead.")]
		public static int Cash { get; set; }

		[Obsolete("Use Player Companion instead.")]
		public static int Bank { get; set; }

		public static int Kills { get; internal set; }

		public static bool IsCheatEnabled { get; internal set; }

		public static bool Blackout { get; internal set; }

		internal static void OnLaunch()
		{
			Start?.Invoke(null, EventArgs.Empty);
		}

		internal static event EventHandler Start;

		internal static void UnloadMod()
		{
			Notification.Show(Strings.Unload);
			Unload?.Invoke(null, EventArgs.Empty);
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
