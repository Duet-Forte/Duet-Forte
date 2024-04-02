using UnityEngine;
using Util.CustomEnum;

public struct Judge
{
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
                    return Color.red;
                case (int)JudgeName.Miss:
                    return Color.black;
                default:
                    return Color.white;
            };
        }
    }
}
