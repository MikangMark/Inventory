using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { WEAPON = 0, AROMER, HEADPHONE, HELMET, VEST, BACKPACK, GLASS, VALUABLE, MEDICAL }
[CreateAssetMenu]
public class ItemData : ScriptableObject//스크립터블 오브젝트스크립트 아이템 크기정보와 이미지정보
{
    public ItemType type;
    public string itemName;
    public int width = 1;       //넓이
    public int height = 1;      //크기
    public Sprite invenIcon;     //이미지
    public Sprite equipIcon;    //장비중 이미지
}
