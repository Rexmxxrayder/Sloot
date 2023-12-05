using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReductionModifier : IDamageModifier {
    private float percentReduction;
    private float flatReduction;
    private bool percentBeforeFlat = true;
    public int Priority => 10;
    public float PercentReduction => percentReduction;
    public float FlatReduction => flatReduction;
    public bool PercentBeforeFlat => percentBeforeFlat;

    public DamageReductionModifier(float percentReduction, float flatReduction = 0, bool percentBeforeFlat = false) {
        this.percentReduction = percentReduction;
        this.flatReduction = flatReduction;
        this.percentBeforeFlat = percentBeforeFlat;
    }

    public int Modify(int value, string type) {
        if (percentBeforeFlat) {
            return FlatReductionModifer(PercentReductionModifer(value));
        } else {
            return PercentReductionModifer(FlatReductionModifer(value));
        }
    }

    private int PercentReductionModifer(int value) {
        return (int)(value * (1 - (percentReduction % 100 / 100)));
    }

    private int FlatReductionModifer(int value) {
        return (int)(value - percentReduction < 0 ? 0 : value - percentReduction);
    }
}
