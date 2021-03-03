// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using CommandPlus.Commanding;
using NoArtifactLights.Cilent;

namespace NoArtifactLights.Engine.Mod.Commands
{
	internal class ForceEventCommand : Command
	{
		public override void Executed(object[] arguments)
		{
			GameProcess.SetForceStart();
		}
	}
}
