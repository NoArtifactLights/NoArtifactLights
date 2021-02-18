// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using LemonUI.Elements;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Resources;
using PlayerCompanion;

namespace NoArtifactLights.Engine.Mod.External.Items
{
	public class HamburgerItem : StackableItem
	{
		public HamburgerItem()
		{
			this.Used += HamburgerItem_Used;
		}

		public override string Name => Strings.FoodBurger;

		public override ScaledTexture Icon => new ScaledTexture("", "");

		private void HamburgerItem_Used(object sender, EventArgs e)
		{
			HungryController.AddHungry(Foods.Hamburger, 1f);
			this.Count--;
		}
	}
}
