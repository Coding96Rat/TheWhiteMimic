using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IGameSetting
{
    [Header("References")]
    [SerializeField] private Transform FollowCam; // Player 하위의 카메라 타겟
    private Transform mainCameraTransform;        // 실제 메인 카메라 (방향 기준점)
    private CinemachineCamera playerVirtualCamera;

    [Space(15)]
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float rotationSmoothTime = 0.1f; // 회전 부드러움 정도

    private float currentSpeed;
    private float turnSmoothVelocity; // 스무스 댐프용 참조 변수
    private bool isMoving;

    // ================================================== Look
    [Space(15)]
    [Header("Looking Parameters")]
    [SerializeField] private float sensitivityAmount;
    private Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);

    [SerializeField] private float pitchLimit = 85f;

    private bool lockOn;
    private float currentPitch;
    private float currentYaw; // 카메라의 좌우 회전값을 관리하는 변수

    private float invertY;
    private int toggledInvertY;

    [Space(15)]
    [Header("Physics Parameters")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 _velocity;
    private bool _isGrounded;

    [Space(15)]
    [Header("Animation Parameters")]
    [SerializeField] private float animationBlendSpeed = 10f;
    private Animator _animator;
    private int xVelocity_AnimParameter;
    private int yVelocity_AnimParameter;
    private int speed_AnimParameter;    
    private int moving_AnimParameter;
    private Vector2 animationVelocity;

    #region Components
    private InputHandler inputHandler;
    public InputHandler InputHandler { get { return inputHandler; } }
    private CharacterController characterController;
    #endregion

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        playerVirtualCamera = FindFirstObjectByType<CinemachineCamera>();

        // [중요] 메인 카메라의 트랜스폼을 가져옵니다. 이동의 기준이 됩니다.
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("Main Camera가 씬에 없습니다!");
        }
    }

    private void Start()
    {
        // 설정 초기화
        toggledInvertY = PlayerPrefs.GetInt("invertY", 0);
        invertY = (toggledInvertY == 1) ? -1.0f : 1.0f;

        sensitivityAmount = PlayerPrefs.GetFloat("sensitivity", 1.0f); // 감도 기본값 1.0 추천
        lookSensitivity = new Vector2(sensitivityAmount, sensitivityAmount);

        if (playerVirtualCamera != null)
        {
            playerVirtualCamera.Lens.FieldOfView = PlayerPrefs.GetFloat("fov", 60);
        }

        //xVelocity_AnimParameter = Animator.StringToHash("X_Velocity");
        //yVelocity_AnimParameter = Animator.StringToHash("Y_Velocity");
        speed_AnimParameter = Animator.StringToHash("Speed");
        moving_AnimParameter = Animator.StringToHash("Moving");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 시작 시 현재 카메라의 각도를 가져옴
        currentYaw = FollowCam.localEulerAngles.y;
    }

    void Update()
    {
        CheckInput();
        GroundUpdate();
        GravityUpdate();

        // [중요] LookUpdate와 MoveUpdate가 서로 영향을 줍니다.
        // 순서는 크게 상관없으나, 보정 로직이 들어가므로 함께 봅니다.
        LookUpdate();
        MoveUpdate();
    }

    private void GroundUpdate()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -3f;
        }
    }

    void CheckInput()
    {
        currentSpeed = inputHandler.IsSprinting ? sprintSpeed : moveSpeed;
        isMoving = inputHandler.MoveInput != Vector2.zero;
        _animator.SetBool(moving_AnimParameter, isMoving);



        // 스프린트 시 속도 조절 (뒤로 갈 땐 느리게 등) 로직은 필요하면 유지
        //if (inputHandler.IsSprinting && inputHandler.MoveInput.y < 0)
        //{
        //    currentSpeed = sprintSpeed / 2;
        //}
    }

    void MoveUpdate()
    {
        // 입력이 없으면 애니메이션만 처리하고 종료
        if (!isMoving)
        {
            if(_animator.GetFloat(speed_AnimParameter) != 0)
                AnimationUpdate(0);
            return;
        }

        // 1. 이동 방향 계산 (카메라 기준)
        // atan2(x, y)를 사용하여 입력 벡터의 각도를 구하고, 카메라의 Y 회전값을 더합니다.
        // 이렇게 하면 "카메라가 보는 앞쪽"이 0도가 됩니다.
        float targetAngle = Mathf.Atan2(inputHandler.MoveInput.x, inputHandler.MoveInput.y) * Mathf.Rad2Deg + mainCameraTransform.eulerAngles.y;

        // 2. 캐릭터 회전 (스무딩 적용)
        // 현재 캐릭터 각도에서 목표 각도로 부드럽게 회전합니다.
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);

        // [핵심] 캐릭터가 회전하기 전 각도를 저장
        float oldAngle = transform.eulerAngles.y;

        // 캐릭터 회전 적용
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // [핵심 - 카메라 보정 로직]
        // 캐릭터 몸통을 'angle'만큼 돌렸으므로, 자식인 카메라도 같이 돌아가 버립니다.
        // 따라서 돌아간 차이(delta)만큼 카메라 Yaw 값을 반대로 돌려줘야 시선이 고정됩니다.
        float deltaAngle = Mathf.DeltaAngle(oldAngle, angle);
        currentYaw -= deltaAngle;

        // 보정된 Yaw값을 FollowCam에 즉시 적용 (안 하면 한 프레임 튀는 현상 발생 가능)
        FollowCam.localRotation = Quaternion.Euler(currentPitch, currentYaw, 0);

        // 3. 실제 이동
        // 계산된 각도(targetAngle) 방향으로 전진합니다.
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        characterController.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

        // 4. 애니메이션
        // 3인칭 어드벤처에서는 보통 캐릭터가 회전해서 전진하므로 X값은 0, Y값(Forward)만 씁니다.
        AnimationUpdate(currentSpeed);
        // *참고: 기존 로직 유지를 원하면 아래 주석 해제하여 사용
        //AnimationUpdate(0, inputHandler.MoveInput.y * currentSpeed);
    }

    void AnimationUpdate(float speed)
    {
        _animator.SetFloat(speed_AnimParameter, speed);
        //animationVelocity.x = Mathf.Lerp(animationVelocity.x, xVel, animationBlendSpeed * Time.deltaTime);
        //animationVelocity.y = Mathf.Lerp(animationVelocity.y, yVel, animationBlendSpeed * Time.deltaTime);

        //_animator.SetFloat(xVelocity_AnimParameter, animationVelocity.x);
        //_animator.SetFloat(yVelocity_AnimParameter, animationVelocity.y);
    }

    void GravityUpdate()
    {
        if (_isGrounded == false)
        {
            _velocity.y += gravity * Time.deltaTime;
        }
        characterController.Move(_velocity * Time.deltaTime);
    }

    void LookUpdate()
    {
        Vector2 lookInputValue = new Vector2(inputHandler.LookInput.x * lookSensitivity.x, inputHandler.LookInput.y * lookSensitivity.y);

        // 1. Pitch (상하) - 기존 유지
        currentPitch -= lookInputValue.y * invertY;

        // 2. Yaw (좌우) - 마우스 입력만큼 누적
        currentYaw += lookInputValue.x;

        // 3. 적용 (플레이어 몸통은 건드리지 않고, FollowCam만 돌림)
        FollowCam.localRotation = Quaternion.Euler(currentPitch, currentYaw, 0);
    }

    public void UpdateGameSettingInGame()
    {
        toggledInvertY = PlayerPrefs.GetInt("invertY", 0);
        invertY = (toggledInvertY == 1) ? -1.0f : 1.0f;

        sensitivityAmount = PlayerPrefs.GetFloat("sensitivity", 0.2f);
        lookSensitivity = new Vector2(sensitivityAmount, sensitivityAmount);

        if (playerVirtualCamera != null)
            playerVirtualCamera.Lens.FieldOfView = PlayerPrefs.GetFloat("fov", 60);
    }
}