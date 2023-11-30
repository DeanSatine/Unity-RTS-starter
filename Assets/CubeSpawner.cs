using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab; // Reference to the cube prefab
    public GameObject spawnableObjectPrefab; // Reference to the object to be spawned

    private bool isDragging = false;
    private GameObject spawnedCube;
    private Rigidbody cubeRigidbody; // Reference to the Rigidbody component

    private void Start()
    {
        cubeRigidbody = cubePrefab.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // If the cube is spawned and being dragged, update its position
        if (isDragging && spawnedCube != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10; // Distance from the camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            spawnedCube.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
        }

        // Check for mouse button down (left mouse button)
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse is over the spawned cube
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == spawnedCube)
            {
                // Start dragging the cube
                isDragging = true;
                cubeRigidbody.isKinematic = true; // Disable physics while dragging
            }
            else
            {
                // Stop dragging the cube if it's not clicked on
                isDragging = false;
            }
        }

        // Check for mouse button up (left mouse button)
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            // Stop dragging the cube
            isDragging = false;
            cubeRigidbody.isKinematic = false; // Enable physics after releasing

            // Start the coroutine to spawn objects every few seconds
            StartCoroutine(SpawnObjectsCoroutine());
        }
    }

    public void OnButtonClick()
    {
        if (spawnedCube != null)
        {
            // Cube is already spawned, do something else (e.g., place it on the map).
            // Implement this based on your requirements.
            Debug.Log("Cube already spawned!");
        }
        else
        {
            // Spawn the cube at a fixed position (can be adjusted based on your needs).
            Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 5f;
            spawnedCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            cubeRigidbody = spawnedCube.GetComponent<Rigidbody>();
        }
    }

    private IEnumerator SpawnObjectsCoroutine()
    {
        while (true)
        {
            // Calculate a random offset from the cube's position
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            // Spawn the object at the cube's position with the random offset
            Instantiate(spawnableObjectPrefab, spawnedCube.transform.position + randomOffset, Quaternion.identity);

            // Adjust the delay (in seconds) between spawns as needed
            yield return new WaitForSeconds(f);
        }
    }
}
