// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using LemonUI.Elements;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Resources;
using PlayerCompanion;

namespace NoArtifactLights.Engine.Mod.External.Items
{
	public class HamburgerItem : FoodItem
	{
		public HamburgerItem() : base(Foods.Hamburger, 1.5f)
		{
		}

		public override string Name => Strings.FoodBurger;

		public override ScaledTexture Icon => new ScaledTexture("", "");
	}
}
