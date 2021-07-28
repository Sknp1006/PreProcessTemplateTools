using HalconDotNet;

namespace PreProcessTemplateTools
{
    class TikaModel
    {
        public HTuple MeanGrayValue { get; set; }
        public string OriginPath { get; set; }
        public string SavePath { get; set; }
        public HTuple MinGrayValue { get; set; }
        public HTuple MaxGrayValue { get; set; }
        public HTuple Width { get; set; }
        public HTuple Height { get; set; }
        public HTuple Status { get; set; }
    }
}
