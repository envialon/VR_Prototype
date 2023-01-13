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


        [Range(0.1f, 1)]
        public float vibrationAmplitude = 1;
        public float vibrationDuration = 1;

        private bool alreadyShot = false;

        int layerMask = 1 << 3;

        public Rigidbody rb;

        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        public void OnEnable()
        {
            alreadyShot = false;
        }

        public void PlayEffects()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }

        public void Shoot(ActivateEventArgs args)
        {
            if (!alreadyShot)
            {
                if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
                {
                    TriggerHaptic(controllerInteractor.xrController);
                }
                PlayEffects();
                if (Physics.Raycast(shotOrigin.position, shotOrigin.forward, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    Debug.DrawRay(shotOrigin.position, shotOrigin.forward * hit.distance, Color.red);
                    EnemyPool.instance.EnemyHit(hit.transform.gameObject.GetComponent<Enemy>().id);
                }
                alreadyShot = true;
            }

        }

        private void TriggerHaptic(XRBaseController controller)
        {
            controller.SendHapticImpulse(vibrationAmplitude, vibrationDuration);
        }
    }
}