using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeManager : MonoBehaviour
{

    public List<Spike> spikes;
    public float delayBetweenSpikes = 0.5f; 
    public float delayBetweenCycles = 2f;

    void Start()
    {
        StartCoroutine(SpikeSequenceLoop());
    }

    IEnumerator SpikeSequenceLoop()
    {
        while (true)
        {
            for (int i = 0; i < spikes.Count; i++)
            {
                spikes[i].Activate();
                yield return new WaitForSeconds(delayBetweenSpikes);
            }

            yield return new WaitForSeconds(delayBetweenCycles);
        }
    }
}
