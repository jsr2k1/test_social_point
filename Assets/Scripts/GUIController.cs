using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour
{
	public Text countDownSec;
	public Text countDownMiliSec;
	public Text startGame;
	public Text endGameWin;
	public Text endGameLost_PlayerDetected;
	public Text endGameLost_CorpseDetected;
	public Text endGameLost_TimeOut;

	public GameObject background;

	//bool _isPlaying = false;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		background.SetActive(true);
		startGame.gameObject.SetActive(true);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		int sec =(int)FindObjectOfType<GameController>().getCountDown();
		int milli =(int)((FindObjectOfType<GameController>().getCountDown() - sec) * 100);
		countDownSec.text = sec.ToString("00");
		countDownMiliSec.text = milli.ToString("00");
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnStartGame()
	{
		//_isPlaying = true;
		background.SetActive(false);
		startGame.gameObject.SetActive(false);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnEndGame(GameController.EGameState state)
	{
		//_isPlaying = false;
		background.SetActive(true);

		switch(state)
		{
			case GameController.EGameState.E_END_GAME_WIN: 
				endGameWin.gameObject.SetActive(true); 
				break;
			case GameController.EGameState.E_END_GAME_PLAYER_DETECTED: 
				endGameLost_PlayerDetected.gameObject.SetActive(true); 
				break;
			case GameController.EGameState.E_END_GAME_CORPSE_DETECTED: 
				endGameLost_CorpseDetected.gameObject.SetActive(true); 
				break;
			case GameController.EGameState.E_END_GAME_TIMEOUT: 
				endGameLost_TimeOut.gameObject.SetActive(true); 
				break;
		}
	}
}


