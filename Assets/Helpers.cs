using System.Collections.Generic;
using UnityEngine;

public class Helpers
{
    public static float vector2DDistance(Vector3 from, Vector3 to)
    {
        float xDiff = from.x - to.x;
        float zDiff = from.z - to.z;
        return Mathf.Sqrt((xDiff * xDiff) + (zDiff * zDiff));
    }

    public static bool ContainsKeyButValueNotNull<X, Y>(Dictionary<X, Y> dictionary, X expectedKey)
    {
        Y actualValue;
        return dictionary.TryGetValue(expectedKey, out actualValue) && actualValue != null;
    }

    public static bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    public static bool IsAnimatorPlaying(string animatorName, Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animatorName);
    }

    public static bool IsAnimatorPlayingAny(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public static string ReplaceInRegionOfSpecialCharacters(string original, char startCharacter, char endCharacter, string replaceString, bool includeSpecialCharacters = false)
    {
        int indexStart = original.IndexOf(startCharacter);
        int indexEnd = original.IndexOf(endCharacter);
        if (includeSpecialCharacters)
        {
            indexStart--;
            indexEnd++;
        }
        // Not found characters
        if (indexStart <= -1 || indexEnd <= -1) return original + " " + startCharacter + replaceString + endCharacter;
        if (indexStart > indexEnd)
        {
            // Swap position
            indexStart = indexStart + indexEnd;
            indexEnd = indexStart - indexEnd;
            indexStart = indexStart - indexEnd;
        }
        original = original.Remove(indexStart + 1, indexEnd - indexStart - 1).Insert(indexStart + 1, replaceString);
        return original;
    }
}
