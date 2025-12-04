using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class PrefabAttribute : Attribute
{
    public string Path { get; }

    public PrefabAttribute(string path) => Path = path;
}