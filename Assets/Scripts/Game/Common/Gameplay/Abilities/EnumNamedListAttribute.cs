using UnityEngine;
public class EnumNamedListAttribute : PropertyAttribute
{
    public string[] names;
    public EnumNamedListAttribute(System.Type namesEnumType)
    {
        this.names = System.Enum.GetNames(namesEnumType);
    }
}