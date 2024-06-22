using Core.Interactions;
using UnityEngine;
using DataSources;
using Gameplay;
using Characters;

namespace AI
{
    public class EnemyBrain : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<PlayerController> playerDataSource;

        [Header("Parameters")]
        [Header("Attack")]
        [SerializeField] private float attackDistance;

        private Character _target; //CHANGED BASE CODE W/ TEACHER'S PERMISSION
        private ISteerable _steerable;

        private void Awake()
        {
            _steerable = GetComponent<ISteerable>();

            if(_steerable == null)
            {
                Debug.LogError($"{name}: cannot find a {nameof(ISteerable)} component!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!playerDataSource)
            {
                Debug.LogError($"{name}: {nameof(playerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            if (playerDataSource.Value != null)
            {
                var playerController = playerDataSource.Value;
                _target = playerController.Character;
            }
        }

        private void Update()
        {
            //TODO: Add logic to get the target from a source/reference system | DONE
            if (_target == null)
                return;

            var directionToTarget = _target.transform.position - transform.position;
            var distanceToTarget = directionToTarget.magnitude;
            if (distanceToTarget < attackDistance)
            {
                _target.ReceiveAttack();
            }
            else
            {
                Debug.DrawRay(transform.position, directionToTarget.normalized, Color.red);
                _steerable.SetDirection(directionToTarget.normalized);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
}
