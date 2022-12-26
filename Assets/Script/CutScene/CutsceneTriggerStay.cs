using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneTriggerStay : MonoBehaviour
{
    private bool isPlayerStay;
    public PlayableDirector timeline_cutscene;
    public Button checkButton;
    
    bool checkButtonclick = false;

    private void Update()
    {
        if(isPlayerStay)
        {
            if (checkButtonclick)
            {
                checkButtonclick = false;
                timeline_cutscene.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isPlayerStay = true;
            
        }
    }

    public void onClickCheckButton()
    {
        checkButtonclick = true;
    }
}
