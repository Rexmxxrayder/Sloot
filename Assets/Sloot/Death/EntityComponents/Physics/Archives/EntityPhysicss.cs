//using System.Linq;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class EntityPhysicss : MonoBehaviour, IEntityAbility {
//    [SerializeField] Rigidbody2D _rb;
//    [SerializeField] List<Force> _forcesDisplay = new List<Force>();
//    [SerializeField] bool _debug = false;
//    [SerializeField, HideInInspector] BetterEvent<Vector2> _onMove = new BetterEvent<Vector2>();

//    SortedDictionary<int, List<Force>> _forces = new SortedDictionary<int, List<Force>>();

//    public Vector2 Velocity => _rb.velocity;
//    public event Action<Vector2> OnMove { add => _onMove += value; remove => _onMove -= value; }

//    private void FixedUpdate() {
//        Vector2 velocity = ComputeForces();
//        if (velocity != Vector2.zero) { _onMove.Invoke(velocity); }
//        _rb.velocity = velocity;
//        foreach (KeyValuePair<int, List<Force>> pair in _forces) {
//            for (int i = 0; i < pair.Value.Count; i++) {
//                if (pair.Value[i].Ignored) { continue; }
//                pair.Value[i].Update(Time.fixedDeltaTime);
//            }
//        }
//    }

//    #region Get/Set

//    public Force Add(float strength, Vector2 direction, int priority) {
//        return Add(strength, direction, 1f, Force.ForceMode.TIMED, AnimationCurve.Linear(0f, 0f, 1f, 1f), AnimationCurve.Linear(1f, 1f, 0f, 0f), priority);
//    }

//    public Force Add(float strength, Vector2 direction, float weight, int priority) {
//        return Add(strength, direction, weight, Force.ForceMode.TIMED, AnimationCurve.Linear(0f, 0f, 1f, 1f), AnimationCurve.Linear(1f, 1f, 0f, 0f), priority);
//    }

//    public Force Add(float strength, Vector2 direction, float weight, Force.ForceMode mode, int priority) {
//        return Add(strength, direction, weight, mode, AnimationCurve.Linear(0f, 0f, 1f, 1f), AnimationCurve.Linear(1f, 1f, 0f, 0f), priority);
//    }

//    public Force Add(float strength, Vector2 direction, float weight, Force.ForceMode mode, AnimationCurve start, AnimationCurve end, int priority) {
//        return Add(strength, direction, weight, mode, AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, AnimationCurve.Linear(1f, 1f, 0f, 0f), 1f, priority);
//    }

//    public Force Add(float strength, Vector2 direction, float weight, Force.ForceMode mode, AnimationCurve start, float startTime, AnimationCurve end, float endTime, int priority) {
//        Force force = new Force(strength, direction, weight, mode, start, startTime, end, endTime);
//        if (!_forces.ContainsKey(priority)) { _forces.Add(priority, new List<Force>()); }
//        _forces[priority].Add(force);
//        _forcesDisplay.Add(force);
//        return force;
//    }

//    public void Add(Force force, int priority) {
//        if (!_forces.ContainsKey(priority)) { _forces.Add(priority, new List<Force>()); }
//        _forcesDisplay.Add(force);
//        _forces[priority].Add(force);
//    }

//    public void Remove(Force force) {
//        try {
//            foreach (KeyValuePair<int, List<Force>> pair in _forces) {
//                if (pair.Value.Contains(force)) {
//                    Remove(force, pair.Key);
//                    _forcesDisplay.Remove(force);
//                }
//            }
//        } catch (System.Exception e) {
//            if(_debug)
//            Debug.LogError(e);
//        }
//    }

//    public void Remove(Force force, int priority) {
//        if (!_forces.ContainsKey(priority)) { return; }
//        if (!_forces[priority].Contains(force)) { return; }

//        _forces[priority].Remove(force);
//        _forcesDisplay.Remove(force);

//        if (_forces[priority].Count <= 0) { _forces.Remove(priority); }
//    }

//    #endregion

//    public Vector2 ComputeForces() {
//        Vector2 force = Vector2.zero;

//        if (_forces.Count == 0) {
//            return force;
//        }

//        float weight = 1f;

//        int maxPriority = _forces.Keys.Last();
//        int minPriority = _forces.Keys.First();

//        try {
//            for (int priority = maxPriority; priority >= minPriority; --priority) {
//                if (!_forces.ContainsKey(priority)) { continue; }

//                int count = 0;
//                for (int index = 0; index < _forces[priority].Count; index++) {
//                    if (!_forces[priority][index].Ignored) {
//                        ++count;
//                    }
//                }

//                float sliceWeight = weight / count;
//                for (int index = 0; index < _forces[priority].Count; ++index) {
//                    if (_forces[priority][index] == null) { continue; }
//                    if (_forces[priority][index].Ignored) { continue; }
//                    // Les inputs ne peuvent End et pique du poids, ces enculay (Do not forget)
//                    if (_forces[priority][index].Mode != Force.ForceMode.INPUT && _forces[priority][index].HasEnded) {
//                        Remove(_forces[priority][index], priority);
//                        continue;
//                    }

//                    float currentWeight = _forces[priority][index].Weight * sliceWeight;
//                    weight -= currentWeight;
//                    force += _forces[priority][index].Evaluate() * currentWeight;
//                }
//            }
//        } catch (KeyNotFoundException k) {
//            if (_debug) { Debug.LogWarning("Happen : " + k); }
//        } catch (System.Exception e) {
//            Debug.LogError(e);
//        }

//        return force;
//    }

//    public void Purge() {
//        _rb.velocity = Vector2.zero;
//        _forcesDisplay.Clear();
//        _forces.Clear();
//    }

//    public void Terminate() {
//        _rb.velocity = Vector2.zero;
//        try {
//            foreach (KeyValuePair<int, List<Force>> key in _forces) {
//                for (int i = 0; i < key.Value.Count; i++) {
//                    key.Value[i].End();
//                }
//            }
//        } catch(KeyNotFoundException k) {
//            if (_debug) { Debug.LogWarning("Happen : " + k); }
//        } catch (System.Exception e) {
//            Debug.LogError(e);
//        }
//    }
//}