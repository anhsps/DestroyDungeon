using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    Transform target;

    //[SerializeField] private float speed = 1.5f;
    [SerializeField] private float distanceAtk = 0.9f;
    [SerializeField] private float coolDownTime = 3f;
    [SerializeField] private float damage = 1f;
    private float timer;
    private bool canAtk = true;
    private Collider zoneCol;

    public int point = 10;
    [HideInInspector] public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        InitialHP();
    }

    // Update is called once per frame
    void Update()
    {
        if (!target || hp <= 0 || !canMove) return;

        transform.LookAt(target.position);
        float distanceTo = Vector3.Distance(transform.position, target.position);
        if (distanceTo > distanceAtk && canMove)
            MoveTo();
        else AttackAnimation();
    }

    private void MoveTo()
    {
        //if (!target) return;
        animator.ResetTrigger("Atk");
        animator.SetBool("Run", true);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        timer = 0;
    }

    private void AttackAnimation()
    {
        animator.SetBool("Run", false);

        if (canAtk)
        {
            animator.SetTrigger("Atk");
            timer = coolDownTime;
            canAtk = false;
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0) canAtk = true;
        }
    }

    public void AttackTarget()
    {
        var player = target.GetComponent<Player>();
        if (!player) return;
        player.TakeDamage(damage);
        SoundManager17.Instance.PlaySound(4);
    }

    public void StopMove()
    {
        canMove = false;
        animator.Rebind();
    }

    public void SetZone(Collider col) => zoneCol = col;

    protected override void Die()
    {
        base.Die();

        if (zoneCol)
        {
            EnemyZone enemyZone = zoneCol.GetComponent<EnemyZone>();
            if (enemyZone) enemyZone.RemoveEnemy(this);
        }
        Score.Instance.IncreaseScore(point);
        Destroy(gameObject);
    }
}
