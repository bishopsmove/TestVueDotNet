namespace TestVueDotNet.Server
{
    public class AppConfig
    {
        public AppConfig()
        {
        }

        public TestVueDotNet.Data.Config Data { get; set; }
        public TestVueDotNet.Server.Config Server { get; set; }
        public TestVueDotNet.Service.Config Service { get; set; }
    }
}
