using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour {

	public GameObject lineUIImagePrefabs;
	private bool m_bshow = false;
	private Vector2 m_fromPos;
	private Vector2 m_toPos;
	private List<GameObject> m_xuxianObjs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(m_bshow)
		{
			float angle = Vector2.Angle(m_fromPos, m_toPos);

			
			GameObject line = newLineUI();
			float linew = line.GetComponent<RectTransform>().rect.width;
			float sin = Mathf.Sin(angle);
			float y = sin*linew;
			float cos = Mathf.Cos(angle);
			float x = cos*linew;
			
			line.transform.position = new Vector3(x, y, 0);
		}
	}

	GameObject newLineUI()
	{
		
		GameObject line = Instantiate(lineUIImagePrefabs);
		return line;
	}


}
