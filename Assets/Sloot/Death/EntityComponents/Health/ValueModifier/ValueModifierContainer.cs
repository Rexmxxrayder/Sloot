using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueModifierContainer<T, U> where T : IValueModifier<U> {
    private List<T> valueModifiers = new();

    public void AddValueModifier(T valueModifier) {
        valueModifiers.Add(valueModifier);
        valueModifiers.Sort(SortValueModifier);
    }

    public void RemoveValueModifier(T valueModifier) {
        if (valueModifiers.Contains(valueModifier)) {
            valueModifiers.Remove(valueModifier);
        }
    }

    private int SortValueModifier(T first, T second) {
        return -first.Priority.CompareTo(second.Priority);
    }

    public static implicit operator Func<U, string, U>(ValueModifierContainer<T, U> vmc) {
        return new Func<U, string, U>((U arg, string argTwo) => vmc.Modify(arg, argTwo));
    }

    public U Modify(U value, string type) {
        for (int i = 0; i < valueModifiers.Count; i++) {
            value = valueModifiers[i].Modify(value, type);
        }
        return value;
    }
}
