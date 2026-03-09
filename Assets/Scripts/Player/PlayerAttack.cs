using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform currentHurtPoint;
  
    [SerializeField] private float attackRadius;
    [SerializeField] private Animator animator;

    private bool isAttacking;

    private void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    OnAttack();
        //}
        animator.SetBool("isAttacking", isAttacking);
    }

    public void OnAttack()
    {
       isAttacking = true;
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(currentHurtPoint.position, attackRadius);
            for (int i = 0; i < hitObjects.Length; i++)
            {
                if (hitObjects[i].gameObject.CompareTag("Enemy"))
                {
                Destroy(hitObjects[i].gameObject);
                }
            }
        
        
    }

    public void OnAttackCompleted()
    {
        isAttacking = false;
    }
}
