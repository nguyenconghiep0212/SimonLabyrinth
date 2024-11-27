using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] internal string trueName;
    [SerializeField] float damage;
    [SerializeField] float fireRate = 1000;
    [SerializeField] float range = 10f;
    [SerializeField] int numberOfTargetable = 1;
    [SerializeField] bool isProjectile;
    [SerializeField] internal bool isEquiped;
    [SerializeField] internal GameObject projectilePrefab;

    [SerializeField] LayerMask targetableLayer;
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] Transform barrel;

    public List<Transform> closetTargets;

    private Animator animator;
    private bool isReadyToShoot = true;
    private List<LineRenderer> targetingLine = new List<LineRenderer>();
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        if (trueName == "") trueName = gameObject.name;

        for (int i = 0; i < numberOfTargetable; i++)
        {
            LineRenderer line = GameObject.Instantiate(GameManager.Instance.weaponTargetingLine, GameObject.FindGameObjectWithTag("Player").transform);
            targetingLine.Add(line);
        }
        foreach (LineRenderer line in targetingLine)
        {
            line.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (isEquiped)
        {
            FindTarget();
        }
    }

    public void Equiped()
    {

    }

    #region ---- || ATTACKING || ----
    public void FindTarget()
    {
        List<Collider2D> closestColliders = new List<Collider2D>(); // To store the closest colliders
        List<float> distances = new List<float>();

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0f, targetableLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.transform.GetComponent<Enemy>())
                {
                    if (!hit.transform.GetComponent<Enemy>().isDead)
                    {
                        float distance = Vector2.Distance(transform.position, hit.point);
                        closestColliders.Add(hit.collider);
                        distances.Add(distance);
                    }
                }
            }
        }

        // Combine distances and colliders into a list of pairs
        List<(Collider2D collider, float distance)> pairedList = new List<(Collider2D, float)>();
        for (int i = 0; i < closestColliders.Count; i++)
        {
            pairedList.Add((closestColliders[i], distances[i]));
        }

        // Sort the list by distance
        pairedList.Sort((a, b) => a.distance.CompareTo(b.distance));

        int count = Mathf.Min(numberOfTargetable, pairedList.Count);
        closetTargets = pairedList.Take(count).Select(i => i.collider.transform).ToList();

        if (closetTargets.Count > 0)
        {
            //DrawTargetingLine();
            FaceToTargetDirection();
            Shoot();

            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inCombat = true;
        }
        else
        {
            transform.rotation = Quaternion.identity;
            foreach (LineRenderer line in targetingLine)
            {
                line.gameObject.SetActive(false);
            }

            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inCombat = false;
        }
    }

    private void DrawTargetingLine()
    {
        for (int i = 0; i < closetTargets.Count; i++)
        {
            targetingLine[i].gameObject.SetActive(true);
            targetingLine[i].SetPosition(0, barrel.position);
            targetingLine[i].SetPosition(1, closetTargets[i].position);
        }
    }

    private void FaceToTargetDirection()
    {
        Vector3 sum = Vector3.zero;

        for (int i = 0; i < closetTargets.Count; i++)
        {
            sum += closetTargets[i].position;
        }
        Vector3 average = sum / closetTargets.Count;

        Vector3 direction = new Vector3(average.x - transform.position.x, average.y - transform.position.y);
        transform.right = direction;
    }

    private void Shoot()
    {
        if (isReadyToShoot)
        {
            animator.SetTrigger("Shoot");
            if (isProjectile)
            {
                ProjectileDamage();
            }
            else
            {
                InstanceDamage();
            }
            isReadyToShoot = false;
            StartCoroutine(ReadyToShoot());
        }
    }
    private void InstanceDamage()
    {
        foreach (Transform target in closetTargets)
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
        }

    }

    private void ProjectileDamage()
    {
        foreach (Transform target in closetTargets)
        {
            GameObject projectile = GameObject.Instantiate(projectilePrefab, barrel.transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().target = target.gameObject;
            projectile.GetComponent<Projectile>().damage = damage;
            projectile.GetComponent<Projectile>().isHostile = false;
        }
    }


    private IEnumerator ReadyToShoot()
    {
        yield return new WaitForSeconds(fireRate);
        isReadyToShoot = true;
    }
    #endregion


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().hoverWeapon = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<Player>().hoverWeapon == this)
                collision.GetComponent<Player>().hoverWeapon = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
