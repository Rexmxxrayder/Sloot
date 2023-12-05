using UnityEngine;
using UnityEngine.Events;

public class EntityCollider3D : EntityCollider<Collision, Collider> {

    #region Setup
    protected override void DefinitiveSetup() {
        if (RootGet<EntityMainCollider3D>() == null) {
            GetRootGameObject().AddComponent<EntityMainCollider3D>();
        }
    }
    #endregion

    #region ColliderFunctions
    private void OnCollisionEnter(Collision collision) {
        if (!isActive) { return; }
        _onCollisionEnterDelegate?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision) {
        if (!isActive) { return; }
        _onCollisionStayDelegate?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision) {
        if (!isActive) { return; }
        _onCollisionExitDelegate?.Invoke(collision);
    }

    private void OnTriggerEnter(Collider collider) {
        if (!isActive) { return; }
        _onTriggerEnterDelegate?.Invoke(collider);
    }

    private void OnTriggerStay(Collider collider) {
        if (!isActive) { return; }
        _onTriggerStayDelegate?.Invoke(collider);
    }

    private void OnTriggerExit(Collider collider) {
        if (!isActive) { return; }
        _onTriggerExitDelegate?.Invoke(collider);
    }
    #endregion
}
