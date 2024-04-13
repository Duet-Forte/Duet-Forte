using Unity.VisualScripting;
using UnityEngine;
using Util;

public static class DamageUIContainer //���� �����̳� �ʿ��ϸ� �װ����� �̵�
{
    private static Sprite[] damageSkins;
    public static Sprite[] Skins
    {
        get
        {
            if (damageSkins == null)
            {
                damageSkins = new Sprite[10];
                for (int i = 0; i < damageSkins.Length; ++i)
                {
                    damageSkins[i] = Resources.Load<Sprite>(Const.SPRITE_PATH + $"Damage/{i}");
                }
            }
            return damageSkins;
        }
    }
    public static Sprite[] RedSkins
    {
        get {
            if (damageSkins == null)
            {
                damageSkins = new Sprite[10];
                for (int i = 0; i < damageSkins.Length; ++i)
                {
                    damageSkins[i] = Resources.Load<Sprite>(Const.SPRITE_PATH + $"Damage/RED_{i}");
                }
            }
            return damageSkins;
        }
    }
    public static Sprite[] BlueSkins
    {
        get
        {
            if (damageSkins == null)
            {
                damageSkins = new Sprite[10];
                for (int i = 0; i < damageSkins.Length; ++i)
                {
                    damageSkins[i] = Resources.Load<Sprite>(Const.SPRITE_PATH + $"Damage/BLUE_{i}");
                }
            }
            return damageSkins;
        }
    }
}
