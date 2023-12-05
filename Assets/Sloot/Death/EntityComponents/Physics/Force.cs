
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Force {
    [SerializeField] Vector3 _direction = Vector3.zero;
    [SerializeField, Range(0f, 1f)] float _weight = 1f;
    [SerializeField] List<AnimationCurve> _curves = new();

    [SerializeField] int _currentCurve = 0;
    [SerializeField] float _currentPercent = 0f;
    bool _ignored = false;
    bool _hasEnded = false;

    [SerializeField] private Action<Force> _onStart;
    [SerializeField] private Action<Force, int> _onNewCurve;
    [SerializeField] private Action<Force> _onEnd;

    #region Properties

    public Vector3 Direction { get => _direction; set => _direction = value.normalized; }
    public float CurrentStrength { get => Evaluate().magnitude; }
    public float Weight { get => _weight; set => _weight = value; }
    public bool Ignored { get => _ignored; set => _ignored = value; }
    public bool HasEnded { get => _hasEnded; }
    public float CurrentCurveDuration { get => _curves[_currentCurve].GetDuration(); set => _curves[_currentCurve].ChangeDuration(value); }
    public List<AnimationCurve> Curves { get => _curves; set => _curves = value; }

    #endregion

    #region Events

    public event Action<Force> OnStart { add => _onStart += value; remove => _onStart -= value; }
    public event Action<Force, int> OnNewCurve { add => _onNewCurve += value; remove => _onNewCurve -= value; }
    public event Action<Force> OnEnd { add => _onEnd += value; remove => _onEnd -= value; }

    #endregion

    #region Constructors

    public Force(List<AnimationCurve> curves, Vector3 direction, float weight = 1f) {
        _direction = direction.normalized;
        _weight = Mathf.Clamp(weight, 0f, 1f);
        _curves = curves;
    }

    public Force(AnimationCurve curve, Vector3 direction, float weight = 1f) :
        this(new List<AnimationCurve> { curve }, direction, weight) {
    }

    public Force(Force force) :
        this(force._curves, force._direction, force._weight) {
    }

    public static Force Const(Vector3 direction, float strength, float duration = 1f, float weight = 1f) {
        return new Force(AnimationCurve.Constant(0f, duration, strength), direction, weight);
    }

    public static Force LinearUp(Vector3 direction, float strength, float duration = 1f, float weight = 1f) {
        return new Force(AnimationCurve.Linear(0f, 0f, duration, strength), direction, weight); ;
    }

    public static Force LinearDown(Vector3 direction, float strength, float duration = 1f, float weight = 1f) {
        return new Force(AnimationCurve.Linear(0f, strength, duration, 0f), direction, weight);
    }

    public static Force LinearTriangleUp(Vector3 direction, float strength, float duration = 1f, float weight = 1f) {
        return new Force(new List<AnimationCurve> {
            AnimationCurve.Linear(0f, 0f, duration / 2f, strength),
            AnimationCurve.Linear(0f, strength, duration / 2f, 0f)},
            direction, weight);
    }

    public static Force LinearTriangleDown(Vector3 direction, float strength, float duration = 1f, float weight = 1f) {
        return new Force(new List<AnimationCurve> {
            AnimationCurve.Linear(0f, strength, duration / 2f, 0f),
            AnimationCurve.Linear(0f, 0f, duration / 2f, strength) },
            direction, weight);
    }

    public static Force Empty() {
        return new Force(AnimationCurve.Constant(0f, 0f, 0f), Vector3.zero, 1f);
    }

    #endregion

    public void AddCurves(List<AnimationCurve> curves) {
        _curves.AddRange(curves);
    }

    public void AddCurve(AnimationCurve curve) {
        AddCurves(new List<AnimationCurve> { curve });
    }

    public void AddCurve(Force force) {
        AddCurves(force._curves);
    }

    public Vector3 Evaluate() {
        return Evaluate(_currentPercent);
    }

    public Vector3 Evaluate(float percent) {
        Mathf.Clamp(0f, 1f, percent);
        return _direction * _curves[_currentCurve].Evaluate(percent);
    }

    public void Update(float deltaTime) {
        if (_currentPercent == 0f && _currentCurve == 0f) {
            _onStart?.Invoke(this);
        }

        _currentPercent += deltaTime / _curves[_currentCurve].GetDuration();
        if (_currentPercent >= 1f && !_hasEnded) {
            NextCurve(_currentPercent - 1f);
        }
    }

    public void Reset() {
        _currentPercent = 0f;
        _currentCurve = 0;
    }

    public void ResetCurve() {
        _currentPercent = 0f;
    }

    public void End() {
        _currentPercent = 1f;
        _currentCurve = _curves.Count - 1;
        _hasEnded = true;
        _onEnd?.Invoke(this);
    }

    public void NextCurve(float newPercent = 0f) {
        _currentPercent = newPercent;
        _currentCurve++;
        if (_currentCurve >= _curves.Count) {
            End();
        } else {
            _onNewCurve?.Invoke(this, _currentCurve);
        }
    }
}



