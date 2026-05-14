using YG;

namespace _Project.Scripts.Services
{
    public class AnalyticService
    {
        public void SendMessage(string messageName)
        {
            YG2.MetricaSend(messageName);
        }
    }
}