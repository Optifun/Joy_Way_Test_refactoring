using JoyWay.Game.Character;
using Mirror;
using UnityEngine;
using UnityEngine.Animations;

namespace JoyWay.Game.Projectiles
{
    public class PickableProjectile : Projectile
    {
        [SerializeField] private ParentConstraint _parentConstraint;

        private bool _isInHand = false;

        public bool CanPick => !_isInHand;
        
        [Server]
        public void Pickup(Transform hand)
        {
            PutInHand(hand);
        }

        [Server]
        public override void Throw(Vector3 direction, uint sender)
        {
            ReleaseFromHand(); 
            base.Throw(direction, sender);
        }

        private void PutInHand(Transform hand)
        {
            _rigidbody.isKinematic = true;
            _collider.isTrigger = false;
            _isInHand = true;
            ConstraintSource newConstraintSource = new ConstraintSource();
            newConstraintSource.sourceTransform = hand;
            newConstraintSource.weight = 1;
            _parentConstraint.AddSource(newConstraintSource);
            _parentConstraint.constraintActive = true;
        }

        private void ReleaseFromHand()
        {
            _rigidbody.isKinematic = false;
            _collider.isTrigger = true;
            _isInHand = false;
            if (_parentConstraint.sourceCount > 0)
            {
                _parentConstraint.RemoveSource(0);
            }
            _parentConstraint.constraintActive = false;
        }
    }
}