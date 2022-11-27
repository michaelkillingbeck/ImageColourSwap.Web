using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ImageHelpers.Interfaces;
using Web.Interfaces;
using Web.Models;

namespace Web.Services;

public class DynamoDbPagedResultsRepository : IImageResultsRepository<PagedResultsModel>
{
    private readonly AmazonDynamoDBClient _client;
    private readonly IResultsModelMapper<Document> _mapper;

    public DynamoDbPagedResultsRepository(AmazonDynamoDBClient client, IResultsModelMapper<Document> mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<PagedResultsModel> LoadResults(string nextPage)
    {
        Amazon.DynamoDBv2.DocumentModel.Table table = Table.LoadTable(_client, "ics-test");
        
        AttributeValue hashKey = new AttributeValue { S = ResultsModel.Id };

        Dictionary<string, Condition> keyConditions = new Dictionary<string, Condition>
        {        
            { 
                "Id",
                new Condition
                {
                    ComparisonOperator = "EQ",
                    AttributeValueList = new List<AttributeValue> { hashKey }
                }
            }
        };

        QueryRequest request = new QueryRequest
        {
            KeyConditions = keyConditions,
            Limit = 5,
            TableName = "ics-test",
        };

        if(string.IsNullOrEmpty(nextPage) == false)
        {
            Dictionary<string, AttributeValue> exclusiveKey = new Dictionary<string, AttributeValue>();
            exclusiveKey.Add("Id", hashKey);
            exclusiveKey.Add("ResultsId", new AttributeValue { S = nextPage });

            request.ExclusiveStartKey = exclusiveKey;
        }

        var result = await _client.QueryAsync(request);

        var documentList = new List<Document>();

        foreach(var item in result.Items)
        {
            var document = new Document();
            foreach(var kvp in item)
            {
                document[kvp.Key] = kvp.Value.S;
            }

            documentList.Add(document);
        }

        var returnModel = new PagedResultsModel
        {
            NextPage = result.LastEvaluatedKey.Any(),
            Results = _mapper.Convert(documentList).ToList()
        };

        if(returnModel.NextPage)
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