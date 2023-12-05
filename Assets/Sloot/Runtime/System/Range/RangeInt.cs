using Sloot;
using System;
using UnityEngine;
using UnityEngine.Events;

public class RangeInt {
    [SerializeField] protected int _currentValue;
    [SerializeField] protected int _maxValue;
    [SerializeField] protected int _minValue;

    public int CurrentValue => _currentValue;
    public int MaxValue => _maxValue;
    public int MinValue => _minValue;

    private Action<int> _onIncreasing;
    private Action<int> _onDecreasing;
    private Action<int> _onOverIncreased;
    private Action<int> _onOverDecreased;

    public event Action<int> OnIncreasing  { add =>  _onIncreasing += value;  remove => _onIncreasing -= value; }
    public event Action<int> OnDecreasing { add => _onDecreasing += value; remove => _onDecreasing -= value; }
    public event Action<int> OnOverIncreased { add => _onOverIncreased += value; remove => _onOverIncreased -= value; }
    public event Action<int> OnOverDecreased { add => _onOverDecreased += value; remove => _onOverDecreased -= value; }


    public int IncreaseOf(int value) {
        value = Mathf.Max(0, value);
        if (value == 0) { return _currentValue; }

        if (_currentValue + value > _maxValue) {
            _currentValue = _maxValue;
            _onOverIncreased?.Invoke(_currentValue + value - _maxValue);
        } else {
            _currentValue += value;

        }
        _onIncreasing?.Invoke(value);
        return _currentValue;
    }

    public int DecreaseOf(int value) {
        value = Mathf.Max(0, value);
        if (value == 0) { return _currentValue; }

        if (_currentValue - value < _minValue) {
            _currentValue = _minValue;
            _onOverDecreased?.Invoke(_minValue - (_currentValue - value));
        } else {
            _currentValue -= value;
        }
        _onDecreasing?.Invoke(value);
        return _currentValue;
    }

    public void EqualTo(int value) {
        if (_currentValue == value) { return; }
        if (_currentValue < value) {
            IncreaseOf(Mathf.Abs(value - _currentValue));
        } else {
            DecreaseOf(Mathf.Abs(value - _currentValue));
        }
    }

    public void NewMaxValue(int newMaxValue) {
        _maxValue = newMaxValue;
        if (_currentValue > _maxValue) {
            _currentValue = _maxValue;
        }
        if (_minValue > _maxValue) {
            _minValue = _maxValue;
        }
    }

    public void NewMinValue(int newMinValue) {
        _minValue = newMinValue;
        if (_currentValue < _minValue) {
            _currentValue = _minValue;
        }
        if (_maxValue < _minValue) {
            _maxValue = _minValue;
        }
    }

    public int GetPercentRange(int percent) {
        return (_maxValue - _minValue) * percent / 100;
    }
    public void RemoveAllListeners() {
        _onIncreasing = null;
        _onDecreasing = null;
        _onOverIncreased = null;
        _onOverDecreased = null;
    }
}
