using UnityEngine;
using Util.CustomEnum;

public struct Judge
{
    public Judge(JudgeName judgeName) { 
    this.name= judgeName;
    }
    private JudgeName name;
    public JudgeName Name { get { return name; } set { name = value; } }
    public Color Color {
        get
        {
            switch ((int)name)
            { 
                case (int)JudgeName.Perfect:
                    return Color.green;
                case (int)JudgeName.Great:
                    return Color.yellow;
                case (int)JudgeName.Good:
                    return Color.blue;
                case (int)JudgeName.Bad:
                    return new Color(255f/255f,100f / 255f, 7f / 255f);
                case (int)JudgeName.Miss:
                    return Color.black;
                default:
                    return Color.white;
            };
        }
    }
}
