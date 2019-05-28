using System;
using Newtonsoft.Json.Linq;

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
        protected ClarifaiPaginatedRequest(IClarifaiHttpClient httpClient) : base(httpClient)
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
                if (Method == RequestMethod.GET)
                {
                    return Url + "?" +
                           (_page != null ? "page=" + _page : "") +
                           (_page != null && _perPage != null ? "&" : "") +
                           (_perPage != null ? "per_page=" + _perPage : "");
                }
                else if (Method == RequestMethod.POST)
                {
                    return Url;
                }
                else
                {
                    throw new InvalidOperationException(
                        "Pagination only supported for GET and POST");
                }
            }
        }

        /// <summary>
        /// Seal this method to avoid accidentally using it instead of PaginatedHttpRequestBody
        /// in children ClarifaiPaginatedRequest.
        /// </summary>
        protected sealed override JObject HttpRequestBody()
        {
            return PaginatedHttpRequestBody();
        }

        /// <summary>
        /// The HTTP request body used in paginated requests.
        /// </summary>
        protected virtual JObject PaginatedHttpRequestBody()
        {
            JObject body = base.HttpRequestBody();

            if (Method == RequestMethod.POST)
            {
                var pagination = new JObject();
                if (_page != null)
                {
                    pagination["page"] = _page;
                }

                if (_perPage != null)
                {
                    pagination["per_page"] = _perPage;
                }

                if (pagination.Count > 0)
                {
                    body["pagination"] = pagination;
                }
            }

            return body;
        }
    }
}
