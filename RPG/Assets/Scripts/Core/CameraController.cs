using UnityEngine;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour
    {
        public Vector3 offset;

        [SerializeField] float zoomSpeed = 50f;
        [SerializeField] float yawSpeed = 100f;

        private Transform player;
        private float currentZoom = 10f;
        private float minZoom = 5f;
        private float maxZoom = 50f;
        private float currentYaw = 0f;


        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            currentZoom -= Input.GetAxis("Vertical") * zoomSpeed * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            currentYaw += Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        }

        void LateUpdate()
        {
            transform.position = player.position - offset * currentZoom;
            transform.LookAt(player);

            transform.RotateAround(player.position, Vector3.up, currentYaw);
        }
    }
}
