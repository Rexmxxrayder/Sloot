using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EntityCollider2D : EntityCollider<Collision2D, Collider2D> {
    #region Setup
    protected override void LoadSetup() {
        if (isMainCollider) {
            foreach (EntityCollider2D collider in GetRootComponents<EntityCollider2D>()) {
                if (collider == this) {
                    continue;
                }

                AssignTo(collider);
            }
        } else {
            if (!GetRoot().TryGetComponent(out EntityCollider2D _)) {
                GetRoot().AddComponent<EntityCollider2D>();
            }
        }
    }

    public static EntityCollider2D GetMainCollider(EntityComponent component) {
        foreach (EntityCollider2D collider in component.GetRoot().GetRootComponents<EntityCollider2D>()) {
            if (collider.isMainCollider) {
                return collider;
            }
        }

        return null;
    }
    #endregion

    #region ColliderFunctions
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!isActive) { return; }
        _onCollisionEnterDelegate.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (!isActive) { return; }
        _onCollisionStayDelegate.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (!isActive) { return; }
        _onCollisionExitDelegate.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (!isActive) { return; }
        _onTriggerEnterDelegate.Invoke(collider);
    }

    private void OnTriggerStay2D(Collider2D collider) {
        if (!isActive) { return; }
        _onTriggerStayDelegate.Invoke(collider);
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (!isActive) { return; }
        _onTriggerExitDelegate.Invoke(collider);
    }
    #endregion
}
