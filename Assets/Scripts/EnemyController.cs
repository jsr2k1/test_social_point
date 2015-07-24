using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
	public GameObject detectFX;
	public float[] targetAngles;
	public float timeToChange = 2f;
	int _currentAngle = 0;
	bool _isDead = false;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		StartCoroutine(CO_ChangeOrientation());
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(!_isDead){
			//Check player in range
			if(targetInRange(FindObjectOfType<PlayerController>().transform.position)){ //TODO: Coger en el Start
				FindObjectOfType<PlayerController>().OnDetected();
			}
			//Check corpse in range
			foreach(EnemyController enemy in FindObjectOfType<GameController>().getEnemies()){ //TODO: Coger en el Start
				if(enemy.isDead()){
					if(targetInRange(enemy.transform.position)){
						FindObjectOfType<GameController>().OnCorpseDetected();
					}
				}
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//TODO: Mejorar la rotacion de los enemigos. Añadir un poco de random?
	IEnumerator CO_ChangeOrientation()
	{
		yield return  new WaitForSeconds(Random.Range(0f, 2f));

		while(true){
			transform.eulerAngles = new Vector3(0f,targetAngles[_currentAngle], 0f);
			yield return  new WaitForSeconds(timeToChange);		
			_currentAngle++;
			if(_currentAngle >= targetAngles.Length)
			{
				_currentAngle = 0;
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnKilled()
	{
		transform.FindChild("CHR_M_OldRanged_A_02").GetComponent<Animator>().SetTrigger("Die"); //TODO: Coger en el Start
		StopAllCoroutines();
		detectFX.SetActive(false); //TODO: Revisar FX
		_isDead = true;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public bool isDead()
	{
		return _isDead;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	bool targetInRange(Vector3 targetPos)
	{
		Vector3 toPlayerVector = targetPos - transform.position;
		return(toPlayerVector.magnitude < 5f && Vector3.Angle(toPlayerVector, transform.forward) < 15f);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnEndGame()
	{
		StopAllCoroutines();
	}
}



