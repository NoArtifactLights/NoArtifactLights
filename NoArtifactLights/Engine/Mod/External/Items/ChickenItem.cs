// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using LemonUI.Elements;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Resources;
using PlayerCompanion;

namespace NoArtifactLights.Engine.Mod.External.Items
{
	public class ChickenItem : StackableItem
	{
		public ChickenItem()
		{
			this.Used += ChickenItem_Used;
		}

		public override string Name => Strings.FoodChicken;

		public override ScaledTexture Icon => new ScaledTexture("", "");

		private void ChickenItem_Used(object sender, EventArgs e)
		{
			HungryController.AddHungry(Foods.Hamburger, 3f);
			this.Count--;
		}
	}
}
