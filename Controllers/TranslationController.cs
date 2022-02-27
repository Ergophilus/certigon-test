using System.Xml.Serialization;
using Certigon.Test.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Certigon.Test.Controllers;

public class TranslationController : ControllerBase
{
    [HttpPost("api/translate/json")]
    public OrganizationDto XmlToJson(IFormFile input)
    {
        if (input.ContentType != "application/xml" && input.ContentType != "text/xml")
        {
            throw new ArgumentException("Invalid input file type.");
        }

        using var stream = new MemoryStream();
        input.CopyTo(stream);
        stream.Position = 0;

        var serializer = new XmlSerializer(typeof(OrganizationDto));
        return serializer.Deserialize(stream) as OrganizationDto ?? throw new InvalidOperationException("Unable to deserialize the XML file.");
    }
}