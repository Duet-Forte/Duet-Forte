using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueDataBase
{
    private List<Dictionary<string, Dialogue[]>> dialogues;

    private Dictionary<string, Sprite> sprites;
    private int currentFieldId;
    public void ResetLine(int fieldId)
    {
        if (dialogues == null)
            dialogues = new List<Dictionary<string, Dialogue[]>>();
        currentFieldId = fieldId;
        List<Dictionary<string, string>> dialogueData = TSVReader.Read("DialogueData/" + currentFieldId + ".tsv");
        if (dialogues.Count <= currentFieldId)
            dialogues.Add(new Dictionary<string, Dialogue[]>());
        TSVReader.ParseData(dialogueData, "Interactable", ParseDialogue);
    }
    public Dialogue GetDialogue(string interactableName, int id)
    {
        return dialogues[currentFieldId][interactableName][id];
    }
    private void ParseDialogue(List<Dictionary<string, string>> data, int[] startColumn, int categoryId)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        List<string> lines = new List<string>();
        StringBuilder sb = new StringBuilder();
        int currentStart = startColumn[categoryId];
        int nextStart = startColumn[categoryId + 1];
        int id = 0;
        string recentSpeaker = "Empty";

        for (int column = currentStart; column < nextStart; ++column)
        {
            Dictionary<string, string> tempDic = data[column];

            if (tempDic["ID"] != string.Empty && column != currentStart)
            {
                dialogueList.Add(new Dialogue(id, lines.ToArray()));
                recentSpeaker = "Empty";
                lines.Clear();
                ++id;
            }
            sb.Append(tempDic["Lines"]);
            if (tempDic["Speaker"] != string.Empty)
            {
                sb.Append(";" + tempDic["Speaker"] + "_" + tempDic["State"]);
                recentSpeaker = tempDic["Speaker"];
            }
            else if (tempDic["State"] != string.Empty)
            {
                sb.Append(";" + recentSpeaker + "_" + tempDic["State"]);
            }
            lines.Add(sb.ToString());
            sb.Clear();
        }
        dialogueList.Add(new Dialogue(id, lines.ToArray()));
        dialogues[currentFieldId].Add(data[currentStart]["Interactable"], dialogueList.ToArray());
        dialogueList = null;
        lines = null;
    }

    public Sprite GetSprite(string fileName)
    {
        if (fileName == "Empty")
            return null;
        if (sprites == null)
            sprites = new Dictionary<string, Sprite>();

        if (!sprites.ContainsKey(fileName))
        {
            sprites[fileName] = Resources.Load<Sprite>("Sprite/Dialogue/" + fileName);
        }
        return sprites[fileName];
    }
}
