using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum CursorTools
{
    Promote = 0,
    Unfollow = 1,
    Verify = 2,
    Select = 3
}

public class PlayerCursor : MonoBehaviour
{
    public static event Action NewToolSelected;
    [SerializeField] private InputAction click;
    [SerializeField] private InputAction pos;
    public CursorTools currentTool = CursorTools.Select;

    public Promote promoter;
    public Unfollow unfollower;
    public Verifier verifier;

    private void OnEnable()
    {
        GameController.StartGame += StartListeningForPresses;
        GameController.StopGame += ResetAllValues;
    }

    private void StartListeningForPresses(GameVars obj)
    {
        click.Enable();
        pos.Enable();
        click.performed += ClickPressed;
        click.canceled += ClickReleased;
    }

    private void OnDisable()
    {
        click.performed -= ClickPressed;
        click.canceled -= ClickReleased;
        click.Disable();
        pos.Disable();
    }

    public void ChangeTool(CursorTools newTool)
    {
        if (currentTool is CursorTools.Unfollow) unfollower.ClearCut();
        ResetCurrentToolButton();

        currentTool = newTool;
    }

    public void ChangeTool(string newTool)
    {
        // Added to try and turn OFF buttons by pressing them again but does not work due to Update calls?
        if (newTool == "Promote" && currentTool == CursorTools.Promote ||
            newTool == "Unfollow" && currentTool == CursorTools.Unfollow ||
            newTool == "Verify" && currentTool == CursorTools.Verify)
        {
            // Turn the tool off and cancel the usage
            ResetCurrentToolButton();
            return;
        }

        if (currentTool is CursorTools.Unfollow) unfollower.ClearCut();

        //new tool button should be set to selected(Grey) (This is done in editor via UnityEvents on the image game object).
        ResetCurrentToolButton();

        currentTool = newTool switch
        {
            "Promote" => CursorTools.Promote,
            "Unfollow" => CursorTools.Unfollow,
            "Verify" => CursorTools.Verify,
            "Select" => CursorTools.Select,
            _ => currentTool
        };
        NewToolSelected?.Invoke();

        //Highlight valid nodes for current tool
        NodeNetworkManager.NodeHighlighter.HighlightValidNodes(currentTool);
    }

    private void ResetCurrentToolButton()
    {
        //Deselect button (White)
        switch (currentTool)
        {
            case CursorTools.Promote:
                CooldownRadialManager.instance.GetRadial((int) CursorTools.Promote).ButtonDeselected();
                break;

            case CursorTools.Unfollow:
                CooldownRadialManager.instance.GetRadial((int) CursorTools.Unfollow).ButtonDeselected();
                break;

            case CursorTools.Verify:
                CooldownRadialManager.instance.GetRadial((int) CursorTools.Verify).ButtonDeselected();
                break;
        }

        //Highlight valid nodes for current tool
        NodeNetworkManager.NodeHighlighter.DeHighlightNodes();
    }

    private void ClickPressed(InputAction.CallbackContext callback)
    {
        // Only if a tool is selected
        if (currentTool is CursorTools.Select) return;

        UseCurrentTool(GetMousePosition(pos.ReadValue<Vector2>()));
    }

    private void ClickReleased(InputAction.CallbackContext callback)
    {
        //If Unfollow - Also check for release or if its held
        if (currentTool is not CursorTools.Unfollow) return;

        var toolUsedSuccessfully = unfollower.EndCut(GetMousePosition(pos.ReadValue<Vector2>()));
        if (toolUsedSuccessfully) ChangeTool(CursorTools.Select);
    }

    private void Update()
    {
        //If Unfollow - Also check for release or if its held
        if (currentTool is not CursorTools.Unfollow) return;

        unfollower.CheckMaxDist(GetMousePosition(pos.ReadValue<Vector2>()));
    }

    private void UseCurrentTool(Vector2 mouseClickPos)
    {
        bool wasToolUsedSuccessfully = false;

        switch (currentTool)
        {
            case CursorTools.Unfollow:
                unfollower.StartCut(mouseClickPos);
                //Keep looking for mouse up
                break;
            case CursorTools.Promote:
                wasToolUsedSuccessfully = promoter.PromoteNodeAtPosition(mouseClickPos);
                break;
            case CursorTools.Verify:
                wasToolUsedSuccessfully = verifier.VerifyNodeAtPos(mouseClickPos);
                break;
            case CursorTools.Select:
                break;
        }

        if (wasToolUsedSuccessfully)
        {
            //A click tool was consumed, return to select mode.
            ChangeTool(CursorTools.Select);
        }
    }

    public static Vector2 GetMousePosition(Vector2 mouseClickPos)
    {
        //Convert to world coords and set
        mouseClickPos = Camera.main.ScreenToWorldPoint(mouseClickPos);

        return mouseClickPos;
    }

    private void ResetAllValues(string s)
    {
        currentTool = CursorTools.Select;
        click.performed -= ClickPressed;
        click.canceled -= ClickReleased;
        click.Disable();
        pos.Disable();
    }
}