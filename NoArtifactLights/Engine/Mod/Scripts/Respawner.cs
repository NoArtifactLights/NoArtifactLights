﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using NoArtifactLights.Engine.Mod.Controller;

namespace NoArtifactLights.Engine.Mod.Scripts
{
	public class Respawner : Script
	{
		public Respawner()
		{
			Tick += Respawner_Tick;
		}

		private void Respawner_Tick(object sender, EventArgs e)
		{
			RespawnController.Loop();
		}
	}
}