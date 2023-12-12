using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EntityRoot : EntityComponent {
    private string type;
    private List<EntityComponent> components = new();

    private bool died;
    private Action _onDeath;
    public event Action OnDeath { add => _onDeath += value; remove => _onDeath -= value; }
    delegate void DeathWay();
    private DeathWay newDeathWay;

    public event Action NewDeathWay { add => newDeathWay = new(value); remove => newDeathWay = null; }
    public string Type => type;
    protected override EntityRoot SetRoot() {
        root = this;
        return root;
    }

    public void AddComponent(EntityComponent entityComponent) {
        if (components.Contains(entityComponent) || entityComponent is EntityRoot) {
            return;
        }

        components.Add(entityComponent);
    }

    public void RemoveComponent(EntityComponent entityComponent) {
        if (!components.Contains(entityComponent)) {
            return;
        }

        components.Remove(entityComponent);
    }

    private void OnValidate() {
        type = gameObject.name;
    }

    public void GiveType(string typeGiven) {
        type ??= typeGiven;
    }

    protected override void DefinitiveSetup() {
        newDeathWay = null;
        _onDeath = null;
    }

    protected override void ResetSetup() {
        died = false;
    }

    public override T GetRootComponent<T>() {
        Debug.Log("Get " + typeof(T));
        foreach (EntityComponent component in components) {
            if (component is T rootComponent) {
                return rootComponent;
            };
        };

        return null;
    }

    public override T[] GetRootComponents<T>() {
        List<T> correspondingComponents = new(); 
        foreach (EntityComponent component in components) {
            if (component is T rootComponent) {
                correspondingComponents.Add(rootComponent);
            };
        };

        return correspondingComponents.ToArray();
    }

    public virtual void Spawn(Vector3 groundPosition, Quaternion rotation = default) {
        transform.position = groundPosition;
        transform.rotation = rotation;
    }

    public override void Die() {
        if (died) {
            return;
        } else {
            died = true;
        }

        _onDeath?.Invoke();
        if (newDeathWay != null) {
            newDeathWay();
        } else {
            Debug.Log("DESTROY " + root.name);
            GetRootGameObject().SetActive(false);
            Destroy(gameObject);
        }
    }

    public void OnDestroy() {
        Die();
    }
}
