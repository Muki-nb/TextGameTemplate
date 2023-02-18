using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEvent
{
	public static Dictionary<int,TextEvent> events = new Dictionary<int,TextEvent>();
	public static TextEvent GetEvent(int id){
		if(events.ContainsKey(id)) return events[id];
		return events[-1];
	}
	
    public int id;
	
	public int imageId;
	public string text;
    public GetStr getText;
	
	public List<int> choiceids;
	public List<Choice> choices;
	public GetChoices getChoices;
	
	public TextEvent(int id){
		this.id = id;
		events[id] = this;
	}
	
	public string GetText(Game g){
		if(getText == null) return text;
		return getText(g);
	}
	
	public List<Choice> GetChoices(Game g){
		if(getChoices == null) 
			if(choices == null){
				List<Choice> results = new List<Choice>();
				foreach(int id in choiceids) results.Add(Choice.GetChoice(id));
				return results;
			}
			else 
				return choices;
		return getChoices(g);
	}
}
