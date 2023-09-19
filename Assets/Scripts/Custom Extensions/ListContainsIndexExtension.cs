using System.Collections.Generic;

public static class ListContainsIndexExtension
{
    public static bool IsNotEmpty<T>(this List<T> list, in int index)
    {
        if (list.Count != 0)
            if (list.Count - 1 <= index)
                return true;

        return false;
    }
}