using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityComponent {
    public static GameObject Player;

    protected override void DefinitiveSetup() {
        if (Player == null) {
            Player = GetRootGameObject();
        } else {
            Destroy(gameObject);
        }
    }
}
