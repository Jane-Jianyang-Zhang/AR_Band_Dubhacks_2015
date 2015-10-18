using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewBehaviourScript1 : MonoBehaviour {
    UnityEngine.UI.Text txt;
    private int currentscore = 0;
    void Start () {
        Debug.Log(gameObject.name);
        Debug.Log("live");
        //txt = GetComponent<UnityEngine.UI.Text>();
        //txt.text = "Score : " + currentscore;

    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("live");
        txt.text = "Score : ";
        Debug.Log("live");
        //currentscore = PlayerPrefs.GetInt("TOTALSCORE");
        //PlayerPrefs.SetInt("SHOWSTARTSCORE", currentscore);
    }
}
