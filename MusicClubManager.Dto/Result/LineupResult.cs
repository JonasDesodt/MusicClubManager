namespace MusicClubManager.Dto.Result
{
    public class LineupResult
    {
        public required int Id { get; set; }

        public string? Name { get; set; }
        public required DateTime Doors { get; set; }
        public bool IsSoldOut { get; set; }

        public EventResult? Event { get; set; }

        public ImageResult? ImageResult { get; set; }

        public required PagedResult<IList<LineupPerformanceResult>> PagedLineupPerformanceResult { get; set; }
    }
}