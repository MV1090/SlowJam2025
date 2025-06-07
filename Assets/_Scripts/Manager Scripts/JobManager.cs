using System;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : Singleton<JobManager>
{
    [SerializeField] BaseJob[] allJobs;
    public BaseJob currentJob;

    public Dictionary<JobState, BaseJob> jobDictionary = new Dictionary<JobState, BaseJob>();

    public int JobIndex;
    public Action jobChanged;
    public enum JobState
    {
        Unemployed, Delivery, TaxiDriver, Sweeper  
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (allJobs == null)
            return;

        foreach(BaseJob job in allJobs)
        {
            if(job == null)
                continue;

            job.InitState(this);

            if (jobDictionary.ContainsKey(job.jobState))
                continue;

            jobDictionary.Add(job.jobState, job);

            //foreach(JobState state in jobDictionary.Keys)
            //    jobDictionary[state].gameObject.SetActive(false);
        }

        //ActivateNewJob(JobState.Unemployed);
    }

    public void ActivateNewJob(JobState state)
    {
        if (!jobDictionary.ContainsKey(state))
            return;

        if(currentJob != null)
        {
            //currentJob.ExitState();
            //currentJob.gameObject.SetActive(false);
        }

        currentJob = jobDictionary[state];
        JobIndex = Array.IndexOf(Enum.GetValues(typeof(JobState)), state);
        //currentJob.gameObject.SetActive(true);
        //currentJob.EnterState();
        jobChanged?.Invoke();

    }

    // Randomly choose a new Job from the Job list
    public void ChooseRandomJob()
    {
        int randI = UnityEngine.Random.Range(0, allJobs.Length);
        currentJob = allJobs[randI];        
    }
}
