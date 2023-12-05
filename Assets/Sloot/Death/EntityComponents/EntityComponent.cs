using Sloot;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EntityComponent : MonoBehaviour, IEntity, IReset {
    protected EntityRoot _root = null;

    private void Awake() {
        SetRoot();
        DefinitiveSetup();
        InstanceReset();
    }

    private void Start() {
        InstanceLoad();
    }

    private void OnDestroy() {
        DestroySetup();
    }

    protected virtual void DefinitiveSetup() { }

    protected virtual void ResetSetup() { }

    protected virtual void LoadSetup() { }

    protected virtual void DestroySetup() { }

    public void InstanceReset() {
        ResetSetup();
    }

    public void InstanceLoad() {
        LoadSetup();
    }

    public virtual EntityRoot SetRoot() {
        if (_root == null) {
            if (gameObject.GetComponent<EntityRoot>() != null) {
                _root = gameObject.GetComponent<EntityRoot>();
                return _root;
            } else if (transform.parent == null) {
                GameObject newParent = new() {
                    name = "Root"
                };

                transform.parent = newParent.transform;
            }

            EntityComponent parentComponent = transform.parent.GetComponent<EntityComponent>();
            _root = parentComponent == null ? transform.parent.AddComponent<EntityRoot>().GetRoot() : parentComponent.GetRoot();
        }

        return _root;
    }

    public virtual EntityRoot GetRoot() {
        if (_root == null) {
            return SetRoot();
        } else {
            return _root;
        }
    }

    public bool StillRoot() {
        return _root != null;
    }

    public GameObject GetRootGameObject() {
        return GetRoot().gameObject;
    }

    public Transform GetRootTransform() {
        return GetRoot().transform;
    }

    public Vector3 GetRootPosition() {
        return GetRoot().transform.position;
    }

    public Quaternion GetRootRotation() {
        return GetRoot().transform.rotation;
    }

    public Vector3 GetRootLocalScale() {
        return GetRoot().transform.localScale;
    }

    public T RootGet<T>() where T : MonoBehaviour, IEntity {
        return GetRootGameObject().GetComponentInChildren<T>();
    }

    public virtual void Die() {
        GetRoot().Die();
    }
}
public static class ExtensionEntityComponent {
    public static EntityRoot GetRoot(this GameObject gO) {
        if (gO.GetComponentInParent<EntityRoot>(true) == null) {
            return null;
        }

        return gO.GetComponentInParent<EntityComponent>(true).GetRoot();
    }

    public static GameObject GetRootGameObject(this GameObject gO) {
        if (gO.GetComponentInParent<EntityComponent>(true) == null) {
            return null;
        }

        return gO.GetComponentInParent<EntityComponent>(true).GetRootGameObject();
    }

    public static Transform GetRootTransform(this GameObject gO) {
        return gO.GetRoot().transform;
    }

    public static Vector3 GetRootPosition(this GameObject gO) {
        return gO.GetRoot().transform.position;
    }

    public static Quaternion GetRootRotation(this GameObject gO) {
        return gO.GetRoot().transform.rotation;
    }

    public static Vector3 GetRootScale(this GameObject gO) {
        return gO.GetRoot().transform.localScale;
    }

    public static T RootGet<T>(this GameObject gO) where T : MonoBehaviour, IEntity {
        if (gO.GetRoot() == null) {
            return null;
        }

        return gO.GetRoot().GetComponentInChildren<T>();
    }

    public static void Die(this GameObject gO) {
        if (gO.GetRoot() == null) {
            return;
        }

        gO.GetRoot().Die();
    }
}
