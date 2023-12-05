using UnityEngine;
using UnityEngine.Events;

public class EntityCollider2D : EntityCollider<Collision2D,Collider2D> {
    #region Setup
    protected override void DefinitiveSetup() {
        if (RootGet<EntityMainCollider2D>() == null) {
            GetRootGameObject().AddComponent<EntityMainCollider2D>();
        }
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
