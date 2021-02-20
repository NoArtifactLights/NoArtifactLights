// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using GTA;
using GTA.Native;
using GTA.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using NLog;
using NoArtifactLights.Engine.Entities.Structures;
using NoArtifactLights.Resources;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace NoArtifactLights.Engine.Mod.Controller
{
	internal static class SaveController
	{
		private static readonly Logger logger = LogManager.GetLogger("SaveController");
		private const string SaveFilePath = "NAL\\game.dat";
		private const int SaveVersion = 7;

		internal static void CheckAndFixDataFolder()
		{
			Directory.CreateDirectory("NAL");
			Directory.CreateDirectory("NAL\\saves");
			if (File.Exists("NALSave.xml")) File.Move("NALSave.xml", "NAL\\Save.xml");
			if (File.Exists("NAL\\Save.xml") || File.Exists("NAL\\game.dat"))
			{
				logger.Warn("Deprecated save found - will not load it!");
				Notification.Show(Strings.DeprecatedXMLSave);
			}
		}

		internal static void SaveGameFile(SaveFile sf, int slot)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			try
			{
				const string tempPath = "NAL\\temp\\raw.dat";
				var fs = File.Create(tempPath);

				logger.Info("Stream opened");

#pragma warning disable CS0618
				using (var writer = new BsonWriter(fs))
#pragma warning restore CS0618
				{
					logger.Info("Writing to BSON");
					JsonSerializer serializer = new JsonSerializer();
					serializer.Serialize(writer, sf);
				}

				var savePath = $"NAL\\saves\\{slot}.dat";

				if (File.Exists(savePath))
				{
					logger.Info("Overwritten save file");
					File.Delete(savePath);
				}

				ZipFile.CreateFromDirectory("NAL\\temp", savePath);
				Directory.Delete("NAL\\temp", true);
			}
			catch (IOException ioex)
			{
				logger.Error("Error while saving game file: \r\n" + ioex.ToString());
				logger.Error("Operation is aborted.");
				Screen.ShowSubtitle("Error");
			}
			sw.Stop();
			logger.Trace("File save cost " + sw.ElapsedMilliseconds + "ms.");
		}

		internal static SaveFile LoadGameFile(int slot)
		{
			string savePath = $"NAL\\saves\\{slot}.dat";

			if (Directory.Exists("NAL\\temp"))
			{
				logger.Info("Overwritten temporary files");
				Directory.Delete("NAL\\temp", true);
			}

			Directory.CreateDirectory("NAL\\temp");
			ZipFile.ExtractToDirectory(savePath, "NAL\\temp");

			SaveFile result;

			FileStream fs = File.OpenRead("NAL\\temp\\raw.dat");
#pragma warning disable CS0618
			using (var br = new BsonReader(fs))
#pragma warning restore CS0618
			{
				JsonSerializer serializer = new JsonSerializer();
				result = serializer.Deserialize<SaveFile>(br);
			}

			Directory.Delete("NAL\\temp", true);
			return result;
		}

		internal static LastSaveFile LoadOldSave()
		{
			if (Directory.Exists("NAL\\temp"))
			{
				logger.Info("Overwritten temporary files");
				Directory.Delete("NAL\\temp", true);
			}

			Directory.CreateDirectory("NAL\\temp");
			ZipFile.ExtractToDirectory("NAL\\game.dat", "NAL\\temp");
			string datBase64 = File.ReadAllText("NAL\\temp\\raw.dat");
			byte[] jsonBytes = Convert.FromBase64String(datBase64);
			string json = Encoding.UTF8.GetString(jsonBytes);

			var contractResolver = new DefaultContractResolver
			{
				NamingStrategy = new SnakeCaseNamingStrategy()
			};

			var result = JsonConvert.DeserializeObject<LastSaveFile>(json, new JsonSerializerSettings
			{
				ContractResolver = contractResolver,
				Formatting = Formatting.None
			});

			Directory.Delete("NAL\\temp", true);
			return result;
		}

		internal static void Load(int slot)
		{
			CheckAndFixDataFolder();
			SaveFile sf;

			if (!File.Exists(SaveFilePath))
			{
				Notification.Show(Strings.NoSave);
				return;
			}

			sf = LoadGameFile(slot);
			if (sf.Version != SaveVersion)
			{
				Notification.Show(Strings.SaveVersion);
				return;
			}

			World.Weather = sf.Status.CurrentWeather;
			World.CurrentTimeOfDay = new TimeSpan(sf.Status.Hour, sf.Status.Minute, 0);
			Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, sf.Blackout);
			Common.blackout = sf.Blackout;
			Game.Player.Character.Position = new GTA.Math.Vector3(sf.PlayerX, sf.PlayerY, sf.PlayerZ);
			Common.counter = sf.Kills;
			Common.difficulty = sf.CurrentDifficulty;
			GameController.SetRelationship(sf.CurrentDifficulty);
			Game.Player.Character.Weapons.RemoveAll();
			Game.Player.Character.Weapons.Give(WeaponHash.Flashlight, 1, false, true);
			if (sf.PlayerHealth > 0)
			{
				Game.Player.Character.Health = sf.PlayerHealth;
			}

			Game.Player.Character.Armor = sf.PlayerArmor;
		}

		internal static void Save(bool blackout, int slot)
		{
			CheckAndFixDataFolder();
			SaveFile sf = new SaveFile
			{
				Version = SaveVersion,
				Status = new WorldStatus(World.Weather, World.CurrentTimeOfDay.Hours, World.CurrentTimeOfDay.Minutes),
				PlayerX = Game.Player.Character.Position.X,
				PlayerY = Game.Player.Character.Position.Y,
				PlayerZ = Game.Player.Character.Position.Z,
				Blackout = blackout,
				Kills = Common.counter,
				CurrentDifficulty = Common.difficulty,
				PlayerHealth = Game.Player.Character.Health,
				PlayerArmor = Game.Player.Character.Armor,
				PlayerHungry = HungryController.Hungry,
				PlayerHydration = HungryController.Water
			};

			SaveGameFile(sf, slot);
		}

		internal static SaveFile UpdateSaveFile(LastSaveFile lsf)
		{
			SaveFile result = new SaveFile
			{
				Blackout = lsf.Blackout,
				CurrentDifficulty = lsf.CurrentDifficulty,
				Kills = lsf.Kills,
				Model = lsf.Model,
				PlayerArmor = lsf.PlayerArmor,
				PlayerHealth = lsf.PlayerHealth,
				PlayerHungry = 10.0f,
				PlayerHydration = 10.0f,
				PlayerX = lsf.PlayerX,
				PlayerY = lsf.PlayerY,
				PlayerZ = lsf.PlayerZ,
				Status = lsf.Status,
				Version = SaveVersion
			};
			return result;
		}
	}
}
