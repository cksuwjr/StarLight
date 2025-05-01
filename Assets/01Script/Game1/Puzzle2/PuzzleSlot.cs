using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    private PuzzleManager puzzleManager;
    public Puzzle puzzle;

    public void Init(PuzzleManager puzzleManager)
    {
        this.puzzleManager = puzzleManager;
        if (transform.GetChild(0).TryGetComponent<Puzzle>(out puzzle))
            puzzle.Init(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            puzzleManager.Current(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            puzzleManager.CurrentOut(this);
    }
}
