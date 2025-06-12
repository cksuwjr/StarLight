using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolMove : MonoBehaviour
{
    private Transform target;
    private Rigidbody rigid;
    private Animator animator;
    private Vector3 direction;
    private float speed = 2f;
    [SerializeField] private float chaseDistance = 0.2f;
    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rigid);
        TryGetComponent<Animator>(out animator);
    }

    void Start()
    {
        target = GameManager.Instance.Player.transform.GetChild(2);

        StartCoroutine("Chase");
    }

    private IEnumerator Chase()
    {
        float distance;
        while (target != null)
        {
            distance = Vector3.Distance(target.position, transform.position);
            if (distance > chaseDistance)
            {
                var origin = direction;
                direction = target.position - transform.position;
                direction.Normalize();

                StartCoroutine(ChangeDirection(origin, direction));

                rigid.velocity = direction * speed * (1 + distance * 0.5f);
                animator.SetBool("Move", true);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.15f);
            if (Vector3.Distance(target.position, transform.position) < chaseDistance)
            {
                rigid.velocity = Vector3.zero;
                animator.SetBool("Move", false);
            }
        }
    }

    private IEnumerator ChangeDirection(Vector3 before, Vector3 after)
    {
        float timer = 0f;

        while (timer < 0.4f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(before, after, timer / 0.4f));

            timer += Time.deltaTime;
            yield return null;
        }
    }
}