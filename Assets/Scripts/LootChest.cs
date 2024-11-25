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
                Vector3 newPosition = SetRandomTargetPosition();
                StartCoroutine(UpdatePosition(item, newPosition));
            }
        }
        isLooted = true;
        Destroy(baseClass.hintUI);
        baseClass.animator.SetBool("IsLooted", true);
    }

    private Vector3 SetRandomTargetPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere; // Random direction in 3D space
        Vector3 targetPosition = transform.position + randomDirection.normalized * throwRange;
        return targetPosition;
    }

    private IEnumerator UpdatePosition(GameObject item, Vector3 newPosition)
    {

        float duration = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            try
            {
                item.transform.position = Vector3.Lerp(item.transform.position, newPosition, t);
            }
            catch (System.Exception)
            {
                print("Item has been picked up");
            }
            yield return null;
        }
        if(item) item.transform.position = newPosition;

    }
}
