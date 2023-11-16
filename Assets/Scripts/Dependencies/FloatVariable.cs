using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "ScriptableObjects/FloatVariable", order = 3)]
public class FloatVariable : Variable<float>
{
    public override void SetValue(float value)
    {
        _value = value;
    }

    // overload
    public void SetValue(FloatVariable value)
    {
        SetValue(value.Value);
    }

    public void ApplyChange(float amount)
    {
        this.Value += amount;
    }

    public void ApplyChange(FloatVariable amount)
    {
        ApplyChange(amount.Value);
    }
}
