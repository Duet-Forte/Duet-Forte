public class DataBase
{
    private static DataBase instance;
    private TopViewEntityDataBase topViewEntityDataBase;
    private DialogueDataBase dialogueDataBase;

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
    private DataBase()
    {
        topViewEntityDataBase = new TopViewEntityDataBase();
        dialogueDataBase = new DialogueDataBase();
    }
}
