// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LemonUI.Elements;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Resources;
using PlayerCompanion;

namespace NoArtifactLights.Engine.Mod.External.Items
{
	public class WaterItem : StackableItem
	{
		public WaterItem()
		{
			this.Used += (sender, e) =>
			{
				HungryController.RefillWater();
			};
		}

		public override string Name => Strings.ItemWater;

		public override ScaledTexture Icon => new ScaledTexture("", "");
	}
}
