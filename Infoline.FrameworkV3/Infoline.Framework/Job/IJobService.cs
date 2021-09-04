using System;


namespace Infoline.Jobs
{
    public interface IJobService : IService
    {
        JobItem GetJob(Guid id);
        void SetStatus(string message, string progresmessage, double progress);
        void SetStatus(double progress, string progresmessage);
        void SetStatus(double progress);
        void SetStatus(int current, int total);
        JobItem CreateJob(Action task);
        JobItem CreateJob(Action<JobItem> task);
        void CollectJobs();
        JobItem CurrentItem { get; }
    }

}
