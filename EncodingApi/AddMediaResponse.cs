namespace EncodingApi
{
    public class AddMediaResponse : XmlResponse
    {
        public AddMediaResponse()
            : this("<response/>")
        {
        }

        public AddMediaResponse(string xml)
            : base(xml)
        {
        }

        public string MediaId
        {
            get
            {
                return GetXmlElementInnerText("MediaID");
            }
        }
    }
}
