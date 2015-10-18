///instruction/////
////this can be used in creating numeric representation of the heart art rate data.
///step 1: Create > Text
///step 2: Add Component > Script > C#
///step 3: Paste following codes

//////////////////////////
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeartRate2 : MonoBehaviour
{

    Text txt2;
    //	public string url = "https://glowing-inferno-4375.firebaseio.com/123";

    System.Timers.Timer timer2 = new System.Timers.Timer(1000);

    // Use this for initialization
    void Start()
    {
        //timer.Elapsed += timerElapsed;
        WWW www = new WWW("https://glowing-inferno-4375.firebaseio.com/123"); // there is a bug here
                                                                              // ERROR MESSAGE: InitWWW can only be called from the main thread.
                                                                              // Constructors and field initializers will be executed from the loading thread when loading a scene.
                                                                              //Don't use this function in the constructor or field initializers, 
                                                                              //instead move initialization code to the Awake or Start function.

        string heartRate2 = "hello";
       // JSONObject jObject = new JSONObject(www.text);
       // string heartRate2 = jObject["heartRate"].ToString();
        // Display your heart rate
        txt2.text = heartRate2;
        txt2.fontSize = 100;

        txt2 = GetComponent<Text>();
        timer2.Start();
    }

    // Update is called once per frame
    void Update()
    {


    }
    /*
    void timerElapsed(object o, System.Timers.ElapsedEventArgs args)
    {



        WWW www = new WWW("https://glowing-inferno-4375.firebaseio.com/123"); // there is a bug here
                                                                              // ERROR MESSAGE: InitWWW can only be called from the main thread.
                                                                              // Constructors and field initializers will be executed from the loading thread when loading a scene.
                                                                              //Don't use this function in the constructor or field initializers, 
                                                                              //instead move initialization code to the Awake or Start function.


        JSONObject jObject = new JSONObject(www.text);
        string heartRate = jObject["heartRate"].ToString();
        // Display your heart rate
        txt.text = heartRate;
    }*/


}

