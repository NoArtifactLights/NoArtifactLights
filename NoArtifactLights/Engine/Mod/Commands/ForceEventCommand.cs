// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandPlus.Commanding;

namespace NoArtifactLights.Engine.Mod.Commands
{
	internal class ForceEventCommand : Command
	{
		public override void Executed(object[] arguments)
		{
			Entry.Process.SetForceStart();
		}
	}
}
