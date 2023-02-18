using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
	public AnimManager animManager;
	
    void Start()
    {
        if(PlayerPrefs.GetInt("Game_Saves",0) == 1){
			Load();
		}else{
			SceneManager.LoadScene("Main");
		}
    }
	
	public TextEvent nowEvent;
	
	public event GameAction EndTurnEvent;
	public void OnEndTurn(){
		if(EndTurnEvent != null) EndTurnEvent(this);
	}
	public void AttachEndTurnEffect(GameAction effect){
		EndTurnEvent += effect;
	}
	
	Dictionary<string,int> data = new Dictionary<string,int>();
	
	public int GetData(string key){
		if(data.ContainsKey(key)) return data[key];
		return 0;
	}
	
	public int SetData(string key,int val, bool overwrite = true){
		if(!data.ContainsKey(key) || overwrite) return data[key] = val;
		return -val;
	}
	
	bool hasSetEvent = false;
	public void SetEvent(int id, bool overwrite = true){
		if(hasSetEvent && !overwrite) return;
		hasSetEvent = true;
		nowEvent = TextEvent.GetEvent(id);
	}
	
	public void ReFresh(){
		animManager.AttachAnim(TypingAnim.Empty);
		animManager.AttachAnim(new ChangePictureAnim(){
			picId = nowEvent.imageId
		});
		animManager.AttachAnim(new TypingAnim(){
			text = nowEvent.GetText(this)
		});
		animManager.AttachAnim(new ShowChoicesAnim(){
			choices = nowEvent.GetChoices(this)
		});
		Save();
	}
	
	void Save(){
		if(hasend){
			ClearData();
			return;
		}
		string keys = "";
		foreach(var pair in data){
			keys += pair.Key + "\n";
			PlayerPrefs.SetInt("Game_Data_" + pair.Key,pair.Value);
		}
		PlayerPrefs.SetString("Game_DataKeys",keys);
		PlayerPrefs.SetInt("Game_NowEventId",nowEvent.id);
		PlayerPrefs.SetInt("Game_Saves",1);
		PlayerPrefs.Save();
	}
	
	void Load(){
		string keys = PlayerPrefs.GetString("Game_DataKeys","");
		foreach(var key in keys.Split("\n")){
			if(key == "") continue;
			data[key] = PlayerPrefs.GetInt("Game_Data_" + key,0);
		}
		nowEvent = TextEvent.GetEvent(PlayerPrefs.GetInt("Game_NowEventId",0));
		ReFresh();
	}
	
	void ClearData(){
		PlayerPrefs.DeleteKey("Game_NowEventId");
		PlayerPrefs.DeleteKey("Game_DataKeys");
		PlayerPrefs.SetInt("Game_Saves",0);
	}
	
	public List<GameObject> choicesGameObjects = new List<GameObject>();
	public GameObject choiceInstance;
	public Transform choicesTransform;
	public void buildChoices(List<Choice> choices){
		List<GameObject> gameObjects = new List<GameObject>();
		foreach(var choice in choices){
			GameObject gameObject = buildChoice(choice);
			Vector3 v = gameObject.transform.localPosition;
			gameObjects.Add(gameObject);
		}
		choicesGameObjects.AddRange(gameObjects);
		
	}
	public GameObject buildChoice(Choice choice){
		string text = choice.GetText(this);
		Sprite sprite = ResourceManager.instance.icons.GetImage(choice.imageId);
		GameObject gameObject = GameObject.Instantiate(choiceInstance,choicesTransform,false);
		gameObject.transform.Find("Text").GetComponent<TMP_Text>().text = text;
		gameObject.transform.Find("Image").GetComponent<Image>().sprite = sprite;
		gameObject.GetComponent<Button>().interactable = choice.Condition(this);
		gameObject.GetComponent<Button>().onClick.AddListener(()=>{this.Choose(choice);});
		int line = text.Length <= 13 ? 1 : (text.Length <= 38 ? 2 : (text.Length + 18) / 19);
		Vector2 v = gameObject.GetComponent<RectTransform>().sizeDelta;
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(v.x,line <= 2 ? 120 : 60*line);
		gameObject.SetActive(true);
		return gameObject;
	}
	
	bool hasend = false;
	public void End(string end){
		Ending ending = Ending.GetEnding(end);
		hasend = true;
	}
	
	public void Choose(Choice choice){
		foreach(GameObject gameObject in choicesGameObjects) gameObject.GetComponent<Button>().interactable = false;
		choice.Effect(this);
		OnEndTurn();
		ReFresh();
	}
	
	public void ChangeBackground(int picId){
		animManager.AttachAnim(new ChangeBackgroundAnim(){
			picId = picId
		});
	}
}
