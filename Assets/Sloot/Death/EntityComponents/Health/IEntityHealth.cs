using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityHealth
{
    public float Health { get; }
    public float MaxHealth { get; }

    public int AddHealth(float heal);

    public int RemoveHealth(float damage);
}
