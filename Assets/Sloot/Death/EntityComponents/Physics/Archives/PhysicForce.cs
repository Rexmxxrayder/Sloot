//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class PhysicForce : MonoBehaviour {
//    [SerializeField] EntityPhysics _entityPhysics;

//    [Header("Movements")]
//    [SerializeField] float _maxSpeed = 10f;
//    [SerializeField] AnimationCurve _acceleration = AnimationCurve.Linear(0f, 0f, 1f, 1f);
//    [SerializeField] float _accelerationTime = 1f;
//    [SerializeField] AnimationCurve _deceleration = AnimationCurve.Linear(0f, 1f, 1f, 0f);
//    [SerializeField] float _decelerationTime = 1f;

//    [SerializeField] BetterEvent _onStart = new BetterEvent();
//    [SerializeField] BetterEvent _onEnd = new BetterEvent();

//    Force _forceMovement;
//    bool _inUse = false;

//    public bool InUse => _inUse;

//    public event Action OnStart { add => _onStart += value; remove => _onStart -= value; }
//    public event Action OnEnd { add => _onEnd += value; remove => _onEnd -= value; }

//    private void Reset() {
//        _entityPhysics = GetComponent<EntityPhysics>();
//    }

//    void Start() {
//        _forceMovement = new Force(
//            _maxSpeed, Vector2.zero, 1f,
//            Force.ForceMode.TIMED,
//            _acceleration, _accelerationTime,
//            _deceleration, _decelerationTime);

//        _forceMovement.OnStart += _OnStart;
//        _forceMovement.OnEnd += _OnEnd;
//    }

//    public void Use(Vector2 direction, int priority) {
//        if (!_inUse) {
//            _forceMovement.Reset();
//            _forceMovement.Direction = direction;
//            _entityPhysics.Add(_forceMovement, priority);
//        } else {
//            _forceMovement.Reset();
//        }
//    }

//    private void _OnStart(Force force) {
//        _inUse = true;
//        _onStart.Invoke();
//    }

//    private void _OnEnd(Force force) {
//        _inUse = false;
//        _onEnd.Invoke();
//    }
//}
