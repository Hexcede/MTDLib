using MTDLib;

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

namespace MTDLib.Powerups {
	class InvinciblePowerup : Powerup {
		protected override void Apply(GameObject target) {
			PlayerController player = target.GetComponent<PlayerController>();
			player.playerHealth.isInvincible.Flip();
		}
	}
}