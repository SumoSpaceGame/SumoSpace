using System;
using UnityEngine;

public class EnumNamedListAttribute : PropertyAttribute
{
    public string[] names;
    public EnumNamedListAttribute(Type namesEnumType)
    {
        this.names = Enum.GetNames(namesEnumType);
    }
}