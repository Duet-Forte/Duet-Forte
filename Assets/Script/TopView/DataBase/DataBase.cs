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

    public TopViewEntityDataBase Entity { get => topViewEntityDataBase; }
    public DialogueDataBase Dialogue { get => dialogueDataBase; }
    public SkillDataBase Skill { get => skillDataBase; }
    public PlayerData Player { get => playerData; }
    public StageDataBase Stage { get => stageDataBase; }
    private DataBase()
    {
        topViewEntityDataBase = new TopViewEntityDataBase();
        dialogueDataBase = new DialogueDataBase();
        skillDataBase = new SkillDataBase();
        playerData = new PlayerData();
        stageDataBase = new StageDataBase();
    }
}
