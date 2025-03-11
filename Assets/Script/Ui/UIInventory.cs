using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    private ItemData selectedItem;
    private int selectedItemIndex = 0;
    public TextMeshProUGUI SelectedItemName;
    public TextMeshProUGUI SelectedItemDescription;
    public TextMeshProUGUI selectedstatName;
    public TextMeshProUGUI selectedstatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

   

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        ClearSelctedItemWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearSelctedItemWindow()
    {
        SelectedItemName.text = string.Empty;
        SelectedItemDescription.text = string.Empty;
        selectedstatName.text = string.Empty;
        selectedstatValue.text = string.Empty;
        
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }
    
    public void Toggle()
    {
        if(IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if(data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        ItemSlot emptySlot = GetEmptySlot();

        if ((emptySlot != null))
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }
        ThrowItem(data); 
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpdateUI()
    {
        for(int i = 0; i<slots.Length;i++)
        {
            if(slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }


    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i<slots.Length;i++)
        {
            if(slots[i].item == data&& slots[i].quantity < data.maxStack)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null)
        {
            return;
        }
        selectedItem = slots[index].item;
        selectedItemIndex = index;

        SelectedItemName.text = selectedItem.displayName;
        SelectedItemDescription.text = selectedItem.description;

        selectedstatName.text = string.Empty;
        selectedstatValue.text = string.Empty;
        
        if (selectedItem.Consumable != null && selectedItem.Consumable.Length > 0)
        {
            for(int i = 0; i < selectedItem.Consumable.Length; i++)
            {
                selectedstatName.text += selectedItem.Consumable[i].type.ToString() + "\n";
                selectedstatValue.text += selectedItem.Consumable[i].value.ToString() + "\n";
            }
        }
        
        useButton.SetActive(selectedItem.itemType == ItemType.Consumable);
        equipButton.SetActive(selectedItem.itemType == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.itemType == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.Consumable.Length; i++)
            {
                switch(selectedItem.Consumable[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.Consumable[i].value);
                        break;
                    case ConsumableType.Speed:
                        if(!condition.isBoost)
                        {
                            condition.StartSpeedBoost(selectedItem.Consumable[i].value);
                        }
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;
        
        if(slots[selectedItemIndex].quantity <= 0 )
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelctedItemWindow();
        }
        UpdateUI();
    }
}
