// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace NoArtifactLights.Engine.Entities
{
	internal static class Configuration
	{
		internal static bool FirstSetup { get; private set; }

		internal static void LoadConfiguration(ScriptSettings settings)
		{
			FirstSetup = settings.GetValue("Main", "FirstSetup", true);
		}

		internal static void CompleteFirstSetup()
		{
			FirstSetup = false;
		}

		internal static void SaveConfiguration(ScriptSettings settings)
		{
			settings.SetValue("Main", "FirstSetup", FirstSetup);
		}
	}
}
