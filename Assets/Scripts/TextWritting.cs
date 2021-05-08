using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class TextWritting: MonoBehaviour
{

	Text txt;
	string story;

	void Awake()
	{
		txt = GetComponent<Text>();
		story = "Please Upload the game to the GGJ Platform before 15:00";
		txt.text = "";

		StartCoroutine("PlayText");
	}

	IEnumerator PlayText()
	{
		foreach (char c in story)
		{
			txt.text += c;
			yield return new WaitForSeconds(0.05f);
		}
	}

}
