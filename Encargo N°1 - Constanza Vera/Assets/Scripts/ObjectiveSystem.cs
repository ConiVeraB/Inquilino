using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ObjectiveSystem : MonoBehaviour
{
    [Header("Variables")]
    public AudioSource objSFX;
    public GameObject objective;
    public GameObject trigger;
    public GameObject textObjetive;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(missionObj());
        }
    }

    public IEnumerator missionObj()
    {
        objSFX.Play();
        objective.GetComponent<Text>().text = "Objetivo: Que esto funcione";
        yield return new WaitForSeconds(5.3f);
        textObjetive.GetComponent<Text>().text = "";
                                                                             trigger.SetActive(false);
        objective.SetActive(false);
    }
}
