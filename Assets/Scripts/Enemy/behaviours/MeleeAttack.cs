using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private Transform checkPlayer;
    private Transform attackPoint;
    private EnemyData enemyData;
    private Animator animator;
    private float RadiusPunch;
    private float time;
    private bool attack;
    private float rayDirection; 

    private void Awake()
    {
        RadiusPunch = 0.7f;
        rayDirection = 1;
        attack = false;
        time = 0;
        animator = GetComponent<Animator>();
        enemyData = GetComponent<EnemyData>();
        attackPoint = transform.Find("AttackPoint");
        checkPlayer= transform.Find("CheckGround");
    }

    private void Update()
    {
        if (enemyData.IsDie) return;
        Attack();
    }

    private void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPlayer.position, Vector2.right * rayDirection, enemyData.DistanceToAttack, enemyData.PlayerLayer);
        
        enemyData.IsAttack = hit;
        animator.SetBool("run",!hit);
        
        if(attack) time += Time.deltaTime;
        
        if (time >= enemyData.TimeToAttack)
        {
            time = 0;
            attack = false;
        }

        if (!hit) return;
        attack = true;
        if (time == 0)
        {
            animator.SetTrigger("attack");
            CheckCollision();
        }
    }

    private void CheckCollision()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, RadiusPunch, enemyData.PlayerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            var obj = player.gameObject.GetComponent<IDamageable>();
            obj?.TakeDamage(enemyData.AttackDamage);
        }
    }
    
    private void OnDrawGizmos()
    {
       Gizmos.color = Color.green;
       Gizmos.DrawWireSphere(attackPoint.position,RadiusPunch);
       Gizmos.DrawLine(checkPlayer.position, checkPlayer.position + (Vector3.right * rayDirection) * enemyData.DistanceToAttack);
    }
}
