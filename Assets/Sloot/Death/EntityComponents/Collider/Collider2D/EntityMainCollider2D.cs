using System.Collections.Generic;
using UnityEngine;

public class EntityMainCollider2D : EntityCollider2D {
    [SerializeField] List<EntityCollider2D> colliders = new();

    protected override void LoadSetup() {
        colliders.Clear();
        foreach (EntityCollider2D collider in GetComponentsInChildren<EntityCollider2D>(true)) {
            if(collider == this) { continue; }
            colliders.Add(collider);
            AssignTo(collider);
        }
    }
}
