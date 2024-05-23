using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Interactions;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private float attackDistance;
    private ITarget _target;
    private ISteerable _directable;

    private void Awake()
    {
        _directable = GetComponent<ISteerable>();
        if( _directable == null)
        {
            Debug.LogError($"{name}: cannot find a {nameof(ISteerable)} component!");
            enabled = false;
        }
    }

    private void Update()
    {
        //TODO: Add logic to get the target from a source/reference system
        if (_target == null)
            return;
        //          AB        =         B        -          A
        var directionToTarget = _target.transform.position - transform.position;
        var distanceToTarget = directionToTarget.magnitude;
        if (distanceToTarget < attackDistance)
        {
            _target.ReceiveAttack();
        }
        else
        {
            _directable.SetDirection(directionToTarget);
        }
    }
}
