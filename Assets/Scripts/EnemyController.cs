using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
	public GameObject detectFX;
	public float[] targetAngles;
	public float timeToChange = 2f;
	int _currentAngle = 0;
	bool _isDead = false;

	PlayerController playerController;
	GameController gameController;
	Animator animator;
	
	public delegate void EnemyKilled();
	public static event EnemyKilled OnEnemyKilledEvent;

	public delegate void CorpseDetected();
	public static event CorpseDetected OnCorpseDetectedEvent;

	public int attackStyle;
	bool bEndGame=false;

	//Patrol
	NavMeshAgent navMeshAgent;
	public Transform[] patrolPoints;
	Vector3 targetPatrolPoint;
	int currentPatrolPoint=0;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();
		gameController = FindObjectOfType<GameController>();
		animator = transform.FindChild("CHR_M_OldRanged_A_02").GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		//StartCoroutine(CO_ChangeOrientation());
		targetPatrolPoint = patrolPoints[0].position;
		navMeshAgent.SetDestination(targetPatrolPoint);
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnEnable()
	{
		PlayerController.OnPlayerDetectedEvent += OnEndGame;
		EnemyController.OnCorpseDetectedEvent += OnEndGame;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnDisable()
	{
		PlayerController.OnPlayerDetectedEvent -= OnEndGame;
		EnemyController.OnCorpseDetectedEvent -= OnEndGame;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(!_isDead && !bEndGame){
			//Check player in range
			if(targetInRange(playerController.transform.position)){
				animator.SetTrigger(attackStyle==1 ? "Attack1" : "Attack2");
				playerController.OnDetected();
			}
			//Check corpse in range
			foreach(EnemyController enemy in gameController.getEnemies()){
				if(enemy.isDead()){
					if(targetInRange(enemy.transform.position)){
						//gameController.OnCorpseDetected();	//Reduce coupling between classes
						if(OnCorpseDetectedEvent!=null){
							OnCorpseDetectedEvent();
						}
					}
				}
			}
			//Patrol
			float distance = Vector3.Distance(transform.position, targetPatrolPoint);
			if(distance < 0.2f){
				currentPatrolPoint = (currentPatrolPoint+1)%patrolPoints.Length;
				targetPatrolPoint = patrolPoints[currentPatrolPoint].position;
				navMeshAgent.SetDestination(targetPatrolPoint);
			}
			animator.SetFloat("LocomotionSpeed", navMeshAgent.velocity.magnitude);
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//TODO: Mejorar la rotacion de los enemigos. Añadir un poco de random?
	IEnumerator CO_ChangeOrientation()
	{
		yield return new WaitForSeconds(Random.Range(0f, 2f));

		while(true){
			transform.eulerAngles = new Vector3(0f,targetAngles[_currentAngle], 0f);
			yield return  new WaitForSeconds(timeToChange);		
			_currentAngle++;
			if(_currentAngle >= targetAngles.Length){
				_currentAngle = 0;
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnKilled()
	{
		animator.SetTrigger("Die");
		StopAllCoroutines();
		navMeshAgent.Stop();
		detectFX.SetActive(false); //TODO: Revisar FX
		_isDead = true;

		if(OnEnemyKilledEvent!=null){
			OnEnemyKilledEvent();
		}
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
		bEndGame=true;
	}
}



