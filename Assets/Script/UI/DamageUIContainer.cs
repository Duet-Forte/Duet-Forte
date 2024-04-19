using Unity.VisualScripting;
using UnityEngine;
using Util;

public static class DamageUIContainer //���� �����̳� �ʿ��ϸ� �װ����� �̵�
{
    private static Sprite[] redDamageSkins;
    private static Sprite[] blueDamageSkins;
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
}
