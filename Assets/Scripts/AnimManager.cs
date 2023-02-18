using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Anim
{
	public IEnumerator Play(AnimManager animManager){
		yield return animManager.StartCoroutine(Effect(animManager));
		yield return animManager.StartCoroutine(End(animManager));
		yield return 0;
	}
	
	public virtual IEnumerator Effect(AnimManager animManager){
		yield return 0;
	}
	
	public IEnumerator End(AnimManager animManager){
		animManager.isplaying = false;
		yield return 0;
	}
}

public class TypingAnim : Anim
{
	public static TypingAnim Empty = new TypingAnim(){text = ""};
	public string text;
	public override IEnumerator Effect(AnimManager animManager){
		var waitingTime = new WaitForSeconds(0.05f);
		var waitingTime2 = new WaitForSeconds(0.175f);
		int length = 0;
		string temp = "";
		if(text != ""){
			animManager.SetText("...");
			yield return waitingTime;
		}
		while(length < text.Length){
			int n = Random.Range(1,6);
			if(length + n > text.Length) n = text.Length - length;
			for(int i = 0;i < n;i++){
				temp += text[length++];
				yield return waitingTime;
			}
			animManager.SetText(temp + "_");
			yield return waitingTime2;
		}
		animManager.SetText(temp);
		yield return 0;
	}
}

public class ChangePictureAnim : Anim
{
	public int picId;
	public override IEnumerator Effect(AnimManager animManager){
		var waitingTime = new WaitForSeconds(0.01f);
		GameObject gameObject = GameObject.Instantiate(animManager.nowImage.gameObject,animManager.pastImages,false);
		animManager.pastGameObjects.Add(gameObject);
		if(animManager.pastGameObjects.Count > 7){
			GameObject.Destroy(animManager.pastGameObjects[0]);
			animManager.pastGameObjects.RemoveAt(0);
		}
		Transform transform = gameObject.transform;
		Image image = gameObject.GetComponent<Image>();
		animManager.nowImage.gameObject.SetActive(false);
		Sprite sprite = ResourceManager.instance.photos.GetImage(picId);
		animManager.nextImage.sprite = sprite;
		yield return 0;
		float angle = (Random.Range(0f,12.5f) + 2.5f) * (Random.Range(0,2) == 1 ? 1 : -1);
		const int n = 20;
		Vector3 angle3 = new Vector3(0,0,angle/n);
		Vector3 scale = new Vector3(0.1f/n,0.1f/n,0.1f/n);
		for(int i = 0;i < n;i++){
			transform.Rotate(angle3);
			transform.localScale -= scale;
			float c = 1f - ((255 - 150) / 255f)*(i + 1)/((float)n);
			image.color = new Color(c,c,c);
			yield return waitingTime;
		}
		animManager.nextImage.gameObject.SetActive(true);
		animManager.nextImageAnimator.SetTrigger("Appear");
		yield return new WaitForSeconds(0.5f);
		animManager.nextImage.gameObject.SetActive(false);
		animManager.nowImage.gameObject.SetActive(true);
		animManager.nowImage.sprite = sprite;
		yield return 0;
	}
}

public class ChangeBackgroundAnim : Anim
{
	public int picId;
	public override IEnumerator Effect(AnimManager animManager){
		animManager.backgroundImage.sprite = ResourceManager.instance.backgrounds.GetImage(picId);
		yield return 0;
	}
}

public class ShowChoicesAnim : Anim
{
	public List<Choice> choices;
	public override IEnumerator Effect(AnimManager animManager){
		Game game = animManager.gameManager;
		foreach(GameObject gameObject in game.choicesGameObjects) GameObject.Destroy(gameObject);
		game.choicesGameObjects.Clear();
		game.buildChoices(choices);
		int n = game.choicesGameObjects.Count;
		/*
		var waitingTime = new WaitForSeconds(0.01f);
		for(int i = 0;i <= 20;i++){
			for(int j = 0;j < n;j++){
				game.choicesGameObjects[j].transform.localPosition = new Vector3(0,(j - (n - 1)/2f) * (i/20f) * 160f,0);
			}
			yield return waitingTime;
		}*/
		float length = 600f/n > 160f ? 160f : 600f/n;
		for(int j = 0;j < n;j++){
			game.choicesGameObjects[j].transform.localPosition = new Vector3(0,-(j - (n - 1)/2f) * length,0);
		}
		yield return 0;
	}
}

public class AnimManager : MonoBehaviour
{
	public Game gameManager;
	
	public Image backgroundImage;
	
	public Image nowImage;
	public Image nextImage;
	public Transform pastImages;
	public List<GameObject> pastGameObjects = new List<GameObject>();
	
	public Animator nextImageAnimator;
	public List<Anim> anims = new List<Anim>();
	public bool isplaying;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isplaying) return;
		if(anims.Count == 0) return;
		StartCoroutine(anims[0].Play(this));
		anims.RemoveAt(0);
		isplaying = true;
    }
	
	public void AttachAnim(Anim anim){
		anims.Add(anim);
	}
	
	public Text text;
	public Text text2;
	public void SetText(string text){
		this.text.text = this.text2.text = text;
	}
	
	public void ShowChoices(List<Choice> choices){
		AttachAnim(new ShowChoicesAnim{
			choices = choices
		});
	}
	
}
