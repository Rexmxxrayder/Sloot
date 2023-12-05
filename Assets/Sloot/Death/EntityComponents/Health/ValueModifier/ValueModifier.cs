using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IValueModifier<T>
{
    public int Priority { get; }

    public abstract T Modify(T value, string type);
}

public interface IHealthModifier : IValueModifier<int> {


}

public interface IDamageModifier : IHealthModifier {


}

public interface IHealModifier : IHealthModifier {


}