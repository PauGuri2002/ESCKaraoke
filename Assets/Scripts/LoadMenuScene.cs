using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoadMenuScene : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data){
        SceneManager.LoadScene("Menu");
    }
}
