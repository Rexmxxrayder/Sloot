//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public enum PhysicPriority {
//    PLAYER_INPUT = 0, DASH = 5, PROJECTION = 10, BLOCK = 20, HIGH_PRIORITY = 50
//}

////public class EntityPhysicMovement : MonoBehaviour, IEntityAbility {
//[SerializeField] EntityPhysics _entityPhysics;

//[Header("Movements")]
//[SerializeField] float _maxSpeed = 10f;
//[SerializeField] AnimationCurve _acceleration = AnimationCurve.Linear(0f, 0f, 1f, 1f);
//[SerializeField] float _accelerationTime = 1f;
//[SerializeField] AnimationCurve _deceleration = AnimationCurve.Linear(0f, 1f, 1f, 0f);
//[SerializeField] float _decelerationTime = 1f;
//[SerializeField, HideInInspector] BetterEvent _trapped = new BetterEvent();
//[SerializeField, HideInInspector] BetterEvent _untrapped = new BetterEvent();

//public event Action Trapped { add => _trapped += value; remove => _trapped -= value; }
//public event Action UnTrapped { add => _untrapped += value; remove => _untrapped -= value; }
//public void InTrap() {
//    _trapped?.Invoke();
//}

//public void OutTrap() {
//    _untrapped?.Invoke();
//}
//Token _cantMoveToken = new Token();
//List<float> _speedModifiers = new List<float>();

//Vector2 _orientation = Vector2.zero;
//Vector2 _direction = Vector2.zero;
//Force _forceMovement;

//public bool CanMove { get => !_cantMoveToken.HasToken; set => _cantMoveToken.AddToken(!value); }
//public bool Moving => _direction != Vector2.zero;

//private void Reset() {
//    _entityPhysics = GetComponent<EntityPhysics>();
//}

//private void Start() {
//    _cantMoveToken.OnFill += _DontMove;

//    _forceMovement = _entityPhysics.Add(
//        _maxSpeed, Vector2.zero, 1f,
//        Force.ForceMode.INPUT,
//        _acceleration, _accelerationTime,
//        _deceleration, _decelerationTime,
//        (int)PhysicPriority.PLAYER_INPUT);
//}

//public void Move(Vector2 direction) {
//    if (!CanMove) { return; }
//    MoveDirection(direction);
//}

//public void Stop() {
//    _forceMovement.Reset();
//    _DontMove();
//    _orientation = Vector2.zero;
//}

//private void MoveDirection(Vector2 direction) {
//    if (_forceMovement == null) { return; }
//    if (_forceMovement.State == Force.ForceState.DECELERATION && direction != Vector2.zero) {
//        _forceMovement.ChangeState(Force.ForceState.ACCELERATION);
//    } else if (_forceMovement.State != Force.ForceState.DECELERATION && direction == Vector2.zero) {
//        _forceMovement.ChangeState(Force.ForceState.DECELERATION);
//    }

//    _forceMovement.Direction = direction != Vector2.zero ? direction : _orientation;

//    if (direction != Vector2.zero) {
//        _orientation = direction;
//    }
//    _direction = direction;
//}

//private void _DontMove() {
//    MoveDirection(Vector2.zero);
//}

//public void AddSpeedModifier(float modifier) {
//    _speedModifiers.Add(modifier);
//    _forceMovement.Strength = ComputeSpeed();
//}

//public void RemoveSpeedModifier(float modifier) {
//    _speedModifiers.Remove(modifier);
//    _forceMovement.Strength = ComputeSpeed();
//}

//private float ComputeSpeed() {
//    float modifier = 1f;
//    for (int i = 0; i < _speedModifiers.Count; i++) {
//        modifier *= _speedModifiers[i];
//    }
//    return _maxSpeed * modifier;
//}

//public void ClearCantMoveToken() {
//    _cantMoveToken.Reset();
//}
//}
