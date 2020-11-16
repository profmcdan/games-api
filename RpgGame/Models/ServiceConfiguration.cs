using Amazon;

namespace RpgGame.Models
{
    public class ServiceConfiguration
    {
        public AWSS3Configuration AWSS3 { get; set; }
    }

    public class AWSS3Configuration
    {
        public string BucketName { get; set; }
    }
} 