// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using NoArtifactLights.Cilent;

namespace NoArtifactLights.Engine.Mod.API
{
	public delegate void ClientTickEventHandler(GameProcess sender);
	public delegate void ClientSelectPedEventHandler(GameProcess sender, Ped ped);

	public static class Functions
	{
		/// <summary>
		/// Occurs each time at a game tick.
		/// </summary>
		public static event ClientTickEventHandler OnTick;

		/// <summary>
		/// Occurs when the game process has selected a <see cref="Ped"/>.
		/// </summary>
		public static event ClientSelectPedEventHandler SelectPed;

		/// <summary>
		/// Occurs when the game process is starting.
		/// </summary>
		public static event EventHandler Initialize;

		/// <summary>
		/// Shows a dialog box and returns the result as integer.
		/// </summary>
		/// <returns>An instance of <see cref="int"/>.</returns>
		public static int GetMoneyAmountInput()
		{
			string temp = Game.GetUserInput(WindowTitle.EnterMessage20, "", 20);
			int result = int.Parse(temp);
			return result;
		}

		public static bool TryGetMoneyAmountInput(out int result)
		{
			bool success = false;
			int ret = 0;

			try
			{
				ret = GetMoneyAmountInput();
				success = true;
			}
			catch
			{
				ret = default(int);
				success = false;
			}
			finally
			{
				result = ret;
			}

			return success;

		}

		internal static void TriggerTick(GameProcess game)
		{
			OnTick(game);
		}

		internal static void TriggerPed(GameProcess sender, Ped p)
		{
			SelectPed(sender, p);
		}

		internal static void TriggerInitialize(GameProcess sender) => Initialize(sender, new EventArgs());
	}
}
