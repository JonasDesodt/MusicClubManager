namespace MusicClubManager.Dto.Transfer
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }

        public IList<ServiceMessage>? Messages { get; set; }
    } 
}