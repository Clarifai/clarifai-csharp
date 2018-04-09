using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Feedbacks
{
    public class FaceFeedback
    {
        private readonly IEnumerable<ConceptFeedback> _identityConceptFeedbacks;

        public FaceFeedback(IEnumerable<ConceptFeedback> identityConceptFeedbacks)
        {
            _identityConceptFeedbacks = identityConceptFeedbacks;
        }

        public JObject Serialize()
        {
            return new JObject(new JProperty("identity",
                new JObject(new JProperty("concepts",
                    _identityConceptFeedbacks.Select(cf => cf.Serialize())))));
        }
    }
}
