using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private Canvas canvas;
    private Image imageComponent;
    [SerializeField] private bool instantiateVisual = true;
    private VisualCardsHandler visualHandler;
    private Vector3 offset;
    private HorizontalCardHolder cardGroup;//NOTE::卡牌所在卡牌区
    private Transform useCardCheckPoint;//NOTE::释放卡牌检测点
    public CardInfo cardInfo;//NOTE::卡牌相关的信息，图案，名称，功能等

    public bool artificeRequired=false;//NOTE::是否为炼化需要的卡牌，方便炼化后销毁
    
    //NOTE::移动相关参数
    [Header("Movement")]
    [SerializeField] private float moveSpeedLimit = 50;//移动速度限制

    //NOTE::选择相关参数
    [Header("Selection")]
    public bool selected;//是否被选择
    public float selectionOffset = 50;//选择后垂直偏移量
    private float pointerDownTime;//鼠标按下的时间
    private float pointerUpTime;//鼠标抬起的时间

    //NOTE::卡牌图片
    [Header("Visual")]
    [SerializeField] private GameObject cardVisualPrefab;
    [HideInInspector] public CardVisual cardVisual;

    //NOTE::卡牌状态，鼠标进入，拖拽中，被拖拽过
    [Header("States")]
    public bool isHovering;
    public bool isDragging;
    [HideInInspector] public bool wasDragged;

    [Header("Events")]
    [HideInInspector] public UnityEvent<Card> PointerEnterEvent;
    [HideInInspector] public UnityEvent<Card> PointerExitEvent;
    [HideInInspector] public UnityEvent<Card, bool> PointerUpEvent;//NOTE::bool表示在Invoke时要添加判断是否执行
    [HideInInspector] public UnityEvent<Card> PointerDownEvent;
    [HideInInspector] public UnityEvent<Card> BeginDragEvent;
    [HideInInspector] public UnityEvent<Card> EndDragEvent;
    [HideInInspector] public UnityEvent<Card, bool> SelectEvent;

    void Start()
    {
        cardInfo = CardManager.instance.GetRandomCardFormPlayerCards();
        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();
        
        useCardCheckPoint = CardManager.instance.useCardCheckPoint;
        //NOTE::是否已经初始化
        if (!instantiateVisual)
            return;

        //NOTE::获取VisualCardsHandler单例
        visualHandler = FindObjectOfType<VisualCardsHandler>();
        
        //NOTE::生成卡牌图片放在VisualCardsHandler或画布节点下，同时获取CardVisual；在VisualCardsHandler节点下方便管理
        cardVisual = Instantiate(cardVisualPrefab, visualHandler ? visualHandler.transform : canvas.transform).GetComponent<CardVisual>();
        cardVisual.Initialize(this);
        cardVisual.cardImage.sprite = cardInfo.sprite;
        cardGroup=transform.parent.parent.GetComponent<HorizontalCardHolder>();
        
        cardInfo.SetTargetCard(this);
    }

    
    void Update()
    {
        ClampPosition();

        //NOTE::拖拽时的平滑移动
        if (isDragging)
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    //NOTE::限制物体位置在屏幕的边界内
    void ClampPosition()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke(this);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = mousePosition - (Vector2)transform.position;
        isDragging = true;
        //NOTE::禁用 GraphicRaycaster 可以防止在拖拽过程中其他 UI 元素接收鼠标事件，从而避免干扰。
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        imageComponent.raycastTarget = false;

        wasDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent.Invoke(this);
        isDragging = false;
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        imageComponent.raycastTarget = true;

        StartCoroutine(FrameWait());

        //NOTE::WaitForEndOfFrame：等待当前帧结束。在当前帧结束后，将 wasDragged 设置为 false，表示物体不再处于被拖拽状态
        IEnumerator FrameWait()
        {
            yield return new WaitForEndOfFrame();
            wasDragged = false;
        }

        if (transform.position.x>useCardCheckPoint.position.x
            && transform.position.y > useCardCheckPoint.position.y
            && EnemyManager.instance.targetEnemy!=null)
        {
            cardInfo.CardFuction();
            DestroyCard();
        }
    }

    public void DestroyCard()
    {
        cardGroup.cards.Remove(this);
        Destroy(transform.parent.gameObject);
    }

    //NOTE::设置鼠标是否进入布尔值
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        isHovering = false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //NOTE::只处理左键事件
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        PointerDownEvent.Invoke(this);
        //NOTE::记录鼠标按下时间后感觉鼠标松开时间判断为短按还是长按
        pointerDownTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        pointerUpTime = Time.time;

        PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);
        
        //NOTE::如果是长按，直接返回，不执行后续逻辑
        if (pointerUpTime - pointerDownTime > .2f)
            return;

        //NOTE::按下期间被拖拽过，直接返回，不执行后续逻辑。
        if (wasDragged)
            return;

        SelectedCard();
    }

    public void SelectedCard()
    {
        //NOTE::切换选中状态
        selected = !selected;
        SelectEvent.Invoke(this, selected);

        //NOTE::如果物体被选中，将其位置向上偏移（selectionOffset）。如果物体未被选中，将其位置重置为 (0, 0, 0)。
        if (selected)
        {
            CardManager.instance.selectedCards.Add(this);
            GameManager.instance.showArtificeButton();
            transform.localPosition += (cardVisual.transform.up * selectionOffset);
        }
        else
        {
            CardManager.instance.selectedCards.Remove(this);
            if (CardManager.instance.selectedCards.Count==0)
            {
                GameManager.instance.hideArtificeButton();
            }
            transform.localPosition = Vector3.zero;
        }
    }

    //NOTE::卡牌位置上移
    public void Deselect()
    {
        if (selected)
        {
            selected = false;
            if (selected)
                transform.localPosition += (cardVisual.transform.up * 50);
            else
                transform.localPosition = Vector3.zero;
        }
    }


    //NOTE::返回卡牌数量减一
    public int SiblingAmount()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
    }

    //NOTE::获取卡槽的索引
    public int ParentIndex()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

    //NOTE::用于计算当前对象在其父级中的归一化位置。归一化位置是一个介于 0 和 1 之间的值，表示当前对象在其父级中的相对位置。
    public float NormalizedPosition()
    {
        //NOTE::ExtensionMethods.Remap：一个扩展方法，用于将一个值从一个范围映射到另一个范围。将索引值从范围 [0, childCount - 1] 映射到 [0, 1]。
        //NOTE::公式为：normalizedPosition = (currentIndex - minIndex) / (maxIndex - minIndex)
        return transform.parent.CompareTag("Slot") ? ExtensionMethods.Remap((float)ParentIndex(), 0, (float)(transform.parent.parent.childCount - 1), 0, 1) : 0;
    }

    private void OnDestroy()
    {
        CardManager.instance.currentCardCount--;
        CardManager.instance.playerThrowCardGroup[cardInfo]++;
        CardManager.instance.selectedCards.Remove(this);
        if (CardManager.instance.selectedCards.Count==0)
        {
            GameManager.instance.hideArtificeButton();
        }
        //NOTE::卡牌销毁的同时销毁卡牌图片
        if(cardVisual != null)
            Destroy(cardVisual.gameObject);
    }

    
}
