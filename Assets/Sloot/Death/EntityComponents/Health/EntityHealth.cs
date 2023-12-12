using Sloot;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : EntityComponent{

    private RangeInt health = new();
    [SerializeField] private int startMaxHealth;
    [SerializeField] private int startHealth;
    public int Health => health.CurrentValue;
    public int MaxHealth => health.MaxValue;

    private List<string> damageList = new();
    private List<string> healList = new();

    private Func<int, string, int> damageModifier;
    private Func<int, string, int> healModifier;
    public event Func<int, string, int> DamageModifier { add => damageModifier = value; remove => damageModifier = null; }
    public event Func<int, string, int> HealModifier { add => healModifier = value; remove => healModifier = null; }

    private Action<int, string> onDamaged;
    private Action<int, string> onHealed;
    private Action<int, string> onOverDamaged;
    private Action<int, string> onOverHealed;
    public event Action<int, string> OnDamaged { add => onDamaged += value; remove => onDamaged -= value; }
    public event Action<int, string> OnHealed { add => onHealed += value; remove => onHealed -= value; }
    public event Action<int, string> OnOverDamaged { add => onOverDamaged += value; remove => onOverDamaged -= value; }
    public event Action<int, string> OnOverHealed { add => onOverHealed += value; remove => onOverHealed -= value; }

    private Action _onZeroHealth;
    public event Action OnZeroHealth { add => _onZeroHealth += value; remove => _onZeroHealth -= value; }
    protected override void ResetSetup() {
        RemoveAllListeners();
        health.NewMinValue(0);
        NewMaxHealth(startMaxHealth);
        health.EqualTo(startHealth);
        health.OnDecreasing += (value) => onDamaged?.Invoke(value, damageList[^1]);
        health.OnIncreasing += (value) => onHealed?.Invoke(value, damageList[^1]);
        health.OnOverDecreased += (value) => onOverDamaged?.Invoke(value, damageList[^1]);
        health.OnOverIncreased += (value) => onOverHealed?.Invoke(value, damageList[^1]);
    }

    protected override void LoadSetup() {
        OnZeroHealth += Die;
        OnZeroHealth += RemoveAllListeners;
        if(GetRootComponent<EntityHealthModfier>() == null) {
            gameObject.AddComponent<EntityHealthModfier>();
        }
    }


    public int AddHealth(int heal, string type = "") {
        if (healModifier != null) {
            heal = healModifier(heal, type);
        }

        if (heal <= 0) {
            return Health;
        }

        healList.Add(type);
        return health.IncreaseOf(heal);
    }

    public int RemoveHealth(int damage, string type = "") {
        if (damageModifier != null) {
            damage = damageModifier(damage, type);
        }

        if(damage <= 0) { 
            return Health;
        }

        damageList.Add(type);
        health.DecreaseOf(damage);
        if (Health == 0) { _onZeroHealth?.Invoke(); }
        return Health;
    }

    public void NewMaxHealth(int newMaxHealth) {
        newMaxHealth = Mathf.Max(0, newMaxHealth);
        health.NewMaxValue(newMaxHealth);
    }

    public int GetPercentHealth(int percent) {
        return health.GetPercentRange(percent);
    }

    public void RemoveAllListeners() {
        health.RemoveAllListeners();
        _onZeroHealth = null;
    }
}
