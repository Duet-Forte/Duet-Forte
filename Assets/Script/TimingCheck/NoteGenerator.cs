using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NoteGenerator : MonoBehaviour
{
    
    [SerializeField] GameObject goNote;
    [SerializeField] Transform noteGeneratingTransform;
    TimingManager theTimingManager;

    
    // Start is called before the first frame update
    void Start()
    {
       /*theTimingManager= GetComponent<TimingManager>();
        Metronome.instance.OnBeating += GenerateNote;*/
    }

    // Update is called once per frame
    void Update()
    {
        
           /* currentTime += Time.deltaTime;
            if (currentTime >= MINUET_TO_SECOND / bitPerMinute)
            {
                GameObject goingNote = Instantiate(goNote, noteGeneratingTransform.position, Quaternion.identity);
                goingNote.transform.parent = gameObject.transform;
                theTimingManager.currentNoteList.Add(goingNote);
                
                currentTime -= MINUET_TO_SECOND / bitPerMinute;
            }*/
        
    }

    private void GenerateNote() {
        GameObject goingNote = Instantiate(goNote, noteGeneratingTransform.position, Quaternion.identity);
        //goingNote.transform.parent = gameObject.transform;
        goingNote.transform.SetParent(this.transform);
        theTimingManager.currentNoteList.Add(goingNote);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note")) {

            theTimingManager.currentNoteList.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
