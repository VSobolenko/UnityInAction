using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/ FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.81f;
    public const float baseSpeed = 6.0f;
    private CharacterController _charController;
    private void Awake()
    {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }
    private void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }
    private void OnSpeedChanged(float value)
    {
        speed = baseSpeed * value;
    }
    private void Start()
    {
        _charController = GetComponent<CharacterController>();
    }
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movment = new Vector3(deltaX, 0, deltaZ);
        movment = Vector3.ClampMagnitude(movment, speed);
        movment.y = gravity;

        movment *= Time.deltaTime;
        movment = transform.TransformDirection(movment);
        _charController.Move(movment);

    }
}
