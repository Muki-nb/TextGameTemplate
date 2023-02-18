using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLoader : MonoBehaviour
{
	public static bool hasload = false;
    void Start()
    {
		if(hasload) return;
        AddChoices();
		AddEvents();
		hasload = true;
    }
	
	void AddChoices(){
		new Choice(1){
			imageId = 1,
			text = "寒风！",
			gameEffect = (g)=>{
				
			}
		};
		new Choice(2){
			imageId = 1,
			text = "月亮！",
			gameEffect = (g)=>{
				
			}
		};
		new Choice(3){
			imageId = 1,
			text = "？！",
			gameEffect = (g)=>{
				
			}
		};
	}
	
	void AddEvents(){
		new TextEvent(0){
			imageId = 1,
			getText = (g)=>{
				if(PlayerPrefs.GetInt("History_FinishedTimes",0) > 0) return "今夜：新月";
				return "今夜：朔";
			},
			choiceids = new List<int>(){1,1,1,2,3}
		};
	}
}
