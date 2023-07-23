using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace KennyJam
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;

        [Range(1F, 5F)]
        [SerializeField] private float _speed = 0.5F;

        public void OnMovement(CallbackContext ctx)
        {
            if (!ctx.performed)
            {
                _rb.velocity = Vector2.zero;
                return;
            }


            _rb.velocity = ctx.ReadValue<Vector2>() * _speed;
        }
    }
}