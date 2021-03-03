// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using GTA;
using GTA.Native;
using GTA.UI;
using NLog;
using NoArtifactLights.Engine.Entities.Enums;
using NoArtifactLights.Resources;
using System;
using System.IO;

namespace NoArtifactLights.Engine.Mod.Controller
{
	public static class GameController
	{
		private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

		public static void CheckSnapshotInformation()
		{
			if(Properties.Settings.Default.LastSnapshot != "0.3.100.22")
			{
				Properties.Settings.Default.LastSnapshot = "0.3.100.22";
				Properties.Settings.Default.Save();
				Notification.Show(Strings.SnapshotInfo);
			}
		}

		/// <summary>
		/// Adds a ped to list of armed peds. These peds will not be re-assigned to certain events.
		/// </summary>
		/// <param name="p"></param>
		public static void AddWeaponedPed(Ped p)
		{
			if (p?.Exists() == false)
			{
				return;
			}

			Entry.weaponedPeds.Add(p);
		}

		internal static void SetRelationshipBetGroupsUInt(Relationship relation, uint group1, uint group2)
		{
			Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relation, group1, group2);
		}

		internal static void CheckForDependencies()
		{
			if (!File.Exists("scripts\\PlayerCompanion.dll"))
			{
				Notification.Show("~h~r~WARNING~w~~n~PlayerCompanion is required for several things to function. If you don't install it, the mod is most likely gonna crash.");
				Notification.Show("PlayerCompanion is ~h~not~w~ bundled. You needs to download and install it manually. See log for links.");
				logger.Warn("PlayerCompanion does not exist. Download it here: https://www.gta5-mods.com/scripts/playercompanion");
			}
		}

		public static void SetRelationship(Difficulty difficulty)
		{
			SetRelationshipBetGroupsUInt(Relationship.Hate, 0x02B8FA80, 0x47033600);
			SetRelationshipBetGroupsUInt(Relationship.Hate, 0x47033600, 0x02B8FA80);
			switch (difficulty)
			{
				case Difficulty.Easy:
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0xA49E591C, 0x02B8FA80);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0x02B8FA80, 0xA49E591C);
					break;

				case Difficulty.Normal:
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0xA49E591C, 0x02B8FA80);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0x02B8FA80, 0xA49E591C);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0xC26D562A, 0x02B8FA80);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0x02B8FA80, 0xC26D562A);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0x45897C40, 0xC26D562A);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0xC26D562A, 0x45897C40);
					break;

				case Difficulty.Nether:
				case Difficulty.Hard:
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0xA49E591C, 0x02B8FA80);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0x02B8FA80, 0xA49E591C);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0xA49E591C, 0x47033600);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0x47033600, 0xA49E591C);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0xC26D562A, 0x02B8FA80);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0x02B8FA80, 0xC26D562A);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0x45897C40, 0xC26D562A);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0xC26D562A, 0x45897C40);
					SetRelationshipBetGroupsUInt(Relationship.Hate, 0x02B8FA80, 0x6F0783F5);
					break;
			}
		}

		internal static void EquipWeapon(this Ped ped)
		{
			WeaponHash wp;
			if(ped.IsInVehicle())
			{
				ped.CurrentVehicle.SoundHorn(1000);
				ped.Task.LeaveVehicle();
			}
			switch (Common.CurrentDifficulty)
			{
				case Difficulty.Easy:
					wp = WeaponHash.PumpShotgun;
					break;

				case Difficulty.Normal:
					wp = WeaponHash.MiniSMG;
					break;

				case Difficulty.Hard:
					wp = WeaponHash.CarbineRifle;
					break;

				case Difficulty.Nether:
					wp = WeaponHash.RPG;
					break;

				default:
					if (new Random().Next(200, 272) == 250) wp = WeaponHash.PumpShotgun;
					else wp = WeaponHash.Pistol;
					break;
			}
			ped.Weapons.Give(wp, 9999, true, true);
			ped.Weapons.Select(wp);
			ped.Task.FightAgainstHatedTargets(15f);
		}
	}
}
