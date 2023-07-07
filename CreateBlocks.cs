using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBlocks : MonoBehaviour
{
    private GameObject SelectMovement;
    [SerializeField] private GameObject Block;
    [SerializeField] private int cantBlocks = 1000;
    [SerializeField] private AudioClipSO FXCreateBlock;
    // Start is called before the first frame update
    void Start()
    {
        SelectMovement = GameObject.Find("SelectMoves");
        this.GetComponent<Button>().onClick.AddListener(() => CreateBlock());
    }

    private void CreateBlock()
    {
        if (cantBlocks > 0 && SelectMovement.transform.childCount < 9)
        {
            cantBlocks--;
            FXCreateBlock?.PlayOneShoot();
            Instantiate(Block, SelectMovement.transform);
        }
        else Debug.Log("no hay mas espacio papu");
    }
}