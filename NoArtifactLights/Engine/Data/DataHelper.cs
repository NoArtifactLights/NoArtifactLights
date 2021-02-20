// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using NoArtifactLights.Engine.Entities.Structures;

namespace NoArtifactLights.Engine.Data
{
	internal static class DataHelper
	{
		internal static byte[] SerializeToBson(SaveFile save)
		{
			MemoryStream ms = new MemoryStream();
			using (BsonWriter writer = new BsonWriter(ms))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(writer, save);
			}
			return ms.ToArray();
		}

		internal static SaveFile DeserializeToSave(byte[] save)
		{
			MemoryStream ms = new MemoryStream(save);
			using (BsonReader reader = new BsonReader(ms))
			{
				JsonSerializer serializer = new JsonSerializer();
				return serializer.Deserialize<SaveFile>(reader);
			}
		}
	}
}
