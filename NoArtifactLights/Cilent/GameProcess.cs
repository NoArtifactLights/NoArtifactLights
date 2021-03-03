// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using System.Diagnostics;
using GTA;
using GTA.Native;
using LemonUI.Scaleform;
using NLog;
using NoArtifactLights.Engine.Entities;
using NoArtifactLights.Engine.Entities.Enums;
using NoArtifactLights.Engine.Mod.API;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Engine.Process;
using NoArtifactLights.Resources;

namespace NoArtifactLights.Cilent
{
	public class GameProcess
	{

		internal static readonly HandleableList peds1 = new HandleableList();
		internal static readonly HandleableList killedPeds = new HandleableList();
		internal static readonly HandleableList weaponedPeds = new HandleableList();

		public Version Version { get; }
		private static readonly Logger logger = LogManager.GetLogger("Client");

		internal static void SetForceStart()
		{
			EventController.forceEvent = true;
		}

		public GameProcess(Version version)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			Version = version;
			logger.Info("Constructing NAL version " + version.ToString());
			Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, true);

			logger.Info("Initializing NAL Program...");
			Initializer.LoadProgram();

			logger.Info("Starting Command Client...");
			sw.Stop();

			Functions.TriggerInitialize(this);

			Common.OnLaunch();
			logger.Info("DONE! Client construction took " + sw.ElapsedMilliseconds + " ms");
		}

		internal void Tick()
		{
			try
			{
				foreach (var ped in World.GetAllPeds())
				{
					if (ped == null)
					{
						continue;
					}
					if (ped.Exists() && ped.HasBeenDamagedBy(Game.Player.Character) && ped.IsDead && !killedPeds.IsDuplicate(ped))
					{
						killedPeds.Add(ped);
						logger.Debug("Pedestrian down by player");
						Game.MaxWantedLevel = 0;
						Game.Player.IgnoredByPolice = true;
						if (Game.Player.Character.Position.DistanceTo(ped.Position) <= 2.5f)
						{
							Common.Earn(new Random().Next(4, 16));
						}

						Common.Kills++;
						if (weaponedPeds.IsDuplicate(ped))
						{
							Common.Earn(50);
							GameUI.DisplayHelp(Strings.ArmedBonus);

							if (ped.AttachedBlip?.Exists() == true)
							{
								ped.AttachedBlip?.Delete();
							}
						}

						switch (Common.Kills)
						{
							case 1:
								GameUI.DisplayHelp(Strings.FirstKill);
								break;

							case 100:
								Common.CurrentDifficulty = Difficulty.Easy;
								GameUI.DisplayHelp(string.Format(Strings.DifficultyHelp, Strings.DifficultyEasy));
								GameController.SetRelationship(Difficulty.Easy);
								break;

							case 300:
								Common.CurrentDifficulty = Difficulty.Normal;
								GameUI.DisplayHelp(string.Format(Strings.DifficultyHelp, Strings.DifficultyNormal));
								GameController.SetRelationship(Difficulty.Normal);
								break;

							case 700:
								Common.CurrentDifficulty = Difficulty.Hard;
								GameUI.DisplayHelp(string.Format(Strings.DifficultyHelp, Strings.DifficultyHard));
								GameController.SetRelationship(Difficulty.Hard);
								break;

							case 1500:
								Common.CurrentDifficulty = Difficulty.Nether;
								GameUI.DisplayHelp(string.Format(Strings.DifficultyHelp, Strings.DifficultyNether));
								GameController.SetRelationship(Difficulty.Nether);
								break;
						}
					}
					if (peds1.IsDuplicate(ped) || !ped.IsHuman)
					{
						continue;
					}
					peds1.Add(ped);
					Functions.TriggerPed(this, ped);
				}

				if (killedPeds.Count >= 6000)
				{
					killedPeds.Clear();
				}
				if (weaponedPeds.Count >= 600)
				{
					weaponedPeds.Clear();
				}
				if (peds1.Count >= 60000)
				{
					peds1.Clear();
				}
				Functions.TriggerTick(this);
			}
			catch (Exception ex)
			{
				GameUI.DisplayHelp(Strings.ExceptionMain);
				logger.Fatal(ex);
				throw;
			}
		}
	}
}
