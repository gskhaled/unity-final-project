using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFader : MonoBehaviour
{
    public AudioSource tenseAudio;
    public AudioSource calmAudio;
    private IEnumerator CrossFadeAudio(AudioSource audioSource1, AudioSource audioSource2, float crossFadeTime, float audioSource2VolumeTarget)
    {
        string debugStart = "<b><color=red>ERROR:</color></b> ";
        int maxLoopCount = 1000000;
        int loopCount = 0;
        float startAudioSource1Volume = audioSource1.volume;

        if (audioSource1 == null || audioSource2 == null)
        {
            Debug.Log(debugStart + transform.name + ".EngineControler.CrossFadeAudio recieved NULL value.\n*audioSource1=" + audioSource1.ToString() + "\n*audioSource2=" + audioSource2.ToString(), gameObject);
            yield return null;
        }
        else
        {
            audioSource2.volume = 0f;
            audioSource2.Play();

            while ((audioSource1.volume > 0f && audioSource2.volume < audioSource2VolumeTarget) && loopCount < maxLoopCount)
            {
                audioSource1.volume -= startAudioSource1Volume * Time.deltaTime / crossFadeTime;
                audioSource2.volume += audioSource2VolumeTarget * Time.deltaTime / crossFadeTime;
                loopCount++;
                yield return null;
            }

            if (loopCount < maxLoopCount)
            {
                audioSource1.Stop();
                audioSource1.volume = startAudioSource1Volume;
            }
            else
            {
                Debug.Log(debugStart + transform.name + ".EngineControler.CrossFadeAudio.loopCount reached max value.\nloopCount=" + loopCount + "\nmaxLoopCount=" + maxLoopCount, gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("TENSE MUSIC"))
        {
            StartCoroutine(CrossFadeAudio(calmAudio, tenseAudio, 4f, 0.9f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("TENSE MUSIC"))
        {
            StartCoroutine(CrossFadeAudio(tenseAudio, calmAudio, 4f, 0.9f));
        }
    }
}
