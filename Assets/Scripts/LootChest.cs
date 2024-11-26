using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootChest : MonoBehaviour, EntityInterface
{
    [SerializeField] float throwRange;
    [SerializeField] List<GameObject> loots;

    private bool isLooted;
    private EntityInteractable baseClass;

    private void Awake()
    {
        baseClass = GetComponent<EntityInteractable>();
    }
    public void Interact()
    {
        if (!isLooted)
        {
            foreach (GameObject itemPrefab in loots)
            {
                GameObject item = GameObject.Instantiate(itemPrefab, transform.position, Quaternion.identity);
                Vector3 newPosition = GameManager.Instance.SetRandomTargetPosition(transform.position, throwRange);
                StartCoroutine(GameManager.Instance.UpdatePosition(item, newPosition));
            }
        }
        isLooted = true;
        Destroy(baseClass.hintUI);
        baseClass.animator.SetBool("IsLooted", true);
    }

    
}
