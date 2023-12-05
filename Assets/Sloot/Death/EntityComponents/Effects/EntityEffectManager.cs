using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static EntityEffect;

public class EntityEffectManager : EntityComponent {
    [SerializeField] public List<string> effectsVisualise = new();
    private List<EntityEffect> effects = new();
    private Action<EntityEffect> onEffectAdd;
    private Action<EntityEffect> onEffectWantAdd;
    private Action<EntityEffect> onEffectRemove ;
    private Action<EntityEffect> onEffectWantRemove;
    public event Action<EntityEffect> OnEffectAdd { add { onEffectAdd += value; } remove { onEffectAdd -= value; } }
    public event Action<EntityEffect> OnEffectWantAdd { add { onEffectWantAdd += value; } remove { onEffectWantAdd -= value; } }
    public event Action<EntityEffect> OnEffectRemove { add { onEffectRemove += value; } remove { onEffectRemove -= value; } }
    public event Action<EntityEffect> OnEffectWantRemove { add { onEffectWantRemove += value; } remove { onEffectWantRemove -= value; } }
    public List<EntityEffect> Effects => effects;

    public void AddEffect(EntityEffect effect) {
        onEffectWantAdd?.Invoke(effect);
        if(effect.Negate) {
            return;
        }

        effects.Add(effect);
        effect.OnEndEffect += OnEndEffect;
        effect.SetupEffect(this);
        onEffectAdd?.Invoke(effect);
    }

    public void FixedUpdate() {
        List<EntityEffect> temporaryEffects = effects;
        for (int i = 0; i < temporaryEffects.Count; i++) {
            effects[i].UpdateEffect(Time.fixedDeltaTime);
        }

        effectsVisualise.Clear();
        foreach (var effect in effects) {
            effectsVisualise.Add($"{effect.Type} {effect.Stack} {effect.TimeRemaining} {effect.CurrentDuration}");
        }
    }
    

    private void OnEndEffect(EntityEffect effect) {
        effects.Remove(effect);
        onEffectRemove?.Invoke(effect);
    }

    public void RemoveEffect(EntityEffect effect) {
        if (effect.Negate) {
            return;
        }

        effect.Negate = true;
        onEffectWantRemove?.Invoke(effect);
        if (!effect.Negate) {
            return;
        }

        effect.CancelEffect();
    }


    public void RemoveEffect(EffectType type) {
        EntityEffect entityEffect = null;
        foreach(var effect in effects) {
            if(effect.Type == type) {
                entityEffect = effect;
                break;
            }
        }

        if (entityEffect != null) {
            RemoveEffect(entityEffect);
        }
    }

    public bool Contains(EffectType type) {
        foreach (var effect in effects) {
            if (effect.Type == type) {
               return true;
            }
        }

        return false;
    }

    public bool Contains(List<EffectType> type) {
        foreach (var effect in effects) {
            if (type.Contains(effect.Type)) {
                return true;
            }
        }

        return false;
    }

    public EntityEffect Get(EffectType type) {
        foreach (var effect in effects) {
            if (effect.Type == type) {
                return effect;
            }
        }

        return null;
    }
}
