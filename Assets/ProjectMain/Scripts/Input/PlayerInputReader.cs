
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private const int BufferSize = 4;

    private readonly SnakeDirection[] directionBuffer =
        new SnakeDirection[BufferSize];

    private int bufferHead;
    private int bufferCount;

    private bool pauseRequested;

    private bool wasStickActive;
    private SnakeDirection lastStickDirection;

    public bool ConsumePausePressed()
    {
        if (!pauseRequested)
            return false;

        pauseRequested = false;
        return true;
    }

    private void Update()
    {
        ReadKeyboard();
        ReadGamepad();
    }

    private void ReadKeyboard()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            pauseRequested = true;

        if (Keyboard.current[Key.UpArrow].wasPressedThisFrame ||
            Keyboard.current[Key.W].wasPressedThisFrame)
        {
            EnqueueDirection(SnakeDirection.Up);
        }
        else if (Keyboard.current[Key.DownArrow].wasPressedThisFrame ||
                 Keyboard.current[Key.S].wasPressedThisFrame)
        {
            EnqueueDirection(SnakeDirection.Down);
        }
        else if (Keyboard.current[Key.LeftArrow].wasPressedThisFrame ||
                 Keyboard.current[Key.A].wasPressedThisFrame)
        {
            EnqueueDirection(SnakeDirection.Left);
        }
        else if (Keyboard.current[Key.RightArrow].wasPressedThisFrame ||
                 Keyboard.current[Key.D].wasPressedThisFrame)
        {
            EnqueueDirection(SnakeDirection.Right);
        }
    }

    private void ReadGamepad()
    {
        if (Gamepad.current == null)
        {
            wasStickActive = false;
            return;
        }

        // D-Pad input
        if (Gamepad.current.dpad.up.wasPressedThisFrame)
            EnqueueDirection(SnakeDirection.Up);

        else if (Gamepad.current.dpad.down.wasPressedThisFrame)
            EnqueueDirection(SnakeDirection.Down);

        else if (Gamepad.current.dpad.left.wasPressedThisFrame)
            EnqueueDirection(SnakeDirection.Left);

        else if (Gamepad.current.dpad.right.wasPressedThisFrame)
            EnqueueDirection(SnakeDirection.Right);

        // Left Stick input
        Vector2 stick = Gamepad.current.leftStick.ReadValue();

        if (stick.magnitude > 0.5f)
        {
            SnakeDirection direction;

            if (Mathf.Abs(stick.x) > Mathf.Abs(stick.y))
            {
                direction = stick.x > 0
                    ? SnakeDirection.Right
                    : SnakeDirection.Left;
            }
            else
            {
                direction = stick.y > 0
                    ? SnakeDirection.Up
                    : SnakeDirection.Down;
            }

            if (!wasStickActive || direction != lastStickDirection)
            {
                EnqueueDirection(direction);
                lastStickDirection = direction;
            }

            wasStickActive = true;
        }
        else
        {
            wasStickActive = false;
        }
    }

    private void EnqueueDirection(SnakeDirection direction)
    {
        // Ignore new input if the buffer is full.
        if (bufferCount >= BufferSize)
            return;

        int index = (bufferHead + bufferCount) % BufferSize;

        directionBuffer[index] = direction;
        bufferCount++;
    }

    public bool TryConsumeDirection(out SnakeDirection direction)
    {
        if (bufferCount == 0)
        {
            direction = SnakeDirection.Right;
            return false;
        }

        direction = directionBuffer[bufferHead];

        bufferHead = (bufferHead + 1) % BufferSize;
        bufferCount--;

        return true;
    }

    public void ClearDirectionBuffer()
    {
        bufferHead = 0;
        bufferCount = 0;
        wasStickActive = false;
    }
}
