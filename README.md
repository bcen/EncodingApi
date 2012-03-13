# EncodingApi (Work in progress)
#### A .NET C# client wrapper for the [encoding.com](http://www.encoding.com/) API

Click [here](http://www.encoding.com/api/category/category/complete_api_documentation) for complete documentation.

### This is a preview of the API, all public APIs are subjected to change.

## Basic Usage

Gets a list of media from server:

    EncodingServiceClient client = new EncodingServiceClient(api_id, api_key);

    foreach (var m in client.GetMediaList())
    {
        Console.WriteLine(m.MediaId);
    }