namespace IF.Lastfm.Demo.Apollo.TestPages.ViewModels
{
    public class PageProgress
    {
        public int CurrentPage { get; set; }
        public int ExpectedPage { get; set; }
        public int? TotalPages { get; set; }

        public bool PageLoadInProgress
        {
            get { return ExpectedPage > CurrentPage; }
        }

        public bool CanGoToNextPage()
        {
            if (TotalPages.HasValue)
            {
                if (CurrentPage >= TotalPages.Value)
                {
                    return false;
                }
            }
            
            ExpectedPage = CurrentPage + 1;
            return true;
        }

        public void PageLoaded(bool success)
        {
            if (success)
            {
                CurrentPage = ExpectedPage;
            }
            else
            {
                ExpectedPage = CurrentPage;
            }
        }
    }
}