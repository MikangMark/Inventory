using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 84;// Ÿ���� ���� ũ�� ���
    public const float tileSizeHeight = 84;// Ÿ���� ���� ũ�� ���

    InventoryItem[,] inventoryItemSlot;// �κ��丮 ������ ���� �迭

    RectTransform rectTransform;

    [SerializeField]
    int gridSizeWidth = 20;// �׸����� ���� ũ��
    [SerializeField]
    int gridSizeHeight = 10;// �׸����� ���� ũ��

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    public InventoryItem PickUpItem(int x, int y)// �������� �����Ͽ� �κ��丮���� �����ϰ� ��ȯ�ϴ� �޼���
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
        {
            return null;
        }

        CleanGridReference(toReturn);

        return toReturn;
    }

    private void CleanGridReference(InventoryItem item)// �������� ������ �κ��丮���� �����ϴ� �޼���
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    private void Init(int width, int height)// �׸��带 �ʱ�ȭ�ϴ� �޼���
    {
        inventoryItemSlot = new InventoryItem[width, height];// �κ��丮 ������ ���� �迭 �ʱ�ȭ
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    internal InventoryItem GetItem(int x, int y)// Ư�� ��ǥ�� �ִ� �������� ��ȯ�ϴ� �޼���
    {
        return inventoryItemSlot[x, y];
    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)// ���콺 ��ġ�� �������� Ÿ�� �׸��� ���� ��ǥ�� ��ȯ�ϴ� �޼���
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)// �������� ������ �� �ִ� ������ ã�� �޼���
    {
        int height = gridSizeHeight - itemToInsert.HEIGHT + 1;
        int width = gridSizeWidth - itemToInsert.WIDTH + 1;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.WIDTH, itemToInsert.HEIGHT))
                {
                    return new Vector2Int(x, y);
                }
                
            }
        }
        return null;
    }

    public bool PlaceItem(InventoryItem inventoryItem,int posX,int posY, ref InventoryItem overlapItem)// �������� �׸��忡 ���԰������� �Ǵ��ϴ� �޼���
    {
        if (BoundryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT) == false)
        {
            return false;
        }
        if (OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)// �������� �׸��忡 �����ϴ� �޼���
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        for (int x = 0; x < inventoryItem.WIDTH; x++)
        {
            for (int y = 0; y < inventoryItem.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }
        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;
        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)// �������� �׸��� �� ��ġ�� ����Ͽ� ��ȯ�ϴ� �޼���
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)// �������� ��ġ���� Ȯ���ϴ� �޼���
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(inventoryItemSlot[posX + x,posY + y] != null)
                {
                    if(overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if(overlapItem != inventoryItemSlot[posX + x,posY + y])
                        {
                            return false;
                        }
                    }
                    
                }
            }
        }

        return true;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)// �������� ������ �� �ִ� �������� Ȯ���ϴ� �޼���
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    bool PositionCheck(int posX,int posY)// ��ǥ�� �׸��� ���� ���� �ִ��� Ȯ���ϴ� �޼���
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }

        if (posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    public bool BoundryCheck(int posX, int posY,int width,int height)// �������� �׸��� ���� ���� �������� Ȯ���ϴ� �޼���
    {
        if(PositionCheck(posX,posY) == false)
        {
            return false;
        }

        posX += width - 1;
        posY += height - 1;
        if(PositionCheck(posX,posY) == false)
        {
            return false;
        }
        return true;
    }
}