using MusicClubManager.Dto.Result;
using MusicClubManager.Dto.Transfer;

namespace MusicClubManager.Ui.Mvc.Models.ViewModels
{
    public class HomeViewModel
    {
        //NEWS

        public required PagedServiceResult<IList<LineupResult>> Confirmed { get; set; }

        public required PagedServiceResult<IList<LineupResult>> Soon { get; set; }
    }
}
