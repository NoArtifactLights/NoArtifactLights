// NoArtifactLights
// (C) RelaperCrystal and contributors. Licensed under GPLv3 or later.

using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using NLog;
using NoArtifactLights.Engine.Mod.Controller;
using NoArtifactLights.Engine.Mod.API;
using NoArtifactLights.Resources;
using System;
using System.Windows.Forms;
using Screen = GTA.UI.Screen;
using System.Drawing;
using LemonUI;
using LemonUI.Menus;
using LemonUI.TimerBars;
using System.Threading;
using CommandPlus.Exceptions;

namespace NoArtifactLights.Engine.Mod.Scripts
{
	[ScriptAttributes(Author = "RelaperCrystal", SupportURL = "https://hotworkshop.atlassian.net/projects/NAL")]
	public class MenuScript : Script
	{
		private ObjectPool lemonPool;
		private NativeMenu mainMenu;
		private NativeItem itemSave;
		private NativeItem itemLoad;
		private NativeItem itemCallCops;
		private NativeItem itemModels;
		private NativeItem itemDifficulty;
		private NativeItem itemKills;
		private NativeCheckboxItem itemCheatEnabled;
		private NativeItem itemCommand;
		private NativeCheckboxItem itemLights;

		private NativeMenu modelMenu;
		private NativeItem itemDefaultModel;
		private NativeItem itemCopModel;

		private NativeMenu saveMenu;
		private NativeItem itemSaveSlot1;
		private NativeItem itemSaveSlot2;
		private NativeItem itemSaveSlot3;

		private NativeMenu loadMenu;
		private NativeItem itemLoadSlot1;
		private NativeItem itemLoadSlot2;
		private NativeItem itemLoadSlot3;

		private NativeMenu buyMenu;
		private NativeItem itemPistol;
		private NativeItem itemPumpShotgun;
		private NativeItem itemCarbineRifle;
		private NativeItem itemBodyArmor;

		private NativeMenu foodMenu;
		private NativeItem itemChicken;
		private NativeItem itemHamburger;
		private NativeItem itemWater;

		private Blip repairBlip;

		private static readonly TimerBarCollection timerBars = new TimerBarCollection();
		internal TimerBarProgress hungryBar = new TimerBarProgress(Strings.BarHungry);
		internal TimerBarProgress waterBar = new TimerBarProgress(Strings.BarWater);
		private Vector3 repair = new Vector3(140.683f, -1081.387f, 28.56039f);

		internal static MenuScript instance;

		internal static void ChangeHungryBarColor(Color cl)
		{
			instance.hungryBar.ForegroundColor = cl;
		}

		internal static void ChangeWaterBarColor(Color cl)
		{
			instance.waterBar.ForegroundColor = cl;
		}

		private static readonly NLog.Logger logger = LogManager.GetLogger("MenuScript");

		public MenuScript()
		{
			Common.Start += Common_Start;
		}

