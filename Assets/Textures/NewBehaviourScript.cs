using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	public GameObject xuxianSpritePrefabs;
	private Texture2D t2d;
	void Start () {
		//add test code
		/*t2d =(Texture2D) Resources.Load ("Assets/Textures/xuxian.png") as Texture2D;
		GameObject gameObject = new GameObject(); 
		gameObject.AddComponent<SpriteRenderer>();
		SpriteRenderer spRender = gameObject.GetComponent<SpriteRenderer>();
		Rect rect = new Rect(0, 0, 1080, 1920);
		Sprite sp = Sprite.Create(t2d, rect, new Vector2(0, 0));
		spRender.sprite = sp;
		gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -8);
		 */
		Debug.Log("ttttttt");

		var logoObject = Instantiate(xuxianSpritePrefabs);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
