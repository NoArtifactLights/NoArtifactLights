// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoArtifactLights.Engine.Mod.Controller;
using PlayerCompanion;

namespace NoArtifactLights.Engine.Mod.External.Items
{
	public abstract class FoodItem : StackableItem
	{
		protected FoodItem(Foods food, float amount)
		{
			this.Used += (sender, e) =>
			{
				HungryController.AddHungry(food, amount);
				this.Count--;
			};
		}
	}
}
