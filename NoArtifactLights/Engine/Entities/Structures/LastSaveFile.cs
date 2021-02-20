﻿// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using NoArtifactLights.Engine.Entities.Enums;

namespace NoArtifactLights.Engine.Entities.Structures
{
	[Serializable]
#pragma warning disable CA1815 // Override equals and operator equals on value types
	public struct LastSaveFile
#pragma warning restore CA1815 // Override equals and operator equals on value types
	{
		public int Version { get; set; }
		public float PlayerX { get; set; }
		public float PlayerY { get; set; }
		public float PlayerZ { get; set; }
		public WorldStatus Status { get; set; }
		public bool Blackout { get; set; }
		public Difficulty CurrentDifficulty { get; set; }
		public int Kills { get; set; }
		public int Cash { get; set; }
		public int Bank { get; set; }
		public string Model { get; set; }
		public SaveWeapon[] Weapons { get; set; }
		public int PlayerArmor { get; set; }
		public int PlayerHealth { get; set; }
		public float PlayerHungry { get; set; }
		public float PlayerHydration { get; set; }
	}
}
