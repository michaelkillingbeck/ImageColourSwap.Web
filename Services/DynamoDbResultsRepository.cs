using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Image_Colour_Swap.Interfaces;
using Web.Interfaces;
using Web.Models;

namespace Web.Services;

public class DynamoDbResultsSaver : IImageResultsRepository<ResultsModel>
{
    private readonly AmazonDynamoDBClient _client;
    private readonly IResultsModelMapper<Document> _mapper;

    public DynamoDbResultsSaver(AmazonDynamoDBClient client, IResultsModelMapper<Document> mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<ResultsModel> LoadResults(string id)
    {
        Amazon.DynamoDBv2.DocumentModel.Table table = Table.LoadTable(_client, "ics-test");
        var document = await table.GetItemAsync(id);
        return _mapper.Convert(document);
    }

    public async Task<bool> SaveResults(ResultsModel results)
    {        
        Amazon.DynamoDBv2.DocumentModel.Table table = Table.LoadTable(_client, "ics-test");
        var document = _mapper.ConvertFrom(results);
        var result = await table.PutItemAsync(document);

        return true;
    }
}