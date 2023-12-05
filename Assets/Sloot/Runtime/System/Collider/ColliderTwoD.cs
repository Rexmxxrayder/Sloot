using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider2D))]
public class ColliderTwoD : MonoBehaviour {

    List<Collision2D> _contacts = new List<Collision2D>();
    List<Collider2D> _colliders = new List<Collider2D>();
    public List<Collision2D> GetContacts { get { return _contacts; } }
    public List<Collider2D> GetColliders { get { return _colliders; } }

    [SerializeField] bool _debug = false;
    [SerializeField] bool _enable = true;

    [SerializeField] UnityEvent<Collision2D> _onCollisionEnter;
    [SerializeField] UnityEvent<Collision2D> _onCollisionExit;
    [SerializeField] UnityEvent<Collider2D> _onTriggerEnter;
    [SerializeField] UnityEvent<Collider2D> _onTriggerExit;

    public event UnityAction<Collision2D> OnCollisionEnter { add => _onCollisionEnter.AddListener(value); remove => _onCollisionEnter.RemoveListener(value); }
    public event UnityAction<Collision2D> OnCollisionExit { add => _onCollisionExit.AddListener(value); remove => _onCollisionExit.RemoveListener(value); }
    public event UnityAction<Collider2D> OnTriggerEnter { add => _onTriggerEnter.AddListener(value); remove => _onTriggerEnter.RemoveListener(value); }
    public event UnityAction<Collider2D> OnTriggerExit { add => _onTriggerExit.AddListener(value); remove => _onTriggerExit.RemoveListener(value); }

    public bool Contact { get { return _contacts.Count == 0 ? false : true; } }
    public bool Enable { get => _enable; set => _enable = value; }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (!_enable) { return; }
        if (_debug) { Debug.Log("Collision Enter " + collision.gameObject.name); }
        _contacts.Add(collision);
        _onCollisionEnter?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (!_enable) { return; }
        if (_debug) { Debug.Log("Collision Exit " + collision.gameObject.name); }
        _contacts.Add(collision);
        _onCollisionExit?.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D Collider) {
        if (!_enable) { return; }
        if (_debug) { Debug.Log("Trigger Enter " + Collider.gameObject.name); }
        _colliders.Add(Collider);
        _onTriggerEnter?.Invoke(Collider);
    }

    private void OnTriggerExit2D(Collider2D Collider) {
        if (!_enable) { return; }
        if (_debug) { Debug.Log("Trigger Exit " + Collider.gameObject.name); }
        _colliders.Add(Collider);
        _onTriggerExit?.Invoke(Collider);
    }
}
