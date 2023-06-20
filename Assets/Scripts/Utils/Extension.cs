using UnityEngine;

public static class Extension
{
    public static bool IsContain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    public static bool IsValid(this GameObject gameObject)
    {
        return gameObject != null && gameObject.activeInHierarchy;
    }

    public static bool IsValid(this Component component)
    {
        return component != null && component.gameObject.activeInHierarchy;
    }
}