		private void Common_Start(object sender, EventArgs e)
		{
			try
			{
				//Thread.CurrentThread.Name = "UI Thread";
				logger.Trace("Loading Main Menu");
				lemonPool = new ObjectPool();
				logger.Trace("Menu Pool created");
#if DEBUG
				mainMenu = new NativeMenu("NAL Beta", Strings.MenuMainTitle);
#else
				mainMenu = new NativeMenu("NAL", Strings.MenuMainTitle);
#endif
				itemLights = new NativeCheckboxItem(Strings.ItemLightsTitle, Strings.ItemLightsSubtitle, true);
				//itemSave = new NativeItem(Strings.ItemSaveTitle, Strings.ItemSaveSubtitle);
				//itemLoad = new NativeItem(Strings.ItemLoadTitle, Strings.ItemLoadSubtitle);
				itemCallCops = new NativeItem(Strings.ItemCopsTitle, Strings.ItemCopsSubtitle);
				itemDifficulty = new NativeItem(Strings.ItemDifficulty, Strings.ItemDIfficultySubtitle);
				itemKills = new NativeItem(Strings.ItemKills, Strings.ItemKillsSubtitle);
				itemCheatEnabled = new NativeCheckboxItem(Strings.ItemCheat, Strings.ItemCheatDescription);
				itemCommand = new NativeItem(Strings.ItemCommand, Strings.ItemCommandDescription);
				itemModels = new NativeItem(Strings.ItemModels, Strings.ItemModelsDescription);

				modelMenu = new NativeMenu("Models", Strings.MenuModel);

				itemDefaultModel = new NativeItem("Default", "The classic NAL Model.");
				itemCopModel = new NativeItem("LSPD Officer", "The cop.");
				modelMenu.Add(itemDefaultModel);
				modelMenu.Add(itemCopModel);
				itemDefaultModel.Activated += ItemDefaultModel_Activated;
				itemCopModel.Activated += ItemCopModel_Activated;
				#region Items - Load & Save
				saveMenu = new NativeMenu(Strings.MenuSaveHeader, "SAVE");
				itemSaveSlot1 = new NativeItem(string.Format(Strings.MenuSaveItem, 1), Strings.MenuSaveItemSubtitle);
				itemSaveSlot2 = new NativeItem(string.Format(Strings.MenuSaveItem, 2), Strings.MenuSaveItemSubtitle);
				itemSaveSlot3 = new NativeItem(string.Format(Strings.MenuSaveItem, 3), Strings.MenuSaveItemSubtitle);

				itemSaveSlot1.Activated += (menu, i) => SaveController.Save(Common.blackout, 1);
				itemSaveSlot2.Activated += (menu, i) => SaveController.Save(Common.blackout, 2);
				itemSaveSlot3.Activated += (menu, i) => SaveController.Save(Common.blackout, 3);

				saveMenu.Add(itemSaveSlot1);
				saveMenu.Add(itemSaveSlot2);
				saveMenu.Add(itemSaveSlot3);
				lemonPool.Add(saveMenu);

				loadMenu = new NativeMenu(Strings.MenuLoadHeader, Strings.MenuLoadSubtitle);
				itemLoadSlot1 = new NativeItem(string.Format(Strings.MenuSaveItem, 1), Strings.MenuLoadItemSubtitle);
				itemLoadSlot2 = new NativeItem(string.Format(Strings.MenuSaveItem, 2), Strings.MenuLoadItemSubtitle);
				itemLoadSlot3 = new NativeItem(string.Format(Strings.MenuSaveItem, 3), Strings.MenuLoadItemSubtitle);

				itemLoadSlot1.Activated += (menu, i) => SaveController.Load(1);
				itemLoadSlot2.Activated += (menu, i) => SaveController.Load(2);
				itemLoadSlot3.Activated += (menu, i) => SaveController.Load(3);

				loadMenu.Add(itemLoadSlot1);
				loadMenu.Add(itemLoadSlot2);
				loadMenu.Add(itemLoadSlot3);
				lemonPool.Add(loadMenu);

				itemSave = mainMenu.AddSubMenu(saveMenu);
				itemSave.Title = Strings.ItemSaveTitle;
				itemSave.Description = Strings.ItemSaveSubtitle;

				itemLoad = mainMenu.AddSubMenu(loadMenu);
				itemLoad.Title = Strings.ItemLoadTitle;
				itemLoad.Description = Strings.ItemLoadSubtitle;
				#endregion

				foodMenu = new NativeMenu("Food", Strings.MenuFoodShopSubtitle);

				itemHamburger = HungryController.CreateFoodSellerItem(Strings.FoodBurger, Foods.Hamburger, 1);
				itemChicken = HungryController.CreateFoodSellerItem(Strings.FoodChicken, Foods.Chicken, 3);
				itemWater = HungryController.GenerateWaterSellItem(Strings.ItemWater, 2);

				foodMenu.Add(itemHamburger);
				foodMenu.Add(itemChicken);
				foodMenu.Add(itemWater);

				logger.Trace("All instances initialized");
				mainMenu.Add(itemLights);
				mainMenu.Add(itemSave);
				mainMenu.Add(itemLoad);
				mainMenu.Add(itemCallCops);

				itemModels = mainMenu.AddSubMenu(modelMenu);
				itemModels.Title = Strings.ItemModels;
				itemModels.Description = Strings.ItemModelsDescription;

				mainMenu.Add(itemDifficulty);
				mainMenu.Add(itemKills);

				lemonPool.Add(mainMenu);
				lemonPool.Add(modelMenu);
				lemonPool.Add(foodMenu);
				logger.Trace("Main Menu Done");
				Tick += MenuScript_Tick;
				KeyDown += MenuScript_KeyDown;
				itemLights.CheckboxChanged += ItemLights_CheckboxEvent;
				itemCallCops.Activated += ItemCallCops_Activated;
				itemCommand.Activated += ItemCommand_Activated;
				itemCheatEnabled.Activated += ItemCheatEnabled_Activated;

				timerBars.Add(hungryBar);
				timerBars.Add(waterBar);

				Common.Unload += Common_Unload;

				logger.Trace("Loading Ammu-Nation Menu");

				buyMenu = new NativeMenu(Strings.AmmuTitle, Strings.AmmuSubtitle);
				itemPistol = AmmuController.GenerateWeaponSellerItem(Strings.AmmuPistol, Strings.AmmuPistolSubtitle, 100);
				itemPumpShotgun = AmmuController.GenerateWeaponSellerItem(Strings.AmmuPumpShotgun, Strings.AmmuPumpShotgunSubtitle, 200);
				itemCarbineRifle = AmmuController.GenerateWeaponSellerItem(Strings.AmmuCarbineRifle, Strings.AmmuCarbineRifleSubtitle, 350);

				itemBodyArmor = new NativeItem(Strings.WeaponBodyArmor, Strings.WeaponBodyArmorDescription)
				{
					AltTitle = "$380"
				};
				logger.Trace("Instances created");
				buyMenu.Add(itemPistol);
				buyMenu.Add(itemPumpShotgun);
				buyMenu.Add(itemBodyArmor);
				lemonPool.Add(buyMenu);
				itemPistol.Activated += ItemPistol_Activated;
				itemPumpShotgun.Activated += ItemPumpShotgun_Activated;
				itemCarbineRifle.Activated += ItemCarbineRifle_Activated;
				itemBodyArmor.Activated += ItemBodyArmor_Activated;

				repairBlip = World.CreateBlip(repair);
				repairBlip.IsFriendly = true;
				repairBlip.IsShortRange = true;
				repairBlip.Sprite = BlipSprite.Garage;
				repairBlip.Color = BlipColor.Blue;
				repairBlip.Name = Strings.RepairBlip;

				HungryController.AddBlipsToAllResellers();
				instance = this;

				this.Aborted += MenuScript_Aborted;
				CommandController.Init();
			}
#pragma warning disable CA1031
			catch (Exception ex)
#pragma warning restore CA1031
			{
				GameUI.DisplayHelp(Strings.ExceptionMenu);
				logger.Fatal(ex, "Error while loading menu");
				Common.UnloadMod(this);
				this.Abort();
			}
		}

