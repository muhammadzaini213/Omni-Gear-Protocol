using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int checkpointIndex;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Checkpoint reached but not confirmed to be player: " + checkpointIndex);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint reached by player: " + checkpointIndex);
            PlayerPrefs.SetInt("CheckpointIndex", checkpointIndex);
            PlayerPrefs.Save();
        }
    }
}