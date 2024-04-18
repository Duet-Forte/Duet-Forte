using UnityEngine;

public class CurrentSkill : Menu
{

    public override void OnPressed()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSelected()
    {
        base.OnSelected();
        Debug.Log(gameObject.name);
    }
}
