using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	EnemyController _target = null;
	bool _isAttacking = false;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		//Getting closer to an enemy
		if(_target != null){
			if(!_isAttacking){
				float distance = Vector3.Distance(transform.position, _target.transform.position);

				//Close enought to an enemy -> Attack
				if(distance < 3f){
					_isAttacking = true;
					transform.FindChild("CHR_M_Viking_A_01").GetComponent<Animator>().SetTrigger("Attack"); //TODO: Obtener la referencia en el Start
					GetComponent<NavMeshAgent>().Stop();
					Invoke("killTarget", 0.25f);
				}
			}
		}
		transform.FindChild("CHR_M_Viking_A_01").GetComponent<Animator>().SetFloat("LocomotionSpeed", GetComponent<NavMeshAgent>().velocity.magnitude);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void killTarget()
	{
		_target.GetComponent<EnemyController>().OnKilled();
		FindObjectOfType<GameController>().OnEnemyKilled();
		_isAttacking = false;
		_target = null;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void goToPosition(Vector3 position)
	{
		if(!_isAttacking){
			_target = null;
			GetComponent<NavMeshAgent>().SetDestination(position);
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void goToEnemy(GameObject enemy)
	{
		if(!_isAttacking){
			_target = enemy.GetComponent<EnemyController>();
			GetComponent<NavMeshAgent>().SetDestination(enemy.transform.position);
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnDetected()
	{
		transform.FindChild("CHR_M_Viking_A_01").GetComponent<Animator>().SetTrigger("Die");
		GetComponent<NavMeshAgent>().Stop();
		FindObjectOfType<GameController>().OnPlayerKilled();
	}
}



