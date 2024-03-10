using System.Collections;

public interface ITurnHandler
{
    public IEnumerator TurnStart();

    public IEnumerator TurnEnd();
}
