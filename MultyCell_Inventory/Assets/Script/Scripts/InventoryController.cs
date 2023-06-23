using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private ItemGrid selectedItemGrid;// 현재 선택된 아이템 그리드
    public GameObject chest;
    ItemGrid vestGrid;
    ItemGrid backPackGrid;
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);// 선택된 아이템 그리드에 따라 강조 효과의 부모를 설정합니다.
        }
    }
    InventoryItem selectedItem;// 현재 선택된 아이템
    InventoryItem overlapItem;// 겹치는 아이템 (덮어씌워지는 경우)
    RectTransform rectTransform;

    [SerializeField]
    List<ItemData> items;// 아이템 데이터 목록
    [SerializeField]
    List<ItemData> eq_items;// 장비아이템 데이터 목록
    [SerializeField]
    GameObject itemPrefab;// 아이템 프리팹
    [SerializeField]
    Transform canvasTransform;// 캔버스의 Transform

    InventoryHighlight inventoryHighlight;// 인벤토리 강조 효과

    [SerializeField]
    List<GameObject> eq_Objects;

    [SerializeField]
    GameObject pickUpZone;

    public List<string> itemTag;
    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();// 인벤토리 강조 효과 컴포넌트를 가져옵니다.
    }
    private void Start()
    {
        itemTag = new List<string>();
        Array enumValues = Enum.GetValues(typeof(ItemType));
        for (int i = 0; i < enumValues.Length; i++)
        {
            ItemType value = (ItemType)enumValues.GetValue(i);
            itemTag.Add(value.ToString());
        }
        for (int i = 0; i < items.Count; i++)
        {
            items[i].height = items[i].save_height;
            items[i].width = items[i].save_width;
        }
    }
    private void Update()
    {
        ItemIconDrag();// 아이템 아이콘 드래그 처리

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedItem == null)
            {
                CreateRandomItem();// 랜덤 아이템 생성
            }

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            InsertRandomItem();// 랜덤 아이템 삽입
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();// 아이템 회전
        }
        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);// 선택된 아이템 그리드가 없을 경우 강조 효과 숨김
            return;
        }
        HandleHighlight();// 강조 효과 처리

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();// 왼쪽 마우스 버튼 클릭 처리
        }
        if (Input.GetMouseButtonDown(1))
        {
            //RightMouseButtonPress();//우클릭시 가방열리는것구현하다가 시간없어서 취소...
        }
        for (int i = 0; i < eq_Objects.Count; i++)
        {
            int count = eq_Objects[i].transform.childCount;
            if (count > 0)
            {
                if (eq_Objects[i].transform.GetChild(0).gameObject.name.Equals("Highlighter"))//아이템에 커서를 올리면 최상단에 하이라이트가있기떄문에 하나빼주기
                {
                    count--;
                }
                if (eq_Objects[i].transform.GetChild(0) != null)
                {
                    for (int j = 0; j < eq_items.Count; j++)
                    {
                        if (eq_Objects[i].transform.GetChild(0).gameObject.name != "Highlighter")
                        {
                            if (eq_Objects[i].transform.GetChild(0).GetComponent<InventoryItem>().itemData.itemName == eq_items[j].itemName.Substring(3, eq_items[j].itemName.Length - 3))
                            {
                                eq_Objects[i].transform.GetChild(0).GetComponent<InventoryItem>().itemData = eq_items[j];
                                eq_Objects[i].transform.GetChild(0).GetComponent<InventoryItem>().Set(eq_items[j]);
                                break;
                            }
                        }
                    }
                }

            }
        }
        if (selectedItem != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    GameObject hitObject = result.gameObject;
                    if (selectedItem.tag.Equals("WEAPON"))
                    {
                        if (hitObject.tag.Substring(0, 4) == "Eq_W")
                        {

                            selectedItem.itemData.width = 5;
                            selectedItem.itemData.height = 2;
                            selectedItem.Set(selectedItem.itemData);
                            break;
                        }
                    }
                    else
                    {
                        if (hitObject.tag.Substring(0, 2) == "Eq")
                        {
                            if (!selectedItem.tag.Equals("WEAPON"))
                            {
                                selectedItem.itemData.width = 2;
                                selectedItem.itemData.height = 2;
                                selectedItem.Set(selectedItem.itemData);
                            }
                            break;
                        }
                    }
                    if (hitObject.tag.Equals("Untagged"))
                    {
                        selectedItem.itemData.width = selectedItem.itemData.save_width;
                        selectedItem.itemData.height = selectedItem.itemData.save_height;
                        selectedItem.Set(selectedItem.itemData);
                    }
                }
                if (results.Count < 1)
                {
                    selectedItem.itemData.width = selectedItem.itemData.save_width;
                    selectedItem.itemData.height = selectedItem.itemData.save_height;
                    selectedItem.Set(selectedItem.itemData);
                }
            }
        }
    }



    private void RotateItem()
    {
        if (selectedItem == null)
        {
            return;
        }
        selectedItem.Rotate();// 선택된 아이템 회전
    }

    private void InsertRandomItem()
    {
        if (selectedItemGrid == null)
        {
            return;
        }
        CreateRandomItem();// 랜덤 아이템 생성
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);// 아이템 삽입
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);// 아이템을 삽입할 수 있는 그리드 위치 찾기

        if (posOnGrid == null)
        {
            return;
        }
        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);// 아이템을 그리드에 삽입
    }

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;


    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();// 마우스 위치에 해당하는 그리드 위치 가져오기
        if (oldPosition == positionOnGrid)
        {
            return;
        }
        oldPosition = positionOnGrid;
        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);// 그리드에서 해당 위치의 아이템 가져오기
            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);// 강조 효과 보여주기
                inventoryHighlight.SetSize(itemToHighlight);// 강조 효과 크기 설정
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);// 강조 효과 위치 설정

            }
            else
            {
                inventoryHighlight.Show(false);// 강조 효과 숨기기

            }

        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.WIDTH, selectedItem.HEIGHT));// 강조 효과 보여주기 여부 설정
            inventoryHighlight.SetSize(selectedItem);// 강조 효과 크기 설정
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);// 강조 효과 위치 설정

        }
    }
    public void CreateItem(int itemId)//아이템번호로 그아이템을 생성
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();// 아이템 프리팹을 인스턴스화하여 아이템 객체 가져오기
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);// 아이템을 캔버스의 자식으로 설정
        rectTransform.SetAsLastSibling();// 아이템을 맨 위로 올리기
        inventoryItem.Set(items[itemId]);// 선택된 아이템 데이터로 아이템 설정
    }
    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();// 아이템 프리팹을 인스턴스화하여 아이템 객체 가져오기
        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);// 아이템을 캔버스의 자식으로 설정
        rectTransform.SetAsLastSibling();// 아이템을 맨 위로 올리기
        int selectedItemID = UnityEngine.Random.Range(0, items.Count);// 아이템 목록에서 랜덤한 인덱스 선택
        inventoryItem.Set(items[selectedItemID]);// 선택된 아이템 데이터로 아이템 설정
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();// 마우스 위치에 해당하는 그리드 위치 가져오기

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);// 아이템을 집어올림

        }
        else
        {
            PlaceItem(tileGridPosition);// 아이템을 놓음
        }
    }
    private void RightMouseButtonPress()//우클릭시 가방과 전투조끼 그리드 나오게 할려했으니 시간부족으로 취소
    {
        if (selectedItem.itemData.type == ItemType.BACKPACK)
        {
            backPackGrid = Instantiate(backPackGrid, canvasTransform).GetComponent<ItemGrid>();
            backPackGrid.GetComponent<RectTransform>().localPosition = Input.mousePosition;
        }
        if (selectedItem.itemData.type == ItemType.VEST)
        {
            vestGrid = Instantiate(chest, canvasTransform).GetComponent<ItemGrid>();
            vestGrid.GetComponent<RectTransform>().localPosition = Input.mousePosition;
        }

    }
    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)//놓이는아이템이 마우스커서가되어있는 타일이 중앙이되도록 설정
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }
        return selectedItemGrid.GetTileGridPosition(position);// 마우스 위치에 해당하는 그리드 위치 계산
    }

    private void PlaceItem(Vector2Int tileGridPosition)//들고있는아이템을 놓는메소드
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);// 아이템을 그리드에 놓을지 판단하는메소드
        if (complete)
        {
            if (selectedItem.itemData.itemName.Substring(0, 2) == "Eq")//들고있는 아이템이 장비관련일경우
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].itemName == selectedItem.itemData.itemName)
                    {
                        selectedItem.itemData = items[i];
                        selectedItem.Set(items[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < items.Count; i++)//모든아이템
            {
                if (selectedItem.itemData.itemName == items[i].itemName)
                {
                    items[i].height = items[i].save_height;
                    items[i].width = items[i].save_width;
                    selectedItem.itemData = items[i];
                    selectedItem.Set(items[i]);
                }
            }
            selectedItem = null;
            if (overlapItem != null) //놓을곳에 아이템이 존재할경우 들고있는아이템이랑 그리드에있는아이템이랑 바꿈
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }

        }

    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);// 아이템을 집어올림
        if (selectedItem == null)
        {
            return;
        }
        if(selectedItem.itemData.itemName.Substring(0,2) == "Eq")//들고있는아이템이 장비관련아이템일경우
        {
            for(int i = 0; i < items.Count; i++)
            {
                if (items[i].itemName == selectedItem.itemData.itemName.Substring(3, selectedItem.itemData.itemName.Length-3))
                {
                    selectedItem.itemData = items[i];
                    selectedItem.Set(items[i]);
                    break;
                }
            }
        }
        selectedItem.transform.parent = pickUpZone.transform;
        selectedItem.transform.SetAsLastSibling();
        selectedItem.transform.parent.SetAsLastSibling();
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;// 아이템 아이콘을 마우스 위치로 이동
        }
    }
}
