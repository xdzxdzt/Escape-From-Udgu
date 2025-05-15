using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPoint : MonoBehaviour
{
    public GameObject ReachTool;
    public GameObject TextNoExit; 
    public GameObject TextToExit;
    public GameObject fadeFX;
    public GameLogic page;
    public string nextSceneName;


    private bool inReach;


    void Start()
    {
        ReachTool.SetActive(true);
        TextNoExit.SetActive(false);
        TextToExit.SetActive(false);
        fadeFX.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            ReachTool.SetActive(true);
            TextNoExit.SetActive(true);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            ReachTool.SetActive(true);
            TextNoExit.SetActive(false);
            TextToExit.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && page.pageCount == 7)
        {
            TextToExit.SetActive(true);
            TextNoExit.SetActive(false);
        }

        if (inReach && Input.GetButtonDown("pickup") && page.pageCount < 7)
        {
            ReachTool.SetActive(true);
        }

        if (inReach && Input.GetButtonDown("pickup") && page.pageCount == 7)
        {
            ReachTool.SetActive(true);
            fadeFX.SetActive(true);
            StartCoroutine(ending());
        }
    }

    IEnumerator ending()
    {
        yield return new WaitForSeconds(.6f);
        SceneManager.LoadScene(nextSceneName);
    }

}
