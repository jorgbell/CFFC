using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedSpriteEffect : MonoBehaviour
{
    // The array of sprite renderers for the stacked sprites
    public SpriteRenderer[] spriteRenderers;

    // The vertical offset for each of the stacked sprites
    public float verticalOffset = 10.0f;

    // The horizontal offset for each of the stacked sprites
    public float horizontalOffset = 5.0f;

    // The direction in which the sprites scroll
    public Vector3 scrollDirection = Vector3.up;

    // The starting positions of the stacked sprites
    private Vector3[] spriteStartPositions;

    [SerializeField]
    private GameObject player;

    private Quaternion initialRotation;

    void Start()
    {
        // Initialize the array of starting positions
        spriteStartPositions = new Vector3[spriteRenderers.Length];
        initialRotation = transform.rotation;

        // Save the starting positions of each of the sprites
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteStartPositions[i] = spriteRenderers[i].transform.localPosition;
        }
    }

    void Update()
    {
        transform.forward = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z).normalized;

        Debug.Log(transform.forward);

        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.yellow);

        float deltaOffset = Input.GetAxis("Horizontal") * 0.001f;
        float deltaVOffset = Input.GetAxis("Vertical") * 0.001f;

        horizontalOffset -= deltaOffset;

        horizontalOffset = Mathf.Clamp(horizontalOffset, -0.2f, 0.2f);

        verticalOffset = Mathf.Clamp(verticalOffset, -0.2f, 0.2f);

        // Update the positions of each of the stacked sprites
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            // Calculate the current position of the sprite
            Vector3 spritePos = spriteStartPositions[i];

            // Add the GameObject's position to the sprite position to account for movement
            spritePos += new Vector3(horizontalOffset * i, verticalOffset * i);

            // Set the position of the sprite
            spriteRenderers[i].transform.localPosition = new Vector3(spritePos.x, spritePos.y, spritePos.z);
        }
    }
}
