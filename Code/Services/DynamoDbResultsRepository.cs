using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ImageHelpers.Interfaces;
using Web.Interfaces;
using Web.Models;

namespace Web.Services;

public class DynamoDbResultsRepository(AmazonDynamoDBClient client, IResultsModelMapper<Document> mapper) : IImageResultsRepository<ResultsModel>
{
    private readonly AmazonDynamoDBClient _client = client;
    private readonly IResultsModelMapper<Document> _mapper = mapper;

    public async Task<ResultsModel> LoadResults(string id)
    {
        AttributeValue hashKey = new() { S = ResultsModel.Id };
        AttributeValue sortKey = new() { S = id };

        Dictionary<string, Condition> keyConditions = new()
        {
            {
                "Id",
                new Condition
                {
                    ComparisonOperator = "EQ",
                    AttributeValueList = [hashKey],
                }
            },
            {
                "ResultsId",
                new Condition
                {
                    ComparisonOperator = "EQ",
                    AttributeValueList = [sortKey],
                }
            },
        };

        QueryRequest request = new()
        {
            KeyConditions = keyConditions,
            Limit = 1,
            TableName = "ics-test",
        };

        QueryResponse result = await _client.QueryAsync(request);

        Document document = [];
        foreach (KeyValuePair<string, AttributeValue> kvp in result.Items[0])
        {
            document[kvp.Key] = kvp.Value.S;
        }

        return _mapper.Convert(document);
    }

    public async Task<bool> SaveResults(ResultsModel results)
    {
        Table table = Table.LoadTable(_client, "ics-test");
        Document document = _mapper.ConvertFrom(results);
        _ = await table.PutItemAsync(document);

        return true;
    }
}