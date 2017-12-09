using System.Collections.Generic;

namespace Thingventory.Core.Services
{
    public interface IFeedbackProvider
    {
        void PopulateFeedbackData(IDictionary<string, string> data);
    }
}
