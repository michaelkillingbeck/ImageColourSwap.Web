using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ImageHelpers.Interfaces;
using Web.Interfaces;
using Web.Models;

namespace Web.Services;

public class DynamoDbResultsRepository : IImageResultsRepository<ResultsModel>
{
    private readonly AmazonDynamoDBClient _client;
    private readonly IResultsModelMapper<Document> _mapper;

    public DynamoDbResultsRepository(AmazonDynamoDBClient client, IResultsModelMapper<Document> mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<ResultsModel> LoadResults(string id)
    {
        Amazon.DynamoDBv2.DocumentModel.Table table = Table.LoadTable(_client, "ics-test");
        
        AttributeValue hashKey = new AttributeValue { S = ResultsModel.Id };
        AttributeValue sortKey = new AttributeValue { S = id };

        Dictionary<string, Condition> keyConditions = new Dictionary<string, Condition>
        {        
            { 
                "Id",
                new Condition
                {
                    ComparisonOperator = "EQ",
                    AttributeValueList = new List<AttributeValue> { hashKey }
                }
            },
            { 
                "ResultsId",
                new Condition
                {
                    ComparisonOperator = "EQ",
                    AttributeValueList = new List<AttributeValue> { sortKey }
                }
            }
        };
        
        QueryRequest request = new QueryRequest
        {
            KeyConditions = keyConditions,
            Limit = 1,
            TableName = "ics-test",
        };

        var result = await _client.QueryAsync(request);

        var document = new Document();
        foreach(var kvp in result.Items[0])
        {
            document[kvp.Key] = kvp.Value.S;
        }

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