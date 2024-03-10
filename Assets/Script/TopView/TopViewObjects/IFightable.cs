using System;

public interface IFightable
{
    public event Action<string> OnFightPlayer; 
    public void EnterBattle();
}
