using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;// 아이템 데이터를 저장하는 변수

    public int HEIGHT// 아이템의 높이를 반환하는 속성
    {
        get 
        {
            if(rotated == false)// 회전되지 않은 경우
            {
                return itemData.height;
            }
            return itemData.width;
        }
    }
    public int WIDTH// 아이템의 너비를 반환하는 속성
    {
        get
        {
            if (rotated == false)// 회전되지 않은 경우
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    public int onGridPositionX;// 그리드에서의 X 위치
    public int onGridPositionY;// 그리드에서의 Y 위치

    public bool rotated = false;// 아이템이 회전되었는지 여부를 나타내는 변수

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;// 아이템 데이터 설정
        GetComponent<Image>().sprite = itemData.invenIcon;// 이미지 설정

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;// 아이템의 너비에 그리드 타일 크기를 곱하여 X 사이즈 설정
        size.y = itemData.height * ItemGrid.tileSizeHeight;// 아이템의 높이에 그리드 타일 크기를 곱하여 Y 사이즈 설정
        GetComponent<RectTransform>().sizeDelta = size;// RectTransform의 사이즈 설정
    }

    internal void Rotate()
    {
        rotated = !rotated;
        RectTransform rectTransform = GetComponent<RectTransform>();// RectTransform 컴포넌트 가져오기
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);// 회전 값을 사용하여 게임 오브젝트 회전 설정
    }
}
