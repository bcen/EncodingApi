# EncodingApi (Work in progress)

## Introduction

EncodingApi is a .NET C# client wrapper for the [encoding.com](http://www.encoding.com/) API.
It allows users to interact with [encoding.com](http://www.encoding.com/) API in an object oriented way.

Current release is a preview of the API; all public APIs are subjected to change without notice.

Click [here](http://www.encoding.com/api/category/category/complete_api_documentation) for the complete official XML API documentation.


## Example

Creating a client object with user id and user key:
    
    EncodingServiceClient client = new EncodingServiceClient("id", "key");
    
Adding media for transcoding:

    Uri[] sources = new Uri[]
    { 
      new Uri("http://www.path2myvideo.com/video.wmv")
      // if multiple source url specified, they will be concatenated.
    };
    
    EncodingFormat[] formats = new EncodingFormat[] { new EncodingFormat("flv") };
    
    try
    {
        string mediaId= client.AddMedia(sources, formats);
    }
    catch (EncodingServiceException ex)
    {
        // logs the exception.
    }

    // Non-blocking call(async)
    
    client.AddMediaAsync(sources, formats,
    (mediaId) =>
    {
        // Do something with the mediaId.
    },
    (errors) =>
    {
        // Handles the errors.
    });
    
Getting a list of media:

    try
    {
        foreach (var m in client.GetMediaList())
        {
            Console.WriteLine(m.MediaStatus);
        }
    }
    catch (EncodingServiceException ex)
    {
        // logs the exception.
    }
    
    // Non-blocking call(async)
    
    client.GetMediaListAsync(
    (mediaList) =>
    {
        foreach (var m in mediaList)
        {
            // ...
        }
    },
    (errors) =>
    {
        foreach (var str in errors)
        {
            // ...
        }
    });
