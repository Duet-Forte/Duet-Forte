using UnityEngine;
using UnityEngine.UI;
using Util;

public class PlayerHealthPointUI : InGameUI
{
    private Image[] healthPointFilling;
    private int currentIndex;

    public void InitSettings(int playerHP)
    {
        GameObject healthPointFramePrefab = Resources.Load<GameObject>(Const.UI_PLAYERHP_FRAME_PATH);
        currentIndex = 0;
        healthPointFilling = new Image[playerHP];

        for (int i = 1; i <= playerHP; ++i)
        {
            GameObject healthPointFrame = Instantiate(healthPointFramePrefab, transform);
            healthPointFilling[playerHP - i] = healthPointFrame.transform.GetChild(0).GetComponent<Image>();
        }
    }

    public void GetDamage(int damage)
    {
        if (currentIndex >= healthPointFilling.Length)
        {
            return;
        }
        for (int i = 0; i < damage; ++i)
        {
            healthPointFilling[currentIndex].enabled = false;
            currentIndex++;
        }
    }

    public void GetHeal(int healAmount)
    {
        if (currentIndex < 0)
        {
            return;
        }
        for (int i = 0; i < healAmount; ++i)
        {
            currentIndex--;
            healthPointFilling[currentIndex].enabled = true;
        }
    }
}
