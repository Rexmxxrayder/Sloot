using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EntityColliderDelegate3D : EntityCollider3D {
    protected Collider collider3D;
    public Collider Collider => collider3D;

    protected override void ResetSetup() {
        base.ResetSetup();
        collider3D = GetComponent<Collider>();
    }
}