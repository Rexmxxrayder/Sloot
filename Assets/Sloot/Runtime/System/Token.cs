using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token {
    private int current = 0;
    private int max = 0;
    public int Current => current;
    public int Max => max;

    private Action<int> onTokenAdd;
    private Action<int> onTokenRemove;
    private Action onZeroToken;
    private Action onNotZeroToken;
    public event Action<int> OnTokenAdd { add { onTokenAdd += value; } remove { onTokenAdd -= value; } }
    public event Action<int> OnTokenRemove { add { onTokenRemove += value; } remove { onTokenRemove -= value; } }
    public event Action OnZeroToken { add { onZeroToken += value; } remove { onZeroToken -= value; } }
    public event Action OnNotZeroToken { add { onNotZeroToken += value; } remove { onNotZeroToken -= value; } }

    public void AddToken(int addToken = 1) {
        if(addToken <= 0) {
            return;
        }

        current += addToken;
        onTokenAdd?.Invoke(addToken);
        onNotZeroToken?.Invoke();
    }

    public void RemoveToken(int removeToken = 1) {
        if (removeToken <= 0) {
            return;
        }

        current -= removeToken;
        if (current == 0) {
            onZeroToken?.Invoke();
            //dont want to activate the event if current < 0
            //so its put before the correction
        }

        if(current < 0) {
            current = 0; //"correction"
        }

        onTokenRemove?.Invoke(removeToken);
    }

    public void ChangeMax(int newMax) {
        max = newMax;
        max = Mathf.Clamp(max, 0, max);
        if (current > max) {
            current = max;
        }
    }
}
