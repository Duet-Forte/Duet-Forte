public class DataBase
{
    private static DataBase instance;
    private TopViewEntityDataBase topViewEntityDataBase;
    private DialogueDataBase dialogueDataBase;
    private SkillDataBase skillDataBase;
    private PlayerData playerData;
    private StageDataBase stageDataBase;
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

    public static TopViewEntityDataBase Entity { get => Instance.topViewEntityDataBase; }
    public static DialogueDataBase Dialogue { get => Instance.dialogueDataBase; }
    public static SkillDataBase Skill { get => Instance.skillDataBase; }
    public static PlayerData Player { get => Instance.playerData;}
    public static StageDataBase Stage { get => Instance.stageDataBase; }
    private DataBase()
    {
        topViewEntityDataBase = new TopViewEntityDataBase();
        dialogueDataBase = new DialogueDataBase();
        skillDataBase = new SkillDataBase();
        playerData = new PlayerData();
        stageDataBase = new StageDataBase();
    }
}
