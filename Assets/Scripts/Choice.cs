using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate string GetStr(Game g);
public delegate bool GetBool(Game g);
public delegate int GetInt(Game g);
public delegate List<Choice> GetChoices(Game g);
public delegate void GameAction(Game g);

public class Choice
{
	public static Dictionary<int,Choice> choices = new Dictionary<int,Choice>();
	public static Choice GetChoice(int id){
		if(choices.ContainsKey(id)) return choices[id];
		return choices[-1];
	}
	
	public int id;
	public int imageId;
	public string text;
    public GetStr getText;
	public GetBool condition;
	public GameAction gameEffect;
	
	public Choice(int id){
		this.id = id;
		choices[id] = this;
	}
	
	public string GetText(Game g){
		if(getText == null) return text;
		return getText(g);
	}
	
	public bool Condition(Game g){
		if(condition == null) return true;
		return condition(g);
	}
	
	public void Effect(Game g){
		if(gameEffect != null) gameEffect(g);
	}
}
