using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONWriter : MonoBehaviour
{
    [System.Serializable]
    public class SongTiming {
        public string country;
        public List<float> cues = new List<float>();
    }

    public SongTiming songTiming = new SongTiming();

    public void writeJSON(){
        string strOut = JsonUtility.ToJson(songTiming);
        File.WriteAllText(Application.dataPath + "/timings/malta.txt", strOut);
    }

    System.DateTime startTime;
    int currentIndex = 0;

    void Start(){
        startTime = System.DateTime.UtcNow;
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            currentIndex++;

            System.TimeSpan ts = System.DateTime.UtcNow - startTime;
            Debug.Log((float) ts.TotalSeconds);
            songTiming.cues.Add((float) ts.TotalSeconds);
        }
        if(Input.GetMouseButtonDown(2)){
            songTiming.country = "malta";
            writeJSON();
        }
    }
}
