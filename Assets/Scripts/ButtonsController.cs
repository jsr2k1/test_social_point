using UnityEngine;
using System.Collections;

public class ButtonsController : MonoBehaviour
{
	public void OnPlayButtonPressed()
	{
		Application.LoadLevel("01 MainScene");
	}
}
