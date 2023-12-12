using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EntityCollider3D : EntityCollider<Collision, Collider> {

    #region Setup
    protected override void LoadSetup() {
        if (isMainCollider) {
            foreach (EntityCollider3D collider in GetRootComponents<EntityCollider3D>()) {
                if (collider == this) {
                    continue;
                }

                AssignTo(collider);
            }
        } else {
            if (!GetRoot().TryGetComponent(out EntityCollider3D _)) {
                GetRoot().AddComponent<EntityCollider3D>();
            }
        }
    }

    public static EntityCollider3D GetMainCollider(EntityComponent component) {
        foreach (EntityCollider3D collider in component.GetRoot().GetRootComponents<EntityCollider3D>()) {
            if (collider.isMainCollider) {
                return collider;
            }
        }

        return null;
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
