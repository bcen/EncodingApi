# EncodingApi (Work in progress)

## Introduction
EncodingApi is a .NET C# client wrapper for the [encoding.com](http://www.encoding.com/) API.
It allows users to interact with [encoding.com]http://www.encoding.com/) API in an object oriented way.

Current release is a preview of the API; all public APIs are subjected to change without notice.

Click [here](http://www.encoding.com/api/category/category/complete_api_documentation) for the complete official XML API documentation.


## Example

Creating a client object with user id and uer key:
    
    EncodingServiceClient client = new EncodingServiceClient("id", "key");
    
Getting a list of media:

    try
    {
        foreach (var m in client.GetMediaList())
        {
            Console.WriteLine(m.MediaId);
        }
    }
    catch (EncodingServiceException ex)
    {
        Console.WriteLine(ex.Message);
    }

    // or getting it asynchronously

    client.GetMediaListAsync(
    (mediaList) =>
    {
        foreach (var m in mediaList)
        {
            Console.WriteLine(m.MediaFile);
        }
    },
    (errors) =>
    {
        foreach (var msg in errors)
        {
            Console.WriteLine(msg);
        }
    });