using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.Input;

namespace VR_Prototype
{
    public class GunBehaviour : MonoBehaviour
    {
        public ParticleSystem[] particles;
        public Transform shotOrigin;
        public Rigidbody rb;

        private LineRenderer line;

        [Range(0.1f, 1)]
        public float vibrationAmplitude = 1;
        public float vibrationDuration = 1;

        public float SphereCastRadius = 0.3f;

        private bool alreadyShot = false;
        private int layerMask = 1 << 3; // Enemigos es la layer 3

        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody>();            
            line = gameObject.AddComponent<LineRenderer>();
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
        }

        public void OnEnable()
        {
            alreadyShot = false;
        }

        [ContextMenu("Play Effects")]
        public void PlayEffects()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }

        public void Shoot(ActivateEventArgs args)
        {
            //if (!alreadyShot)
            //{
                if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
                {
                    TriggerHaptic(controllerInteractor.xrController);
                }
                PlayEffects();
                
                line.SetPosition(0, shotOrigin.position);
                line.SetPosition(1, shotOrigin.position + shotOrigin.forward * 100);                
                line.enabled = true;

                if (Physics.SphereCast(shotOrigin.position, SphereCastRadius, shotOrigin.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    EnemyPool.instance.EnemyHit(hit.transform.gameObject.GetComponent<Enemy>().id);
                    
                }
                alreadyShot = true;
            //}

        }

        private void TriggerHaptic(XRBaseController controller)
        {
            controller.SendHapticImpulse(vibrationAmplitude, vibrationDuration);
        }
    }
}