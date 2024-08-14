namespace MusicClubManager.Cms.Wpf.Interfaces
{
   public interface ISelect<TModel>
    {
        TModel? SelectedItem { get; set; }

        void Select(TModel item);
    }
}
