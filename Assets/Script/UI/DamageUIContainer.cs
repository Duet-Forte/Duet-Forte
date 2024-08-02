using Unity.VisualScripting;
using UnityEngine;
using Util;

public static class DamageUIContainer //추후 컨테이너 필요하면 그곳으로 이동
{
    private static Sprite[] redDamageSkins;
    private static Sprite[] blueDamageSkins;
    private static Sprite[] trueDamageSkins;
    public static Sprite[] RedSkins
    {
        get {
            if (redDamageSkins == null)
            {
                redDamageSkins = new Sprite[10];
                for (int i = 0; i < redDamageSkins.Length; ++i)
                {
                    redDamageSkins[i] = Resources.Load<Sprite>(Const.SPRITE_PATH + $"Damage/RED_{i}");
                }
            }
            return redDamageSkins;
        }
    }
    public static Sprite[] BlueSkins
    {
        get
        {
            if (blueDamageSkins == null)
            {
                blueDamageSkins = new Sprite[10];
                for (int i = 0; i < blueDamageSkins.Length; ++i)
                {
                    blueDamageSkins[i] = Resources.Load<Sprite>(Const.SPRITE_PATH + $"Damage/BLUE_{i}");
                }
            }
            return blueDamageSkins;
        }
    }

    public static Sprite[] TrueSkins
    {
        get
        {
            if (trueDamageSkins == null)
            {
                trueDamageSkins = new Sprite[10];
                for (int i = 0; i < trueDamageSkins.Length; ++i)
                {
                    trueDamageSkins[i] = Resources.Load<Sprite>(Const.SPRITE_PATH + $"Damage/TRUE_{i}");
                }
            }

            return trueDamageSkins;
        }


    }
}
