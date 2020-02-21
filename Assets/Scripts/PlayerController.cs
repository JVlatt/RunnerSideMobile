using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private bool _right = true;

    [Header("Vertical Movement")]
    [SerializeField]
    private float _jumpDuration = 1.0f;
    [SerializeField]
    private float _jumpForce = 3.0f;
    private AnimationCurve _jumpCurve;
    private float _jumpTimer = 0;
    private bool _isJumping = false;
    private Vector3 _initPos = new Vector3();
    [SerializeField]
    private float _gravity = 2.0f;

    [Header("WallJump")]
    [SerializeField]
    private float _wallJumpDuration = 2.0f;
    private bool _isWallJumping = false;
    private float _wallJumpTimer = 0f;
    private void OnValidate()
    {
        _jumpCurve = new AnimationCurve(new Keyframe[2]
        {
        new Keyframe(0f,0f,0f,1f),
        new Keyframe(_jumpDuration,_jumpForce)
        });
    }
    void Update()
    {
        HorizontalMovement();
        VerticalMovement();
    }

    private void HorizontalMovement()
    {
        if(!_isWallJumping)
        {
            Vector3 newPos = transform.position;
            if (_right)
            {
                newPos.x += _speed * Time.deltaTime;
            }
            else
            {
                newPos.x -= _speed * Time.deltaTime;
            }
            transform.position = newPos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            _isJumping = false;
        }
        if(collision.gameObject.CompareTag("Wall"))
        {
            if(_isJumping)
            {
                _isWallJumping = true;
                _isJumping = false;
                _wallJumpTimer = 0f;
            }
        }
    }

    private void VerticalMovement()
    {
        Vector3 newPos = transform.position;
        if(_isJumping)
        {
            _jumpTimer += Time.deltaTime;
            if (_jumpTimer < _jumpDuration)
            {
                newPos.y = _initPos.y + _jumpCurve.Evaluate(_jumpTimer);
            }
            else
            {
                newPos.y -= _gravity * Time.deltaTime;
            }
            transform.position = newPos;
        }
        else
        {
            _jumpTimer = 0f;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _initPos = transform.position;
                if(_isWallJumping)
                {
                    _right = !_right;
                    _isWallJumping = false;
                }
                _isJumping = true;
            }
        }
        
    }
}
