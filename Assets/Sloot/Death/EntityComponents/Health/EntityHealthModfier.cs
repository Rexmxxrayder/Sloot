using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealthModfier : EntityComponent
{
    private ValueModifierContainer<IDamageModifier, int> damageModifier = new();
    private ValueModifierContainer<IHealModifier, int> healModifier = new();
    private EntityHealth entityHealth;
    protected override void LoadSetup() {
        entityHealth = RootGet<EntityHealth>();
        if(entityHealth != null ) {
            entityHealth.DamageModifier += damageModifier;
            entityHealth.HealModifier += healModifier;
        }
    }    

    public void AddModifier(IHealthModifier modifier) {
        if(modifier is IDamageModifier) {
            damageModifier.AddValueModifier((IDamageModifier)modifier);
        }else if(modifier is IHealModifier) {
            healModifier.AddValueModifier((IHealModifier)modifier);
        }
    }

    public void RemoveModifier(IHealthModifier modifier) {
        if (modifier is IDamageModifier) {
            damageModifier.RemoveValueModifier((IDamageModifier)modifier);
        } else if (modifier is IHealModifier) {
            healModifier.RemoveValueModifier((IHealModifier)modifier);
        }
    }
}
