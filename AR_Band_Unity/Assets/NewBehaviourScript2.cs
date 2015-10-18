using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewBehaviourScript2 : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        Debug.Log("I'm alive1");
        StartCoroutine(DoWWW());
        //InvokeRepeating("DoWWW", 1, 1.0F);
        bool isOk = true;
        //JSONObject jObject = new JSONObject(www.text);
    }

    void Example(long calories, int heartRate, long distance, long skinTemperature, int steps)
    {
        GetComponent<TextMesh>().text = "heart rate: " + heartRate + System.Environment.NewLine +
            " calories: " + calories + System.Environment.NewLine +
            " distance: " + distance + System.Environment.NewLine +
            " Skin Temperature: " + skinTemperature + System.Environment.NewLine +
            " Steps: " + steps;
    }

    IEnumerator DoWWW()
    {
        WWW hs_post = new WWW("https://glowing-inferno-4375.firebaseio.com/123.json");
        yield return new WaitForSeconds(1);
        
        Debug.Log(hs_post.text);
        JSONObject user = new JSONObject(hs_post.text);
        long calorie = (long)user["Calorie"].n;
        long distance = (long)user["Distance"].n;
        int heartRate = (int)user["HeartRate"].n;
        long skinTemperature = (long)user["SkinTemperature"].n;
        int steps = (int)user["Steps"].n;
        //Debug.Log("steps:" + steps);

        Example(calorie, heartRate, distance, skinTemperature, steps);

        StartCoroutine(DoWWW());
    }


    // Update is called once per frame
    void Update () {
        //StartCoroutine(DoWWW());
    }
}
