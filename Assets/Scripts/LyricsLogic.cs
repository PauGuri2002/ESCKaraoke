using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LyricsLogic : MonoBehaviour
{
    // PARSE JSON

    public TextAsset JSONFile;

    [System.Serializable]
    public class Song{
        public string country;
        public string[] lyrics;
        public float[] cues;
    }

    [System.Serializable]
    public class SongArray{
        public Song[] songArray;
    }

    SongArray songs = new SongArray();

    // END PARSE JSON

    public int songNumber = 0;
    Song song;
    public AudioClip[] audios;
    public AudioClip[] voicedAudios;
    AudioSource audioSource;
    int currentIndex = 0;
    System.DateTime startTime;
    string songDuration;

    public Text lyricsText;
    public Text followupText;
    public Text songDurationText;
    public Slider songSlider;
    public GameObject voicedToggle;
    public GameObject[] playbackSprites;

    void Start(){
        songs = JsonUtility.FromJson<SongArray>(JSONFile.text);
        songNumber = SceneThru.SongId;
        song = songs.songArray[songNumber];
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audios[songNumber];
        songDuration = secondsToTime(audioSource.clip.length);

        startTime = System.DateTime.UtcNow;
        lyricsText.text = song.lyrics[currentIndex];
        followupText.text = song.lyrics[currentIndex + 1];
        songSlider.maxValue = audioSource.clip.length;
        voicedToggle.SetActive(voicedAudios[songNumber] != null);
    }

    void Update(){
        var index = System.Array.FindIndex(song.cues, findNextCue);
        if(index != currentIndex){
            Debug.Log("found index: " + index + ", current index: " + currentIndex);
            lyricsText.text = song.lyrics[index];
            followupText.text = song.lyrics[index + 1];
            currentIndex = index;
        }

        songDurationText.text = secondsToTime(audioSource.time) + " / " + songDuration;
        songSlider.value = audioSource.time;
        if(audioSource.isPlaying){
            playbackSprites[0].SetActive(false);
            playbackSprites[1].SetActive(true);
        } else {
            playbackSprites[0].SetActive(true);
            playbackSprites[1].SetActive(false);
        }
        // if(Input.GetMouseButtonDown(0)){
        //     currentIndex++;
        //     lyricsText.text = song.lyrics[currentIndex];
        // }
    }

    public void toggleVoicedSong(bool voicedOn){
        var currentTime = audioSource.time;
        audioSource.clip = voicedOn ? voicedAudios[songNumber] : audios[songNumber];
        audioSource.time = currentTime;
        audioSource.Play();
    }

    public void movePlayhead(System.Single value){
        audioSource.time = value;
    }

    public void togglePlayback(){
        if(!audioSource.isPlaying){
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }

    private bool findNextCue(float cue){
        return cue > audioSource.time;
    }

    private static string secondsToTime(float seconds){
        string convertedTime = Mathf.Floor(seconds/60).ToString("00") + ":" + Mathf.Floor(seconds%60).ToString("00");
        return convertedTime;
    }
}
