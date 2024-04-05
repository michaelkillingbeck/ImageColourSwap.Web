using Amazon.DynamoDBv2.DocumentModel;
using Web.Interfaces;
using Web.Models;

namespace Web.Helpers;

public class DynamoDocumentResultsModelMapper : IResultsModelMapper<Document>
{
    public IEnumerable<ResultsModel> Convert(IEnumerable<Document> sourceCollection)
    {
        List<ResultsModel> returnList = [];

        foreach (Document document in sourceCollection)
        {
            returnList.Add(Convert(document));
        }

        return returnList;
    }

    public ResultsModel Convert(Document sourceModel)
    {
        return new ResultsModel
        {
            OutputImage = sourceModel["result"],
            PalletteImage = sourceModel["pallette"],
            ResultsId = sourceModel["ResultsId"],
            SourceImage = sourceModel["source"],
        };
    }

    public Document ConvertFrom(ResultsModel resultsModel)
    {
        Document document = new()
        {
            ["Id"] = ResultsModel.Id,
            ["ResultsId"] = resultsModel.ResultsId,
            ["pallette"] = resultsModel.PalletteImage,
            ["result"] = resultsModel.OutputImage,
            ["source"] = resultsModel.SourceImage,
        };

        return document;
    }
}