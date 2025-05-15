using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTest : MonoBehaviour
{
  
    public GameObject collectText;

    public AudioSource collectSound;

    private GameObject page;

    private bool inReach;

    public GameObject gameLogic;

    public int pages;

    void Start()
    {
   
        collectText.SetActive(false);

        inReach = false;

        gameLogic = GameObject.FindWithTag("GameLogic");

        page = this.gameObject;
    }

    // Метод OnTriggerEnter вызывается, когда другой коллайдер (игрок в нашем случае) входит в триггер
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;

            collectText.SetActive(true);
        }
    }

    // Метод OnTriggerExit вызывается, когда другой коллайдер покидает триггер
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;

            collectText.SetActive(false);
        }
    }

    void Update()
    {
    
        if (inReach && Input.GetButtonDown("pickup"))
        {

            gameLogic.GetComponent<GameLogic>().pageCount += 1;

            pages = gameLogic.GetComponent<GameLogic>().pageCount;

            collectSound.Play();

            collectText.SetActive(false);
 
            page.SetActive(false);

            inReach = false;
        }
    }
}