using Amazon.DynamoDBv2.DocumentModel;
using Web.Interfaces;
using Web.Models;

namespace Web.Helpers;

public class DynamoDocumentResultsModelMapper : IResultsModelMapper<Document>
{
    public ResultsModel Convert(Document sourceModel)
    {
        return new ResultsModel
        {
            OutputImage = sourceModel["result"],
            PalletteImage = sourceModel["pallette"],
            SourceImage = sourceModel["source"],
        };
    }

    public Document ConvertFrom(ResultsModel resultsModel)
    {
        Document document = new Document();
        document["Id"] = resultsModel.Id;
        document["pallette"] = resultsModel.PalletteImage;
        document["result"] = resultsModel.OutputImage;
        document["source"] = resultsModel.SourceImage;

        return document;
    }
}