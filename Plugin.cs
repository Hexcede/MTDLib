using HarmonyLib;

using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using flanne;
using flanne.Core;

namespace MTDLib
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInProcess("MinutesTillDawn.exe")]
	public class MTD : BaseUnityPlugin
	{
		public static MTD Instance { get; private set; }
		public static ConfigEntry<bool> disableAchievements;
		public static ConfigEntry<bool> enableInvincibility;
		public static ConfigEntry<bool> infiniteRerolls;
		public static ConfigEntry<bool> allPowerupsRepeatable;
		internal static ManualLogSource Log;

		private UnityEvent onBattleStarted;
		public UnityEvent onBattleReady;
		private UnityEvent onTitleScreen;
		private UnityEvent onUpdate;

		private List<Powerup> powerupsToLoad;

		// Scene management
		private void SceneChanged(Scene oldScene, Scene newScene) {
			if (newScene == null || newScene.name == null) {
				Log.LogWarning("Scene changed, but new Scene is null or has no name.");
				return;
			}

			if (newScene.name == "Battle") {
				if (onBattleStarted != null)
					onBattleStarted.Invoke();
			}
			else if (newScene.name == "TitleScreen") {
				if (onTitleScreen != null)
					onTitleScreen.Invoke();
			}
		}

		public GameController game {
			get {
				return GameObject.FindObjectOfType<GameController>();
			}
		}

		public MTD() {
			Harmony.CreateAndPatchAll(typeof(Patch.PowerupMenuState_Patch));

			if (Instance == null) {
				Instance = this;
			}
			else if (Instance != this) {
				DestroyImmediate(gameObject);
				return;
			}

			// Logging
			Log = base.Logger;

			// Scene management
			if (onBattleStarted == null)
				onBattleStarted = new UnityEvent();
			if (onTitleScreen == null)
				onTitleScreen = new UnityEvent();
			if (onUpdate == null)
				onUpdate = new UnityEvent();
			if (onBattleReady == null)
				onBattleReady = new UnityEvent();
			if (powerupsToLoad == null)
				powerupsToLoad = new List<Powerup>();
			SceneManager.activeSceneChanged += SceneChanged;

			// Config
			disableAchievements = Config.Bind("General", "Disable Achievements", true, "Disable Steam achievements in the game.");
			enableInvincibility = Config.Bind("General", "Enable invincibility", false, "Enable invincibility.");
			infiniteRerolls = Config.Bind("General", "Infinite rerolls", true, "Allow you to reroll infinitely on any character.");
			allPowerupsRepeatable = Config.Bind("General", "All powerups repeatable", true, "Make all powerups repeatable.");
		}

		private void Awake()
		{
			// Done
			Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			// Update when battle starts
			onBattleStarted.AddListener(UpdateAchievementsDisabled);
			// Update when setting changed
			disableAchievements.SettingChanged += new System.EventHandler((object sender, System.EventArgs e) => UpdateAchievementsDisabled());

			// Register event listeners
			onBattleStarted.AddListener(() => Log.LogInfo("Battle started!"));
			onTitleScreen.AddListener(() => Log.LogInfo("On the title screen!"));

			if (enableInvincibility.Value)
				powerupsToLoad.Add(ScriptableObject.CreateInstance<Powerups.InvinciblePowerup>());
			if (infiniteRerolls.Value)
				powerupsToLoad.Add(ScriptableObject.CreateInstance<Powerups.InfiniteRerollPowerup>());
			if (allPowerupsRepeatable.Value)
				powerupsToLoad.Add(ScriptableObject.CreateInstance<Powerups.AllPowerupsRepeatable>());

			onBattleReady.AddListener(() => {
				// Loop over all powerups and call ApplyAndNotify
				foreach (Powerup powerup in powerupsToLoad) {
					Log.LogInfo($"Giving custom default powerup {powerup.GetType().Name}");
					UnityEngine.Debug.Log($"Giving custom default powerup {powerup.GetType().Name}");
					powerup.ApplyAndNotify(game.player.gameObject);
				}
			});

			onBattleStarted.AddListener(() => {
				// Poll for various objects each Update until ready
				UnityAction whenReady = null;
				whenReady = () => {
					if (game == null) {
						return;
					}
					var player = game.player;
					if (player == null) {
						return;
					}
					if (player.gameObject == null) {
						return;
					}
					var playerHealth = game.playerHealth;
					if (playerHealth == null) {
						return;
					}
					if (playerHealth.isInvincible == null)
						return;
					onUpdate.RemoveListener(whenReady); // We are ready, remove the temporary listener

					// Invoke ready event
					onBattleReady.Invoke();
				};
				onUpdate.AddListener(whenReady);
			});

			// In case of reloading
			SceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
			UpdateAchievementsDisabled();
		}

		private void Update() {
			onUpdate.Invoke();
		}

		// Updates whether or not achievements are enabled on the current map based on the config option
		private void UpdateAchievementsDisabled() {
			if (SelectedMap.MapData != null && disableAchievements != null)
				SelectedMap.MapData.achievementsDisabled = disableAchievements.Value;
		}

		private void OnDestroy() {
			Harmony.UnpatchAll();

			SceneManager.activeSceneChanged -= SceneChanged;
			onBattleStarted.RemoveAllListeners();
			onTitleScreen.RemoveAllListeners();
		}
	}
}