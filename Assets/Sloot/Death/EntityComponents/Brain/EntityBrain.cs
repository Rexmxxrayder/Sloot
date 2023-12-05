using System;
using UnityEngine;

public abstract class EntityBrain : EntityComponent {
    [SerializeField] protected Vector3 visor;
    [SerializeField] protected Transform selected;
    private Token actionsBlocked = new();

    #region Properties
    public Vector3 Visor { get { return visor; } }
    public Transform Selected { get { return selected; } }
    public bool CanAct {
        get => actionsBlocked.Current == 0; 
        set {
            if (value) {
                actionsBlocked.RemoveToken();
            } else {
                actionsBlocked.AddToken();
            }
        }
    }
    #endregion

    private Action onCanActAgain;
    private Action onCannotAct;
    public event Action OnCanActAgain { add { onCanActAgain += value; } remove { onCanActAgain -= value; } }
    public event Action OnCannotAct { add { onCannotAct += value; } remove { onCannotAct -= value; } }

    public override EntityRoot SetRoot() {
        if (_root == null) {
            if (TryGetComponent(out EntityRoot root)) {
                _root = root;
            } else {
                _root = gameObject.AddComponent<EntityRoot>().GetRoot();
            }
        }

        return _root;
    }

    protected override void DefinitiveSetup() {
        actionsBlocked.OnZeroToken += () => onCanActAgain?.Invoke();
        actionsBlocked.OnNotZeroToken += () => onCannotAct?.Invoke();
    }

    protected override void LoadSetup() {
        foreach (EntityBodyPart ebp in GetComponentsInChildren<EntityBodyPart>()) {
            ebp.AssignBrain(this);
        }
    }
}
