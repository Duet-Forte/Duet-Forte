using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class DialogueDataBase
{
    private List<Dictionary<string, Dialogue[]>> dialogues;
    private Dictionary<string, int> ids;
    private Dictionary<string, Sprite> sprites;
    private int currentFieldId;
    private StringBuilder sb = new StringBuilder(); 
    public void ResetLine(int fieldId)
    {
        currentFieldId = fieldId;

        if (dialogues == null)
            dialogues = new List<Dictionary<string, Dialogue[]>>();

        if(ids == null)
            ids = new Dictionary<string, int>();

        while (dialogues.Count <= currentFieldId + 1)
        {
            dialogues.Add(new Dictionary<string, Dialogue[]>());
        }

        if (dialogues[currentFieldId].Count <= 0)
        {
            List<Dictionary<string, string>> dialogueData = TSVReader.Read("DialogueData/" + currentFieldId + ".tsv");
            TSVReader.ParseData(dialogueData, "Interactable", ParseDialogue);
        }
    }
    public Dialogue GetDialogue(string interactableName)
    {
        int id = GetID(interactableName);

        if(id == -1)
        {
            Debug.Log($"대화파일이 손상되었습니다 : {interactableName}");
            return default;
        }

        return dialogues[currentFieldId][interactableName][id];
    }

    public void PlusID(string speakerName, int plusAmount)
    {
        SetID(speakerName, GetID(speakerName) + plusAmount);
    }
    public void SetID(string speakerName, int id)
    {
        sb.Clear();
        sb.Append(speakerName);
        sb.Append(currentFieldId);
        if (ids.ContainsKey(sb.ToString()))
            ids[sb.ToString()] = id;
        else
            ids.Add(sb.ToString(), id);
    }

    public int GetID(string speakerName)
    {
        int answer;
        sb.Clear();
        sb.Append(speakerName);
        sb.Append(currentFieldId);
        if (!ids.ContainsKey(sb.ToString()))
            return 0;
        else if (ids.TryGetValue(sb.ToString(), out answer))
            return answer;
        else
            return -1;
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
        string recentState = string.Empty;

        for (int column = currentStart; column < nextStart; ++column)
        {
            Dictionary<string, string> tempDic = data[column];

            if (tempDic["ID"] != string.Empty && column != currentStart)
            {
                dialogueList.Add(new Dialogue(lines.ToArray()));
                recentSpeaker = "Empty";
                recentState = string.Empty;
                lines.Clear();
                ++id;
            }
            sb.Append(tempDic["Lines"]);
            if (tempDic["Speaker"] != string.Empty)
            {
                recentSpeaker = tempDic["Speaker"];
                recentState = tempDic["State"];
            }
            else if (tempDic["State"] != string.Empty)
                recentState = tempDic["State"];

            if (tempDic["Event"] == string.Empty)
            {
                sb.Append(';');
                sb.Append(recentSpeaker);
                sb.Append('/');
                sb.Append(recentState);
            }
            else
            {
                sb.Append(';');
                sb.Append(recentSpeaker);
                sb.Append(';');
                sb.Append(tempDic["Event"]);
                sb.Append('/');
                sb.Append(tempDic["EventName"]);
            }
            lines.Add(sb.ToString());
            sb.Clear();
        }
        dialogueList.Add(new Dialogue(lines.ToArray()));
        dialogues[currentFieldId].Add(data[currentStart]["Interactable"], dialogueList.ToArray());
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
