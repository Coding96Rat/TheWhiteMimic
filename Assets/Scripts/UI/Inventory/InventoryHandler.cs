using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{

    private List<Item> items = new List<Item>();
    private List<Item> mainItem = new List<Item>();
    private InventoryUI inventoryUI;

    private int itemIndicator = 0;
    private int slotIndicator = 0;
    private int slotMoveDirection;


    [Space(15)]
    [Header("Player UI Handler State")]
    [SerializeField]
    private float wheelIncreaseAmount = 0.0f;
    private float wheelAmountPlus = 0;
    private float wheelAmountMinus = 0;


    void Awake()
    {
    }
    void Start()
    {
        inventoryUI = FindFirstObjectByType<InventoryUI>();
    }

    void Update()
    {
        #region Wheel Scroll Update

        if (mainItem.Count > 0)
        {

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            slotMoveDirection = 0;
            if (scrollInput != 0)
            {
                if (scrollInput < 0)
                {
                    wheelAmountPlus = 0.0f;
                    wheelAmountMinus += wheelIncreaseAmount * scrollInput;
                }
                else if (scrollInput > 0)
                {
                    wheelAmountMinus = 0.0f;
                    wheelAmountPlus += wheelIncreaseAmount * scrollInput;
                }


                if (wheelAmountPlus > 1.0f)
                {
                    // this is left slot move
                    wheelAmountPlus = 0.0f;
                    slotMoveDirection = -1;
                }
                else if (wheelAmountMinus < -1.0f)
                {
                    // this is right slot move
                    wheelAmountMinus = 0.0f;
                    slotMoveDirection = 1;
                }

                itemIndicator += slotMoveDirection;

                if (itemIndicator <= 0)
                    itemIndicator = 0;

                else if (itemIndicator >= items.Count - 1)
                {
                    itemIndicator = items.Count - 1;
                }
            }
        }
        #endregion
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public void PlayerPickItem(Item item)
    {
        items.Add(item);
        item.transform.parent = this.transform;
        item.gameObject.SetActive(false);

        // Update UI Item information
        // And then try update ui slot in this method
        //inventoryUI.UpdateItemSlotList();
    }

    //public void PlayerUseItem(int id)
    //{

    //}


}
