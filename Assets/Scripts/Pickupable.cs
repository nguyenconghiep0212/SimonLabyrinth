using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{

    [SerializeField] GameManager.Pickupable usableType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (usableType)
            {
                case GameManager.Pickupable.medbag:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().medbag++;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().medbagUI.text = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().medbag.ToString();
                    break;
                case GameManager.Pickupable.battery:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().battery++;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().batteryUI.text = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().battery.ToString();
                    break;
                case GameManager.Pickupable.gold:
                    GameManager.Instance.UpdateGold(1); 
                    break;
                case GameManager.Pickupable.exp:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeExperince(Random.Range(1,2));
                    break;

            }
            Destroy(gameObject);
        }
    }
}
