using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DemoManager : MonoBehaviour
{
    private Image black;
    // Start is called before the first frame update
    void Start()
    {
        black = GetComponent<Image>();
        black.DOFade(0f, 2f);

        StartCoroutine(LoadMainMenu());
    }
    IEnumerator LoadMainMenu() {
        yield return new WaitForSeconds(2f);
        while (true) {
            if (Input.anyKeyDown)
            {

                Debug.Log("press anyKey");
                
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
            }

            yield return null;
        }
    
    }
    // Update is called once per frame
    
}
