using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	EnemyController _target = null;
	bool _isAttacking = false;

	Animator animator;
	//GameController gameController;	//Reduce coupling between classes using events
	NavMeshAgent navMeshAgent;

	public delegate void PlayerDetected();
	public static event PlayerDetected OnPlayerDetectedEvent;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = transform.FindChild("CHR_M_Viking_A_01").GetComponent<Animator>();
		//gameController = FindObjectOfType<GameController>();
	}

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
					animator.SetTrigger("Attack");
					navMeshAgent.Stop();

					//Invoke("KillTarget", 0.25f);	//Coroutines has less overhead than Invoke method
					StartCoroutine(KillTarget());
				}
			}
		}
		//TODO: Revisar, quizas se puede poner un IF por si acaso
		animator.SetFloat("LocomotionSpeed", navMeshAgent.velocity.magnitude);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator KillTarget()
	{
		yield return new WaitForSeconds(0.25f);
		_target.GetComponent<EnemyController>().OnKilled();
		//gameController.OnEnemyKilled(); //Not necessary, using events to reduce coupling
		_isAttacking = false;
		_target = null;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void GoToPosition(Vector3 position)
	{
		if(!_isAttacking){
			_target = null;
			navMeshAgent.SetDestination(position);
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void GoToEnemy(GameObject enemy)
	{
		if(!_isAttacking){
			_target = enemy.GetComponent<EnemyController>();
			navMeshAgent.SetDestination(enemy.transform.position);
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnDetected()
	{
		animator.SetTrigger("Die");
		navMeshAgent.Stop();
		//gameController.OnPlayerDetected();

		if(OnPlayerDetectedEvent!=null){
			OnPlayerDetectedEvent();
		}
	}
}



