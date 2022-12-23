using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Same as PointerClickMovement
[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool useNewInput;

    public float rotSpeed = 15.0f;
    public float movSpeed = 6.0f;
    public float jumpSpeed = 16.0f;
    public float gravity = -9.81f;
    public float terminalVelocity = -10f;
    public float minFall = -1.5f;
    public float pushForce = 3.0f;

    public float deceleration = 25f;
    public float targetBuffer = 1.5f;
    
    private CharacterController _characterController;
    private Animator _animator;
    private ControllerColliderHit _contact;
    private float _vertSpeed;

    private float _curSpeed;
    private Vector3 _targetPos = Vector3.zero;
    
    private static readonly int SpeedAnimatorKey = Animator.StringToHash("speed");
    private static readonly int JumpingAnimatorKey = Animator.StringToHash("jumping");

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _vertSpeed = minFall;
    }

    private void Update()
    {
        if (useNewInput)
            UpdateNewInput();
        else
            UpdateOldInput();
    }

    private void UpdateNewInput()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var mouseHit))
            {
                var hitObject = mouseHit.transform.gameObject;
                if (hitObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    _targetPos = mouseHit.point;
                    _curSpeed = movSpeed;   
                }
            }
        }

        if (_targetPos != Vector3.one)
        {
            if (_curSpeed > movSpeed * 0.5f)
            {
                var adjustedPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
                var targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            }

            movement = _curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);

            if (Vector3.Distance(_targetPos, transform.position) < targetBuffer)
            {
                _curSpeed -= deceleration * Time.deltaTime;
                if (_curSpeed <= 0)
                {
                    _targetPos = Vector3.one;
                }
            }
        }
        _animator.SetFloat(SpeedAnimatorKey, movement.sqrMagnitude);
        
        SetupUSeRayCastVerticalSpeed(ref movement);
        movement.y = _vertSpeed;
        
        movement *= Time.deltaTime;
        _characterController.Move(movement);
    }
    
    private void UpdateOldInput()
    {
        Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * movSpeed;
            movement.z = vertInput * movSpeed;
            movement = Vector3.ClampMagnitude(movement, movSpeed);
            
            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;
            
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }
        _animator.SetFloat(SpeedAnimatorKey, movement.sqrMagnitude);
        
        //SetupStandardVerticalSpeed();
        SetupUSeRayCastVerticalSpeed(ref movement);
        movement.y = _vertSpeed;
        
        movement *= Time.deltaTime;
        _characterController.Move(movement);
    }
    
    private void SetupUSeRayCastVerticalSpeed(ref Vector3 activeMoveDirection)
    {
        var hitGround = false;
        RaycastHit hit;
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            var check = (_characterController.height + _characterController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
            }
            else
            {
                _vertSpeed = minFall;
                _animator.SetBool(JumpingAnimatorKey, false);
            }
        }
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }

            if (_contact != null)
            {
                _animator.SetBool(JumpingAnimatorKey, true);
            }
            if (_characterController.isGrounded)
            {
                //activeMoveDirection = _contact.normal * movSpeed;
                //activeMoveDirection += _contact.normal * movSpeed;

                if (Vector3.Dot(activeMoveDirection, _contact.normal) < 0)
                {
                    activeMoveDirection = _contact.normal * movSpeed;
                }
                else
                {
                    activeMoveDirection += _contact.normal * movSpeed;
                }
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;

        var body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }

    // Old variant use only CharacterController.IsGrounded
    private void SetupStandardVerticalSpeed()
    {
        if (_characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
            }
            else
            {
                _vertSpeed = minFall;
            }
        }
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }
        }
        
    }
}
