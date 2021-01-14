using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public int waitFor;
    public GameObject fadeTo;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Joel"))
            StartCoroutine(GoingIn());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Joel"))
            StartCoroutine(GoingOut());
    }

    IEnumerator GoingIn()
    {
        Debug.Log("going in");
        fadeTo.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(waitFor);
        Debug.Log("stopping other audio and playing myself");
        fadeTo.GetComponent<AudioSource>().Stop();
        Debug.Log(fadeTo.GetComponent<AudioSource>().isPlaying);
        GetComponent<AudioSource>().volume = 0;
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetTrigger("FadeIn");
        GetComponent<AudioSource>().volume = 1;
    }

    IEnumerator GoingOut()
    {
        Debug.Log("going out");
        GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(waitFor);
        Debug.Log("stopping myself audio and playing other");
        GetComponent<AudioSource>().Stop();
        fadeTo.GetComponent<AudioSource>().volume = 0;
        fadeTo.GetComponent<AudioSource>().Play();
        fadeTo.GetComponent<Animator>().SetTrigger("FadeIn");
        fadeTo.GetComponent<AudioSource>().volume = 1;
    }
}
