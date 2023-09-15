using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GUIKeyWorlds")]
public class GUIKeyWorlds : LocalizationSO
{
    [Header("WordTools")]
    public string updateKeyWorld;
    public string costKeyWord;
    [Header("Currency")]
    public string secKeyWord;
    public string manaKeyWord;

    public override void GetLocalizationLines(List<string> lines)
    {
        TryGetLine(lines, updateKeyWorld);
        TryGetLine(lines, costKeyWord);
        TryGetLine(lines, secKeyWord);
        TryGetLine(lines, manaKeyWord);
    }

    public override void SetLocalizationLines(string[] lines, ref int lastIndex, ref int skipTime)
    {
        updateKeyWorld = ReadLine(lines, ref lastIndex, ref skipTime);
        costKeyWord = ReadLine(lines, ref lastIndex, ref skipTime);
        secKeyWord = ReadLine(lines, ref lastIndex, ref skipTime);
        manaKeyWord = ReadLine(lines, ref lastIndex, ref skipTime);
    }
}
