// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandPlus.Commanding;
using NoArtifactLights.Engine.Mod.Controller;

namespace NoArtifactLights.Engine.Mod.Commands
{
	public class SetDehydrationCommand : Command
	{
		public SetDehydrationCommand()
		{
			this.ArgumentTypes.Add(typeof(float));
		}

		public override void Executed(object[] arguments)
		{
			float water = this.VerifyAndConstruct<float>(0, arguments[0]);
			if(water >= 10f)
			{
				water = 10f;
			}
			if(water < 0)
			{
				water = 0f;
			}

			HungryController.Water = water;
		}
	}
}
