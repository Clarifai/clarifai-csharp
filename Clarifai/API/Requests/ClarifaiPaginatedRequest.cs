namespace Clarifai.API.Requests
{
    /// <summary>
    /// A paginated Clarifai request. It divides the results into pages.
    /// </summary>
    /// <typeparam name="T">the type of the response content</typeparam>
    public abstract class ClarifaiPaginatedRequest<T> : ClarifaiRequest<T>
    {
        private int? _page;
        private int? _perPage;

        /// <inheritdoc />
        protected ClarifaiPaginatedRequest(IClarifaiClient client) : base(client)
        { }

        /// <summary>
        /// The page of the results. Counting starts with 1.
        /// </summary>
        /// <param name="page">the page</param>
        /// <returns>this request instance</returns>
        public ClarifaiPaginatedRequest<T> Page(int page)
        {
            _page = page;
            return this;
        }

        /// <summary>
        /// Number of outputs to have per page.
        /// </summary>
        /// <param name="perPage">number per page</param>
        /// <returns>this request instance</returns>
        public ClarifaiPaginatedRequest<T> PerPage(int perPage)
        {
            _perPage = perPage;
            return this;
        }

        /// <inheritdoc />
        protected override string BuildUrl()
        {
            if (_page == null && _perPage == null)
            {
                return Url;
            }
            else
            {
                return Url + "?" +
                       (_page != null ? "page=" + _page : "") +
                       (_page != null && _perPage != null ? "&" : "") +
                       (_perPage != null ? "per_page=" + _perPage : "");
            }
        }
    }
}
