using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CardVisual : MonoBehaviour
{
    private bool initalize = false;
    
    [Header("Card")]
    public Card parentCard;
    private Transform cardTransform;
    private Vector3 rotationDelta;
    private int savedIndex;
    Vector3 movementDelta;
    private Canvas canvas;

    [Header("References")]
    public Transform visualShadow;
    private float shadowOffset = 20;
    private Vector2 shadowDistance;
    private Canvas shadowCanvas;
    [SerializeField] private Transform shakeParent;
    [SerializeField] private Transform tiltParent;
    public Image cardImage;

    //NOTE::跟随动画参数
    [Header("Follow Parameters")]
    [SerializeField] private float followSpeed = 30;//跟随速度

    //NOTE::旋转动画参数
    [Header("Rotation Parameters")]
    [SerializeField] private float rotationAmount = 20;//旋转强度
    [SerializeField] private float rotationSpeed = 20;//旋转速度
    [SerializeField] private float autoTiltAmount = 30;//自动旋转强度
    [SerializeField] private float manualTiltAmount = 20;//鼠标拖动时倾斜的强度
    [SerializeField] private float tiltSpeed = 20;//鼠标拖动时倾斜的速度

    //NOTE::拖缩放动画参数
    [Header("Scale Parameters")]
    [SerializeField] private bool scaleAnimations = true; //是否启用
    [SerializeField] private float scaleOnHover = 1.15f;//选中时的缩放
    [SerializeField] private float scaleOnSelect = 1.25f;//拖动时的缩放
    [SerializeField] private float scaleTransition = .15f;//动画持续时间
    [SerializeField] private Ease scaleEase = Ease.OutBack;//NOTE::动画缓动效果，默认：动画在结束时有一个 “回弹” 的动作

    //NOTE::选择时的动画参数
    [Header("Select Parameters")]
    [SerializeField] private float selectPunchAmount = 20;//振动强度

    //NOTE::鼠标进入时的动画参数
    [Header("Hober Parameters")]
    [SerializeField] private float hoverPunchAngle = 5;//旋转角度
    [SerializeField] private float hoverTransition = .15f;//动画持续时间

    //NOTE::交换时的旋转动画参数
    [Header("Swap Parameters")]
    [SerializeField] private bool swapAnimations = true; //NOTE::是否启用交换卡牌图片执行的旋转动画
    [SerializeField] private float swapRotationAngle = 30;//偏转角
    [SerializeField] private float swapTransition = .15f;//动画持续时间
    [SerializeField] private int swapVibrato = 5;//震动频率

    //NOTE::卡牌扇形旋转参数设置
    [Header("Curve")]
    [SerializeField] private CurveParameters curve;

    [SerializeField]private float curveYOffset;
    [SerializeField]private float curveRotationOffset;
    private Coroutine pressCoroutine;

    private void Start()
    {
        shadowDistance = visualShadow.localPosition;
    }

    public void Initialize(Card target, int index = 0)
    {
        //NOTE::Declarations 初始化卡牌图片位置，获取当前画布和影子所在画布
        parentCard = target;
        cardTransform = target.transform;
        canvas = GetComponent<Canvas>();
        shadowCanvas = visualShadow.GetComponent<Canvas>();

        //NOTE::Event Listening 添加事件监听
        parentCard.PointerEnterEvent.AddListener(PointerEnter);
        parentCard.PointerExitEvent.AddListener(PointerExit);
        parentCard.BeginDragEvent.AddListener(BeginDrag);
        parentCard.EndDragEvent.AddListener(EndDrag);
        parentCard.PointerDownEvent.AddListener(PointerDown);
        parentCard.PointerUpEvent.AddListener(PointerUp);
        parentCard.SelectEvent.AddListener(Select);

        //NOTE::Initialization 初始化完成
        initalize = true;
    }

    public void UpdateIndex(int length)
    {
        transform.SetSiblingIndex(parentCard.transform.parent.GetSiblingIndex());
        //NOTE::设置卡牌图片索引为卡槽所在索引
        //NOTE::将当前物体的顺序设置为 parentCard 的父级的顺序。换句话说，当前物体会被移动到 parentCard 的父级在层级中的位置。
    }

    void Update()
    {
        //NOTE::初始化未完成或设置的parentCard为空时直接返回
        if (!initalize || parentCard == null) return;

        HandPositioning();
        SmoothFollow();
        FollowRotation();
        CardTilt();

    }

    private void HandPositioning()
    {
        //NOTE::计算垂直偏移值
        curveYOffset = (curve.positioning.Evaluate(parentCard.NormalizedPosition()) * curve.positioningInfluence) * parentCard.SiblingAmount();
        //NOTE::NormalizedPosition 返回一个 [0, 1] 范围的值，表示当前卡片在其父级中的相对位置（例如，从左到右或从上到下）。
        //NOTE::curve.positioningInfluence：一个影响因子，用于调整曲线偏移的强度。parentCard.SiblingAmount()：返回当前卡片的父级中子对象的数量（即卡片总数）。
        
        //NOTE::卡牌数小于五张时垂直偏移为0
        curveYOffset = parentCard.SiblingAmount() < 5 ? 0 : curveYOffset;
        
        //NOTE::根据卡片的归一化位置计算旋转偏移。
        curveRotationOffset = curve.rotation.Evaluate(parentCard.NormalizedPosition());
    }

    //NOTE::平滑跟随实现
    private void SmoothFollow()
    {
        //NOTE::获取卡牌图片垂直偏移量
        Vector3 verticalOffset = (Vector3.up * (parentCard.isDragging ? 0 : curveYOffset));
        
        //NOTE::Vector3.Lerp：用于在两个点之间进行线性插值。参数： 当前位置，目标位置，插值进度值
        //NOTE::卡牌图片要移动到的位置为卡牌位置加上垂直偏移量
        transform.position = Vector3.Lerp(transform.position, cardTransform.position + verticalOffset, followSpeed * Time.deltaTime);
    }

    //NOTE::平滑旋转实现
    private void FollowRotation()
    {
        //NOTE::计算未拖动和拖动情况卡牌图片到卡牌间的距离
        Vector3 movement = (transform.position - cardTransform.position);
        //NOTE::拖拽情况可计算位置插值
        movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
        
        //NOTE::感觉是否为拖拽计算旋转偏移，rotationAmount为旋转强度
        Vector3 movementRotation = (parentCard.isDragging ? movementDelta : movement) * rotationAmount;
        //NOTE::平滑插值
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -60, 60));
        //NOTE::应用插值   Mathf.Clamp(rotationDelta.x, -60, 60)限制偏转角度
    }

    private void CardTilt()
    {
        //NOTE::如果卡片正在拖拽，保持 savedIndex 不变。如果没有拖拽，更新 savedIndex 为卡片在其父级中的索引（ParentIndex()）。这是为了在拖拽时保持倾斜效果的连续性。
        savedIndex = parentCard.isDragging ? savedIndex : parentCard.ParentIndex();
        
        //NOTE::使用 Mathf.Sin 和 Mathf.Cos 生成正弦和余弦波形，用于实现自动倾斜效果。如果卡片处于鼠标进入（非拖动）状态（isHovering），将波形的振幅缩小到 0.2 倍，以实现更微妙的动态效果。
        float sine = Mathf.Sin(Time.time + savedIndex) * (parentCard.isHovering ? .2f : 1);
        float cosine = Mathf.Cos(Time.time + savedIndex) * (parentCard.isHovering ? .2f : 1);

        //NOTE::计算鼠标位置与卡片位置的偏移量（offset）。如果卡片处于鼠标进入（非拖动）状态，根据偏移量动态调整 X 和 Y 轴的倾斜角度。
        //NOTE::tiltX：根据鼠标垂直偏移量调整 X 轴倾斜。tiltY：根据鼠标水平偏移量调整 Y 轴倾斜。manualTiltAmount：鼠标拖动时倾斜的强度。
        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float tiltX = parentCard.isHovering ? ((offset.y * -1) * manualTiltAmount) : 0;
        float tiltY = parentCard.isHovering ? ((offset.x) * manualTiltAmount) : 0;
        
        //NOTE::正在拖拽：Z轴倾斜不变。 没有拖拽：根据曲线偏移（curveRotationOffset）、旋转影响因子（curve.rotationInfluence）和卡片数量（SiblingAmount()）计算 Z 轴倾斜角度。
        float tiltZ = parentCard.isDragging ? tiltParent.eulerAngles.z : (curveRotationOffset * (curve.rotationInfluence * parentCard.SiblingAmount()));

        //NOTE::Mathf.LerpAngle 对倾斜角度进行平滑插值。
        float lerpX = Mathf.LerpAngle(tiltParent.eulerAngles.x, tiltX + (sine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        float lerpY = Mathf.LerpAngle(tiltParent.eulerAngles.y, tiltY + (cosine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        float lerpZ = Mathf.LerpAngle(tiltParent.eulerAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

        //NOTE::最终应用
        tiltParent.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
    }

    private void Select(Card card, bool state)
    {
        //NOTE::停止所有 ID 为 2 的动画（例如之前的旋转或缩放动画）。true 参数表示强制停止动画，即使它尚未完成。
        DOTween.Kill(2, true);
        
        float dir = state ? 1 : 0;
        
        //NOTE::shakeParent.up：表示沿 shakeParent 的上方向（通常是 Y 轴）。selectPunchAmount：动画的强度，表示卡片在选中时的“弹出”距离。
        //NOTE::dir：控制动画是否执行（选中时执行，取消选中时不执行）。
        //NOTE::scaleTransition：动画的持续时间。10：振动频率，表示动画的抖动次数。1：振动强度。
        shakeParent.DOPunchPosition(shakeParent.up * selectPunchAmount * dir, scaleTransition, 10, 1);
        
        //NOTE::Vector3.forward：表示沿 Z 轴旋转。hoverPunchAngle / 2：旋转角度，表示卡片在选中时的旋转幅度。
        //NOTE::hoverTransition：动画的持续时间。20：振动频率。1：振动强度。.SetId(2)：为动画设置 ID 2，便于后续控制（例如停止动画）。
        shakeParent.DOPunchRotation(Vector3.forward * (hoverPunchAngle/2), hoverTransition, 20, 1).SetId(2);

        //NOTE::scaleAnimations：一个布尔值，控制是否启用缩放动画。scaleOnHover：目标缩放值，表示卡片在选中时的缩放比例。
        //NOTE::scaleTransition：动画的持续时间。scaleEase：动画的缓动效果，例如 Ease.OutBack 或 Ease.Linear。
        if(scaleAnimations)
            transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);

    }

    public void Swap(float dir = 1)
    {
        //NOTE::是否启用交换卡牌图片执行的旋转动画
        if (!swapAnimations)
            return;

        //NOTE::暂停选择的旋转动画
        DOTween.Kill(2, true);
        
        //NOTE::执行交换旋转动画，设置动画ID为3
        shakeParent.DOPunchRotation((Vector3.forward * swapRotationAngle) * dir, swapTransition, swapVibrato, 1).SetId(3);
    }

    private void BeginDrag(Card card)
    {
        //NOTE::拖动时的缩放动画
        //NOTE::scaleEase：动画的缓动效果（例如 Ease.OutBack 或 Ease.Linear），使动画更加平滑。
        if(scaleAnimations)
            transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);

        //NOTE::overrideSorting：设置为 true，表示启用画布的排序覆盖功能。
        canvas.overrideSorting = true;
    }

    //NOTE::类比上面
    private void EndDrag(Card card)
    {
        canvas.overrideSorting = false;
        transform.DOScale(1, scaleTransition).SetEase(scaleEase);
    }

    private void PointerEnter(Card card)
    {
        //NOTE::播放鼠标进入时的缩放动画
        if(scaleAnimations)
            transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);

        //NOTE::暂停旋转动画（旋转动画类型可能会不同）
        DOTween.Kill(2, true);
        //NOTE::播放鼠标进入时的旋转动画并设置动画ID为2
        shakeParent.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
    }

    //NOTE::类别上面
    private void PointerExit(Card card)
    {
        //NOTE::未被拖拽时播放缩放动画
        if (!parentCard.wasDragged)
            transform.DOScale(1, scaleTransition).SetEase(scaleEase);
    }

    //NOTE::鼠标按键松开执行
    private void PointerUp(Card card, bool longPress)
    {
        //NOTE::是否启用缩放动画，根据长按或短按设置目标缩放值
        if(scaleAnimations)
            transform.DOScale(longPress ? scaleOnHover : scaleOnSelect, scaleTransition).SetEase(scaleEase);
        //NOTE::用画布的排序覆盖功能。
        canvas.overrideSorting = false;

        //NOTE::将阴影的位置更新为 shadowDistance，可能用于调整阴影的显示效果。
        visualShadow.localPosition = shadowDistance;
        //NOTE::确保阴影在鼠标松开时正确显示在卡片下方
        shadowCanvas.overrideSorting = true;
    }

    //NOTE::类别上面
    private void PointerDown(Card card)
    {
        if(scaleAnimations)
            transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);
            
        visualShadow.localPosition += (-Vector3.up * shadowOffset);
        shadowCanvas.overrideSorting = false;
    }

    //NOTE::销毁时终止该对象上的所有动画
    void OnDestroy() {
        
        DOTween.KillAll(); 
    }
}
