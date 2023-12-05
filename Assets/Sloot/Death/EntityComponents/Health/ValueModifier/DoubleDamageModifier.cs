using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamageModifer : IDamageModifier {
    public int Priority => 1;

    public int Modify(int value, string type) {
        if (type == "") {
            return value * 2;
        }

        return value;
    }
}
