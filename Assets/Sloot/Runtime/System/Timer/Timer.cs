using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Sloot {
    public class Timer {
        public enum TimerState {
            RUNNING,
            RUNNINGOFFSET,
            PAUSED,
            WAITING
        }

        float _duration = 1f;
        float _currentDuration = 0f;
        bool _loop = true;

        TimerState state = TimerState.WAITING;

        MonoBehaviour manager;
        Coroutine coroutine;

        public float Duration { get => _duration; set => _duration = value; }
        public float CurrentDuration => _currentDuration;
        public bool IsLooping => _loop;

        [SerializeField] private Action _onActivate;
        [SerializeField] private Action _onStart;
        [SerializeField] private Action _onEndOffset;
        [SerializeField] private Action _onPause;
        [SerializeField] private Action _onContinue;
        [SerializeField] private Action _onStop;
        [SerializeField] private Action _onEnd;
        [SerializeField] private Action _onReset;

        public event Action OnActivate { add => _onActivate += value; remove => _onActivate -= value; }
        public event Action OnStart { add => _onStart += value; remove => _onStart -= value; }
        public event Action OnEndOffset { add => _onEndOffset += value; remove => _onEndOffset -= value; }
        public event Action OnPause { add => _onPause += value; remove => _onPause -= value; }
        public event Action OnContinue { add => _onContinue += value; remove => _onContinue -= value; }
        public event Action OnStop { add => _onStop += value; remove => _onStop -= value; }
        public event Action OnEnd { add => _onEnd += value; remove => _onEnd -= value; }
        public event Action OnReset { add => _onReset += value; remove => _onReset -= value; }

        public Timer(MonoBehaviour theManager, float duration, Action onActivateFunction = null, bool loop = true) {
            manager = theManager;
            _duration = duration;
            _loop = loop;
            if (onActivateFunction != null) {
                OnActivate += onActivateFunction;
            }
        }

        public Timer Start(float offset = 0) {
            Stop();
            coroutine = manager.StartCoroutine(StartTimer(offset));
            return this;
        }

        public void Pause() {
            if (state != TimerState.RUNNING && state != TimerState.RUNNINGOFFSET) { return; }
            state = TimerState.PAUSED;
            _onPause?.Invoke();
        }

        public void Continue() {
            if (state != TimerState.PAUSED) { return; }
            state = TimerState.RUNNING;
            _onContinue?.Invoke();
        }



        public void Stop() {
            if (state == TimerState.WAITING) {
                return;
            }
            state = TimerState.WAITING;
            if (coroutine != null) {
                manager.StopCoroutine(coroutine);
            }
            _onStop?.Invoke();
        }

        public void Reset() {
            if (state == TimerState.WAITING) { return; }
            _currentDuration = 0;
            _onReset?.Invoke();
        }
        public TimerState GetState() {
            return state;
        }

        public void StopLoop() {
            _loop = false;
        }

        public void StartLoop() {
            _loop = true;
        }
        void End() {
            _onEnd?.Invoke();
            state= TimerState.WAITING;
        }

        IEnumerator StartTimer(float offset) {
            state = TimerState.RUNNINGOFFSET;
            _onStart?.Invoke();
            if (0 < offset) {
                float _offsetduration = 0;
                OnReset += () => _offsetduration = 0;
                while (_offsetduration < offset) {
                    yield return null;
                    if (state == TimerState.RUNNINGOFFSET)
                        _offsetduration += Time.deltaTime;
                }
                OnReset -= () => _offsetduration = 0;
            }
            state = TimerState.RUNNING;
            _onEndOffset?.Invoke();
            do {
                _currentDuration = 0f;
                while (_currentDuration < _duration) {
                    yield return null;
                    if (state == TimerState.RUNNING)
                        _currentDuration += Time.deltaTime;
                }
                _onActivate?.Invoke();
            } while (_loop);
            End();
        }
    }
}
