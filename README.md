# EncodingApi (Work in progress)
#### A .NET C# client wrapper for the [encoding.com](http://www.encoding.com/) API

This is a preview of the API, all public APIs are subjected to change.

Click [here](http://www.encoding.com/api/category/category/complete_api_documentation) for complete documentation.


## Example

Gets a list of media:

    EncodingServiceClient client = new EncodingServiceClient("id", "key");

    foreach (var m in client.GetMediaList())
    {
        Console.WriteLine(m.MediaId);
    }
    
or do it in the raw way:

    var response = client.SendGetMediaListRequest();
    
    foreach (var m in response.MediaList)
    {
        Console.WriteLine(m.MediaId);
    }

it also can be done asynchronously:

    client.GetMediaList((mediaList) =>
    {
        foreach (var m in mediaList)
        {
            Console.WriteLine(m.MediaId);
        }
    }, (errors) =>
    {
        foreach (var message in errors)
        {
            Console.WriteLine(message);
        }
    });