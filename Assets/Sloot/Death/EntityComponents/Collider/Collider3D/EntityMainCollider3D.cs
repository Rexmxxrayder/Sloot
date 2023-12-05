using System.Collections.Generic;
using UnityEngine;

public class EntityMainCollider3D : EntityCollider3D {
    [SerializeField] List<EntityCollider3D> colliders = new();

    protected override void LoadSetup() {
        colliders.Clear();
        foreach (EntityCollider3D collider in GetComponentsInChildren<EntityCollider3D>()) {
            if(collider == this) { continue; }
            colliders.Add(collider);
            AssignTo(collider);
        }
    }
}
