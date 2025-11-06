using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 원 스프라이트가 굴러다니며 이동하는 컨트롤러
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class RollingCircleController : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    
    [Header("회전 설정")]
    [SerializeField] private float circleRadius = 0.5f; // 원의 반지름 (스프라이트 크기에 맞게 조정)
    [SerializeField] private bool useSpriteRendererForRadius = true; // 스프라이트 렌더러에서 자동으로 반지름 계산
    
    [Header("참조")]
    [Tooltip("Input Actions 에셋을 할당하세요. 비워두면 자동으로 찾습니다.")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Rigidbody2D rb;
    private InputAction moveAction;
    private Vector2 moveInput;
    private float totalDistanceTraveled = 0f; // 총 이동 거리
    private Vector3 lastPosition; // 이전 프레임 위치
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // SpriteRenderer가 없으면 자동으로 찾기
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        // 스프라이트 렌더러에서 반지름 자동 계산
        if (useSpriteRendererForRadius && spriteRenderer != null && spriteRenderer.sprite != null)
        {
            // 스프라이트의 bounds를 기반으로 반지름 계산
            float spriteSize = Mathf.Max(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y);
            circleRadius = spriteSize * 0.5f;
        }
        
        // Input System 설정
        SetupInputActions();
    }
    
    /// <summary>
    /// Input Actions 설정 (자동으로 찾거나 할당된 것을 사용)
    /// </summary>
    private void SetupInputActions()
    {
        // 할당된 Input Actions가 없으면 자동으로 찾기 시도
        if (inputActions == null)
        {
#if UNITY_EDITOR
            // 에디터에서만 자동으로 찾기 (런타임에서는 Resources.Load 사용)
            inputActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/InputSystem_Actions.inputactions");
            
            if (inputActions != null)
            {
                Debug.Log("[RollingCircleController] Input Actions를 자동으로 찾았습니다: InputSystem_Actions");
            }
#else
            // 런타임에서는 Resources 폴더에서 로드 시도
            inputActions = Resources.Load<InputActionAsset>("InputSystem_Actions");
#endif
        }
        
        // Input Actions에서 Move 액션 찾기
        if (inputActions != null)
        {
            var playerMap = inputActions.FindActionMap("Player");
            if (playerMap != null)
            {
                moveAction = playerMap.FindAction("Move");
                if (moveAction == null)
                {
                    Debug.LogError("[RollingCircleController] 'Player' 액션 맵에서 'Move' 액션을 찾을 수 없습니다!");
                }
            }
            else
            {
                Debug.LogError("[RollingCircleController] 'Player' 액션 맵을 찾을 수 없습니다!");
            }
        }
        else
        {
            Debug.LogError("[RollingCircleController] Input Actions 에셋을 찾을 수 없습니다! Inspector에서 'Input Actions' 필드에 'InputSystem_Actions' 에셋을 할당해주세요.");
        }
    }
    
    private void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.Enable();
        }
        
        lastPosition = transform.position;
    }
    
    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.Disable();
        }
    }
    
    private void Update()
    {
        // 입력 읽기
        if (moveAction != null)
        {
            moveInput = moveAction.ReadValue<Vector2>();
        }
    }
    
    private void FixedUpdate()
    {
        // 이동 처리
        Move();
        
        // 회전 처리 (굴러다니는 효과)
        RotateBasedOnMovement();
        
        // 현재 위치 저장
        lastPosition = transform.position;
    }
    
    /// <summary>
    /// 물리 기반 이동 처리
    /// </summary>
    private void Move()
    {
        Vector2 targetVelocity = moveInput * moveSpeed;
        Vector2 velocityChange = targetVelocity - rb.linearVelocity;
        
        // 가속도와 감속도 적용
        float changeRate = moveInput.magnitude > 0.1f ? acceleration : deceleration;
        velocityChange = Vector2.ClampMagnitude(velocityChange, changeRate * Time.fixedDeltaTime);
        
        rb.linearVelocity += velocityChange;
    }
    
    /// <summary>
    /// 이동 거리에 따라 원을 회전시켜 굴러다니는 효과 구현
    /// </summary>
    private void RotateBasedOnMovement()
    {
        // 이전 프레임과의 이동 거리 계산
        Vector3 currentPosition = transform.position;
        float frameDistance = Vector3.Distance(currentPosition, lastPosition);
        
        // 이동 방향 확인
        Vector2 moveDirection = (currentPosition - lastPosition).normalized;
        
        // 이동 중일 때만 회전
        if (frameDistance > 0.001f && moveInput.magnitude > 0.1f)
        {
            // 원의 둘레 계산
            float circumference = 2f * Mathf.PI * circleRadius;
            
            // 이동 거리에 비례한 회전 각도 계산 (라디안을 도로 변환)
            float rotationAngle = (frameDistance / circumference) * 360f;
            
            // 이동 방향에 따라 회전 방향 결정
            // 오른쪽으로 이동하면 시계 방향, 왼쪽으로 이동하면 반시계 방향
            float rotationDirection = Mathf.Sign(moveDirection.x);
            
            // Y축 기준으로 회전 (2D에서는 Z축 회전)
            transform.Rotate(0f, 0f, -rotationAngle * rotationDirection);
            
            // 총 이동 거리 누적
            totalDistanceTraveled += frameDistance;
        }
    }
    
    /// <summary>
    /// 외부에서 이동 속도 설정
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
    
    /// <summary>
    /// 외부에서 원의 반지름 설정
    /// </summary>
    public void SetCircleRadius(float radius)
    {
        circleRadius = radius;
    }
    
    /// <summary>
    /// 현재 이동 속도 반환
    /// </summary>
    public Vector2 GetVelocity()
    {
        return rb.linearVelocity;
    }
    
    /// <summary>
    /// 총 이동 거리 반환
    /// </summary>
    public float GetTotalDistanceTraveled()
    {
        return totalDistanceTraveled;
    }
    
    private void OnDrawGizmosSelected()
    {
        // 에디터에서 원의 반지름 시각화
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
}

