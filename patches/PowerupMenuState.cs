using System;
using HarmonyLib;
using System.Reflection;

using flanne;
using flanne.Core;
using UnityEngine.UI;

namespace MTDLib.Patch
{
	class PowerupMenuState_Patch {
		[HarmonyPatch(typeof(PowerupMenuState), "OnReroll")]
		[HarmonyPostfix]
		static void InfiniteRerollPatch() {
			if (Powerups.InfiniteRerollPowerup.Enabled)
				MTD.Instance.game.powerupRerollButton.gameObject.SetActive(true);
		}

		[HarmonyPatch(typeof(PowerupMenuState), "OnConfirm")]
		[HarmonyPrefix]
		static bool AllPowerupsRepeatablePatch(object sender, Powerup e, out bool __state) {
			if (!e.isRepeatable && Powerups.AllPowerupsRepeatable.Enabled) {
				e.isRepeatable = true;
				__state = true;
			}
			else {
				__state = false;
			}
			return true;
		}
		[HarmonyPatch(typeof(PowerupMenuState), "OnConfirm")]
		[HarmonyPostfix]
		static void AllPowerupsRepeatablePatch_Post(object sender, Powerup e, bool __state) {
			if (__state && e.isRepeatable && Powerups.AllPowerupsRepeatable.Enabled)
				e.isRepeatable = false;
		}
	}
}