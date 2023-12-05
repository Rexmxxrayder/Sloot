using System;
using UnityEngine.Events;

public abstract class EntityCollider<T,U> : EntityComponent {
    protected bool isActive = true;
    public virtual bool IsActive {
        get { return isActive; }
        set {
            isActive = value;
        }
    }

    protected override void ResetSetup() {
        ResetListeners();
    }

    #region Events
    protected Action<T> _onCollisionEnterDelegate;
    protected Action<T> _onCollisionExitDelegate;
    protected Action<T> _onCollisionStayDelegate;
    protected Action<U> _onTriggerEnterDelegate;
    protected Action<U> _onTriggerExitDelegate;
    protected Action<U> _onTriggerStayDelegate;

    public event Action<T> OnCollisionEnterDelegate { add => _onCollisionEnterDelegate += value; remove => _onCollisionEnterDelegate -= value; }
    public event Action<T> OnCollisionExitDelegate { add => _onCollisionExitDelegate += value; remove => _onCollisionExitDelegate -= value; }
    public event Action<T> OnCollisionStayDelegate { add => _onCollisionStayDelegate += value; remove => _onCollisionStayDelegate -= value; }
    public event Action<U> OnTriggerEnterDelegate { add => _onTriggerEnterDelegate += value; remove => _onTriggerEnterDelegate -= value; }
    public event Action<U> OnTriggerExitDelegate { add => _onTriggerExitDelegate += value; remove => _onTriggerExitDelegate -= value; }
    public event Action<U> OnTriggerStayDelegate { add => _onTriggerStayDelegate += value; remove => _onTriggerStayDelegate -= value; }
    #endregion

    protected void AssignTo(EntityCollider<T,U> entityCollider) {
        entityCollider.OnCollisionEnterDelegate += (x) => _onCollisionEnterDelegate?.Invoke(x);
        entityCollider.OnCollisionExitDelegate += (x) => _onCollisionExitDelegate?.Invoke(x);
        entityCollider.OnCollisionStayDelegate += (x) => _onCollisionStayDelegate?.Invoke(x);
        entityCollider.OnTriggerEnterDelegate += (x) => _onTriggerEnterDelegate?.Invoke(x);
        entityCollider.OnTriggerExitDelegate += (x) => _onTriggerExitDelegate?.Invoke(x);
        entityCollider.OnTriggerStayDelegate += (x) => _onTriggerStayDelegate?.Invoke(x);
    }

    public void ResetListeners() {
        _onCollisionEnterDelegate = null;
        _onCollisionExitDelegate = null;
        _onCollisionStayDelegate = null;
        _onTriggerEnterDelegate= null;
        _onTriggerExitDelegate= null;
        _onTriggerStayDelegate = null;
    }
}
