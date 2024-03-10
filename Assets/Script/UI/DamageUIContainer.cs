using UnityEngine;
using Util;

public static class DamageUIContainer //추후 컨테이너 필요하면 그곳으로 이동
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
}