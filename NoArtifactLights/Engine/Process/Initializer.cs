// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using GTA;
using GTA.UI;
using GTA.Math;
using GTA.Native;
using NLog;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Engine.Mod.API.Events;
using NoArtifactLights.Engine.Entities.Enums;

namespace NoArtifactLights.Engine.Process
{
	internal static class Initializer
	{
		private static bool isLoaded = false;
		private static bool isLoading = false;

		private static readonly Logger logger = LogManager.GetLogger("Initializer");

		internal static void LoadProgram()
		{
			if (!LoadInternal()) throw new InvalidProgramException();
		}

		private static bool LoadInternal()
		{
			if (isLoading || isLoaded) return false;
			isLoading = true;

			// START INITIAL LOADING PROCESS

			logger.Trace("Loading multi-player maps");
			Function.Call(Hash._LOAD_MP_DLC_MAPS);
			Function.Call(Hash._USE_FREEMODE_MAP_BEHAVIOR, true);
			logger.Trace("Setting player position and giving weapons");
			Game.Player.Character.Position = new Vector3(459.8501f, -1001.404f, 24.91487f);
			logger.Trace("Setting relationship and game settings");
			GameController.SetRelationship(Difficulty.Initial);

			Game.MaxWantedLevel = 0;
			Game.Player.IgnoredByPolice = true;
			Game.Player.ChangeModel("a_m_m_bevhills_02");

			Notification.Show("~y~~h~HINT~w~You can now set the color of the a_m_m_bevhills_02 to R=0, G=114, B=118.");

			Screen.FadeIn(1000);
			logger.Trace("Registering events");
			EventController.RegisterEvent(typeof(ArmedPed));
			EventController.RegisterEvent(typeof(StealCar));

			// END INITIAL LOADING PROCESS

			isLoaded = true;
			isLoading = false;
			return true;
		}
	}
}
