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
	GameController gameController;

	public Animator levelFailedPopUp;
	public Animator levelCompletedPopUp;

	//bool _isPlaying = false;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		gameController = FindObjectOfType<GameController>();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		background.SetActive(true);
		startGame.gameObject.SetActive(true);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		int sec =(int)gameController.getCountDown();
		int milli =(int)((gameController.getCountDown() - sec) * 100);
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
				//endGameWin.gameObject.SetActive(true);
				StartCoroutine(ShowLevelCompletedPopUp());
				break;
			case GameController.EGameState.E_END_GAME_PLAYER_DETECTED: 
				//endGameLost_PlayerDetected.gameObject.SetActive(true);
				StartCoroutine(ShowLevelFailedPopUp());
				break;
			case GameController.EGameState.E_END_GAME_CORPSE_DETECTED: 
				//endGameLost_CorpseDetected.gameObject.SetActive(true); 
				StartCoroutine(ShowLevelFailedPopUp());
				break;
			case GameController.EGameState.E_END_GAME_TIMEOUT: 
				//endGameLost_TimeOut.gameObject.SetActive(true); 
				StartCoroutine(ShowLevelFailedPopUp());
				break;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator ShowLevelFailedPopUp()
	{
		yield return new WaitForSeconds(1.0f);
		levelFailedPopUp.SetTrigger("ShowPopUp");
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	IEnumerator ShowLevelCompletedPopUp()
	{
		yield return new WaitForSeconds(1.0f);
		levelCompletedPopUp.SetTrigger("ShowPopUp");
	}
}






