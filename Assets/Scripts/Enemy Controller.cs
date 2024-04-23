using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float velocity;
    public float distance;
    public Vector3 endPosition;
    public Vector3 startPosition;

    private bool movingToEnd;
    private string collidingWith;
    private AudioSource enemyAudio;

    private void Awake()
    {
        enemyAudio = GetComponent<AudioSource>();
        enemyAudio.Play();
    }

    private void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector2(startPosition.x, startPosition.y + distance);
    }

    private void Update()
    {
        EnemyMovement();

        if(collidingWith == "Player")
        {
            enemyAudio.Stop();
        }
    }

    private void EnemyMovement()
    {
        Vector2 goingToPosition = (movingToEnd) ? endPosition : startPosition;
        transform.position = Vector3.MoveTowards(transform.position, goingToPosition, velocity * Time.deltaTime);

        if (transform.position == endPosition)
            movingToEnd = false;
        if (transform.position == startPosition)
            movingToEnd = true;
    }

    #region OnTrigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collidingWith = collision.gameObject.tag;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidingWith = "";
    }
    #endregion
}
