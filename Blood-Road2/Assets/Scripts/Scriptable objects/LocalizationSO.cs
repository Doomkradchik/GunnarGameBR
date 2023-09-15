using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class LocalizationSO : ScriptableObject
{
    protected void TryGetLine(List<string> lines, string content)
    {
        if (string.IsNullOrWhiteSpace(content) == false)
        {
            lines.Add(content);
            return;
        }

        if(lines.Count == 0)
        {
            lines.Add("%1");
            return;
        }

        var last = lines.Last();
        if (string.IsNullOrEmpty(last))
            throw new System.InvalidOperationException();

        if(last[0] == '%')
        {
            string numberString = last.Substring(1);
            lines[lines.Count - 1] = $"%{int.Parse(numberString) + 1}";
            return;
        }

        lines.Add("%1");
    }

    protected string ReadLine(string[] lines, ref int lastIndex, ref int skipTime)
    {
        if (skipTime > 0)
        {
            skipTime--;
            return string.Empty;
        }
        var last = lines[lastIndex++];
        if (last[0] == '%')
        {
            skipTime = int.Parse(last.Substring(1)) - 1;
            return string.Empty;
        }

        return last;
    }

    public abstract void GetLocalizationLines(List<string> lines);
    public abstract void SetLocalizationLines(string[] lines, ref int lastIndex, ref int skipTime);
}