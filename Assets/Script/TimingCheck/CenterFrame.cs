using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK;
using Unity.VisualScripting;

public class CenterFrame : MonoBehaviour
{
    
    [SerializeField]bool musicStart = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note")&&!musicStart) 
        {
            musicStart = true;
            //BGMSTART
            AkSoundEngine.PostEvent("Combat_Stage_01_BGM", gameObject);
            AkSoundEngine.SetSwitch("Stage01", "StageStart", gameObject);
                       
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
