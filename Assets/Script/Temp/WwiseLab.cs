using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WwiseLab : MonoBehaviour
{
      public void OnClick(string name)
    {
        AkSoundEngine.PostEvent(name, gameObject);
        AkSoundEngine.SetSwitch("Dirt", "PlayerFootStep_Dirt", gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
