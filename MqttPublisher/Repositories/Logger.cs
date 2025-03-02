namespace MqttPublisher.Repositories
{
    public interface IAppLogger
    {
        void Log(string log);
        List<string> GetLogs();
    }

    public class AppLogger : IAppLogger
    {
        private List<string> _logs = new List<string>();

        public AppLogger() { }

        public void Log(string log) 
        {
            _logs.Insert(0, log);

            if(_logs.Count > 30)
                _logs.RemoveAt(_logs.Count - 1);
        }  

        public List<string> GetLogs() 
        {
            return _logs;
        }
    }
}
