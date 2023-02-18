using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending
{
	public static Dictionary<string,Ending> endings = new Dictionary<string,Ending>();
	public static Ending GetEnding(string id){
		if(endings.ContainsKey(id)) return endings[id];
		return endings["ERROR"];
	}
    public string id;
	public int imageId;
	public int backgroundId;
	public string text;
    public GetStr getText;
	public string GetText(Game g){
		if(getText == null) return text;
		return getText(g);
	}
	public Ending(string id){
		this.id = id;
		endings[id] = this;
	}
}
