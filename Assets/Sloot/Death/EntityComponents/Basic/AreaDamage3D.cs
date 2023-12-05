using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AreaDamage3D : EntityRoot {
    [Header("Damage")]
    [SerializeField] public int enterDamage;
    [SerializeField] public int stayDamage;
    [SerializeField] public float damageTick = 1f;
    [SerializeField] public int exitDamage;
    [SerializeField] public string damageType = "";
    [Header("Damageables")]
    [SerializeField] protected List<string> damageables = new();
    [Header("Destroy")]
    [SerializeField] protected bool destroyOnCollision;
    [SerializeField] protected bool destroyOnTrigger;
    [SerializeField] protected List<string> entitiesInsideVisual = new();
    protected Dictionary<EntityHealth, float> entitiesInside = new();
    protected Action<EntityHealth> onEnter;
    public event Action<EntityHealth> OnEnter { add { onEnter += value; } remove { onEnter -= value; } }
    protected Action<EntityHealth> onStayDamage;
    public event Action<EntityHealth> OnStayDamage { add { onStayDamage += value; } remove { onStayDamage -= value; } }
    protected Action<EntityHealth> onExit;
    public event Action<EntityHealth> OnExit { add { onExit += value; } remove { onExit -= value; } }

    private void OnEnable() {
        entitiesInside.Clear();
    }

    private void AddEntityHealth(Collision collision) {
        AddEntityHealth(collision.gameObject.RootGet<EntityHealth>(), true);
    }

    private void AddEntityHealth(Collider collision) {
        AddEntityHealth(collision.gameObject.RootGet<EntityHealth>(), false);
    }

    protected virtual void AddEntityHealth(EntityHealth health, bool Collision) {
        if (health != null && !entitiesInside.ContainsKey(health) && damageables.Contains(health.GetRoot().tag)) {
            entitiesInside.Add(health, 0f);
            if (enterDamage != 0) {
                health.RemoveHealth(enterDamage, damageType);
            }

            health.GetRoot().OnDeath += () => entitiesInside.Remove(health);
            onEnter?.Invoke(health);
        }

        if (Collision && destroyOnCollision || !Collision && destroyOnTrigger) {
            Die();
        }
    }

    private void RemoveEntityHealth(Collision collision) {
        RemoveEntityHealth(collision.gameObject.RootGet<EntityHealth>());
    }

    private void RemoveEntityHealth(Collider collision) {
        RemoveEntityHealth(collision.gameObject.RootGet<EntityHealth>());
    }

    private void RemoveEntityHealth(EntityHealth health) {
        if (health == null || !entitiesInside.ContainsKey(health)) {
            return;
        }

        entitiesInside.Remove(health);
        if (exitDamage != 0) {
            health.RemoveHealth(exitDamage, damageType);
        }

        onExit?.Invoke(health);
    }

    private void FixedUpdate() {
        if (stayDamage != 0) {
            foreach (KeyValuePair<EntityHealth, float> health in entitiesInside.ToList()) {
                float time = health.Value;
                time += Time.fixedDeltaTime;
                if (time >= damageTick) {
                    health.Key.RemoveHealth(stayDamage, damageType);
                    time -= damageTick;
                    onStayDamage?.Invoke(health.Key);
                }

                entitiesInside[health.Key] = time;
            }
        }

        entitiesInsideVisual.Clear();
        entitiesInsideVisual.AddRange(entitiesInside.Keys.Where(item => item != null).Select(item => item.name).ToList());
    }

    protected override void ResetSetup() {
        base.ResetSetup();
        if(RootGet<EntityMainCollider3D>() == null) { 
            GetRoot().AddComponent<EntityMainCollider3D>();
        }
    }

    protected override void LoadSetup() {
        base.LoadSetup();
        RootGet<EntityMainCollider3D>().OnCollisionEnterDelegate += AddEntityHealth;
        RootGet<EntityMainCollider3D>().OnTriggerEnterDelegate += AddEntityHealth;
        RootGet<EntityMainCollider3D>().OnCollisionStayDelegate += AddEntityHealth;
        RootGet<EntityMainCollider3D>().OnTriggerStayDelegate += AddEntityHealth;
        RootGet<EntityMainCollider3D>().OnCollisionExitDelegate += RemoveEntityHealth;
        RootGet<EntityMainCollider3D>().OnTriggerExitDelegate += RemoveEntityHealth;

        if (damageables.Count == 0) {
            UnityEngine.Debug.LogWarning("No damageables in the AreaDamage3D of " + GetRoot().name);
        }
    }
}
