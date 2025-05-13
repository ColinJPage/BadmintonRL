using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloatModVariable : ModVariable<float, Modifier<float>>
{
    Func<float> baseValueFunc;
    public FloatModVariable(float baseValue = 0f)
    {
        baseValueFunc = () => baseValue;
    }
    public FloatModVariable(Func<float> baseValue)
    {
        baseValueFunc = baseValue;
    }
    protected override float BaseValue()
    {
        return baseValueFunc();
    }
}

public class FloatMultiplier : Modifier<float>
{
    Func<float> factorLambda;
    public float Factor => factorLambda();
    public FloatMultiplier(float factor) : this(() => { return factor; })
    {
    }
    public FloatMultiplier(Func<float> factorLambda)
    {
        SetFactorLambda(factorLambda);
    }

    public override float Modify(ref float data, in bool mode)
    {
        data *= Factor;
        return data;
    }
    public void SetFactor(float newFactor)
    {
        SetFactorLambda(() => { return newFactor; });
    }
    public void SetFactorLambda(Func<float> lambda)
    {
        this.factorLambda = lambda;
    }
}

public class FloatAdder : Modifier<float>
{
    Func<float> factorLambda;
    public float Factor => factorLambda();
    public FloatAdder(float factor) : this(() => { return factor; })
    {
    }
    public FloatAdder(Func<float> factorLambda)
    {
        SetFactorLambda(factorLambda);
    }

    public override float Modify(ref float data, in bool mode)
    {
        data += Factor;
        return data;
    }
    public void SetFactor(float newFactor)
    {
        SetFactorLambda(() => { return newFactor; });
    }
    public void SetFactorLambda(Func<float> lambda)
    {
        this.factorLambda = lambda;
    }
}

