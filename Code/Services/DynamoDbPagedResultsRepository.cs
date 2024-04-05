using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ImageHelpers.Interfaces;
using Web.Interfaces;
using Web.Models;

namespace Web.Services;

public class DynamoDbPagedResultsRepository(AmazonDynamoDBClient client, IResultsModelMapper<Document> mapper) : IImageResultsRepository<PagedResultsModel>
{
    private readonly AmazonDynamoDBClient _client = client;
    private readonly IResultsModelMapper<Document> _mapper = mapper;

    public async Task<PagedResultsModel> LoadResults(string id)
    {
        AttributeValue hashKey = new() { S = ResultsModel.Id };

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
        };

        QueryRequest request = new()
        {
            KeyConditions = keyConditions,
            Limit = 5,
            TableName = "ics-test",
        };

        if (!string.IsNullOrEmpty(id))
        {
            Dictionary<string, AttributeValue> exclusiveKey = new()
            {
                { "Id", hashKey },
                { "ResultsId", new AttributeValue { S = id } },
            };

            request.ExclusiveStartKey = exclusiveKey;
        }

        QueryResponse result = await _client.QueryAsync(request);

        List<Document> documentList = [];

        foreach (Dictionary<string, AttributeValue>? item in result.Items)
        {
            Document document = [];
            foreach (KeyValuePair<string, AttributeValue> kvp in item)
            {
                document[kvp.Key] = kvp.Value.S;
            }

            documentList.Add(document);
        }

        PagedResultsModel returnModel = new()
        {
            NextPage = result.LastEvaluatedKey.Count == 0,
            Results = _mapper.Convert(documentList).ToList(),
        };

        if (returnModel.NextPage)
        {
            returnModel.PageMarkers.Add(result.LastEvaluatedKey["ResultsId"].S);
        }
        else
        {
            returnModel.PageMarkers.Add("end");
        }

        return returnModel;
    }

    public Task<bool> SaveResults(PagedResultsModel results)
    {
        throw new NotImplementedException();
    }
}