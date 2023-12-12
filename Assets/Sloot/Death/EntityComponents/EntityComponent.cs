using Sloot;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EntityComponent : MonoBehaviour, IReset {
    protected EntityRoot root = null;

    private void Awake() {
        SetRoot();
        DefinitiveSetup();
        InstanceReset();
    }

    private void Start() {
        InstanceLoad();
    }

    private void OnDestroy() {
        root.RemoveComponent(this);
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

    protected virtual EntityRoot SetRoot() {
        if (root == null) {
            if (gameObject.TryGetComponent(out EntityRoot entityRoot)) {
                root = entityRoot;
            } else {
                if (transform.parent == null) {
                    GameObject newParent = new() {
                        name = "Root"
                    };

                    transform.parent = newParent.transform;
                }

                EntityComponent parentComponent = transform.parent.GetComponent<EntityComponent>();
                root = parentComponent == null ? transform.parent.AddComponent<EntityRoot>().GetRoot() : parentComponent.GetRoot();
            }
        }

        root.AddComponent(this);
        return root;
    }

    public virtual EntityRoot GetRoot() {
        if (root == null) {
            return SetRoot();
        } else {
            return root;
        }
    }

    public bool StillRoot() {
        return root != null;
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

    public virtual T GetRootComponent<T>() where T : EntityComponent {
        return GetRoot().GetRootComponent<T>();
    }

    public virtual T[] GetRootComponents<T>() where T : EntityComponent {
        return GetRoot().GetRootComponents<T>();
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

    public static T GetRootComponent<T>(this GameObject gO) where T : EntityComponent {
        if (gO.GetRoot() == null) {
            return null;
        }

        return gO.GetRoot().GetRootComponent<T>();
    }

    public static T[] GetRootComponents<T>(this GameObject gO) where T : EntityComponent {
        if (gO.GetRoot() == null) {
            return null;
        }

        return gO.GetRoot().GetRootComponents<T>();
    }

    public static void Die(this GameObject gO) {
        if (gO.GetRoot() == null) {
            return;
        }

        gO.GetRoot().Die();
    }
}
