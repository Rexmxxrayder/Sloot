using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EntityRoot : EntityComponent {
    private string type;
    bool died;
    private Action _onDeath;
    public event Action OnDeath { add => _onDeath += value; remove => _onDeath -= value; }

    delegate void DeathWay();
    DeathWay newDeathWay;
    public event Action NewDeathWay { add => newDeathWay = new(value); remove => newDeathWay = null; }
    public string Type => type;
    public override EntityRoot SetRoot() {
        _root = this;
        return _root;
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
            Debug.Log("DESTROY " + _root.name);
            GetRootGameObject().SetActive(false);
            Destroy(gameObject);
        }
    }

    public void OnDestroy() {
        Die();
    }
}
