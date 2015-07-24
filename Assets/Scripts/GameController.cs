using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public PlayerController playerController;
	GUIController guiController;

	public enum EGameState{
		E_START_GAME,
		E_INGAME,
		E_END_GAME_WIN,
		E_END_GAME_PLAYER_DETECTED,
		E_END_GAME_CORPSE_DETECTED,
		E_END_GAME_TIMEOUT
	}
	int _remainingEnemies;
	EnemyController[] _aEnemies;
	float _countDownValue = 1500f;	//TODO: Change this to 15
	EGameState _currentState = EGameState.E_START_GAME;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		Input.simulateMouseWithTouches = true;
		_aEnemies = FindObjectsOfType<EnemyController>();
		_remainingEnemies = _aEnemies.Length;
		guiController = FindObjectOfType<GUIController>();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnEnable()
	{
		EnemyController.OnEnemyKilledEvent += OnEnemyKilled;
		EnemyController.OnCorpseDetectedEvent += OnCorpseDetected;
		PlayerController.OnPlayerDetectedEvent += OnPlayerDetected;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
		EnemyController.OnEnemyKilledEvent -= OnEnemyKilled;
		EnemyController.OnCorpseDetectedEvent -= OnCorpseDetected;
		PlayerController.OnPlayerDetectedEvent -= OnPlayerDetected;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		//Starting the game
		if(_currentState == EGameState.E_START_GAME){
			if(Input.GetMouseButtonDown(0)){
				OnStartGame();
			}
		}
		//Playing the game
		else if(_currentState == EGameState.E_INGAME)
		{
			_countDownValue -= Time.deltaTime;

			//Time is running
			if(_countDownValue > 0){
				if(Input.GetMouseButtonDown(0)){
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if(Physics.Raycast(ray, out hit)){
						//Attack the enemy
						if(hit.collider.tag == "Enemy"){
							playerController.GoToEnemy(hit.collider.gameObject);
						}//Move to position
						else{
							playerController.GoToPosition(hit.point);
						}
					}
				}
			}
			//Time out -> Game Over
			else{
				_countDownValue = 0f;
				OnTimeOut();
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnStartGame()
	{
		guiController.OnStartGame();
		_currentState = EGameState.E_INGAME;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnEnemyKilled()
	{
		_remainingEnemies--;

		//All enemies are dead
		if(_remainingEnemies <= 0){
			_currentState = EGameState.E_END_GAME_WIN;
			guiController.OnEndGame(_currentState);

			//Not necessary because all coroutines are disabled when an enemy has been killed
			//Disable all enemies
			//foreach(EnemyController enemy in getEnemies()){
			//	enemy.OnEndGame();
			//}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnPlayerDetected()
	{
		_currentState = EGameState.E_END_GAME_PLAYER_DETECTED;
		guiController.OnEndGame(_currentState);

		//Not necessary because now all enemies are subscribed to the OnPlayerDetectedEvent
		//Disable all enemies
		//foreach(EnemyController enemy in getEnemies()){
		//	enemy.OnEndGame();
		//}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnCorpseDetected()
	{
		_currentState = EGameState.E_END_GAME_CORPSE_DETECTED;
		guiController.OnEndGame(_currentState);

		//Not necessary because now all enemies are subscribed to the OnCorpseDetectedEvent
		//Disable all enemies
		//foreach(EnemyController enemy in getEnemies()){
		//	enemy.OnEndGame();
		//}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnTimeOut()
	{
		_currentState = EGameState.E_END_GAME_TIMEOUT;
		guiController.OnEndGame(_currentState);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public EnemyController[] getEnemies()
	{
		return _aEnemies;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public float getCountDown()
	{
		return _countDownValue;
	}
}


