using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	public static ResourceManager instance;
	public List<Sprite> images;
	
	public class ImagesManager
	{
		public Dictionary<int,Sprite> imageData = new Dictionary<int,Sprite>();
		public Sprite undefined = null;
		public Sprite GetImage(int id){
			if(imageData.ContainsKey(id)) return imageData[id];
			return undefined;
		}
		public string name;
		public void Load(Dictionary<int,string> data){
			foreach(var pair in data){
				imageData[pair.Key] = Resources.Load<Sprite>("Images/" + name + "/" + pair.Value);
			}
		}
	}
	
	public ImagesManager backgrounds = new ImagesManager{
		name = "backgrounds"
	};
	public ImagesManager photos = new ImagesManager{
		name = "photos"
	};
	public ImagesManager icons = new ImagesManager{
		name = "icons"
	};
	public ImagesManager endings = new ImagesManager{
		name = "endings"
	};
	
    
	void Awake()
	{
		if(instance == null)
			instance = this;
	}
	// Start is called before the first frame update
    void Start()
    {
        photos.Load(new Dictionary<int,string>(){
			{1 , "start"}
		});
        icons.Load(new Dictionary<int,string>(){
			{1 , "!"}
		});
    }

    
}
