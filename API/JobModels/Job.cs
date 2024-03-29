using API.UserModels;

namespace API.JobModels
{
    public class Job
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }


        public Guid UserId { get; init; }


        public Job()
        {
            Id = Guid.NewGuid();

        }

    }
}
