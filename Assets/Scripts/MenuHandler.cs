using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public GameObject loadingScreen;
    public Scrollbar loadingBar;

    public void startSong(int id){
        SceneThru.SongId = id;
        StartCoroutine(LoadSceneInBackground());
    }

    IEnumerator LoadSceneInBackground(){
        AsyncOperation operation = SceneManager.LoadSceneAsync("KaraokePlayer");
        loadingScreen.SetActive(true);

        while(!operation.isDone){
            loadingBar.size = operation.progress;
            yield return null;
        }
    }
}
