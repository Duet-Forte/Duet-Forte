using UnityEngine;
using Util;

public struct Judge
{
    private CustomEnum.JudgeName name;
    public CustomEnum.JudgeName Name { get { return name; } set { name = value; } }
    public Color Color {
        get
        {
            switch ((int)name)
            { 
                case (int)CustomEnum.JudgeName.Perfect:
                    return Color.green;
                case (int)CustomEnum.JudgeName.Great:
                    return Color.yellow;
                case (int)CustomEnum.JudgeName.Good:
                    return Color.blue;
                case (int)CustomEnum.JudgeName.Bad:
                    return Color.red;
                case (int)CustomEnum.JudgeName.Miss:
                    return Color.black;
                default:
                    return Color.white;
            };
        }
    }
}
