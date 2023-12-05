using System;
using UnityEngine;

[System.Serializable]
public abstract class EntityEffect {
    public enum EffectType {
        ICE,
        POWDER,
        CLEANSE,
        FREEZE,
        FOCUS,
        EMERALD,
        INSANITY,
        MADNESS,
        NONE
    }

    public abstract EffectType Type {
        get;
    }

    protected EntityEffectManager entityEffectManager;
    protected float Currentduration = - 1;
    protected float elapsedTime = 0;
    protected bool end = false;
    protected bool cancel = false;
    protected bool negate = false;
    [SerializeField] protected int stack = 1;
    public abstract int MaxStack {
        get;
    }
    public abstract float OfficialDuration {
        get;
    }

    public int Stack => stack;
    public float CurrentDuration { get => Currentduration; set => Currentduration = value; }
    public float ElapsedTime => elapsedTime;
    public float TimeRemaining => Currentduration - elapsedTime;
    public bool IsEnd => end;
    public bool Negate { get => negate; set => negate = value; }

    private Action<EntityEffect> onEndEffect;
    public event Action<EntityEffect> OnEndEffect { add { onEndEffect += value; } remove { onEndEffect -= value; } }

    public virtual void SetupEffect(EntityEffectManager effectManager) {
        if (Currentduration == -1) {
            Currentduration = OfficialDuration;
        }
        entityEffectManager = effectManager;
        entityEffectManager.OnEffectAdd += EffectAdd;
        entityEffectManager.OnEffectRemove += EffectRemove;
        entityEffectManager.OnEffectWantAdd += EffectTryingAdd;
        entityEffectManager.OnEffectWantRemove += EffectTryingRemove;
    }

    public virtual void UpdateEffect(float deltaTime) {
        elapsedTime += deltaTime;
        if (elapsedTime >= Currentduration) {
            EndEffect();
        }
    }

    public virtual bool EndEffect() {
        if (end) {
            return false;
        }

        end = true;
        entityEffectManager.OnEffectAdd -= EffectAdd;
        entityEffectManager.OnEffectRemove -= EffectRemove;
        entityEffectManager.OnEffectWantAdd -= EffectTryingAdd;
        entityEffectManager.OnEffectWantRemove -= EffectTryingRemove;
        onEndEffect?.Invoke(this);
        return !cancel;
    }

    public virtual void CancelEffect() {
        if(cancel) {
            return;
        }

        cancel = true;
        EndEffect();
    }

    protected virtual void EffectAdd(EntityEffect newEffect) {
        if (newEffect == this) {
            return;
        }
    }

    protected virtual void EffectTryingAdd(EntityEffect newEffect) {
        if (newEffect == this) {
            return;
        }

        if (newEffect.Type == Type) {
            AddStack(newEffect.Stack);
            newEffect.Negate = true;
        }
    }

    protected virtual void EffectRemove(EntityEffect effect) {
        if (effect == this) {
            return;
        }
    }

    protected virtual void EffectTryingRemove(EntityEffect newEffect) {
        if (newEffect == this) {
            return;
        }
    }

    public virtual void AddStack(int stackNumber) {
        stackNumber = Mathf.Clamp(stackNumber, 0, MaxStack);
        stack += stackNumber;
        if (stack > MaxStack) {
            stack = MaxStack;
        }
    }

    public virtual void RemoveStack(int stackNumber) {
        stackNumber = Mathf.Clamp(stackNumber, 0, MaxStack);
        stack += stackNumber;
        if (stack <= 0) {
            stack = MaxStack;
        }
    }
}
