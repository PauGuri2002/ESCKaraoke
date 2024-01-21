using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text songDuration;
    public AudioClip audioClip;
    Image container;
    public Color unselectedColor;
    public Color accentColor;
    AudioSource audioSource;
    Coroutine fadeIn, fadeOut, colorIn, colorOut;

    void Start(){
        container = GetComponent<Image>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = 0;
        songDuration.text = secondsToTime(audioSource.clip.length);
    }

    public void OnPointerEnter(PointerEventData data){
        audioSource.time = Random.Range(5, audioSource.clip.length - 10);
        audioSource.Play();
        if(fadeOut != null){ StopCoroutine(fadeOut); }
        fadeIn = StartCoroutine(LerpVolume(1,0.5f));
        if(colorOut != null){ StopCoroutine(colorOut); }
        colorIn = StartCoroutine(LerpColor(accentColor,0.2f));
    }

    public void OnPointerExit(PointerEventData data){
        if(fadeIn != null){ StopCoroutine(fadeIn); }
        fadeOut = StartCoroutine(LerpVolume(0,0.5f));
        if(colorIn != null){ StopCoroutine(colorIn); }
        colorOut = StartCoroutine(LerpColor(unselectedColor,0.2f));
    }

    IEnumerator LerpVolume(float endValue, float duration){
        float time = 0;
        float startValue = audioSource.volume;
        while (time < duration){
            audioSource.volume = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = endValue;
        if(endValue == 0){
            audioSource.Pause();
        }
    }

    IEnumerator LerpColor(Color endValue, float duration){
        float time = 0;
        Color startValue = container.color;
        while (time < duration)
        {
            container.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        container.color = endValue;
    }

    void Update(){
        if(audioSource.time >= audioSource.clip.length){
            audioSource.time = 0;
            audioSource.Play();
        }
    }

    private static string secondsToTime(float seconds){
        string convertedTime = Mathf.Floor(seconds/60).ToString("00") + ":" + Mathf.Floor(seconds%60).ToString("00");
        return convertedTime;
    }
}
