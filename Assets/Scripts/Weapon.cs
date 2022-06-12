using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] 
    private int baseDamage;
    
    private BoxCollider _collider;

    [SerializeField] 
    private GameObject wielder;

    private Animator _wielderAnimator;

    private List<GameObject> _alreadyHit;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
        _wielderAnimator = wielder.GetComponent<Animator>();
        _alreadyHit = new List<GameObject>();
    }

    private void Update()
    {
        if (_wielderAnimator == null)
            return;

        _collider.enabled = _wielderAnimator.GetCurrentAnimatorStateInfo(0).IsName("AttackNormal");
    }

    public void Attack()
    {
        _collider.enabled = true;
        _alreadyHit = new List<GameObject>();
    }

    public void EndAttack()
    {
        _collider.enabled = false;
    }

    private void HitObstacle(Collider obstacle)
    {
        // Only hit if the collider is not part of the weapon wielder layer
        // (if player is wielding weapon, they can not hit themselves or other players
        // and enemies cannot hit each other)
        if (wielder.layer != obstacle.gameObject.layer)
        {
            var hittable = obstacle.GetComponent<Hittable>();
            
            Debug.Log(_alreadyHit.Contains(obstacle.gameObject.transform.root.gameObject));

            if (hittable != null && !_alreadyHit.Contains(obstacle.gameObject.transform.root.gameObject))
            {
                hittable.Hit(baseDamage);
                _alreadyHit.Add(obstacle.gameObject.transform.root.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_collider.enabled)
            HitObstacle(other);
    }
}
