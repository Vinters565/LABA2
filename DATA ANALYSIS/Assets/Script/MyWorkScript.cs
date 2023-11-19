using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class MyWorkScript : MonoBehaviour
{
    public AudioClip niceClip;
    public AudioClip goodSpeak;
    public AudioClip normalSpeak;
    public AudioClip badSpeak;
    public GameObject hit;
    private AudioSource selectAudio;
    private Dictionary<string, float> dataSet = new Dictionary<string, float>();
    private List<(float, float)> coordinatSet = new List<(float, float)>();
    private bool statusStart = false;
    private int i = 1;

    void Start()
    {
        StartCoroutine(GoogleSheets());
    }

    void Update()
    {
        if (i > dataSet.Count) return;

        hit.gameObject.transform.SetPositionAndRotation(new Vector3(coordinatSet[i - 1].Item1, coordinatSet[i - 1].Item2, 100), Quaternion.Euler(0,0,0));
        
        if (dataSet["Mon_" + i.ToString()] <= 10 & statusStart == false & i <= dataSet.Count)
        {
            StartCoroutine(PlaySelectAudioNice());
            Instantiate(hit);
            Debug.Log(dataSet["Mon_" + i.ToString()]);
        }

        if (dataSet["Mon_" + i.ToString()] > 10 & dataSet["Mon_" + i.ToString()] < 30 & statusStart == false & i <= dataSet.Count)
        {
            StartCoroutine(PlaySelectAudioGood());
            Instantiate(hit);
            Debug.Log(dataSet["Mon_" + i.ToString()]);
        }

        if (dataSet["Mon_" + i.ToString()] >= 30 & dataSet["Mon_" + i.ToString()] < 45 & statusStart == false & i <= dataSet.Count)
        {
            StartCoroutine(PlaySelectAudioNormal());
            Instantiate(hit);
            Debug.Log(dataSet["Mon_" + i.ToString()]);
        }

        if (dataSet["Mon_" + i.ToString()] >= 45 & statusStart == false & i <= dataSet.Count)
        {
            StartCoroutine(PlaySelectAudioBad());
            Instantiate(hit);
            Debug.Log(dataSet["Mon_" + i.ToString()]);
        }
    }

    IEnumerator GoogleSheets()
    {
        UnityWebRequest curentResp = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1bOCGF25JTi4I7fgY0GAodgUYyeQ2oQ6B9q3JWTWAhek/values/Лист1?key=AIzaSyAB9EsNUhAQKZlMth67FcRO_0L72TOh0x4");
        yield return curentResp.SendWebRequest();
        string rawResp = curentResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);
        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList;
            dataSet.Add(("Mon_" + selectRow[0]), float.Parse(selectRow[3]));
            coordinatSet.Add((float.Parse(selectRow[1]), float.Parse(selectRow[2])));
        }
    }

    IEnumerator PlaySelectAudioNice()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = niceClip;
        selectAudio.Play();
        yield return new WaitForSeconds(3);
        statusStart = false;
        i++;
    }

    IEnumerator PlaySelectAudioGood()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = goodSpeak;
        selectAudio.Play();
        yield return new WaitForSeconds(3);
        statusStart = false;
        i++;
    }
    IEnumerator PlaySelectAudioNormal()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = normalSpeak;
        selectAudio.Play();
        yield return new WaitForSeconds(3);
        statusStart = false;
        i++;
    }
    IEnumerator PlaySelectAudioBad()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = badSpeak;
        selectAudio.Play();
        yield return new WaitForSeconds(4);
        statusStart = false;
        i++;
    }
}
