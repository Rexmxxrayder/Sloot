using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sloot;
using System.Reflection;
using System.Net.Mail;

public class EntityPhysics : EntityComponent {
    public enum PhysicPriority {
        INPUT = 1, DASH = 2, BLOCK = 3, PROJECTION = 4, ENVIRONNEMENT = 5, SYSTEM = 6
    }

    [SerializeField] Rigidbody _rb;
    [SerializeField] Vector3 _velocity;
    [SerializeField] List<Force> _forcesDisplay = new ();
    [SerializeField] bool _debug = false;

    SortedDictionary<int, List<Force>> _forces = new ();

    public Vector3 Velocity => _velocity;
    public Vector3 Direction => _velocity.normalized;


    private void FixedUpdate() {
        _velocity = ComputeForces();
        _rb.velocity = Velocity;
    }

    #region EntityComponentFunctions
    protected override void DefinitiveSetup() {
        if (_rb == null) {
            if (root.GetComponent<Rigidbody>() == null) {
                _rb = GetRootGameObject().AddComponent<Rigidbody>();
            } else {
                _rb = root.GetComponent<Rigidbody>();
            }
        }
        _rb.useGravity = false;
        _rb.freezeRotation = true;
        Purge();
    }

    #endregion

    #region Get/Set

    public void Add(Force force, PhysicPriority forcePriority, bool CrushEqualAndUnderForces = true) {
        if (!_forces.ContainsKey((int)forcePriority)) { _forces.Add((int)forcePriority, new List<Force>()); }
        if (CrushEqualAndUnderForces) {
            for (int priority = (int)forcePriority; priority > 0; --priority) {
                if (!_forces.ContainsKey(priority)) { continue; }
                for (int index = 0; index < _forces[priority].Count; ++index) {
                    if (_forces[priority][index] == null) { continue; }
                    if (_forces[priority][index].Ignored) { continue; }
                    _forces[priority][index].End();
                }
            }
        }
        _forcesDisplay.Add(force);
        _forces[(int)forcePriority].Add(force);
    }


    public void Remove(Force force) {
        try {
            foreach (KeyValuePair<int, List<Force>> pair in _forces) {
                if (pair.Value.Contains(force)) {
                    Remove(force, pair.Key);
                    _forcesDisplay.Remove(force);
                }
            }
        } catch (System.Exception e) {
            if (_debug)
                Debug.LogError(e);
        }
    }

    public void Remove(Force force, int priority) {
        if (!_forces.ContainsKey(priority)) { return; }
        if (!_forces[priority].Contains(force)) { return; }

        _forces[priority].Remove(force);
        _forcesDisplay.Remove(force);

        if (_forces[priority].Count <= 0) { _forces.Remove(priority); }
    }

    #endregion

    #region ForcesFunction

    public Vector3 ComputeForces() {
        Vector3 force = Vector3.zero;

        if (_forces.Count == 0) {
            return force;
        }

        float weight = 1f;

        int maxPriority = _forces.Keys.Last();
        int minPriority = _forces.Keys.First();

        try {
            for (int priority = maxPriority; priority >= minPriority; --priority) {
                if (!_forces.ContainsKey(priority)) { continue; }
                float maxWeightTaken = 0;

                for (int index = 0; index < _forces[priority].Count; ++index) {
                    if (_forces[priority][index] == null) { continue; }
                    if (_forces[priority][index].Ignored) { continue; }

                    _forces[priority][index].Update(Time.fixedDeltaTime);
                    float currentWeightTaken = Mathf.Min(weight, _forces[priority][index].Weight);
                    force += _forces[priority][index].Evaluate() * currentWeightTaken;
                    maxWeightTaken = Mathf.Max(currentWeightTaken, maxWeightTaken);
                    if (_forces[priority][index].HasEnded) {
                        Remove(_forces[priority][index], priority);
                        continue;
                    }
                }

                weight -= maxWeightTaken;
                weight = Mathf.Max(0, weight);
            }

        } catch (KeyNotFoundException k) {
            if (_debug) { Debug.LogWarning("Happen : " + k); }
        } catch (Exception e) {
            Debug.LogError(e);
        }

        return force;
    }
    public void ModifyForces(Func<Force, Force> func, PhysicPriority minPriority = PhysicPriority.INPUT, PhysicPriority maxPriority = PhysicPriority.SYSTEM) {
        for (int priority = (int)maxPriority; priority >= (int)minPriority; --priority) {
            if (!_forces.ContainsKey(priority)) { continue; }
            for (int index = 0; index < _forces[priority].Count; ++index) {
                if (_forces[priority][index] == null) { continue; }
                if (_forces[priority][index].Ignored) { continue; }
                _forces[priority][index] = func(_forces[priority][index]);
            }
        }
    }

    public void Purge() {
        _rb.velocity = Vector3.zero;
        try {
            foreach (KeyValuePair<int, List<Force>> key in _forces) {
                for (int i = 0; i < key.Value.Count; i++) {
                    key.Value[i].End();
                }
            }
        } catch (KeyNotFoundException k) {
            if (_debug) { Debug.LogWarning("Happen : " + k); }
        } catch (System.Exception e) {
            Debug.LogError(e);
        }
    }
    #endregion

}
