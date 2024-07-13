using MusicClubManager.Cms.Blazor.Models.FormModels;

namespace MusicClubManager.Cms.Blazor.Models.Contexts
{
    public class CreateLineupContext
    {
        public CreateLineupPropertiesFormModel PropertiesFormModel { get; set; } = new();
        public CreateLineupPropertiesFormModel? SavedPropertiesFormModel { get; set; }


        public CreateEditLineupPerformanceFormModel CreateEditPerformanceFormModel { get; set; } = new();
        public IList<CreateEditLineupPerformanceFormModel> SavedPerformanceFormModels { get; set; } = [];
    }
}