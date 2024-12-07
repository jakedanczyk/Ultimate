using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MacroNutrients
{//grams of protein, fat, and carbs
    public float p;
    public float f;
    public float c;

    public MacroNutrients(float ip, float ifat, float ic)
    {
        p = ip; f = ifat; c = ic;
    }
    public MacroNutrients(MacroNutrients macros)
    {
        p = macros.p; f = macros.f; c = macros.c;
    }
    public static MacroNutrients operator *(float lhs, MacroNutrients macros)
    {
        return new MacroNutrients(lhs * macros.p, lhs*macros.f, lhs*macros.c);
    }
    public static MacroNutrients operator *(MacroNutrients macros, float rhs)
    {
        return new MacroNutrients(rhs * macros.p, rhs * macros.f, rhs * macros.c);
    }
    public static MacroNutrients operator -(MacroNutrients lhs, MacroNutrients rhs)
    {
        return new MacroNutrients(lhs.p - rhs.p, lhs.f - rhs.f, lhs.c - rhs.c);
    }

    public static MacroNutrients operator +(MacroNutrients lhs, MacroNutrients rhs)
    {
        return new MacroNutrients(lhs.p + rhs.p, lhs.f + rhs.f, lhs.c + rhs.c);
    }
}

public abstract class CreatureNutrition : ICreatureNutrition
{
}
