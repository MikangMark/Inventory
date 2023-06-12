using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private ItemGrid selectedItemGrid;// ���� ���õ� ������ �׸���
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);// ���õ� ������ �׸��忡 ���� ���� ȿ���� �θ� �����մϴ�.
        }
    }
    InventoryItem selectedItem;// ���� ���õ� ������
    InventoryItem overlapItem;// ��ġ�� ������ (��������� ���)
    RectTransform rectTransform;

    [SerializeField]
    List<ItemData> items;// ������ ������ ���
    [SerializeField] 
    GameObject itemPrefab;// ������ ������
    [SerializeField] 
    Transform canvasTransform;// ĵ������ Transform

    InventoryHighlight inventoryHighlight;// �κ��丮 ���� ȿ��
    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();// �κ��丮 ���� ȿ�� ������Ʈ�� �����ɴϴ�.
    }
    private void Update()
    {
        ItemIconDrag();// ������ ������ �巡�� ó��

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedItem == null)
            {
                CreateRandomItem();// ���� ������ ����
            }
            
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            InsertRandomItem();// ���� ������ ����
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();// ������ ȸ��
        }
        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);// ���õ� ������ �׸��尡 ���� ��� ���� ȿ�� ����
            return;
        }
        HandleHighlight();// ���� ȿ�� ó��

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();// ���� ���콺 ��ư Ŭ�� ó��
        }
    }

    private void RotateItem()
    {
        if(selectedItem == null)
        {
            return;
        }
        selectedItem.Rotate();// ���õ� ������ ȸ��
    }

    private void InsertRandomItem()
    {
        if(selectedItemGrid == null)
        {
            return;
        }
        CreateRandomItem();// ���� ������ ����
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);// ������ ����
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);// �������� ������ �� �ִ� �׸��� ��ġ ã��

        if (posOnGrid == null)
        {
            return;
        }
        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);// �������� �׸��忡 ����
    }

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;


    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();// ���콺 ��ġ�� �ش��ϴ� �׸��� ��ġ ��������
        if (oldPosition == positionOnGrid)
        {
            return;
        }
        oldPosition = positionOnGrid;
        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);// �׸��忡�� �ش� ��ġ�� ������ ��������
            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);// ���� ȿ�� �����ֱ�
                inventoryHighlight.SetSize(itemToHighlight);// ���� ȿ�� ũ�� ����
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);// ���� ȿ�� ��ġ ����
            }
            else
            {
                inventoryHighlight.Show(false);// ���� ȿ�� �����
            }
            
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.WIDTH, selectedItem.HEIGHT));// ���� ȿ�� �����ֱ� ���� ����
            inventoryHighlight.SetSize(selectedItem);// ���� ȿ�� ũ�� ����
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);// ���� ȿ�� ��ġ ����
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();// ������ �������� �ν��Ͻ�ȭ�Ͽ� ������ ��ü ��������
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);// �������� ĵ������ �ڽ����� ����
        rectTransform.SetAsLastSibling();// �������� �� ���� �ø���
        int selectedItemID = UnityEngine.Random.Range(0, items.Count);// ������ ��Ͽ��� ������ �ε��� ����
        inventoryItem.Set(items[selectedItemID]);// ���õ� ������ �����ͷ� ������ ����
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();// ���콺 ��ġ�� �ش��ϴ� �׸��� ��ġ ��������

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);// �������� ����ø�

        }
        else
        {
            PlaceItem(tileGridPosition);// �������� ����
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)//���̴¾������� ���콺Ŀ�����Ǿ��ִ� Ÿ���� �߾��̵ǵ��� ����
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }
        return selectedItemGrid.GetTileGridPosition(position);// ���콺 ��ġ�� �ش��ϴ� �׸��� ��ġ ���
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);// �������� �׸��忡 ����
        if (complete)
        {
            selectedItem = null;
            if(overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
                Debug.Log("aa");
            }
        }
        
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);// �������� ����ø�
        selectedItem.transform.SetAsLastSibling();
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;// ������ �������� ���콺 ��ġ�� �̵�
        }
    }
}
