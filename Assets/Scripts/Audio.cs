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
        fadeTo.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(waitFor);
        fadeTo.GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayDelayed(0.001f);
        GetComponent<Animator>().SetTrigger("FadeIn");
    }

    IEnumerator GoingOut()
    {
        GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(waitFor);
        GetComponent<AudioSource>().Stop();
        fadeTo.GetComponent<AudioSource>().PlayDelayed(0.001f);
        fadeTo.GetComponent<Animator>().SetTrigger("FadeIn");
    }
}
