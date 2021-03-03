// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using LemonUI.Elements;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Resources;
using PlayerCompanion;

namespace NoArtifactLights.Engine.Mod.External.Items
{
	public class ChickenItem : FoodItem
	{
		public ChickenItem() : base(Foods.Chicken, 2.5f)
		{
		}

		public override string Name => Strings.FoodChicken;

		public override ScaledTexture Icon => new ScaledTexture("", "");
	}
}
