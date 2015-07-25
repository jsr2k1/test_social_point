using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Script_FPS : MonoBehaviour
{
	public float updateInterval = 1.0f;

	float accum = 0; // FPS accumulated over the interval
	int frames = 0; // Frames drawn over the interval
	float timeleft; // Left time for current interval
	string sFPS, sMinFPS, sMaxFPS;
	float minFPS=99999999999;
	float maxFPS=0;
	
	Text text_fps;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		text_fps = GetComponent<Text>();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		timeleft = updateInterval;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		if(timeleft <= 0.0){
			float fps = accum / frames;

			minFPS = Mathf.Min(minFPS, fps);
			maxFPS = Mathf.Max(maxFPS, fps);

			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
			
			text_fps.text = "FPS: " + (int)fps + " Min: " + (int)minFPS + " Max: " + (int)maxFPS;
		}
		//reset all values
		if(Input.GetKeyDown(KeyCode.R)){
			minFPS=999999;
			maxFPS=0;
		}
	}
}