		private void ItemCheatEnabled_Activated(object sender, EventArgs e)
		{
			Common.IsCheatEnabled = itemCheatEnabled.Checked;
		}

		private void ItemCommand_Activated(object sender, EventArgs e)
		{
			string str = Game.GetUserInput(WindowTitle.EnterMessage60, "", short.MaxValue);
			if(!Common.IsCheatEnabled)
			{
				GameUI.DisplayHelp(Strings.NoPermission);
				return;
			}
			try
			{
				CommandController.Run(str);
			}
			catch (UnexceptedValueException uvex)
			{
				Notification.Show(uvex.Message);
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception ex)
			{
				Notification.Show(ex.ToString());
			}
#pragma warning restore CA1031 // Do not catch general exception types
			finally
			{
				logger.Info("User typed command: " + str);
			}
		}

		private void ItemCopModel_Activated(object sender, EventArgs args)
		{
			Game.Player.ChangeModel("s_m_y_cop_01");
			//selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
			//itemDefaultModel.SetRightBadge(UIMenuItem.BadgeStyle.None);
			itemSave.Enabled = false;
		}

		private void ItemDefaultModel_Activated(object sender, EventArgs args)
		{
			Game.Player.ChangeModel("a_m_m_bevhills_02");
			//selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
			//itemCopModel.SetRightBadge(UIMenuItem.BadgeStyle.None);
			itemSave.Enabled = false;
		}

