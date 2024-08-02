using System.Collections;
using UnityEngine;
using DG.Tweening;
public class DefenseQTE : MonoBehaviour
{
    float secondPerBeat;
    GameObject outCircle;
    Vector3 outCircleScale = new Vector3(7, 7, 7);
    
    public void InitSetting(double spb) {
        this.secondPerBeat = (float)spb;
        outCircle = transform.GetChild(1).gameObject;
        
        
    }
    
    
    public void StartQTE() {
        gameObject.SetActive(true);
        StartCoroutine(InvokeQTE());
    }
    IEnumerator InvokeQTE() {
        bool isDone = false;
        
        outCircle.transform.localScale = outCircleScale;
        outCircle.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), secondPerBeat + secondPerBeat / 10);
        while (!isDone) {
            if (Input.GetKeyDown(KeyCode.F)|| Input.GetKeyDown(KeyCode.J)) {
                isDone = true;
                EndQTE();
                break;
            }
            if (outCircle.transform.localScale == new Vector3(0.9f, 0.9f, 0.9f)) {
                isDone = true;
            }
            yield return null;

        }
        EndQTE();

    }
    private void EndQTE() {
        gameObject.SetActive(false);
    
    }
}
