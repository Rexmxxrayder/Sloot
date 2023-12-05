using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ability : MonoBehaviour {

    [SerializeField] protected float cooldown = 1f;
    protected float timeRemainingCooldown = 0f;
    protected bool inUse = false;
    public bool IsAvailable => timeRemainingCooldown == 0f;
    public bool InUse => inUse;
    private Action _onAvailable;
    public event Action OnAvailable { add => _onAvailable += value; remove => _onAvailable -= value; }
    protected List<Ability> abilities = new ();
    public virtual void Launch(EntityBrain brain, bool isUp) {
        if (!IsAvailable) {
            return;
        }

        if(isUp) {
            LaunchAbilityUp(brain);
        } else {
            LaunchAbilityDown(brain);
        }

        foreach (Ability ability in abilities) {
            ability.Launch(brain, isUp);
        }
    }

    protected virtual void Awake() {
        for (int i = 0; i < transform.childCount; i++) {
            Ability childAbility = transform.GetChild(i).GetComponent<Ability>();
            if (childAbility != null) {
                abilities.Add(childAbility);
            }
        }
    }

    public void StartCooldown() {
        StartCoroutine(CooldownManager());
    }

    private IEnumerator CooldownManager() {
        inUse = false;
        timeRemainingCooldown = cooldown;
        while (timeRemainingCooldown > 0f) {
            yield return null;
            timeRemainingCooldown -= Time.deltaTime;
        }

        timeRemainingCooldown = 0f;
        _onAvailable?.Invoke();
    }
    protected virtual void LaunchAbilityUp(EntityBrain brain) { }
    protected virtual void LaunchAbilityDown(EntityBrain brain) { }

    public virtual void Cancel() {
        StopAllCoroutines();
        StartCooldown();
    }
}
