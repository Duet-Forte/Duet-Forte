using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataBase
{
    private static DataBase instance;
    private FieldDataBase fieldDataBase;
    private DialogueDataBase dialogueDataBase;
    private SkillDataBase skillDataBase;
    private QuestDataBase questDataBase;
    private StageDataBase stageDataBase;

    private PlayerData playerData;
    public static DataBase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataBase();
            }
            return instance;
        }
    }

    public static FieldDataBase Field { get => Instance.fieldDataBase; }
    public static DialogueDataBase Dialogue { get => Instance.dialogueDataBase; }
    public static SkillDataBase Skill { get => Instance.skillDataBase; }
    public static StageDataBase Stage { get => Instance.stageDataBase; }
    public static QuestDataBase Quest { get => Instance.questDataBase; }
    public static PlayerData Player { get => Instance.playerData; }
    private DataBase()
    {
        fieldDataBase = new FieldDataBase();
        dialogueDataBase = new DialogueDataBase();
        skillDataBase = new SkillDataBase();
        questDataBase = new QuestDataBase();
        stageDataBase = new StageDataBase();
        playerData = new PlayerData();
    }

    public void Save(int saveIndex)
    {
        SaveData data = new SaveData();
        data.SetData(saveIndex);
        string path = Application.persistentDataPath + "/" + Util.Const.SAVE_FILE_NAME;
        string saveData = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, saveData);
    }

    public int Load() 
    {
        if (instance == null)
        {
            instance = new DataBase();
        }
        string path = Application.persistentDataPath + "/" + Util.Const.SAVE_FILE_NAME;
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
            Field.ChangeField(data.FieldID);
            Player.LoadData(data.Level, data.EXP, data.Gold);
            Dialogue.LoadData(data.Dialogues);
            Quest.LoadData(data.Quests);
            return data.SaveID;
        }
        else
            return -1;
    }

}

[System.Serializable]
public class SaveData
{
    // From PlayerData
    [SerializeField] private int playerLevel;
    [SerializeField] private int playerEXP;
    [SerializeField] private int gold;

    // From DialogueDataBase
    [SerializeField] private List<string> dialogueKeys;
    [SerializeField] private List<int> dialogueValues;

    // From QuestDataBase
    [SerializeField] private List<int> questKeys;
    [SerializeField] private List<bool> questValues;

    // From FieldDataBase
    [SerializeField] private int fieldID;

    // Save location
    [SerializeField] private int saveID;

    // Properties
    public int Level { get => playerLevel; }
    public int EXP { get => playerEXP; }
    public int Gold { get => gold; }
    public Dictionary<string, int> Dialogues
    {
        get
        {
            var dict = new Dictionary<string, int>();
            for (int i = 0; i < dialogueKeys.Count; i++)
            {
                dict[dialogueKeys[i]] = dialogueValues[i];
            }
            return dict;
        }
    }
    public Dictionary<int, bool> Quests
    {
        get
        {
            var dict = new Dictionary<int, bool>();
            for (int i = 0; i < questKeys.Count; i++)
            {
                dict[questKeys[i]] = questValues[i];
            }
            return dict;
        }
    }
    public int FieldID { get => fieldID; }
    public int SaveID { get => saveID; }

    // SetData Method
    public void SetData(int saveID)
    {
        this.saveID = saveID;

        playerLevel = DataBase.Player.Level;
        playerEXP = DataBase.Player.EXP;
        gold = DataBase.Player.EXP;

        dialogueKeys = new List<string>(DataBase.Dialogue.IDDic.Keys);
        dialogueValues = new List<int>(DataBase.Dialogue.IDDic.Values);

        questKeys = new List<int>();
        questValues = new List<bool>();
        foreach (var quest in DataBase.Quest.Quests)
        {
            questKeys.Add(quest.Key.ID);  // Assuming Quest has a ToString method that gives a unique string
            questValues.Add(quest.Value);
        }

        fieldID = DataBase.Field.ID;
    }
}

