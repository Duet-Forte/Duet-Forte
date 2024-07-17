using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject onOffObject;
    [SerializeField] private bool isOn;
    [SerializeField] private string[] switchSoundName, onSoundName;
    [SerializeField] private string pointRange;
    private List<uint> onSoundID;

    public void InteractPlayer(PlayerController controller)
    {
        if (onSoundID == null)
            onSoundID = new List<uint>();
        isOn ^= true;
        OnSwitchSound();
        onOffObject.SetActive(isOn);
        controller.IsStopped = false;
        GameManager.FieldManager.onPointChange -= SetSound;
        GameManager.FieldManager.onPointChange += SetSound;
    }

    public void OnSwitchSound()
    {
        if (isOn)
        {
            if (switchSoundName != null)
            {
                foreach (var switchSound in switchSoundName)
                {
                    AkSoundEngine.PostEvent(switchSound, gameObject);
                }
            }
            if (onSoundName != null)
            {
                foreach (var onSound in onSoundName)
                {
                    uint temp = AkSoundEngine.PostEvent(onSound, gameObject);
                    if (!onSoundID.Contains(temp))
                        onSoundID.Add(temp);
                }
            }
        }
        else
        {
            if (switchSoundName != null)
            {
                foreach (var switchSound in switchSoundName)
                {
                    AkSoundEngine.PostEvent(switchSound, gameObject);
                }
            }
            if (onSoundName != null)
            {
                foreach (var onSound in onSoundID)
                {
                    AkSoundEngine.StopPlayingID(onSound);
                }
            }
        }
    }

    public void SetSound(string pointName)
    {
        if (pointRange == pointName && isOn)
        {
            foreach (var onSound in onSoundName)
            {
                uint temp = AkSoundEngine.PostEvent(onSound, gameObject);
                if (!onSoundID.Contains(temp))
                    onSoundID.Add(temp);
            }
        }
        else
        {
            foreach (var onSound in onSoundID)
            {
                AkSoundEngine.StopPlayingID(onSound);
            }
        }
    }
}
