
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject MenuPrefab;

    private PlayerInput playerInput;
    // 플레이어와 UI의 상호작용을 관리하는 스크립트
    private bool uiOpen;


    private InventoryUI inventoryUI;
    private CanvasGroup inventoryCanvasGroup;
    private bool inventoryOn;

    private MainMenuHandler mainMenuHandler;
    private bool mainMenuOn;

    private void Awake()
    {
        inventoryUI = GetComponentInChildren<InventoryUI>();
        if(inventoryUI != null)
            inventoryCanvasGroup = inventoryUI.GetComponent<CanvasGroup>();

        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerInput>(out PlayerInput playerInput))
        {
            this.playerInput = playerInput;
        }
        if (MenuPrefab != null)
        {
            GameObject mainMenuObject = Instantiate(MenuPrefab, this.transform);
            mainMenuHandler = mainMenuObject.GetComponentInChildren<MainMenuHandler>();
            
        }
    }

    private void LimitCursor(bool isOn)
    {
        if (isOn)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Inventory()
    {
        if (inventoryUI == null)
            return;

        // 메인메뉴가 켜져있을 땐 인벤토리 접근 불가
        if (mainMenuOn)
            return;
        bool isOn = inventoryUI.InventoryTrigger();
        playerInput.enabled = !isOn;
        LimitCursor(isOn);
    }

    public void Menu()
    {
        bool isOn = mainMenuHandler.MainMenuTrigger();

        playerInput.enabled = !isOn;
        LimitCursor(isOn);
    }

}