		private void ItemCarbineRifle_Activated(object sender, EventArgs args)
		{
			AmmuController.SellWeapon(350, 50, WeaponHash.CarbineRifle);
		}

		private void MenuScript_Aborted(object sender, EventArgs e)
		{
			if (repairBlip?.Exists() == true)
			{
				repairBlip.Delete();
			}

			if (buyMenu != null) buyMenu.Visible = false;
			if (mainMenu != null)
			{
				mainMenu.Visible = false;
				itemLights.CheckboxChanged -= ItemLights_CheckboxEvent;
				itemCallCops.Activated -= ItemCallCops_Activated;
			}

			Tick -= MenuScript_Tick;
			KeyDown -= MenuScript_KeyDown;

			mainMenu = null;
			buyMenu = null;
		}

		private void Common_Unload(object sender, EventArgs e)
		{
			if(!sender.Equals(this))
			{
				Abort();
			}
		}

		private void ItemBodyArmor_Activated(object sender, EventArgs args) => AmmuController.SellArmor(50, 380);
		private void ItemPumpShotgun_Activated(object sender, EventArgs args) => AmmuController.SellWeapon(200, 50, WeaponHash.PumpShotgun);
		private void ItemPistol_Activated(object sender, EventArgs args) => AmmuController.SellWeapon(100, 100, WeaponHash.Pistol);

		private void ItemCallCops_Activated(object sender, EventArgs args)
		{
			Function.Call(Hash.CREATE_INCIDENT, 7, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, 2, 3.0f, new OutputArgument());
		}

		private void ItemLights_CheckboxEvent(object sender, EventArgs args)
		{
			Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, itemLights.Checked);
		}

		private void MenuScript_Tick(object sender, EventArgs e)
		{
			lemonPool.Process();
			timerBars.Process();

			hungryBar.Progress = HungryController.ProgressBarStatus;
			waterBar.Progress = HungryController.WaterBarStatus;
			if (AmmuController.DistanceToAmmu())
			{
				GameUI.DisplayHelp(Strings.AmmuOpenShop);
			}
			if (HungryController.IsPlayerCloseReseller())
			{
				GameUI.DisplayHelp(Strings.FoodOpenShop);
			}
			if (repair.DistanceTo(Game.Player.Character.Position) <= 10f && Game.Player.Character.IsInVehicle())
			{
				GameUI.DisplayHelp(Strings.RepairHelp);
			}
		}

		private void MenuScript_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.N:
					mainMenu.Visible = !mainMenu.Visible;
					itemDifficulty.AltTitle = Strings.ResourceManager.GetString("Difficulty" + Common.difficulty.ToString());
					itemKills.AltTitle = Common.counter.ToString();
					break;
				case Keys.E:
					if (mainMenu.Visible) return;

					if (buyMenu.Visible)
					{
						buyMenu.Visible = false;
						return;
					}

					if (AmmuController.DistanceToAmmu() && !lemonPool.AreAnyVisible)
					{
						buyMenu.Visible = true;
					}

					if (HungryController.IsPlayerCloseReseller() && !lemonPool.AreAnyVisible)
					{
						foodMenu.Visible = true;
					}

					if (repair.DistanceTo(Game.Player.Character.Position) <= 10f && Game.Player.Character.IsInVehicle())
					{
						if (!Game.Player.Character.CurrentVehicle.IsDamaged)
						{
							Screen.ShowSubtitle(Strings.RepairUndamaged);
							return;
						}
						if (!Common.Cost(100)) break;
						Game.Player.Character.CurrentVehicle.Repair();
						Screen.ShowSubtitle(Strings.RepairSuccess);
					}

					break;
			}
		}
	}
}
