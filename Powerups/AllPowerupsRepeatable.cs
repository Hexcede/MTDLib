using System.Collections;

using UnityEngine;
using flanne;

namespace MTDLib.Powerups {
	class AllPowerupsRepeatable : Powerup {
		public static bool Enabled = false;
		protected override void Apply(GameObject target) {
			PlayerController player = target.GetComponent<PlayerController>();
			var game = MTD.Instance.game;
			if (player == game.player.GetComponent<PlayerController>()) {
				Enabled = true;
			}
		}
	}
}