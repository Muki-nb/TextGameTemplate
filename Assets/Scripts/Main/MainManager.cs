using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
	public GameObject continueGame;
    void Start()
    {
		Application.targetFrameRate = 90;
        if(PlayerPrefs.GetInt("Game_Saves",0) == 1) continueGame.SetActive(true);
    }
	
	public void LoadScene(string name){
		SceneManager.LoadScene(name);
	}
	
	public void NewGame(){
		PlayerPrefs.SetInt("Game_Saves",1);
		PlayerPrefs.SetInt("Game_NowEventId",0);
	}
}
