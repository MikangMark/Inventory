using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 84;// 타일의 가로 크기 상수
    public const float tileSizeHeight = 84;// 타일의 세로 크기 상수

    InventoryItem[,] inventoryItemSlot;// 인벤토리 아이템 슬롯 배열

    RectTransform rectTransform;

    [SerializeField]
    int gridSizeWidth = 20;// 그리드의 가로 크기
    [SerializeField]
    int gridSizeHeight = 10;// 그리드의 세로 크기

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    public InventoryItem PickUpItem(int x, int y)// 아이템을 선택하여 인벤토리에서 제거하고 반환하는 메서드
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
        {
            return null;
        }

        CleanGridReference(toReturn);

        return toReturn;
    }

    private void CleanGridReference(InventoryItem item)// 아이템의 참조를 인벤토리에서 제거하는 메서드
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    private void Init(int width, int height)// 그리드를 초기화하는 메서드
    {
        inventoryItemSlot = new InventoryItem[width, height];// 인벤토리 아이템 슬롯 배열 초기화
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    internal InventoryItem GetItem(int x, int y)// 특정 좌표에 있는 아이템을 반환하는 메서드
    {
        return inventoryItemSlot[x, y];
    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)// 마우스 위치를 기준으로 타일 그리드 상의 좌표를 반환하는 메서드
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)// 아이템을 삽입할 수 있는 공간을 찾는 메서드
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

    public bool PlaceItem(InventoryItem inventoryItem,int posX,int posY, ref InventoryItem overlapItem)// 아이템을 그리드에 삽입가능한지 판단하는 메서드
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

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)// 아이템을 그리드에 삽입하는 메서드
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

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)// 아이템의 그리드 상 위치를 계산하여 반환하는 메서드
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)// 아이템이 겹치는지 확인하는 메서드
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

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)// 아이템을 삽입할 수 있는 공간인지 확인하는 메서드
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

    bool PositionCheck(int posX,int posY)// 좌표가 그리드 범위 내에 있는지 확인하는 메서드
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

    public bool BoundryCheck(int posX, int posY,int width,int height)// 아이템이 그리드 내에 삽입 가능한지 확인하는 메서드
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
