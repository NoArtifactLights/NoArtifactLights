using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandPlus.Commanding;
using GTA;

namespace NoArtifactLights.Engine.Mod.Commands
{
	internal class HealCommand : Command
	{
		public override void Executed(object[] arguments)
		{
			Game.Player.Character.Health = 200;
		}
	}
}
