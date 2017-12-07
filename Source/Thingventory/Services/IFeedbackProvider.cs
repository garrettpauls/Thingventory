using System.Collections.Generic;

namespace Thingventory.Services
{
    public interface IFeedbackProvider
    {
        void PopulateFeedbackData(IDictionary<string, string> data);
    }
}
